namespace MifuminSoft.funyak.Collider
{
    public class PointCollider : ColliderBase
    {
        public double X => Left;
        public double Y => Top;

        public void SetPoint(double x, double y) => UpdatePosition(x, y, x, y);
    }
}
