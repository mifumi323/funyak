using System.Collections.Generic;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    public class ImageResourceFileInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("chip")]
        public Dictionary<string, ImageChipInfo> Chip { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, ImageChipInfo> Metadata { get; set; }
    }
}
