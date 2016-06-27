using Newtonsoft.Json.Linq;
using Pandorum.Core;
using Pandorum.Core.Cryptography;
using Pandorum.Core.Net;
using Pandorum.Core.Time;
using Pandorum.Net;
using Pandorum.Net.Authentication;
using Pandorum.Core.Options.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pandorum.Stations;
using Pandorum.Core.Json;
using static Pandorum.Core.Json.JsonHelpers;

namespace Pandorum
{
    public class PandoraClient : IDisposable
    {
        internal IPandoraJsonClient _baseClient;

        private StationsClient _stations;

        public PandoraClient()
            : this(PandoraEndpoints.Tuner.HttpsUri, PandoraEndpoints.Tuner.iOS)
        {
        }

        public PandoraClient(string endpoint, IPartnerInfo partnerInfo)
            : this(endpoint, partnerInfo, new VerifyingJsonClient())
        {
        }

        public PandoraClient(string endpoint, IPartnerInfo partnerInfo, IPandoraJsonClient baseClient)
        {
            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));
            if (partnerInfo == null)
                throw new ArgumentNullException(nameof(partnerInfo));
            if (baseClient == null)
                throw new ArgumentNullException(nameof(baseClient));

            _baseClient = baseClient;
            Settings = new PandoraClientSettings(baseClient.Settings)
            {
                Endpoint = endpoint,
                PartnerInfo = partnerInfo
            };
        }

        public PandoraClientSettings Settings { get; }

        // Entry point for other clients

        public StationsClient Stations =>
            _stations ?? (_stations = new StationsClient(this));

        // Authentication

        // TODO: Make all of these accept opaque types
        // and have these overloads implemented as
        // extension methods.

        public async Task<bool> CheckLicensing()
        {
            var response = await _baseClient.CheckLicensing().ConfigureAwait(false);
            var result = GetResult(response);
            return (bool)result["isAllowed"];
        }

        public async Task Login(string username, string password)
        {
            await PartnerLogin().ConfigureAwait(false);
            await UserLogin(username, password).ConfigureAwait(false);
        }

        // Partner login

        public async Task PartnerLogin()
        {
            var options = CreatePartnerLoginOptions();
            var response = await _baseClient.PartnerLogin(options).ConfigureAwait(false);
            var result = GetResult(response);
            HandlePartnerLogin(result);
        }

        private PartnerLoginOptions CreatePartnerLoginOptions()
        {
            return new PartnerLoginOptions
            {
                Username = Settings.PartnerInfo.Username,
                Password = Settings.PartnerInfo.Password,
                DeviceModel = Settings.PartnerInfo.DeviceId,
                Version = "5" // hard-coded, for now it's 5
                // TODO: Other parameters
            };
        }

        private void HandlePartnerLogin(JToken result)
        {
            // First parse the syncTime, subtract it from the
            // current Unix time, and set the SyncTimestamp
            var syncTime = (string)result["syncTime"];
            var parsedTimestamp = DateTimeOffset.UtcNow.ToUnixTime() - DecryptSyncTime(syncTime);
            _baseClient.Settings.SyncTimestamp = parsedTimestamp;

            // Set partner_id, auth_token
            _baseClient.Settings.AuthToken = (string)result["partnerAuthToken"];
            _baseClient.Settings.PartnerId = (string)result["partnerId"];
        }

        // User login

        public async Task UserLogin(string username, string password)
        {
            var options = CreateUserLoginOptions(username, password);
            var response = await _baseClient.UserLogin(options).ConfigureAwait(false);
            var result = GetResult(response);
            HandleUserLogin(result);
        }

        private UserLoginOptions CreateUserLoginOptions(string username, string password)
        {
            return new UserLoginOptions
            {
                LoginType = "user",
                Username = username,
                Password = password,
                PartnerAuthToken = _baseClient.Settings.AuthToken
                // TODO: ReturnCapped = true
            };
        }

        private void HandleUserLogin(JToken result)
        {
            // Overwrite the partnerAuthToken with userAuthToken
            _baseClient.Settings.AuthToken = (string)result["userAuthToken"];
            _baseClient.Settings.UserId = (string)result["userId"];
        }

        // Helpers

        private long DecryptSyncTime(string input)
        {
            var decryptedString = BlowfishEcb.DecryptHexToString(input, Settings.PartnerInfo.DecryptPassword);
            decryptedString = decryptedString.Substring(4); // skip first four bytes of garbage
            return long.Parse(decryptedString.Replace("\u0002", string.Empty)); // TODO: Can other control chars appear at the end?
            // Maybe it should just be decryptedString.Substring(4, 10) instead
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
