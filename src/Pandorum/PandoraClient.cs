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
            return response["result"]["isAllowed"].ToObject<bool>();
        }

        protected void CheckStatus(JObject response)
        {
            var status = response["stat"].ToObject<string>();
            if (status != "ok")
                throw new HttpRequestException(); // TODO: Find more appropriate exception
        }

        // Request logic

        protected async Task<JObject> GetResponseObject(params string[] parameters)
        {
            return JObject.Parse(await GetResponseString(parameters));
        }

        protected Task<string> GetResponseString(params string[] parameters)
        {
            var uri = BuildUri(parameters);
            return _httpClient.GetStringAsync(uri);
        }

        // TODO: Move to QueryBuilder class?
        private string BuildUri(string[] parameters)
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
