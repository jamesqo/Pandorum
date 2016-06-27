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
        private readonly string _musicToken;

        internal Song(SongDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Score = dto.Score;
            Name = dto.SongName;
            ArtistName = dto.ArtistName;
            _musicToken = dto.MusicToken;
        }
        
        public string Name { get; }
        public string ArtistName { get; }
        internal int Score { get; }

        SeedType ISeed.SeedType => SeedType.Song;
        string ICreatableSeed.MusicToken => _musicToken;

        public override string ToString() => Name;
    }
}
