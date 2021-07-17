using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public sealed class SegmentPlateCollider : PlateCollider
    {
        public Segment2D Segment { get; private set; }

        public SegmentPlateCollider(MapObjectBase owner) : base(owner) { }

        public void SetSegment(Segment2D segment) => UpdatePosition(Segment = segment);

        public override bool IsCollided(NeedleCollider needleCollider, out PlateNeedleCollision collision)
        {
            var isCollided = Segment.TryGetCrossPoint(needleCollider.ActualNeedle, out var crossPoint);
            collision = isCollided ? new PlateNeedleCollision(this, needleCollider, crossPoint, PlateInfo, Segment) : default;
            return isCollided;
        }

        public override void Shift(double dx, double dy) => SetSegment(new Segment2D(Segment.Start + new Vector2D(dx, dy), Segment.End + new Vector2D(dx, dy)));
    }
}
