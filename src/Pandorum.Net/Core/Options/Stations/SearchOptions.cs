using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Options.Stations
{
    public class SearchOptions
    {
        public string SearchText { get; set; }

        public OptionalBool IncludeNearMatches { get; set; }
        public OptionalBool IncludeGenreStations { get; set; }
    }
}
