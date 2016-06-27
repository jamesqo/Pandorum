﻿using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    // TODO: Maybe this should be named to just 'Genre',
    // to avoid confusion with actual stations.
    // Note that things like client.Stations.Genre
    // might have to be changed too.

    public class GenreStation : ICreatableSeed, IGenreStationInfo
    {
        private readonly string _musicToken;

        internal GenreStation(GenreStationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Score = dto.Score;
            Name = dto.StationName;
            _musicToken = dto.MusicToken;
        }

        internal GenreStation(GenreStationDto2 dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            Name = dto.StationName;
            Debug.Assert(dto.StationId == dto.StationToken); // documented to be the same so far
            _musicToken = dto.StationToken; // stationToken is actually a musicToken here
        }

        public string Name { get; }
        internal int Score { get; }

        SeedType ISeed.SeedType => SeedType.GenreStation;
        string ICreatableSeed.MusicToken => _musicToken;

        public override string ToString() => Name;
    }
}
