using System.Collections.Generic;
using MapBaker.Configuration.Serialization;

namespace MapBaker.Configuration
{
    public struct PluginConfiguration
    {
        public float DrawDelay { get; set; }

        public Resolution RenderResolution { get; set; }

        public Resolution ResultResolution { get; set; }

        public ServerPath BackgroundTexturePath { get; set; }

        public ServerPath ResultPath { get; set; }

        public DetailsConfiguration DetailsConfiguration { get; set; }

        public LightConfiguration LightConfiguration { get; set; }

        public WaterConfiguration WaterConfiguration { get; set; }

        public IEnumerable<SplatConfiguration> SplatConfigurations { get; set; }
    }
}