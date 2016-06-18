using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net.Options
{
    public class UserLoginOptions
    {
        private OptionalBool _returnGenreStations;
        private OptionalBool _returnCapped;
        private OptionalBool _includePandoraOneInfo;
        private OptionalBool _includeDemographics;
        private OptionalBool _includeAdAttributes;
        private OptionalBool _returnStationList;
        private OptionalBool _includeStationAtUrl;
        private OptionalBool _includeStationSeeds;
        private OptionalBool _includeShuffleInsteadOfQuickMix;
        private OptionalBool _returnCollectTrackLifetimeStats;
        private OptionalBool _returnIsSubscriber;
        private OptionalBool _xplatformAdCapable;
        private OptionalBool _complimentarySponsorSupported;
        private OptionalBool _includeSubscriptionExpiration;
        private OptionalBool _returnHasUsedTrial;
        private OptionalBool _returnUserstate;
        private OptionalBool _includeAccountMessage;
        private OptionalBool _includeUserWebname;
        private OptionalBool _includeListeningHours;
        private OptionalBool _includeFacebook;
        private OptionalBool _includeTwitter;
        private OptionalBool _includeDailySkipLimit;
        private OptionalBool _includeSkipDelay;
        private OptionalBool _includeGoogleplay;
        private OptionalBool _includeShowUserRecommendations;
        private OptionalBool _includeAdvertiserAttributes;

        public string LoginType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PartnerAuthToken { get; set; }

        public bool? ReturnGenreStations
        {
            get { return _returnGenreStations; }
            set { _returnGenreStations = value; }
        }
        public bool? ReturnCapped
        {
            get { return _returnCapped; }
            set { _returnCapped = value; }
        }
        public bool? IncludePandoraOneInfo
        {
            get { return _includePandoraOneInfo; }
            set { _includePandoraOneInfo = value; }
        }
        public bool? IncludeDemographics
        {
            get { return _includeDemographics; }
            set { _includeDemographics = value; }
        }
        public bool? IncludeAdAttributes
        {
            get { return _includeAdAttributes; }
            set { _includeAdAttributes = value; }
        }
        public bool? ReturnStationList
        {
            get { return _returnStationList; }
            set { _returnStationList = value; }
        }
        public bool? IncludeStationAtUrl
        {
            get { return _includeStationAtUrl; }
            set { _includeStationAtUrl = value; }
        }
        public bool? IncludeStationSeeds
        {
            get { return _includeStationSeeds; }
            set { _includeStationSeeds = value; }
        }
        public bool? IncludeShuffleInsteadOfQuickMix
        {
            get { return _includeShuffleInsteadOfQuickMix; }
            set { _includeShuffleInsteadOfQuickMix = value; }
        }
        public string StationArtSize { get; set; } // This is optional
        public bool? ReturnCollectTrackLifetimeStats
        {
            get { return _returnCollectTrackLifetimeStats; }
            set { _returnCollectTrackLifetimeStats = value; }
        }
        public bool? ReturnIsSubscriber
        {
            get { return _returnIsSubscriber; }
            set { _returnIsSubscriber = value; }
        }
        public bool? XplatformAdCapable // TODO: See if XPlatform can be used here
        {
            get { return _xplatformAdCapable; }
            set { _xplatformAdCapable = value; }
        }
        public bool? ComplimentarySponsorSupported
        {
            get { return _complimentarySponsorSupported; }
            set { _complimentarySponsorSupported = value; }
        }
        public bool? IncludeSubscriptionExpiration
        {
            get { return _includeSubscriptionExpiration; }
            set { _includeSubscriptionExpiration = value; }
        }
        public bool? ReturnHasUsedTrial
        {
            get { return _returnHasUsedTrial; }
            set { _returnHasUsedTrial = value; }
        }
        public bool? ReturnUserstate // TODO: ReturnUserState?
        {
            get { return _returnUserstate; }
            set { _returnUserstate = value; }
        }
        public bool? IncludeAccountMessage
        {
            get { return _includeAccountMessage; }
            set { _includeAccountMessage = value; }
        }
        public bool? IncludeUserWebname
        {
            get { return _includeUserWebname; }
            set { _includeUserWebname = value; }
        }
        public bool? IncludeListeningHours
        {
            get { return _includeListeningHours; }
            set { _includeListeningHours = value; }
        }
        public bool? IncludeFacebook
        {
            get { return _includeFacebook; }
            set { _includeFacebook = value; }
        }
        public bool? IncludeTwitter
        {
            get { return _includeTwitter; }
            set { _includeTwitter = value; }
        }
        public bool? IncludeDailySkipLimit
        {
            get { return _includeDailySkipLimit; }
            set { _includeDailySkipLimit = value; }
        }
        public bool? IncludeSkipDelay
        {
            get { return _includeSkipDelay; }
            set { _includeSkipDelay = value; }
        }
        public bool? IncludeGoogleplay // TODO: IncludeGooglePlay?
        {
            get { return _includeGoogleplay; }
            set { _includeGoogleplay = value; }
        }
        public bool? IncludeShowUserRecommendations
        {
            get { return _includeShowUserRecommendations; }
            set { _includeShowUserRecommendations = value; }
        }
        public bool? IncludeAdvertiserAttributes
        {
            get { return _includeAdvertiserAttributes; }
            set { _includeAdvertiserAttributes = value; }
        }
    }
}
