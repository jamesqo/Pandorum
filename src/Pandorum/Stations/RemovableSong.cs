using Pandorum.Core.DataTransfer;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class RemovableSong : IRemovableSeed, ISongInfo
    {
        private readonly string _seedId;

        internal RemovableSong(RemovableSongDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.SongName;
            ArtistName = dto.ArtistName;
            ArtUrl = dto.ArtUrl;

            _seedId = dto.SeedId;
        }

        public string Name { get; }
        public string ArtistName { get; }

        public string ArtUrl { get; }

        SeedType ISeed.SeedType => SeedType.Song;
        string IRemovableSeed.SeedId => _seedId;
    }
}
