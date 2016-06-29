using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pandorum.Core.DataTransfer.Stations;
using Pandorum.Core.Json;
using Pandorum.Core.Options.Stations;
using Pandorum.Stations.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Pandorum.Core.Json.JsonHelpers;

namespace Pandorum.Stations
{
    public class GenresClient : IClientWrapper
    {
        private readonly PandoraClient _inner;

        internal GenresClient(PandoraClient inner)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));

            _inner = inner;
        }

        PandoraClient IClientWrapper.InnerClient => _inner;

        public async Task<string> Checksum()
        {
            var options = CreateChecksumOptions();
            var response = await this.JsonClient().GetGenreStationsChecksum(options).ConfigureAwait(false);
            var result = GetResult(response);
            return (string)result["checksum"];
        }

        public async Task<IEnumerable<Category>> List()
        {
            var response = await this.JsonClient().GetGenreStations().ConfigureAwait(false);
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
