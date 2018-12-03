using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class RectangleCollider : RegionCollider
    {
        public RectangleCollider(IMapObject owner) : base(owner) { }

        public void SetPosition(double left, double top, double right, double bottom) => UpdatePosition(left, top, right, bottom);

        public override bool Contains(PointCollider pointCollider) => ContainsInAABB(pointCollider.X, pointCollider.Y);
    }
}
