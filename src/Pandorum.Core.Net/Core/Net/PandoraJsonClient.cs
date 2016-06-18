using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pandorum.Core.Net.Options;
using Newtonsoft.Json;
using Pandorum.Core.Net.Authentication;

namespace Pandorum.Core.Net
{
    public class PandoraJsonClient : IPandoraJsonClient
    {
        private HttpClient _httpClient;

        public PandoraJsonClient(string endpoint)
            : this(endpoint, PandoraEndpoints.Tuner.iOS)
        {
        }

        public PandoraJsonClient(string endpoint, IPartnerInfo partnerInfo)
            : this(endpoint, partnerInfo, new HttpClient())
        {
        }

        public PandoraJsonClient(string endpoint, IPartnerInfo partnerInfo, HttpClient baseClient)
        {
            Endpoint = endpoint;
            PartnerInfo = partnerInfo;
            _httpClient = baseClient;
        }

        public string Endpoint { get; }
        public IPartnerInfo PartnerInfo { get; }

        // API functionality

        public Task<JObject> CheckLicensing()
        {
            var uri = CreateUriBuilder()
                .WithMethod("test.checkLicensing")
                .ToString();
            return _httpClient.GetJsonAsync(uri);
        }

        public Task<JObject> PartnerLogin(PartnerLoginOptions options)
        {
            var uri = CreateUriBuilder()
                .WithMethod("auth.partnerLogin")
                .ToString();
            var body = JsonConvert.SerializeObject(options);
            var content = new StringContent(body);
            return _httpClient.PostAndReadJsonAsync(uri, content);
        }

        public Task<JObject> UserLogin(UserLoginOptions options)
        {
            var uri = CreateUriBuilder()
                .WithMethod("auth.userLogin")
                .ToString();
            var body = JsonConvert.SerializeObject(options);
            // TODO
            return default(Task<JObject>);
        }

        // Helpers

        private PandoraUriBuilder CreateUriBuilder()
        {
            return new PandoraUriBuilder(Endpoint);
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
