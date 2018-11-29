using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class RegionCollider: ColliderBase
    {
        public RegionCollider(IMapObject owner) : base(owner) { }

        public void SetPosition(double left, double top, double right, double bottom) => UpdatePosition(left, top, right, bottom);
    }
}
