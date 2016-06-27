using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class GenreStationsClient
    {
        private readonly PandoraClient _inner;

        internal GenreStationsClient(PandoraClient inner)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));

            _inner = inner;
        }

        public async Task<string> Checksum()
        {
            var options = CreateChecksumOptions();
            var response = await _inner._baseClient.GetGenreStationsChecksum(options).ConfigureAwait(false);
            var result = GetResult(response);
            return (string)result["checksum"];
        }

        public async Task<IEnumerable<Category>> List()
        {
            var response = await _inner._baseClient.GetGenreStations().ConfigureAwait(false);
            var result = GetResult(response);
            return CreateCategories(result);
        }

        private static GetGenreStationsChecksumOptions CreateChecksumOptions()
        {
            return new GetGenreStationsChecksumOptions();
        }

        private static IEnumerable<Category> CreateCategories(JToken result)
        {
            var settings = new JsonSerializerSettings().WithCamelCase();
            var serializer = settings.ToSerializer();
            var dtos = result["categories"].ToEnumerable<CategoryDto>(serializer);
            return dtos.Select(c => new Category(c));
        }
    }
}
