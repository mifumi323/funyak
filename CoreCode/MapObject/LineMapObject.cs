using System;
using MifuminSoft.funyak.CollisionHelper;

namespace MifuminSoft.funyak.MapObject
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
            segment = new CollidableSegment()
            {
                Segment = new Segment2D(x1, y1, x2, y2),
                HitUpper = false,
                HitBelow = false,
                HitLeft = false,
                HitRight = false,
                Friction = 1.0,
            };
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 始点のX座標
        /// </summary>
        public double X1
        {
            get
            {
                return segment.Segment.Start.X;
            }
            set
            {
                segment.Segment.Start.X = value;
            }
        }

        /// <summary>
        /// 始点のY座標
        /// </summary>
        public double Y1
        {
            get
            {
                return segment.Segment.Start.Y;
            }
            set
            {
                segment.Segment.Start.Y = value;
            }
        }

        /// <summary>
        /// 終点のX座標
        /// </summary>
        public double X2
        {
            get
            {
                return segment.Segment.End.X;
            }
            set
            {
                segment.Segment.End.X = value;
            }
        }

        /// <summary>
        /// 終点のY座標
        /// </summary>
        public double Y2
        {
            get
            {
                return segment.Segment.End.Y;
            }
            set
            {
                segment.Segment.End.Y = value;
            }
        }

        /// <summary>
        /// 上の当たり判定
        /// </summary>
        public bool HitUpper
        {
            get
            {
                return segment.HitUpper;
            }
            set
            {
                segment.HitUpper = value;
            }
        }

        /// <summary>
        /// 下の当たり判定
        /// </summary>
        public bool HitBelow
        {
            get
            {
                return segment.HitBelow;
            }
            set
            {
                segment.HitBelow = value;
            }
        }

        /// <summary>
        /// 左の当たり判定
        /// </summary>
        public bool HitLeft
        {
            get
            {
                return segment.HitLeft;
            }
            set
            {
                segment.HitLeft = value;
            }
        }

        /// <summary>
        /// 右の当たり判定
        /// </summary>
        public bool HitRight
        {
            get
            {
                return segment.HitRight;
            }
            set
            {
                segment.HitRight = value;
            }
        }

        /// <summary>
        /// 色(未指定または無効な色の場合透明)
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 摩擦力(1.0が通常)
        /// </summary>
        public double Friction
        {
            get
            {
                return segment.Friction;
            }
            set
            {
                segment.Friction = value;
            }
        }

        public double X { get { return (X1 + X2) / 2; } }
        public double Y { get { return (Y1 + Y2) / 2; } }
        public double Left { get { return Math.Min(X1, X2); } }
        public double Right { get { return Math.Max(X1, X2); } }
        public double Top { get { return Math.Min(Y1, Y2); } }
        public double Bottom { get { return Math.Max(Y1, Y2); } }

        private CollidableSegment segment;

        public Segment2D ToSegment2D() => segment.Segment;

        public CollidableSegment ToCollidableSegment() => segment;
    }
}
