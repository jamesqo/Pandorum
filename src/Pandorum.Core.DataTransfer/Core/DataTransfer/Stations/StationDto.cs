using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class StationDto
    {
        public string StationToken { get; set; }
        public string StationName { get; set; }
        public DateTimeOffsetDto DateCreated { get; set; }
        public bool IsShared { get; set; }
        public string StationDetailUrl { get; set; }
        public string StationSharingUrl { get; set; }
        public bool AllowRename { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowAddMusic { get; set; }
        public bool SuppressVideoAds { get; set; }
        public bool AllowEditDescription { get; set; }
        public string[] Genre { get; set; }
        public bool IsQuickMix { get; set; }
        public bool RequiresCleanAds { get; set; }
        public string[] QuickMixStationIds { get; set; }
    }
}
