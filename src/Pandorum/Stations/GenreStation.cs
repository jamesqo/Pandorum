using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class GenreStation : Seed
    {
        internal GenreStation(GenreStationDto dto) : base(dto?.MusicToken)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Score = dto.Score;
            Name = dto.StationName;
        }

        public string Name { get; }
        internal int Score { get; }

        public override string ToString() => Name;
    }
}
