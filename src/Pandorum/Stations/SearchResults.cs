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
            Artists = dto.Artists.Select(a => new Artist(a));
            Genres = dto.GenreStations?.Select(g => new Genre(g)) ?? ImmutableCache.EmptyArray<Genre>();
        }

        public string Explanation { get; }

        public IEnumerable<Song> Songs { get; }
        public IEnumerable<Artist> Artists { get; }
        public IEnumerable<Genre> Genres { get; }

        public IEnumerator<SearchResult> GetEnumerator()
        {
            foreach (var song in Songs)
            {
                yield return new SearchResult(song.Name, song, song.Search.Score);
            }

            foreach (var artist in Artists)
            {
                yield return new SearchResult(artist.Name, artist, artist.Search.Score);
            }

            foreach (var genre in Genres)
            {
                yield return new SearchResult(genre.Name, genre, genre.Search.Score);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
