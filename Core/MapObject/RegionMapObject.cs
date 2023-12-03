using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.MapObject
{
    public sealed class RegionMapObject : MapObjectBase
    {
        public RegionCollider Collider = null!; // 初期化直後にすぐこれも代入すべし。
        public string? Color;

        public override double X
        {
            get => (Collider.Left + Collider.Right) * 0.5;
            set => Collider.Shift(value - X, 0.0);
        }
        public override double Y
        {
            get => (Collider.Top + Collider.Bottom) * 0.5;
            set => Collider.Shift(0.0, value - Y);
        }
        public override void OnJoin(ColliderCollection colliderCollection) => colliderCollection.Add(Collider);
        public override void OnLeave(ColliderCollection colliderCollection) => colliderCollection.Remove(Collider);
    }
}
