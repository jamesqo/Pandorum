using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pandorum.Core.Net.Http
{
    public static class HttpClientExtensions
    {
        public static Task<JObject> GetJsonAsync(this HttpClient client, string requestUri)
        {
            return AwaitAndReadJson(client.GetStringAsync(requestUri));
        }

        public static Task<JObject> PostAndReadJsonAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            return AwaitAndReadJson(client.PostAndReadStringAsync(requestUri, content));
        }

        public static Task<string> PostAndReadStringAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            return AwaitAndReadAs(client.PostAsync(requestUri, content), c => c.ReadAsStringAsync());
        }

        private async static Task<T> AwaitAndReadAs<T>(Task<HttpResponseMessage> task, Func<HttpContent, Task<T>> func)
        {
            var response = await task.ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await func(response.Content).ConfigureAwait(false);
        }

        private async static Task<JObject> AwaitAndReadJson(Task<string> task)
        {
            return JObject.Parse(await task.ConfigureAwait(false));
        }
    }
}
