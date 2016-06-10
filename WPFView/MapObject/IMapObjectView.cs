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
        /// <param name="args">更新のパラメータ</param>
        void Update(MapObjectViewUpdateArgs args);
    }
}
