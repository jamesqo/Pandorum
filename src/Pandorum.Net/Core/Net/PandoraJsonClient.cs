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
using Pandorum.Core.Time;
using Pandorum.Core.Options.Stations;

namespace Pandorum.Core.Net
{
    public class PandoraJsonClient : IPandoraJsonClient
    {
        private HttpClient _httpClient;

        public PandoraJsonClient()
            : this(new JsonClientSettings())
        {
        }

        public PandoraJsonClient(JsonClientSettings settings)
            : this(settings, new HttpClient())
        {
        }

        public PandoraJsonClient(IJsonClientSettings settings, HttpClient baseClient)
        {
            Settings = settings;
            _httpClient = baseClient;
        }

        public IJsonClientSettings Settings { get; }

        // API functionality

        // Authentication

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
            body = BlowfishEcb.EncryptStringToHex(body, Settings.PartnerInfo.EncryptPassword);
            return LoadContentAndReturn(body,
                content => _httpClient.PostAndReadJsonAsync(uri, content));
        }

        // Helpers

        private PandoraUriBuilder CreateUriBuilder()
        {
            return new PandoraUriBuilder(Settings.Endpoint);
        }

        private long CalculateSyncTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTime() + Settings.SyncTimestamp;
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

        // TODO: Move this up? Or not
        // Stations

        public Task<JObject> GetStationList(GetStationListOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> GetStationListChecksum()
        {
            throw new NotImplementedException();
        }

        public Task<JObject> Search(SearchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> CreateStation(CreateStationOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> AddMusic(AddMusicOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> DeleteMusic(DeleteMusicOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> RenameStation(RenameStationOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> DeleteStation(DeleteStationOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> GetStation(GetStationOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> DeleteFeedback(DeleteFeedbackOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> GetGenreStations()
        {
            throw new NotImplementedException();
        }

        public Task<JObject> GetGenreStationsChecksum(GetGenreStationsChecksumOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> ShareStation(ShareStationOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> TransformSharedStation(TransformSharedStationOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> SetQuickMix(SetQuickMixOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
