using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Options.Stations
{
    public class GetStationListOptions
    {
        public OptionalBool IncludeStationAtUrl { get; set; }
        public string StationArtSize { get; set; } // Optional?
        public OptionalBool IncludeAdAttributes { get; set; }
        public OptionalBool IncludeStationSeeds { get; set; }
        public OptionalBool IncludeShuffleInsteadOfQuickMix { get; set; }
        public OptionalBool IncludeRecommendations { get; set; }
        public OptionalBool IncludeExplanations { get; set; }
    }
}
