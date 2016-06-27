using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Json
{
    internal static class JsonHelpers
    {
        public static JToken GetResult(JObject response)
        {
            // Separated into a new method, to
            // reduce the change of typos
            return response["result"];
        }
    }
}
