using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public abstract class RegionCollider: ColliderBase
    {
        /// <summary>
        /// 衝突時に呼ばれるコールバックを指定します。
        /// </summary>
        public RegionPointCollision.Listener OnCollided { get; set; }

        public RegionCollider(MapObjectBase owner) : base(owner) { }

        public bool Contains(PointCollider pointCollider, out RegionPointCollision collision)
        {
            var contains = Contains(pointCollider.X, pointCollider.Y);
            collision = contains ? new RegionPointCollision()
            {
                Region = this,
                Point = pointCollider
            } : default;
            return contains;
        }

        public abstract bool Contains(double x, double y);

        protected bool ContainsInAABB(double x, double y) => Left <= x && x <= Right && Top <= y && y <= Bottom;
    }
}
