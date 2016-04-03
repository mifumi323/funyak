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
            // TODO: 線の表示処理
        }
    }
}
