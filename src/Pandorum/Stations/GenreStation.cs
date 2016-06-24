using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class GenreStation : Seed
    {
        [JsonConstructor]
        private GenreStation(string musicToken, int score, string stationName)
            : base(musicToken)
        {
            Debug.Assert(stationName != null);

            Score = score;
            Name = stationName;
        }

        [JsonConstructor]
        private GenreStation(string stationToken, string stationName)
            : base(stationToken) // in this case stationToken is actually a musicToken
        {
            Debug.Assert(stationName != null);

            Name = stationName;
        }

        public int? Score { get; }
        public string Name { get; }

        public override string ToString() => Name;
    }
}
