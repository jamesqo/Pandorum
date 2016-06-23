using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class PandoraGenreStationsClient
    {
        private readonly PandoraClient _inner;

        internal PandoraGenreStationsClient(PandoraClient inner)
        {
            Debug.Assert(inner != null);

            _inner = inner;
        }

        public Task<string> Checksum()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PandoraCategory>> List()
        {
            throw new NotImplementedException();
        }
    }
}
