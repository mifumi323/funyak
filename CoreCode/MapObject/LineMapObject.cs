using System;
using MifuminSoft.funyak.Core.CollisionHelper;

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
        public double X1
        {
            get
            {
                return x1;
            }
            set
            {
                x1 = value;
                segment = null;
            }
        }
        private double x1 = 0;

        /// <summary>
        /// 始点のY座標
        /// </summary>
        public double Y1
        {
            get
            {
                return y1;
            }
            set
            {
                y1 = value;
                segment = null;
            }
        }
        private double y1 = 0;

        /// <summary>
        /// 終点のX座標
        /// </summary>
        public double X2
        {
            get
            {
                return x2;
            }
            set
            {
                x2 = value;
                segment = null;
            }
        }
        private double x2 = 0;

        /// <summary>
        /// 終点のY座標
        /// </summary>
        public double Y2
        {
            get
            {
                return y2;
            }
            set
            {
                y2 = value;
                segment = null;
            }
        }
        private double y2 = 0;

        /// <summary>
        /// 上の当たり判定
        /// </summary>
        public bool HitUpper { get; set; } = false;

        /// <summary>
        /// 下の当たり判定
        /// </summary>
        public bool HitBelow { get; set; } = false;

        /// <summary>
        /// 左の当たり判定
        /// </summary>
        public bool HitLeft { get; set; } = false;

        /// <summary>
        /// 右の当たり判定
        /// </summary>
        public bool HitRight { get; set; } = false;

        /// <summary>
        /// 色(未指定または無効な色の場合透明)
        /// </summary>
        public string Color { get; set; }

        public double X { get { return (X1 + X2) / 2; } }
        public double Y { get { return (Y1 + Y2) / 2; } }
        public double Left { get { return Math.Min(X1, X2); } }
        public double Right { get { return Math.Max(X1, X2); } }
        public double Top { get { return Math.Min(Y1, Y2); } }
        public double Bottom { get { return Math.Max(Y1, Y2); } }

        private Segment2D segment = null;

        public Segment2D ToSegment2D()
        {
            if (segment == null)
            {
                segment = new Segment2D(X1, Y1, X2, Y2);
            }
            return segment;
        }
    }
}
