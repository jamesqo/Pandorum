using Newtonsoft.Json.Linq;
using Pandorum.Core;
using Pandorum.Core.Net;
using Pandorum.Options;
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
            : this(new PandoraClientSettings())
        {
        }

        public PandoraClient(IPandoraClientSettings settings)
            : this(settings, new VerifyingJsonClient())
        {
        }

        public PandoraClient(IPandoraClientSettings settings, IPandoraJsonClient baseClient)
        {
            Settings = settings;
            _baseClient = baseClient;
        }

        public IPandoraClientSettings Settings { get; }

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
