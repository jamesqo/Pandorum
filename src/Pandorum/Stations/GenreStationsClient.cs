﻿using Pandorum.Core.Json;
using Pandorum.Core.Options.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class GenreStationsClient : IJsonProcessor
    {
        private readonly PandoraClient _inner;

        internal GenreStationsClient(PandoraClient inner)
        {
            Debug.Assert(inner != null);

            _inner = inner;
        }

        public Task<string> Checksum()
        {
            var options = CreateChecksumOptions();
            return this.AwaitAndSelectResult(
                _inner._baseClient.GetGenreStationsChecksum(options),
                (result, _) => (string)result["checksum"]);
        }

        public Task<IEnumerable<Category>> List()
        {
            throw new NotImplementedException();
        }

        private static GetGenreStationsChecksumOptions CreateChecksumOptions()
        {
            return new GetGenreStationsChecksumOptions();
        }
    }
}
