using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pandorum.Core.Options;
using Newtonsoft.Json;
using Pandorum.Net.Authentication;
using Pandorum.Core.Cryptography;
using System.Text;
using System.Buffers;
using System.Diagnostics;
using Pandorum.Net;
using Pandorum.Core.Options.Authentication;

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
            return LoadContentAndReturn(body,
                content => _httpClient.PostAndReadJsonAsync(uri, content));
        }

        public Task<JObject> UserLogin(UserLoginOptions options)
        {
            var uri = CreateUriBuilder()
                .WithMethod("auth.userLogin")
                .ToString();
            var body = JsonConvert.SerializeObject(options);
            body = BlowfishEcb.EncryptStringToHex(body, PartnerInfo.EncryptPassword);
            return LoadContentAndReturn(body,
                content => _httpClient.PostAndReadJsonAsync(uri, content));
        }

        // Helpers

        private PandoraUriBuilder CreateUriBuilder()
        {
            return new PandoraUriBuilder(Endpoint);
        }

        // TODO: PooledStringContent so we can use a simple using statement

        private static T LoadContentAndReturn<T>(string text, Func<HttpContent, T> func)
        {
            return LoadContentAndReturn(text, Encoding.UTF8, func);
        }

        private static T LoadContentAndReturn<T>(string text, Encoding encoding, Func<HttpContent, T> func)
        {
            int byteCount = encoding.GetByteCount(text);

            var pooled = ArrayPool<byte>.Shared.Rent(byteCount);
            try
            {
                int written = encoding.GetBytes(text, 0, text.Length, pooled, 0);
                Debug.Assert(byteCount == written);
                using (var content = new ByteArrayContent(pooled, 0, written))
                {
                    return func(content);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(pooled);
            }
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
