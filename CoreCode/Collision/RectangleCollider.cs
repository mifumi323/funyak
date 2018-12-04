using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class RectangleCollider : RegionCollider
    {
        public RectangleCollider(IMapObject owner) : base(owner) { }

        public void SetPosition(double left, double top, double right, double bottom, IColliderUpdatePositionListener listener) => UpdatePosition(left, top, right, bottom, listener);

        public override bool Contains(double x, double y) => ContainsInAABB(x, y);
    }
}
