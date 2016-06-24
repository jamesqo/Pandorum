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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Station>> List(out string checksum)
        {
            throw new NotImplementedException();
        }

        public Task<SearchResults> Search(string searchText)
        {
            return this.AwaitAndSelectResult(
                _inner._baseClient.Search(CreateSearchOptions(searchText)),
                (result, _) => result.CamelCaseToObject<SearchResults>());
        }

        private static SearchOptions CreateSearchOptions(string searchText)
        {
            var options = new SearchOptions();
            options.SearchText = searchText;
            // TODO: Other parameters
            return options;
        }
    }
}
