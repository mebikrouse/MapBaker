using System.Collections.Generic;
using MapBaker.Configuration.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace MapBaker.Configuration
{
    public struct LightConfiguration
    {
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 LightDirection { get; set; }

        [JsonConverter(typeof(ColorConverter))]
        public Color LightColor { get; set; }

        public float LightIntensity { get; set; }

        [JsonConverter(typeof(ColorConverter))]
        public Color AmbientColor { get; set; }

        public float AmbientIntensity { get; set; }

        [JsonConverter(typeof(ColorConverter))]
        public Color SpecularColor { get; set; }

        public float SpecularIntensity { get; set; }

        public IEnumerable<string> RaycastLayers { get; set; }
    }
}