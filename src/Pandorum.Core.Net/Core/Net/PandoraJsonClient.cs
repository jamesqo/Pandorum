using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pandorum.Core.Net.Options;

namespace Pandorum.Core.Net
{
    public class PandoraJsonClient : IPandoraJsonClient
    {
        private HttpClient _httpClient;

        public PandoraJsonClient(string endpoint)
            : this(endpoint, new HttpClient())
        {
        }

        public PandoraJsonClient(string endpoint, HttpClient baseClient)
        {
            Endpoint = endpoint;
            _httpClient = baseClient;
        }

        public string Endpoint { get; }

        // API functionality

        public Task<JObject> CheckLicensing(CheckLicensingOptions options)
        {
            var uri = CreateUriBuilder()
                .WithMethod("test.checkLicensing")
                .ToString();
            return GetJsonAsync(uri);
        }

        public Task<JObject> PartnerLogin(PartnerLoginOptions options)
        {
        }

        // Helpers

        private PandoraUriBuilder CreateUriBuilder()
        {
            return new PandoraUriBuilder(Endpoint);
        }

        private Task<JObject> GetJsonAsync(string requestUri)
        {
            return _httpClient.GetJsonAsync(requestUri);
        }

        // Dispose logic

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                    _httpClient = null;
                }
            }
        }
    }
}
