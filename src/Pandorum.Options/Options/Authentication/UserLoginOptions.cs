using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Options.Authentication
{
    public class UserLoginOptions
    {
        public string LoginType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PartnerAuthToken { get; set; }

        public OptionalBool ReturnGenreStations { get; set; }
        public OptionalBool ReturnCapped { get; set; }
        public OptionalBool IncludePandoraOneInfo { get; set; }
        public OptionalBool IncludeDemographics { get; set; }
        public OptionalBool IncludeAdAttributes { get; set; }
        public OptionalBool ReturnStationList { get; set; }
        public OptionalBool IncludeStationAtUrl { get; set; }
        public OptionalBool IncludeStationSeeds { get; set; }
        public OptionalBool IncludeShuffleInsteadOfQuickMix { get; set; }
        public string StationArtSize { get; set; } // NOTE: This is optional
        public OptionalBool ReturnCollectTrackLifetimeStats { get; set; }
        public OptionalBool ReturnIsSubscriber { get; set; }
        public OptionalBool XplatformAdCapable { get; set; } // TODO: See if XPlatform can be used here
        public OptionalBool ComplimentarySponsorSupported { get; set; }
        public OptionalBool IncludeSubscriptionExpiration { get; set; }
        public OptionalBool ReturnHasUsedTrial { get; set; }
        public OptionalBool ReturnUserstate { get; set; } // TODO: ReturnUserState?
        public OptionalBool IncludeAccountMessage { get; set; }
        public OptionalBool IncludeUserWebname { get; set; }
        public OptionalBool IncludeListeningHours { get; set; }
        public OptionalBool IncludeFacebook { get; set; }
        public OptionalBool IncludeTwitter { get; set; }
        public OptionalBool IncludeDailySkipLimit { get; set; }
        public OptionalBool IncludeSkipDelay { get; set; }
        public OptionalBool IncludeGoogleplay { get; set; } // TODO: IncludeGooglePlay?
        public OptionalBool IncludeShowUserRecommendations { get; set; }
        public OptionalBool IncludeAdvertiserAttributes { get; set; }
    }
}
