using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Json
{
    internal static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings AddConverter(this JsonSerializerSettings settings, JsonConverter converter)
        {
            settings.Converters.Add(converter);
            return settings;
        }

        public static JsonSerializer ToSerializer(this JsonSerializerSettings settings)
        {
            return JsonSerializer.CreateDefault(settings);
        }

        public static JsonSerializerSettings WithCamelCase(this JsonSerializerSettings settings)
        {
            return settings.WithContractResolver(new CamelCasePropertyNamesContractResolver());
        }

        public static JsonSerializerSettings WithContractResolver(this JsonSerializerSettings settings, IContractResolver resolver)
        {
            settings.ContractResolver = resolver;
            return settings;
        }

        public static JsonSerializerSettings WithDefaultValueHandling(this JsonSerializerSettings settings, DefaultValueHandling handling)
        {
            settings.DefaultValueHandling = handling;
            return settings;
        }
    }
}
