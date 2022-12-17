using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.MapObject
{
    public class NeedleMapObject : SegmentMapObject
    {
        private readonly NeedleCollider collider;

        public PlateAttributeFlag Reactivities
        {
            get => collider.Reactivities;
            set => collider.Reactivities = value;
        }

        public PlateNeedleCollision.Listener? OnCollided
        {
            get => collider.OnCollided;
            set => collider.OnCollided = value;
        }

        public NeedleMapObject() : base()
        {
            collider = new NeedleCollider(this);
        }

        public override void OnJoin(Map map, ColliderCollection colliderCollection) => colliderCollection.Add(collider);
        public override void OnLeave(Map map, ColliderCollection colliderCollection) => colliderCollection.Remove(collider);

        public override void CheckCollision(CheckMapObjectCollisionArgs args)
        {
            base.CheckCollision(args);
            collider.SetSegment(new Segment2D(X1, Y1, X2, Y2));
        }
    }
}
