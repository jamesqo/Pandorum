using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Song : IAddableSeed, ISongInfo
    {
        public struct SearchInfo
        {
            internal SearchInfo(int score)
            {
                Score = score;
            }

            public int Score { get; }
        }

        private readonly string _musicToken;

        internal Song(SongDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.SongName;
            ArtistName = dto.ArtistName;
            Search = new SearchInfo(dto.Score);
            _musicToken = dto.MusicToken;
        }
        
        public string Name { get; }
        public string ArtistName { get; }
        public SearchInfo Search { get; }

        SeedType ISeed.SeedType => SeedType.Song;
        string ICreatableSeed.MusicToken => _musicToken;

        public override string ToString() => Name;
    }
}
