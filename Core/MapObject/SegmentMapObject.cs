using System;

namespace MifuminSoft.funyak.MapObject
{
    public class SegmentMapObject : MapObjectBase, IBounds
    {
        /// <summary>始点のX座標</summary>
        public double X1 { get; set; }
        /// <summary>始点のY座標</summary>
        public double Y1 { get; set; }
        /// <summary>終点のX座標</summary>
        public double X2 { get; set; }
        /// <summary>終点のY座標</summary>
        public double Y2 { get; set; }

        /// <summary>
        /// 色(未指定または無効な色の場合透明)
        /// </summary>
        public string? Color { get; set; }

        public override double X
        {
            get => (X1 + X2) / 2;
            set
            {
                var diff = value - X;
                X1 += diff;
                X2 += diff;
            }
        }
        public override double Y
        {
            get => (Y1 + Y2) / 2;
            set
            {
                var diff = value - Y;
                Y1 += diff;
                Y2 += diff;
            }
        }

        public double Left => Math.Min(X1, X2);
        public double Right => Math.Max(X1, X2);
        public double Top => Math.Min(Y1, Y2);
        public double Bottom => Math.Max(Y1, Y2);
    }
}
