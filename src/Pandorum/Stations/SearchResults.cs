using Newtonsoft.Json;
using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace Pandorum.Stations
{
    public class SearchResults : IEnumerable<SearchResult>
    {
        [JsonConstructor] // used by Json.NET
        private SearchResults(Song[] songs, Artist[] artists, GenreStation[] genreStations)
        {
            Debug.Assert(songs != null && artists != null);

            Songs = songs.AsReadOnly();
            Artists = artists.AsReadOnly();
            GenreStations = genreStations?.AsReadOnly();
        }

        public IEnumerable<Song> Songs { get; }
        public IEnumerable<Artist> Artists { get; }
        public IEnumerable<GenreStation> GenreStations { get; }

        public IEnumerator<SearchResult> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
