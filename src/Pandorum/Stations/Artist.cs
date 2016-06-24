using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Artist : Seed
    {
        [JsonConstructor]
        private Artist(string artistName, string musicToken, int score)
            : base(musicToken)
        {
            Debug.Assert(artistName != null);

            Score = score;
            Name = artistName;
        }

        public int Score { get; }
        public string Name { get; }

        public override string ToString() => Name;
    }
}
