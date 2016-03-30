using System.Drawing;

namespace MifuminSoft.funyak.View.MapObjectView
{
    public interface IMapObjectView
    {
        /// <summary>
        /// 表示優先順位
        /// 小さいほど先に描かれ、大きいほど手前に描かれる
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 表示処理
        /// <param name="graphics">表示に使うGraphicsオブジェクト</param>
        /// </summary>
        void Display(Graphics graphics);
    }
}
