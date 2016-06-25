using Newtonsoft.Json;
using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Pandorum.Core.DataTransfer.Stations;

namespace Pandorum.Stations
{
    public class SearchResults : IEnumerable<SearchResult>
    {
        internal SearchResults(SearchResultsDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Explanation = dto.Explanation;
            Songs = dto.Songs.Select(s => new Song(s));
        }

        public string Explanation { get; }

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
