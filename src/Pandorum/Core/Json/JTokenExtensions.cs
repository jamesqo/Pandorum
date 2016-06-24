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

        public static IEnumerable<T> ToEnumerable<T>(this JToken token)
        {
            // If we pass in IEnumerable<T> as a type parameter then
            // Json.NET will create a List<T>, which is insecure since
            // it can be downcasted and modified. Instead, create an array
            // then wrap it in a ReadOnlyCollection<T>
            return token.ToObject<T[]>().AsReadOnly();
        }

        public static IEnumerable<T> ToEnumerable<T>(this JToken token, SerializationOptions options, params JsonConverter[] converters)
        {
            return token.ToObject<T[]>(options, converters).AsReadOnly();
        }

        public static T ToObject<T>(this JToken token, SerializationOptions options, params JsonConverter[] converters)
        {
            var serializer = CreateSerializer(options, converters);
            return token.ToObject<T>(serializer);
        }

        private static JsonSerializer CreateSerializer(SerializationOptions options, params JsonConverter[] converters)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters = converters;
            if ((options & SerializationOptions.CamelCaseProperties) != 0)
            {
                settings.ContractResolver = CamelCaseResolver;
            }
            return JsonSerializer.CreateDefault(settings);
        }
    }
}
