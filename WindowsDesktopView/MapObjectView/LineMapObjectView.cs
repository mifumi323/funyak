using System.Drawing;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.View.MapObjectView
{
    public class LineMapObjectView : IMapObjectView
    {
        public int Priority { get { return -1; } }
        public LineMapObject MapObject { get; protected set; }

        public LineMapObjectView(LineMapObject mapObject)
        {
            MapObject = mapObject;
        }

        public void Display(Graphics graphics)
        {
            graphics.DrawLine(Pens.White, (float)MapObject.X1, (float)MapObject.Y1, (float)MapObject.X2, (float)MapObject.Y2);
        }
    }
}
