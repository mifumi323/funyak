using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public abstract class PlateCollider : ColliderBase
    {
        public PlateCollider(MapObjectBase owner) : base(owner) { }

        public abstract bool IsCollided(NeedleCollider needleCollider, out PlateNeedleCollision collision);
    }
}
