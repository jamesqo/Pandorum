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
            
            using (var content = new PooledStringContent(body))
            {
                return await _httpClient.PostAndReadJsonAsync(uri, content).ConfigureAwait(false);
            }
        }

        // Aggregator method that enscapulates common workflow
        // Create URI -> Serialize options -> encrypt -> send POST
        // request -> parse as JObject
        // TODO: Find a better name for this?
        private Task<JObject> TypicalWorkflow(string method, object options)
        {
            var uri = CreateUri(method);
            var body = SerializeObject(options);
            return PostAndReadJson(uri, body);
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
            return TypicalWorkflow("user.getStationList", options);
        }

        public Task<JObject> GetStationListChecksum()
        {
            // No options to pass in, so substitute with an empty object
            return TypicalWorkflow("user.getStationListChecksum", ImmutableCache.EmptyObject);
        }

        public Task<JObject> Search(SearchOptions options)
        {
            return TypicalWorkflow("music.search", options);
        }

        public Task<JObject> CreateStation(CreateStationOptions options)
        {
            return TypicalWorkflow("station.createStation", options);
        }

        public Task<JObject> AddMusic(AddMusicOptions options)
        {
            return TypicalWorkflow("station.addMusic", options);
        }

        public Task<JObject> DeleteMusic(DeleteMusicOptions options)
        {
            return TypicalWorkflow("station.deleteMusic", options);
        }

        public Task<JObject> RenameStation(RenameStationOptions options)
        {
            return TypicalWorkflow("station.renameStation", options);
        }

        public Task<JObject> DeleteStation(DeleteStationOptions options)
        {
            return TypicalWorkflow("station.deleteStation", options);
        }

        public Task<JObject> GetStation(GetStationOptions options)
        {
            return TypicalWorkflow("station.getStation", options);
        }

        public Task<JObject> DeleteFeedback(DeleteFeedbackOptions options)
        {
            return TypicalWorkflow("station.deleteFeedback", options);
        }

        public Task<JObject> GetGenreStations()
        {
            return TypicalWorkflow("station.getGenreStations", ImmutableCache.EmptyObject);
        }

        public Task<JObject> GetGenreStationsChecksum(GetGenreStationsChecksumOptions options)
        {
            return TypicalWorkflow("station.getGenreStationsChecksum", options);
        }

        public Task<JObject> ShareStation(ShareStationOptions options)
        {
            return TypicalWorkflow("station.shareStation", options);
        }

        public Task<JObject> TransformSharedStation(TransformSharedStationOptions options)
        {
            return TypicalWorkflow("station.transformSharedStation", options);
        }

        public Task<JObject> SetQuickMix(SetQuickMixOptions options)
        {
            return TypicalWorkflow("user.setQuickMix", options);
        }
    }
}
