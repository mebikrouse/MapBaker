using System.Collections.Generic;

namespace MapBaker.Configuration
{
    public struct DetailsConfiguration
    {
        public IEnumerable<MaterialConfiguration> MaterialConfigurations { get; set; }

        public IEnumerable<string> RaycastLayers { get; set; }
    }
}