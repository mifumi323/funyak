using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public sealed class RectangleCollider : RegionCollider
    {
        public RegionInfo RegionInfo { get; private set; } = new RegionInfo();

        public RectangleCollider(MapObjectBase owner) : base(owner) { }

        public void SetPosition(double left, double top, double right, double bottom) => UpdatePosition(left, top, right, bottom);

        public override bool Contains(double x, double y, out RegionInfo regionInfo)
        {
            regionInfo = RegionInfo;
            return ContainsInAABB(x, y);
        }

        public override void Shift(double dx, double dy) => SetPosition(Left + dx, Top + dy, Right + dx, Bottom + dy);
    }
}
