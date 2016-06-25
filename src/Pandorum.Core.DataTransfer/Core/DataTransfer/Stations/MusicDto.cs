using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    // TODO: ExtendedGenreStationDto, if that exists

    public class MusicDto
    {
        public ExtendedSongDto[] Songs { get; set; }
        public ExtendedArtistDto[] Artists { get; set; }
    }
}
