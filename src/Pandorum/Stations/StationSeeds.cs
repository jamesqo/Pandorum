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

            Songs = dto.Songs.Select(s => new RemovableSong(s));
            Artists = dto.Artists.Select(a => new RemovableArtist(a));
            Genres = dto.Genres.Select(g => new RemovableGenre(g));
        }

        public IEnumerable<RemovableSong> Songs { get; }
        public IEnumerable<RemovableArtist> Artists { get; }
        public IEnumerable<RemovableGenre> Genres { get; }
    }
}
