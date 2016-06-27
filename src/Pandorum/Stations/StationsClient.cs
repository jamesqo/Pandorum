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
using static Pandorum.Core.Json.JsonHelpers;

namespace Pandorum.Stations
{
    public class StationsClient
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
        
        public async Task<IRemovableSeed> AddSeed(IStation station, IAddableSeed seed)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));

            var options = CreateAddSeedOptions(station, seed);
            var response = await _inner._baseClient.AddMusic(options).ConfigureAwait(false);
            var result = GetResult(response);
            return CreateRemovableSeed(result, seed.SeedType);
        }

        public async Task<string> Checksum()
        {
            var response = await _inner._baseClient.GetStationListChecksum().ConfigureAwait(false);
            var result = GetResult(response);
            return (string)result["checksum"];
        }

        public async Task<Station> Create(IAddableSeed seed)
        {
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));

            var options = CreateCreateOptions(seed);
            var response = await _inner._baseClient.CreateStation(options).ConfigureAwait(false);
            var result = GetResult(response);
            return CreateStation(result);
        }

        public async Task Delete(IStation station)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));

            var options = CreateDeleteOptions(station);
            await _inner._baseClient.DeleteStation(options).ConfigureAwait(false);
        }

        public async Task<ExpandedStation> ExpandInfo(IStation station)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));

            var options = CreateExpandInfoOptions(station);
            var response = await _inner._baseClient.GetStation(options).ConfigureAwait(false);
            var result = GetResult(response);
            return CreateExpandedStation(result);
        }

        public async Task<IEnumerable<Station>> List()
        {
            var options = CreateStationListOptions();
            var response = await _inner._baseClient.GetStationList(options).ConfigureAwait(false);
            var result = GetResult(response);
            return CreateStations(result);
        }

        // out/ref have issues with async as well as lambdas,
        // so as a workaround we make the user allocate a
        // ChecksumReference on the heap and modify that
        // TODO: Avoid code dup with other List() overload
        // TODO: Maybe when C# 7 is released, this can return
        // Task<(IEnumerable<Station>, string)> using value tuples
        public async Task<IEnumerable<Station>> List(ChecksumReference reference)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            var options = CreateStationListOptions();
            var response = await _inner._baseClient.GetStationList(options).ConfigureAwait(false);
            var result = GetResult(response);
            return CreateStationsAndSetChecksum(result, reference);
        }

        public async Task RemoveSeed(IRemovableSeed seed)
        {
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));

            var options = CreateRemoveSeedOptions(seed);
            await _inner._baseClient.DeleteMusic(options).ConfigureAwait(false);
        }

        public async Task<Station> Rename(IStation station, string newName)
        {
            if (station == null || newName == null)
                throw new ArgumentNullException(station == null ? nameof(station) : nameof(newName));

            var options = CreateRenameOptions(station, newName);
            var response = await _inner._baseClient.RenameStation(options).ConfigureAwait(false);
            var result = GetResult(response);
            return CreateStation(result);
        }

        public async Task<SearchResults> Search(string searchText)
        {
            if (searchText == null)
                throw new ArgumentNullException(nameof(searchText));

            var options = CreateSearchOptions(searchText);
            var response = await _inner._baseClient.Search(options).ConfigureAwait(false);
            var result = GetResult(response);
            return CreateSearchResults(result);
        }

        // Options

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

            switch (seed.SeedType)
            {
                case SeedType.Artist:
                    musicType = "artist";
                    break;
                case SeedType.Song:
                case SeedType.GenreStation: // TODO: Is this correct for genreStations?
                    musicType = "song";
                    break;
                default:
                    throw new ArgumentException("The supplied seed must be an artist, song, or genre station.", nameof(seed));
            }

            return new CreateStationOptions { MusicType = musicType, MusicToken = seed.MusicToken };
        }

        private static AddMusicOptions CreateAddSeedOptions(IStation station, IAddableSeed seed)
        {
            return new AddMusicOptions { StationToken = station.Token, MusicToken = seed.MusicToken };
        }

        private static DeleteMusicOptions CreateRemoveSeedOptions(IRemovableSeed seed)
        {
            return new DeleteMusicOptions { SeedId = seed.SeedId };
        }

        // Result processing

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

        private static IRemovableSeed CreateRemovableSeed(JToken result, SeedType type)
        {
            var settings = new JsonSerializerSettings().WithCamelCase();
            var serializer = settings.ToSerializer();

            switch (type)
            {
                case SeedType.Artist:
                    var artistDto = result.ToObject<ExpandedArtistDto>(serializer);
                    return new ExpandedArtist(artistDto);
                case SeedType.GenreStation:
                    // TODO
                    return default(IRemovableSeed);
                case SeedType.Song:
                    var songDto = result.ToObject<ExpandedSongDto>(serializer);
                    return new ExpandedSong(songDto);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
