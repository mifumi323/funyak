using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class PointCollider : ColliderBase
    {
        public double X => Left;
        public double Y => Top;

        public PointCollider(IMapObject owner) : base(owner) { }

        public void SetPoint(double x, double y, IColliderUpdatePositionListener listener) => UpdatePosition(x, y, x, y, listener);
    }
}
