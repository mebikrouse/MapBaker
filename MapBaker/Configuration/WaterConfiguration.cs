using System.Collections.Generic;

namespace MapBaker.Configuration
{
    public struct WaterConfiguration
    {
        public ServerPath TexturePath { get; set; }

        public float MinOpacity { get; set; }

        public float MaxDepth { get; set; }

        public IEnumerable<string> RaycastLayers { get; set; }
    }
}