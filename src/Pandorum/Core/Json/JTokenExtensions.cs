using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Json
{
    internal static class JTokenExtensions
    {
        private static IContractResolver s_camelCaseResolver;

        private static IContractResolver CamelCaseResolver =>
            s_camelCaseResolver ?? (s_camelCaseResolver = new CamelCasePropertyNamesContractResolver());

        public static T CamelCasedToObject<T>(this JToken token)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = CamelCaseResolver;
            var serializer = JsonSerializer.CreateDefault(settings);
            return token.ToObject<T>(serializer);
        }
    }
}
