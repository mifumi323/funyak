using System.Windows;
using System.Windows.Controls;

namespace MifuminSoft.funyak.View.MapObject
{
    public interface IMapObjectView
    {
        /// <summary>
        /// 表示先のCanvas
        /// </summary>
        Canvas Canvas { get; set; }

        /// <summary>
        /// 表示を更新します
        /// </summary>
        /// <param name="offset">表示領域での位置オフセット量</param>
        /// <param name="scale">拡大率</param>
        /// <param name="area">マップ領域での表示範囲</param>
        void Update(Point offset, double scale, Rect area);
    }
}
