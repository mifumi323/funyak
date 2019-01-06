using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Collision
{
    public struct PlateNeedleCollision
    {
        public delegate void Listener(ref PlateNeedleCollision collision);

        public PlateCollider Plate { get; set; }
        public NeedleCollider Needle { get; set; }
        public Vector2D CrossPoint { get; set; }
    }
}
