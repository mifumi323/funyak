using MifuminSoft.funyak.MapEnvironment;

namespace MifuminSoft.funyak
{
    /// <summary>
    /// マップオブジェクトの更新時の引数を格納します。
    /// </summary>
    public class UpdateMapObjectArgs
    {
        private Map map;

        public UpdateMapObjectArgs(Map map)
        {
            this.map = map;
        }

        /// <summary>
        /// マップの幅を取得します。
        /// </summary>
        public double MapWidth => map.Width;

        /// <summary>
        /// マップの高さを取得します。
        /// </summary>
        public double MapHeight => map.Height;

        /// <summary>
        /// 重力を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>重力(0.0が無重力、1.0が通常)</returns>
        public double GetGravity(double x, double y) => map.GetGravity(x, y);

        /// <summary>
        /// 風速を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>風速(0.0：無風、正の数：右向きの風、負の数：左向きの風)</returns>
        public double GetWind(double x, double y) => map.GetWind(x, y);

        /// <summary>
        /// 指定した位置の環境情報を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>環境</returns>
        public IMapEnvironment GetEnvironment(double x, double y) => map.GetEnvironment(x, y);
    }
}
