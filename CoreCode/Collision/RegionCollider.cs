namespace MifuminSoft.funyak.Collision
{
    public class RegionCollider: ColliderBase
    {
        public void SetPosition(double left, double top, double right, double bottom) => UpdatePosition(left, top, right, bottom);
    }
}
