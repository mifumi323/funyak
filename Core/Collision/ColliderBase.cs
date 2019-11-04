using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    /// <summary>
    /// 衝突図形を表します。
    /// </summary>
    public abstract class ColliderBase : IBounds
    {
        public double Left { get; private set; }
        public double Top { get; private set; }
        public double Right { get; private set; }
        public double Bottom { get; private set; }

        public MapObjectBase Owner { get; private set; }

        public ColliderBase(MapObjectBase owner) => Owner = owner;

        /// <summary>
        /// 平行移動します。
        /// </summary>
        /// <param name="dx">X座標の差分</param>
        /// <param name="dy">Y座標の差分</param>
        public abstract void Shift(double dx, double dy);

        protected void UpdatePosition(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// segmentを包含する位置に更新します。
        /// </summary>
        /// <param name="segment"></param>
        protected void UpdatePosition(Segment2D segment)
        {
            double left, top, right, bottom;
            if (segment.Start.X <= segment.End.X)
            {
                left = segment.Start.X;
                right = segment.End.X;
            }
            else
            {
                left = segment.End.X;
                right = segment.Start.X;
            }
            if (segment.Start.Y <= segment.End.Y)
            {
                top = segment.Start.Y;
                bottom = segment.End.Y;
            }
            else
            {
                top = segment.End.Y;
                bottom = segment.Start.Y;
            }
            UpdatePosition(left, top, right, bottom);
        }
    }
}
