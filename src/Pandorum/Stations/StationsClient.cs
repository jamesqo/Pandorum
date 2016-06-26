using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Pandorum.Core;
using Pandorum.Core.DataTransfer.Stations;
using Pandorum.Core.Json;
using Pandorum.Core.Options.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class StationsClient : IJsonProcessor
    {
        private readonly PandoraClient _inner;

        private GenreStationsClient _genre;

        internal StationsClient(PandoraClient inner)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));

            _inner = inner;
        }

        public GenreStationsClient Genre =>
            _genre ?? (_genre = new GenreStationsClient(_inner));
        
        public Task<IRemovableSeed> AddSeed(IStation station, IAddableSeed seed)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));

            var options = CreateAddSeedOptions(station, seed);
            return
        }

        public Task<string> Checksum()
        {
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetStationListChecksum(),
                (result, _) => (string)result["checksum"]);
        }

        public Task<Station> Create(IAddableSeed seed)
        {
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));

            var options = CreateCreateOptions(seed);
            return this.AwaitAndSelectResult(
                _inner._baseClient.CreateStation(options),
                (result, _) => CreateStation(result));
        }

        public async Task Delete(IStation station)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));

            var options = CreateDeleteOptions(station);
            await _inner._baseClient.DeleteStation(options).ConfigureAwait(false);
        }

        public Task<ExpandedStation> ExpandInfo(IStation station)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));

            var options = CreateExpandInfoOptions(station);
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetStation(options),
                (result, _) => CreateExpandedStation(result));
        }

        public Task<IEnumerable<Station>> List()
        {
            var options = CreateStationListOptions();
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetStationList(options),
                (result, _) => CreateStations(result));
        }

        // out/ref have issues with async as well as lambdas,
        // so as a workaround we make the user allocate a
        // ChecksumReference on the heap and modify that
        // TODO: Remove closure allocations from here
        // TODO: Avoid code dup with other List() overload
        // TODO: Maybe when C# 7 is released, this can return
        // Task<(IEnumerable<Station>, string)> using value tuples
        public Task<IEnumerable<Station>> List(ChecksumReference reference)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            var options = CreateStationListOptions();
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetStationList(options),
                (result, _) => CreateStationsAndSetChecksum(result, reference));
        }

        public Task<Station> Rename(IStation station, string newName)
        {
            if (station == null || newName == null)
                throw new ArgumentNullException(station == null ? nameof(station) : nameof(newName));

            var options = CreateRenameOptions(station, newName);
            return this.AwaitAndSelectResult(
                _inner._baseClient.RenameStation(options),
                (result, _) => CreateStation(result));
        }

        public Task<SearchResults> Search(string searchText)
        {
            if (searchText == null)
                throw new ArgumentNullException(nameof(searchText));

            return this.AwaitAndSelectResult(
                _inner._baseClient.Search(CreateSearchOptions(searchText)),
                (result, _) => CreateSearchResults(result));
        }

        private static SearchOptions CreateSearchOptions(string searchText)
        {
            return new SearchOptions
            {
                SearchText = searchText,
                IncludeGenreStations = true,
                IncludeNearMatches = true,
            };
        }

        private static GetStationListOptions CreateStationListOptions()
        {
            return new GetStationListOptions();
        }

        private static RenameStationOptions CreateRenameOptions(IStation station, string newName)
        {
            return new RenameStationOptions
            {
                StationToken = station.Token,
                StationName = newName
            };
        }

        private static DeleteStationOptions CreateDeleteOptions(IStation station)
        {
            return new DeleteStationOptions { StationToken = station.Token };
        }

        private static GetStationOptions CreateExpandInfoOptions(IStation station)
        {
            return new GetStationOptions
            {
                StationToken = station.Token,
                IncludeExtendedAttributes = true
            };
        }

        private static CreateStationOptions CreateCreateOptions(IAddableSeed seed)
        {
            string musicType;

            if (seed is Artist)
                musicType = "artist";
            else if (seed is Song || seed is GenreStation)
                musicType = "song"; // TODO: Is this correct for genreStations?
            else
                throw new ArgumentException("The supplied seed must be an artist, song, or genre station.", nameof(seed));

            return new CreateStationOptions { MusicType = musicType, MusicToken = seed.MusicToken };
        }

        private static IEnumerable<Station> CreateStations(JToken result)
        {
            var settings = new JsonSerializerSettings().WithCamelCase();
            var serializer = settings.ToSerializer();
            var dtos = result["stations"].ToEnumerable<StationDto>(serializer);
            return dtos.Select(s => new Station(s));
        }

        private static IEnumerable<Station> CreateStationsAndSetChecksum(JToken result, ChecksumReference reference)
        {
            var stations = CreateStations(result);
            reference.Checksum = (string)result["checksum"];
            return stations;
        }

        private static SearchResults CreateSearchResults(JToken result)
        {
            var settings = new JsonSerializerSettings().WithCamelCase();
            var serializer = settings.ToSerializer();
            var dto = result.ToObject<SearchResultsDto>(serializer);
            return new SearchResults(dto);
        }

        private static Station CreateStation(JToken result)
        {
            var settings = new JsonSerializerSettings().WithCamelCase();
            var serializer = settings.ToSerializer();
            var dto = result.ToObject<StationDto>(serializer);
            return new Station(dto);
        }

        private static ExpandedStation CreateExpandedStation(JToken result)
        {
            var settings = new JsonSerializerSettings().WithCamelCase();
            var serializer = settings.ToSerializer();
            var dto = result.ToObject<ExpandedStationDto>(serializer);
            return new ExpandedStation(dto);
        }
    }
}
