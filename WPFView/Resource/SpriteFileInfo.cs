using System.Collections.Generic;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    public class SpriteFileInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("chip")]
        public Dictionary<string, SpriteChipInfo> Chip { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, SpriteChipInfo> Metadata { get; set; }

        public void SetToImageResource(Sprite imageResource)
        {
            imageResource.Chip = Chip;
            imageResource.Metadata = Metadata;
        }

        public static SpriteFileInfo FromImageResource(Sprite imageResource)
        {
            var irfi = new SpriteFileInfo();
            irfi.Chip = imageResource.Chip;
            irfi.Metadata = imageResource.Metadata;
            return irfi;
        }

        public static string ToSpriteInfoFileName(string imageFilename)
        {
            return imageFilename + ".json";
        }
    }
}
