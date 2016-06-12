using System;

namespace MifuminSoft.funyak.Core.MapObject
{
    /// <summary>
    /// 線のマップオブジェクト
    /// </summary>
    public class LineMapObject : IStaticMapObject
    {
        /// <summary>
        /// 線のマップオブジェクトを初期化します。
        /// </summary>
        /// <param name="x1">始点のX座標</param>
        /// <param name="y1">始点のY座標</param>
        /// <param name="x2">終点のX座標</param>
        /// <param name="y2">終点のY座標</param>
        public LineMapObject(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// 始点のX座標
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// 始点のY座標
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// 終点のX座標
        /// </summary>
        public double X2 { get; set; }

        /// <summary>
        /// 終点のY座標
        /// </summary>
        public double Y2 { get; set; }

        public double X { get { return (X1 + X2) / 2; } }
        public double Y { get { return (Y1 + Y2) / 2; } }
        public double Left { get { return Math.Min(X1, X2); } }
        public double Right { get { return Math.Max(X1, X2); } }
        public double Top { get { return Math.Min(Y1, Y2); } }
        public double Bottom { get { return Math.Max(Y1, Y2); } }
    }
}
