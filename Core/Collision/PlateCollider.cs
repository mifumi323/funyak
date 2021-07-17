using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public abstract class PlateCollider : ColliderBase
    {
        public PlateInfo PlateInfo { get; private set; } = new PlateInfo();

        /// <summary>
        /// 衝突時に呼ばれるコールバックを指定します。
        /// </summary>
        public PlateNeedleCollision.Listener? OnCollided { get; set; }

        public PlateCollider(MapObjectBase owner) : base(owner) { }

        public abstract bool IsCollided(NeedleCollider needleCollider, out PlateNeedleCollision collision);
    }
}
