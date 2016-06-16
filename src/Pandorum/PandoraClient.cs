using Newtonsoft.Json.Linq;
using Pandorum.Core;
using Pandorum.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pandorum
{
    public class PandoraClient : IDisposable
    {
        private string _endpoint;
        private HttpClient _httpClient;

        public PandoraClient()
            : this(useHttps: true) // TODO: Which one is better? HTTP or HTTPS?
        {
        }

        public PandoraClient(bool useHttps)
            // TODO: Which one is better? Internal or non-internal?
            : this(useHttps ?
                  PandoraEndpoints.TunerHttps :
                  PandoraEndpoints.TunerHttp)
        {
        }

        public PandoraClient(string endpoint)
            : this(endpoint, new HttpClient())
        {
        }

        public PandoraClient(string endpoint, HttpClient baseClient)
        {
            _endpoint = endpoint;
            _httpClient = baseClient;
        }

        // API functionality

        public Task<bool> CheckLicensing()
        {
            var uri = new PandoraUriBuilder()
                .WithMethod("test.checkLicensing")
                .ToString();
            return GetJsonAndMap(uri,
                obj => (bool)obj["result"]["isAllowed"]);
        }

        private async Task<T> GetJsonAndMap<T>(string uri, Func<JObject, T> func)
        {
            var response = await _httpClient.GetJsonAsync(uri).ConfigureAwait(false);
            CheckStatus(response);
            return func(response);
        }

        private static void CheckStatus(JObject response)
        {
            var status = (string)response["stat"];
            if (status != "ok")
                throw new PandoraStatusException();
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
                if (_endpoint != null)
                    _endpoint = null;
            }
        }
    }
}
