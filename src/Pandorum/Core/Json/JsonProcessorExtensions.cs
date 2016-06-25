using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Json
{
    internal static class JsonProcessorExtensions
    {
        // TODO: Better name? These methods don't exactly scream JSON

        public async static Task AwaitAndSelectResult<T>(this T processor, Task<JObject> task, Action<JToken, T> action)
            where T : IJsonProcessor
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            var response = await task.ConfigureAwait(false);
            action(response["result"], processor);
        }

        public async static Task<TOutput> AwaitAndSelectResult<T, TOutput>(this T processor, Task<JObject> task, Func<JToken, T, TOutput> func)
            where T : IJsonProcessor
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            var response = await task.ConfigureAwait(false);
            return func(response["result"], processor);
        }
    }
}
