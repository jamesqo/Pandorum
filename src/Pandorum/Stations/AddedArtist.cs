using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class AddedArtist : IRemovableSeed, IArtistInfo
    {
        private readonly string _seedId;

        internal AddedArtist(AddedArtistDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // TODO
        }

        public string Name { get; }

        string IRemovableSeed.SeedId => _seedId;
        SeedType ISeed.SeedType => SeedType.Artist;
    }
}
