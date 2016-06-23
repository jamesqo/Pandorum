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
using Pandorum.Core.Net.Http;
using System.Net.Http.Headers;
using Pandorum.Core.Json;
using Newtonsoft.Json.Serialization;

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
            var uri = CreateUri("test.checkLicensing");
            return _httpClient.GetJsonAsync(uri);
        }

        public Task<JObject> PartnerLogin(PartnerLoginOptions options)
        {
            var uri = CreateUri("auth.partnerLogin");
            var body = SerializeObject(options, includeSyncTime: false, includeAuthToken: false);
            return PostAndReadJson(uri, body, encrypt: false); // POST body for partner login isn't encrypted
        }

        public Task<JObject> UserLogin(UserLoginOptions options)
        {
            var uri = CreateUri("auth.userLogin");
            var body = SerializeObject(options, includeSyncTime: true, includeAuthToken: false);
            return PostAndReadJson(uri, body);
        }

        // Helpers

        private PandoraUriBuilder CreateUriBuilder()
        {
            return new PandoraUriBuilder(Settings.Endpoint);
        }

        private string EncryptToHex(string input)
        {
            return BlowfishEcb.EncryptStringToHex(input, Settings.PartnerInfo.EncryptPassword);
        }

        private long CalculateSyncTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTime() + Settings.SyncTimestamp;
        }

        private string SerializeObject(object obj, bool includeSyncTime = true, bool includeAuthToken = true)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new OptionalBoolConverter() },
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            var serializer = JsonSerializer.CreateDefault(settings);
            var jobject = JObject.FromObject(obj, serializer);

            if (includeSyncTime)
                jobject["syncTime"] = CalculateSyncTime();
            if (includeAuthToken)
                jobject["userAuthToken"] = Settings.AuthToken;

            return jobject.ToString();
        }

        private string CreateUri(string method)
        {
            return CreateUriBuilder()
                .WithMethod(method)
                .WithAuthToken(Settings.AuthToken)
                .WithPartnerId(Settings.PartnerId)
                .WithUserId(Settings.UserId)
                .ToString();
        }

        private async Task<JObject> PostAndReadJson(string uri, string body, bool encrypt = true)
        {
            if (encrypt)
            {
                body = EncryptToHex(body);
            }

            // TODO: Is using PooledStringContent worth the
            // extra Task allocations/complexity here? (I think so)
            using (var content = new PooledStringContent(body))
            {
                return await _httpClient.PostAndReadJsonAsync(uri, content).ConfigureAwait(false);
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
            var uri = CreateUri("user.getStationListChecksum");
            var body = SerializeObject(ImmutableCache.EmptyObject);
            return PostAndReadJson(uri, body);
        }

        public Task<JObject> Search(SearchOptions options)
        {
            var uri = CreateUri("music.search");
            var body = SerializeObject(options);
            return PostAndReadJson(uri, body);
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
