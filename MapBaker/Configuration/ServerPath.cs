using Newtonsoft.Json;
using Oxide.Core;

namespace MapBaker.Configuration
{
    public struct ServerPath
    {
        public string Path { get; set; }

        [JsonIgnore]
        public string FullPath => System.IO.Path.Combine(Interface.Oxide.DataDirectory, Path);
    }
}