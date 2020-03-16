using System;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Utils;
using Newtonsoft.Json;

namespace Dyndle.Modules.Search.Convertors
{
    /// <inheritdoc />
    public class TConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, SearchUtil.ResponseModel);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ISearchResultItem);
        }
    }
}
