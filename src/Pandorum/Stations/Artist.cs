using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Artist : Seed
    {
        internal Artist(ArtistDto dto) : base(dto?.MusicToken)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Score = dto.Score;
            Name = dto.ArtistName;
            IsLikelyMatch = dto.LikelyMatch;
        }

        public string Name { get; }
        internal int Score { get; }
        internal bool IsLikelyMatch { get; }

        public override string ToString() => Name;
    }
}
