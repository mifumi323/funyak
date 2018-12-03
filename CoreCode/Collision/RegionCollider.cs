using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public abstract class RegionCollider: ColliderBase
    {
        public RegionCollider(IMapObject owner) : base(owner) { }

        public abstract bool Contains(PointCollider pointCollider);

        protected bool ContainsInAABB(double x, double y) => Left <= x && x <= Right && Top <= y && y <= Bottom;
    }
}
