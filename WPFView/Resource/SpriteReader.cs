using System;
using System.IO;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    public static class SpriteReader
    {
        public static Sprite Read(string filename)
        {
            Sprite resource = null;
            using (var fileStream = File.OpenRead(filename))
            {
                resource = new BitmapSprite(fileStream);
            }
            try
            {
                var jsonfile = SpriteFileInfo.ToSpriteInfoFileName(filename);
                var jsondata = File.ReadAllText(jsonfile);
                var info = JsonConvert.DeserializeObject<SpriteFileInfo>(jsondata);
                info.SetToImageResource(resource);
            }
            catch (Exception e) { }
            return resource;
        }
    }
}
