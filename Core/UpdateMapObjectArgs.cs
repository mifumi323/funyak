using MifuminSoft.funyak.MapEnvironment;

namespace MifuminSoft.funyak
{
    /// <summary>
    /// マップオブジェクトの更新時の引数を格納します。
    /// </summary>
    public class UpdateMapObjectArgs
    {
        private readonly Map map;

        public UpdateMapObjectArgs(Map map) => this.map = map;

        /// <summary>
        /// マップの幅を取得します。
        /// </summary>
        public double MapWidth => map.Width;

        /// <summary>
        /// マップの高さを取得します。
        /// </summary>
        public double MapHeight => map.Height;

        /// <summary>
        /// マップの重力を取得します。
        /// </summary>
        /// <returns>重力(0.0が無重力、1.0が通常)</returns>
        public double MapGravity => map.Gravity;
    }
}
