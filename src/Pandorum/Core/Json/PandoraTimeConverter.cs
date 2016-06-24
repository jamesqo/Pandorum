using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pandorum.Core.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Json
{
    public class PandoraTimeConverter : JsonConverter
    {
        // Only used for deserialization so far
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTimeOffset) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var milliseconds = (long)obj["time"];
            var offset = DateTimeHelpers.FromUnixTime(milliseconds / 1000);
            return offset;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
