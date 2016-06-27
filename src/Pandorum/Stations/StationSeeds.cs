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
            GenreStations = dto.Genres.Select(g => new RemovableGenreStation(g));
        }

        public IEnumerable<RemovableSong> Songs { get; }
        public IEnumerable<RemovableArtist> Artists { get; }
        public IEnumerable<RemovableGenreStation> GenreStations { get; }
    }
}
