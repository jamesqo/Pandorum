using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class GenreStationsClient
    {
        private readonly PandoraClient _inner;

        internal GenreStationsClient(PandoraClient inner)
        {
            Debug.Assert(inner != null);

            _inner = inner;
        }

        public Task<string> Checksum()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> List()
        {
            throw new NotImplementedException();
        }
    }
}
