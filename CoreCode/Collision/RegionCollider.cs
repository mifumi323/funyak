using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public abstract class RegionCollider: ColliderBase
    {
        public RegionCollider(IMapObject owner) : base(owner) { }

        public bool Contains(PointCollider pointCollider) => Contains(pointCollider.X, pointCollider.Y);

        public abstract bool Contains(double x, double y);

        protected bool ContainsInAABB(double x, double y) => Left <= x && x <= Right && Top <= y && y <= Bottom;
    }
}
