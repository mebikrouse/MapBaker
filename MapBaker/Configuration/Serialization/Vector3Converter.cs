using System;
using Newtonsoft.Json;
using UnityEngine;

namespace MapBaker.Configuration.Serialization
{
    public class Vector3Converter : JsonConverter
    {
        private class SerializableVector3
        {
            public float X { get; set; }

            public float Y { get; set; }

            public float Z { get; set; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var vector = (Vector3) value;
            var serializableVector = new SerializableVector3() {X = vector.x, Y = vector.y, Z = vector.z};
            serializer.Serialize(writer, serializableVector);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var serializableVector = serializer.Deserialize<SerializableVector3>(reader);
            return new Vector3(serializableVector.X, serializableVector.Y, serializableVector.Z);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Vector3).IsAssignableFrom(objectType);
        }
    }
}