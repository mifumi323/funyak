using System.Drawing;

namespace MifuminSoft.funyak.View.MapObjectView
{
    public class MainMapObjectView : IMapObjectView
    {
        public int Priority { get { return 0; } }

        public void Display(Graphics graphics)
        {
            // TODO: 主人公の表示処理
        }
    }
}
