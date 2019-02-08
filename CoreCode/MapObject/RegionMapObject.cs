using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.MapObject
{
    public sealed class RegionMapObject : MapObjectBase
    {
        public RegionCollider Collider;
        public string BackgroundColor;

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
        public override void OnJoin(Map map, CollisionManager collisionManager) => collisionManager.Add(Collider);
        public override void OnLeave(Map map, CollisionManager collisionManager) => collisionManager.Remove(Collider);
    }
}
