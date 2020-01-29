using System;
using Newtonsoft.Json;
using UnityEngine;

namespace MapBaker.Configuration.Serialization
{
    public class ColorConverter : JsonConverter
    {
        private struct SerializableColor
        {
            public float R { get; set; }

            public float G { get; set; }

            public float B { get; set; }

            public float A { get; set; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var color = (Color) value;
            var serializableColor = new SerializableColor() {R = color.r, G = color.g, B = color.b, A = color.a};
            serializer.Serialize(writer, serializableColor);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var serializableColor = serializer.Deserialize<SerializableColor>(reader);
            return new Color(serializableColor.R, serializableColor.G, serializableColor.B, serializableColor.A);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Color).IsAssignableFrom(objectType);
        }
    }
}