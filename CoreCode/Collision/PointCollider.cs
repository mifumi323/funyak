using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class PointCollider : ColliderBase
    {
        public double X => Left;
        public double Y => Top;

        public PointCollider(MapObjectBase owner) : base(owner) { }

        public void SetPoint(double x, double y) => UpdatePosition(x, y, x, y);
    }
}
