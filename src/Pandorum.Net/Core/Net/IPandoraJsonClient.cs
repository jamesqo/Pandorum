using Newtonsoft.Json.Linq;
using Pandorum.Core.Options;
using Pandorum.Core.Options.Authentication;
using Pandorum.Core.Options.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    public interface IPandoraJsonClient : IDisposable
    {
        Task<JObject> CheckLicensing();
        Task<JObject> PartnerLogin(PartnerLoginOptions options);
        Task<JObject> UserLogin(UserLoginOptions options);

        Task<JObject> GetStationList(GetStationListOptions options);
        Task<JObject> GetStationListChecksum();
        Task<JObject> Search(SearchOptions options);
        Task<JObject> CreateStation(CreateStationOptions options);
        Task<JObject> AddMusic(AddMusicOptions options);
        Task<JObject> DeleteMusic(DeleteMusicOptions options);
        Task<JObject> RenameStation(RenameStationOptions options);
        Task<JObject> DeleteStation(DeleteStationOptions options);
        Task<JObject> GetStation(GetStationOptions options);
        Task<JObject> DeleteFeedback(DeleteFeedbackOptions options);
        Task<JObject> GetGenreStations();
        Task<JObject> GetGenreStationsChecksum(GetGenreStationsChecksumOptions options);
        Task<JObject> ShareStation(ShareStationOptions options);
        Task<JObject> TransformSharedStation(TransformSharedStationOptions options);
        Task<JObject> SetQuickMix(SetQuickMixOptions options);
    }
}
