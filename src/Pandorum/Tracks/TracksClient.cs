using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Tracks
{
    public class TracksClient : IPandoraClient
    {
        private readonly PandoraClient _inner;

        internal TracksClient(PandoraClient inner)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));

            _inner = inner;
        }

        PandoraClient IPandoraClient.Inner => _inner;
    }
}
