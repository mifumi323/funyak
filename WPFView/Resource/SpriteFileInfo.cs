using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    public class SpriteFileInfo
    {
        public class FrameGroup
        {
            [JsonProperty("k")]
            public string Key { get; set; }
            [JsonProperty("d")]
            public int Duration { get; set; }
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("chip")]
        public Dictionary<string, SpriteChipInfo> Chip { get; set; }

        [JsonProperty("animation")]
        public Dictionary<string, FrameGroup[]> Animation { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, string> Metadata { get; set; }

        public void SetToImageResource(Sprite imageResource)
        {
            imageResource.Chip = Chip;
            imageResource.Animation = new Dictionary<string, string[]>();
            if (Animation != null)
            {
                foreach (var animation in Animation)
                {
                    var key = animation.Key;
                    var groups = animation.Value;
                    var animationData =new List<string>();
                    foreach (var group in groups)
                    {
                        var frameKey = group.Key;
                        var duration = group.Duration;
                        for (int i = 0; i < duration; i++)
                        {
                            animationData.Add(frameKey);
                        }
                    }
                    imageResource.Animation[key] = animationData.ToArray();
                }
            }
            imageResource.Metadata = Metadata;
        }

        public static SpriteFileInfo FromImageResource(Sprite imageResource)
        {
            var irfi = new SpriteFileInfo();
            irfi.Chip = imageResource.Chip;
            irfi.Animation = new Dictionary<string, FrameGroup[]>();
            foreach (var animation in imageResource.Animation)
            {
                string currentKey = null;
                var groups = new List<FrameGroup>();
                FrameGroup currentGroup = null;
                foreach (var frameKey in animation.Value)
                {
                    if (frameKey != currentKey)
                    {
                        currentGroup = new FrameGroup()
                        {
                            Key = frameKey,
                            Duration = 0,
                        };
                        currentKey = frameKey;
                        groups.Add(currentGroup);
                    }
                    currentGroup.Duration++;
                }
                irfi.Animation[animation.Key] = groups.ToArray();
            }
            irfi.Metadata = imageResource.Metadata;
            return irfi;
        }

        public static string ToSpriteInfoFileName(string imageFilename)
        {
            return imageFilename + ".json";
        }
    }
}
