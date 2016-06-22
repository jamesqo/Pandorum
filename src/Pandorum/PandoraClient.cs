using Newtonsoft.Json.Linq;
using Pandorum.Core;
using Pandorum.Core.Net;
using Pandorum.Net;
using Pandorum.Net.Authentication;
using Pandorum.Options.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pandorum
{
    public class PandoraClient : IDisposable
    {
        private IPandoraJsonClient _baseClient;

        public PandoraClient()
            : this(PandoraEndpoints.Tuner.HttpsUri,
                  PandoraEndpoints.Tuner.iOS)
        {
        }

        // TODO: Maybe just a PandoraClient(string) ctor should
        // be added here, but then how would we infer the partnerInfo?

        // Also changes to the endpoint might need to effect changes
        // to the PartnerInfo in Settings

        public PandoraClient(string endpoint, IPartnerInfo partnerInfo)
            : this(endpoint, partnerInfo, new VerifyingJsonClient())
        {
        }

        public PandoraClient(string endpoint, IPartnerInfo partnerInfo, IPandoraJsonClient baseClient)
        {
            _baseClient = baseClient;
            Settings = new PandoraClientSettings(baseClient.Settings)
            {
                Endpoint = endpoint,
                PartnerInfo = partnerInfo
            };
        }

        public PandoraClientSettings Settings { get; }

        // Authentication

        public Task<bool> CheckLicensing()
        {
            return AwaitAndSelectResult(
                _baseClient.CheckLicensing(),
                (result, _) => (bool)result["isAllowed"]);
        }

        // TODO: Could we somehow expose the syncTime/auth
        // details to the user without actually logging in?
        // Async methods can't have ref/out params, so we
        // can't do something like
        //
        // public async Task PartnerLogin(out PartnerLoginInfo info)
        //
        // unforunately.

        public Task PartnerLogin(PartnerLoginOptions options)
        {
            return AwaitAndSelectResult(
                _baseClient.PartnerLogin(options),
                (result, self) => self.HandlePartnerLogin(result));
        }

        private void HandlePartnerLogin(JToken result)
        {
            var syncTime = (string)result["syncTime"];
        }

        // Helpers

        private long DecryptSyncTime(string input)
        {

        }

        private async Task AwaitAndSelectResult(Task<JObject> task, Action<JToken, PandoraClient> action)
        {
            var response = await task.ConfigureAwait(false);
            action(response["result"], this);
        }

        private async Task<T> AwaitAndSelectResult<T>(Task<JObject> task, Func<JToken, PandoraClient, T> func)
        {
            var response = await task.ConfigureAwait(false);
            return func(response["result"], this);
        }
        
        // Dispose logic

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_baseClient != null)
                {
                    _baseClient.Dispose();
                    _baseClient = null;
                }
            }
        }
    }
}
