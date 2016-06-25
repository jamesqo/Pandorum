using Pandorum.Core.DataTransfer;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class ExpandedSong : ISong, IExpandedSeed
    {
        internal ExpandedSong(ExpandedSongDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.SongName;
            ArtistName = dto.ArtistName;
            ArtUrl = dto.ArtUrl;
            DateCreated = dto.DateCreated.ToDateTimeOffset();
            Remover = new SeedRemover(dto.SeedId);
        }

        public string Name { get; }
        public string ArtistName { get; }

        public string ArtUrl { get; }
        public DateTimeOffset DateCreated { get; }
        public ISeedRemover Remover { get; }
    }
}
