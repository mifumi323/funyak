using System.Drawing;
using ImageMagick;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.View.MapObjectView
{
    public class MainMapObjectView : IMapObjectView
    {
        public int Priority { get { return 0; } }
        public MainMapObject MapObject { get; protected set; }

        public MainMapObjectView(MainMapObject mapObject)
        {
            MapObject = mapObject;
        }

        public void Display(Graphics graphics)
        {
            using (MagickImage image = new MagickImage("icon0.gif"))
            {
                using (var icon = image.ToBitmap())
                {
                    graphics.DrawImage(icon, (float)MapObject.Left, (float)MapObject.Top);
                }
            }
        }
    }
}
