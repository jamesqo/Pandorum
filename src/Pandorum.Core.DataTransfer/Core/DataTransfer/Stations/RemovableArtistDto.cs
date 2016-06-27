using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class RemovableArtistDto
    {
        public string ArtistName { get; set; }
        public string MusicToken { get; set; } // NOTE: different from ICreatableSeed.MusicToken
        public string SeedId { get; set; }
        public string ArtUrl { get; set; }
    }
}
