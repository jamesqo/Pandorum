using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class StationSeedsDto
    {
        public RemovableSongDto[] Songs { get; set; }
        public RemovableArtistDto[] Artists { get; set; }
        public RemovableGenreDto[] Genres { get; set; }
    }
}
