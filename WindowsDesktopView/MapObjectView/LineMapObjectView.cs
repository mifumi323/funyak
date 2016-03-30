using System.Drawing;

namespace MifuminSoft.funyak.View.MapObjectView
{
    public class LineMapObjectView : IMapObjectView
    {
        public int Priority { get { return -1; } }

        public void Display(Graphics graphics)
        {
            // TODO: 線の表示処理
        }
    }
}
