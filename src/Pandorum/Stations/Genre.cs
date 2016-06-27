using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Genre : ICreatableSeed, IGenreInfo
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

        internal Genre(GenreDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.StationName;
            Search = new SearchInfo(dto.Score);
            _musicToken = dto.MusicToken;
        }

        internal Genre(GenreDto2 dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            Name = dto.StationName;
            Debug.Assert(dto.StationId == dto.StationToken); // documented to be the same so far
            _musicToken = dto.StationToken; // stationToken is actually a musicToken here
        }

        public string Name { get; }
        public SearchInfo Search { get; } // TODO: Would be nice to throw an exception if
        // this was initialized by GenreDto2, where this isn't set

        SeedType ISeed.SeedType => SeedType.Genre;
        string ICreatableSeed.MusicToken => _musicToken;

        public override string ToString() => Name;
    }
}
