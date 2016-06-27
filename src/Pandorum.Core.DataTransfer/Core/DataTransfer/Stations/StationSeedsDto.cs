using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    // TODO: ExpandedGenreStationDto, if that exists

    public class StationSeedsDto
    {
        public RemovableSongDto[] Songs { get; set; }
        public RemovableArtistDto[] Artists { get; set; }
        public RemovableGenreStationDto[] Genres { get; set; }
    }
}
