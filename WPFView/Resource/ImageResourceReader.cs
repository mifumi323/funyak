using System.IO;

namespace MifuminSoft.funyak.View.Resource
{
    public static class ImageResourceReader
    {
        public static ImageResource Read(string filename)
        {
            using (var fileStream = File.OpenRead(filename))
            {
                return new BitmapImageResource(fileStream, null);
            }
        }
    }
}
