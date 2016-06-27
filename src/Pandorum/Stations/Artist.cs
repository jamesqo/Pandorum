using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Artist : IAddableSeed, IArtistInfo
    {
        private readonly string _musicToken;

        internal Artist(ArtistDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Score = dto.Score;
            Name = dto.ArtistName;
            IsLikelyMatch = dto.LikelyMatch;
            _musicToken = dto.MusicToken;
        }

        public string Name { get; }
        internal int Score { get; }
        internal bool IsLikelyMatch { get; }

        // musicToken starts with C for composers,
        // R for artists
        public bool IsComposer => _musicToken[0] == 'C';

        SeedType ISeed.SeedType => SeedType.Artist;
        string ICreatableSeed.MusicToken => _musicToken;

        public override string ToString() => Name;
    }
}
