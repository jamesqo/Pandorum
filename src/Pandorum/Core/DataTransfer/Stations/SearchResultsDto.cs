using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    internal class SearchResultsDto
    {
        public bool NearMatchesAvailable { get; set; }
        public string Explanation { get; set; }
        public SongDto[] Songs { get; set; }
        public ArtistDto[] Artists { get; set; }
        public GenreStationDto[] GenreStations { get; set; }
    }
}
