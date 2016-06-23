using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class PandoraStationsClient : IAsyncClient
    {
        private readonly PandoraClient _inner;

        private PandoraGenreStationsClient _genre;

        internal PandoraStationsClient(PandoraClient inner)
        {
            Debug.Assert(inner != null);

            _inner = inner;
        }

        public PandoraGenreStationsClient Genre =>
            _genre ?? (_genre = new PandoraGenreStationsClient(_inner));

        public Task<string> Checksum()
        {
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetStationListChecksum(),
                (result, _) => (string)result["checksum"]);
        }

        public Task<IEnumerable<PandoraStation>> List()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PandoraStation>> List(out string checksum)
        {
            throw new NotImplementedException();
        }

        public Task<PandoraSearchResults> Search(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
