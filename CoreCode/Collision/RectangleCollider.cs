using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class RectangleCollider : RegionCollider
    {
        public RectangleCollider(MapObjectBase owner) : base(owner) { }

        public void SetPosition(double left, double top, double right, double bottom) => UpdatePosition(left, top, right, bottom);

        public override bool Contains(double x, double y) => ContainsInAABB(x, y);
    }
}
