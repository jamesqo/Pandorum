using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    internal class SongDto
    {
        public string ArtistName { get; set; }
        public string MusicToken { get; set; }
        public string SongName { get; set; }
        public int Score { get; set; }
    }
}
