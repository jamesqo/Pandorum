using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Pandorum.Core.Options;
using System.Diagnostics;
using Pandorum.Core.Options.Authentication;
using Pandorum.Core.Options.Stations;

namespace Pandorum.Core.Net
{
    public class VerifyingJsonClient : IPandoraJsonClient
    {
        private IPandoraJsonClient _inner;

        public VerifyingJsonClient(IPandoraJsonClient inner)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));

            _inner = inner;
        }

        public IJsonClientSettings Settings => _inner.Settings;

        public Task<JObject> CheckLicensing()
        {
            return AwaitAndCheck(_inner.CheckLicensing());
        }

        public Task<JObject> PartnerLogin(PartnerLoginOptions options)
        {
            return AwaitAndCheck(_inner.PartnerLogin(options));
        }

        public Task<JObject> UserLogin(UserLoginOptions options)
        {
            return AwaitAndCheck(_inner.UserLogin(options));
        }

        public Task<JObject> GetStationList(GetStationListOptions options)
        {
            return AwaitAndCheck(_inner.GetStationList(options));
        }

        public Task<JObject> GetStationListChecksum()
        {
            return AwaitAndCheck(_inner.GetStationListChecksum());
        }

        public Task<JObject> Search(SearchOptions options)
        {
            return AwaitAndCheck(_inner.Search(options));
        }

        public Task<JObject> CreateStation(CreateStationOptions options)
        {
            return AwaitAndCheck(_inner.CreateStation(options));
        }

        public Task<JObject> AddMusic(AddMusicOptions options)
        {
            return AwaitAndCheck(_inner.AddMusic(options));
        }

        public Task<JObject> DeleteMusic(DeleteMusicOptions options)
        {
            return AwaitAndCheck(_inner.DeleteMusic(options));
        }

        public Task<JObject> RenameStation(RenameStationOptions options)
        {
            return AwaitAndCheck(_inner.RenameStation(options));
        }

        public Task<JObject> DeleteStation(DeleteStationOptions options)
        {
            return AwaitAndCheck(_inner.DeleteStation(options));
        }

        public Task<JObject> GetStation(GetStationOptions options)
        {
            return AwaitAndCheck(_inner.GetStation(options));
        }

        public Task<JObject> DeleteFeedback(DeleteFeedbackOptions options)
        {
            return AwaitAndCheck(_inner.DeleteFeedback(options));
        }

        public Task<JObject> GetGenreStations()
        {
            return AwaitAndCheck(_inner.GetGenreStations());
        }

        public Task<JObject> GetGenreStationsChecksum(GetGenreStationsChecksumOptions options)
        {
            return AwaitAndCheck(_inner.GetGenreStationsChecksum(options));
        }

        public Task<JObject> ShareStation(ShareStationOptions options)
        {
            return AwaitAndCheck(_inner.ShareStation(options));
        }

        public Task<JObject> TransformSharedStation(TransformSharedStationOptions options)
        {
            return AwaitAndCheck(_inner.TransformSharedStation(options));
        }

        public Task<JObject> SetQuickMix(SetQuickMixOptions options)
        {
            return AwaitAndCheck(_inner.SetQuickMix(options));
        }

        // Dispose logic

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_inner != null)
                {
                    _inner.Dispose();
                    _inner = null;
                }
            }
        }

        // Helper methods

        private async Task<JObject> AwaitAndCheck(Task<JObject> task)
        {
            var result = await task.ConfigureAwait(false);
            CheckStatus(result);
            return result;
        }

        private void CheckStatus(JObject response)
        {
            var status = (string)response["stat"];
            if (status != "ok")
            {
                Debug.Assert(status == "fail");

                int code = (int)response["code"];
                string message = (string)response["message"];
                throw new PandoraStatusException(code, message);
            }
        }
    }
}
