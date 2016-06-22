using Newtonsoft.Json.Linq;
using Pandorum.Core;
using Pandorum.Core.Net;
using Pandorum.Net;
using Pandorum.Net.Authentication;
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

        // API functionality

        public Task<bool> CheckLicensing()
        {
            return AwaitAndSelectResult(
                _baseClient.CheckLicensing(),
                result => (bool)result["isAllowed"]);
        }

        // Helpers

        private async static Task<T> AwaitAndSelectResult<T>(Task<JObject> task, Func<JToken, T> func)
        {
            var response = await task.ConfigureAwait(false);
            return func(response["result"]);
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
