using System.Collections.Generic;

namespace MapBaker.Configuration
{
    public struct MaterialConfiguration
    {
        public ServerPath TexturePath { get; set; }

        public IEnumerable<string> Patterns { get; set; }
    }
}