using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Pandorum.Core;
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
            Debug.Assert(inner != null);

            _inner = inner;
        }

        public GenreStationsClient Genre =>
            _genre ?? (_genre = new GenreStationsClient(_inner));

        public Task<string> Checksum()
        {
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetStationListChecksum(),
                (result, _) => (string)result["checksum"]);
        }

        public Task<IEnumerable<Station>> List()
        {
            var options = CreateStationListOptions();
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetStationList(options),
                (result, _) => CreateList(result));
        }

        // out/ref have issues with async as well as lambdas,
        // so commenting this out for now
        // TODO: Find a way to expose this
        // public Task<IEnumerable<Station>> List(out string checksum)

        public Task<SearchResults> Search(string searchText)
        {
            return this.AwaitAndSelectResult(
                _inner._baseClient.Search(CreateSearchOptions(searchText)),
                (result, _) => CreateSearchResults(result));
        }

        private static SearchOptions CreateSearchOptions(string searchText)
        {
            var options = new SearchOptions();
            options.SearchText = searchText;
            // TODO: Other parameters
            return options;
        }

        private static GetStationListOptions CreateStationListOptions()
        {
            return new GetStationListOptions();
        }

        private static IEnumerable<Station> CreateList(JToken result)
        {
            var options = SerializationOptions.CamelCaseProperties;
            return result["stations"].ToEnumerable<Station>(options);
        }

        private static SearchResults CreateSearchResults(JToken result)
        {
            var options = SerializationOptions.CamelCaseProperties;
            return result.ToObject<SearchResults>(options);
        }
    }
}
