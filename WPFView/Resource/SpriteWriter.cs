using System.IO;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    public class SpriteWriter
    {
        public static void WriteFileInfo(Sprite sprite, string imageFilename)
        {
            var jsonfile = SpriteFileInfo.ToSpriteInfoFileName(imageFilename);
            var info = SpriteFileInfo.FromImageResource(sprite);
            var jsondata = JsonConvert.SerializeObject(info, new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            File.WriteAllText(jsonfile, jsondata);
        }
    }
}
