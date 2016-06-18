using System;
using System.IO;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    public static class ImageResourceReader
    {
        public static ImageResource Read(string filename)
        {
            ImageResource resource = null;
            using (var fileStream = File.OpenRead(filename))
            {
                resource = new BitmapImageResource(fileStream, null);
            }
            try
            {
                var jsonfile = filename + ".json";
                var jsondata = File.ReadAllText(jsonfile);
                var info = JsonConvert.DeserializeObject<ImageResourceFileInfo>(jsondata);
                resource.Chip = info.Chip;
                resource.Metadata = info.Metadata;
            }
            catch (Exception e) { }
            return resource;
        }
    }
}
