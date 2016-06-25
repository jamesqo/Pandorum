using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pandorum.Core.DataTransfer.Stations;
using Pandorum.Core;

namespace Pandorum.Stations
{
    public class StationSeeds
    {
        internal StationSeeds(StationSeedsDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Songs = dto.Songs.Select(s => new ExpandedSong(s));
            Artists = dto.Artists.Select(a => new ExpandedArtist(a));
        }

        public IEnumerable<ExpandedSong> Songs { get; }
        public IEnumerable<ExpandedArtist> Artists { get; }
    }
}
