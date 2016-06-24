using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Song : Seed
    {
        [JsonConstructor]
        private Song(string artistName, string musicToken, string songName, int score)
            : base(musicToken)
        {
            Debug.Assert(artistName != null && songName != null);

            Score = score;
            Name = songName;
            ArtistName = artistName;
        }

        public int Score { get; }
        public string Name { get; }
        public string ArtistName { get; }

        public override string ToString() => Name;
    }
}
