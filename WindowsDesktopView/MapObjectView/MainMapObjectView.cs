using System.Drawing;
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
            graphics.DrawEllipse(Pens.Yellow, (float)MapObject.Left, (float)MapObject.Top, (float)(MapObject.Right - MapObject.Left), (float)(MapObject.Bottom - MapObject.Top));
        }
    }
}
