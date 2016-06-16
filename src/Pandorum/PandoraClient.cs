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
            : this(useHttps: true) // TODO: Which one is better?
        {
        }

        public PandoraClient(bool useHttps)
            // TODO: Which one is better?
            : this(useHttps ? PandoraEndpoints.TunerHttps : PandoraEndpoints.TunerHttp)
        {
        }

        public PandoraClient(string endpoint)
        {
            _endpoint = endpoint;
            _httpClient = new HttpClient();
        }

        // API functionality

        public async Task<bool> CheckLicensing()
        {
            var response = await GetResponseObject("method", "test.checkLicensing");
            CheckStatus(response);
            return (bool)response["result"]["isAllowed"];
        }

        protected void CheckStatus(JObject response)
        {
            var status = (string)response["stat"];
            if (status != "ok")
                throw new PandoraStatusException();
        }

        // Request logic

        // TODO: Move to QueryBuilder class?
        private string BuildUri(params string[] parameters)
        {
            if (parameters.Length % 2 != 0)
                throw new ArgumentException("The query parameters should have an even number of key/values.", nameof(parameters));

            var builder = new QueryBuilder();
            for (int i = 0; i < parameters.Length; i += 2)
            {
                string key = parameters[i];
                string value = parameters[i + 1];
                builder.Add(key, value);
            }
            return _endpoint + builder.ToString();
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
                if (_endpoint != null) _endpoint = null;
            }
        }
    }
}
