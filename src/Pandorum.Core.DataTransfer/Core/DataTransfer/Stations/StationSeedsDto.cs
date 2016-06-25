using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    // TODO: ExpandedGenreStationDto, if that exists

    public class StationSeedsDto
    {
        public ExpandedSongDto[] Songs { get; set; }
        public ExpandedArtistDto[] Artists { get; set; }
    }
}
