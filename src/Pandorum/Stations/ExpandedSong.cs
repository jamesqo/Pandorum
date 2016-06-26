using Pandorum.Core.DataTransfer;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class ExpandedSong : IRemovableSeed, ISongInfo
    {
        private readonly string _seedId;

        internal ExpandedSong(ExpandedSongDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.SongName;
            ArtistName = dto.ArtistName;
            ArtUrl = dto.ArtUrl;
            DateCreated = dto.DateCreated.ToDateTimeOffset();

            _seedId = dto.SeedId;
        }

        public string Name { get; }
        public string ArtistName { get; }

        public string ArtUrl { get; }
        public DateTimeOffset DateCreated { get; }

        SeedType ISeed.SeedType => SeedType.Song;
        string IRemovableSeed.SeedId => _seedId;
    }
}
