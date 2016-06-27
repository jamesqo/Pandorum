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
        public struct SearchInfo
        {
            internal SearchInfo(int score, bool likelyMatch)
            {
                Score = score;
                IsLikelyMatch = likelyMatch;
            }

            public int Score { get; }
            public bool IsLikelyMatch { get; }
        }

        private readonly string _musicToken;

        internal Artist(ArtistDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.ArtistName;
            Search = new SearchInfo(dto.Score, dto.LikelyMatch);
            _musicToken = dto.MusicToken;
        }

        public string Name { get; }
        public SearchInfo Search { get; }

        // musicToken starts with C for composers,
        // R for artists
        public bool IsComposer => _musicToken[0] == 'C';

        SeedType ISeed.SeedType => SeedType.Artist;
        string ICreatableSeed.MusicToken => _musicToken;

        public override string ToString() => Name;
    }
}
