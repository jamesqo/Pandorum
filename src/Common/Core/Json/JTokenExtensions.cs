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
        public static IEnumerable<T> ToEnumerable<T>(this JToken token)
        {
            // If we pass in IEnumerable<T> as a type parameter then
            // Json.NET will create a List<T>, which is insecure since
            // it can be downcasted and modified. Instead, create an array
            // then wrap it in a ReadOnlyCollection<T>
            return token.ToObject<T[]>().AsReadOnly();
        }

        public static IEnumerable<T> ToEnumerable<T>(this JToken token, JsonSerializer jsonSerializer)
        {
            return token.ToObject<T[]>(jsonSerializer).AsReadOnly();
        }
    }
}
