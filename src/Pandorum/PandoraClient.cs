﻿using Newtonsoft.Json.Linq;
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

namespace Pandorum
{
    public class PandoraClient : IDisposable
    {
        private IPandoraJsonClient _baseClient;

        public PandoraClient()
            : this(PandoraEndpoints.Tuner.HttpsUri,
                  PandoraEndpoints.Tuner.iOS)
        {
        }

        // TODO: Maybe just a PandoraClient(string) ctor should
        // be added here, but then how would we infer the partnerInfo?

        // Also changes to the endpoint might need to effect changes
        // to the PartnerInfo in Settings

        public PandoraClient(string endpoint, IPartnerInfo partnerInfo)
            : this(endpoint, partnerInfo, new VerifyingJsonClient())
        {
        }

        public PandoraClient(string endpoint, IPartnerInfo partnerInfo, IPandoraJsonClient baseClient)
        {
            if (endpoint == null || partnerInfo == null || baseClient == null)
            {
                string paramName = (endpoint == null) ?
                    nameof(endpoint) :
                    (partnerInfo == null) ?
                    nameof(partnerInfo) :
                    nameof(baseClient);

                throw new ArgumentNullException(paramName);
            }

            _baseClient = baseClient;
            Settings = new PandoraClientSettings(baseClient.Settings)
            {
                Endpoint = endpoint,
                PartnerInfo = partnerInfo
            };
        }

        public PandoraClientSettings Settings { get; }

        // Authentication

        public Task<bool> CheckLicensing()
        {
            return AwaitAndSelectResult(
                _baseClient.CheckLicensing(),
                (result, _) => (bool)result["isAllowed"]);
        }

        // TODO: Could we somehow expose the syncTime/auth
        // details to the user without actually logging in?
        // Async methods can't have ref/out params, so we
        // can't do something like
        //
        // public async Task PartnerLogin(out PartnerLoginInfo info)
        //
        // unforunately.

        public Task PartnerLogin()
        {
            return AwaitAndSelectResult(
                _baseClient.PartnerLogin(CreatePartnerLoginOptions()),
                (result, self) => self.HandlePartnerLogin(result));
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
            var settings = _baseClient.Settings; // cache interface method call
            // TODO: Maybe this 'violates the contract' of the interface, e.g.
            // you could have a IJsonClientSettings that when set_SyncTimestamp
            // was called changed the value of _baseClient.Settings
            settings.SyncTimestamp = parsedTimestamp;

            // Set partner_id, auth_token
            settings.AuthToken = (string)result["partnerAuthToken"];
            settings.PartnerId = (string)result["partnerId"];
        }

        // Helpers

        private long DecryptSyncTime(string input)
        {
            var decryptedString = BlowfishEcb.DecryptHexToString(input, Settings.PartnerInfo.DecryptPassword);
            decryptedString = decryptedString.Substring(4); // skip first four bytes of garbage
            return long.Parse(decryptedString.Replace("\u0002", string.Empty)); // TODO: Can other control chars appear at the end?
        }

        private async Task AwaitAndSelectResult(Task<JObject> task, Action<JToken, PandoraClient> action)
        {
            var response = await task.ConfigureAwait(false);
            action(response["result"], this);
        }

        private async Task<T> AwaitAndSelectResult<T>(Task<JObject> task, Func<JToken, PandoraClient, T> func)
        {
            var response = await task.ConfigureAwait(false);
            return func(response["result"], this);
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
