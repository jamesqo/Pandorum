using Pandorum.Core.DataTransfer.Stations;
using Pandorum.Stations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class RemovableGenre : IRemovableSeed, IGenreInfo
    {
        private readonly string _seedId;

        internal RemovableGenre(RemovableGenreDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.GenreName;
            _seedId = dto.SeedId;
        }

        public string Name { get; }

        string IRemovableSeed.SeedId => _seedId;
        SeedType ISeed.SeedType => SeedType.Genre;
    }
}
