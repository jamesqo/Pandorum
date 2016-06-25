using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Song : Seed
    {
        internal Song(SongDto dto) : base(dto?.MusicToken)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Score = dto.Score;
            Name = dto.SongName;
            ArtistName = dto.ArtistName;
        }
        
        public string Name { get; }
        public string ArtistName { get; }
        internal int Score { get; }

        public override string ToString() => Name;
    }
}
