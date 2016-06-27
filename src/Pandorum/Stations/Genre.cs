﻿using Newtonsoft.Json;
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
        private readonly string _musicToken;

        internal Genre(GenreDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Score = dto.Score;
            Name = dto.StationName;
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
        internal int Score { get; }

        SeedType ISeed.SeedType => SeedType.Genre;
        string ICreatableSeed.MusicToken => _musicToken;

        public override string ToString() => Name;
    }
}