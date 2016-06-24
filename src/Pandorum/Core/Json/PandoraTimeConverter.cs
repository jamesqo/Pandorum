using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Json
{
    public class PandoraTimeConverter : JsonConverter
    {
        private static readonly DateTimeOffset s_unixEpoch =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        // Only used for deserialization so far
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTimeOffset) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var unixTime = (long)obj["time"];
            var offset = TranslateUnixTimeMillis(unixTime);
            return offset;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        // This can be removed once we target .NET 4.6, since
        // that intros DateTimeOffset.FromUnixTimeMilliseconds
        // TODO: Move this into Pandorum.Core.Time (Common subdir?)
        private static DateTimeOffset TranslateUnixTimeMillis(long unixTime)
        {
            return s_unixEpoch.AddMilliseconds(unixTime);
        }
    }
}
