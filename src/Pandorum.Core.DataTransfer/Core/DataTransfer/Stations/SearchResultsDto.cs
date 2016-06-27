using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class SearchResultsDto
    {
        public bool NearMatchesAvailable { get; set; }
        public string Explanation { get; set; }
        public SongDto[] Songs { get; set; }
        public ArtistDto[] Artists { get; set; }
        public GenreDto[] GenreStations { get; set; }
    }
}
