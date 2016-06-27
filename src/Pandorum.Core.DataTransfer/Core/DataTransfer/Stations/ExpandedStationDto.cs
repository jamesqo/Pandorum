using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    // MAINTAINABILITY: Although ExpandedStationDto
    // shares a lot of properties with StationDto,
    // we don't use inheritance since in the future
    // responses from getStationList may include things
    // not in getStation.

    public class ExpandedStationDto
    {
        public bool SuppressVideoAds { get; set; }
        public string StationId { get; set; }
        public bool AllowAddMusic { get; set; }
        public DateTimeOffsetDto DateCreated { get; set; }
        public string StationDetailUrl { get; set; }
        public string ArtUrl { get; set; }
        public bool RequiresCleanAds { get; set; }
        public string StationToken { get; set; }
        public string StationName { get; set; }
        public StationSeedsDto Music { get; set; }
        public bool IsShared { get; set; }
        public bool AllowDelete { get; set; }
        public string[] Genre { get; set; }
        public bool IsQuickMix { get; set; }
        public bool AllowRename { get; set; }
        public string StationSharingUrl { get; set; }
        public FeedbackDto Feedback { get; set; }
        public string[] QuickMixStationIds { get; set; }
        public bool AllowEditDescription { get; set; }
    }
}
