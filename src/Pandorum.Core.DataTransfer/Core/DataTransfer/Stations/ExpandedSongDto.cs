using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class ExpandedSongDto
    {
        public string SeedId { get; set; }
        public string ArtistName { get; set; }
        public DateTimeOffsetDto DateCreated { get; set; }
        public string ArtUrl { get; set; }
        public string SongName { get; set; }
        public string MusicToken { get; set; } // different from the MusicToken in SongDto
    }
}
