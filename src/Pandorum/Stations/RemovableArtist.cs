using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class RemovableArtist : IRemovableSeed, IArtistInfo
    {
        private readonly string _seedId;

        internal RemovableArtist(RemovableArtistDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // TODO
        }

        public string Name { get; }

        SeedType ISeed.SeedType => SeedType.Artist;
        string IRemovableSeed.SeedId => _seedId;
    }
}
