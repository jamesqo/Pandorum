using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class ArtistDto
    {
        public string ArtistName { get; set; }
        public string MusicToken { get; set; }
        public bool LikelyMatch { get; set; }
        public int Score { get; set; }
    }
}
