using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class SearchResults
    {
        [JsonConstructor] // used by Json.NET
        private SearchResults(
            IEnumerable<Song> songs,
            IEnumerable<Artist> artists,
            IEnumerable<GenreStation> genreStations)
        {
            Debug.Assert(songs != null && artists != null && genreStations != null);

            Songs = songs;
            Artists = artists;
            GenreStations = genreStations;
        }

        public IEnumerable<Song> Songs { get; }
        public IEnumerable<Artist> Artists { get; }
        public IEnumerable<GenreStation> GenreStations { get; }
    }
}
