using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class SegmentPlateCollider : PlateCollider
    {
        public Segment2D Segment { get; private set; }

        public SegmentPlateCollider(IMapObject owner) : base(owner) { }

        public void SetSegment(Segment2D segment) => UpdatePosition(Segment = segment);

        public override PlateNeedleCollision GetCollision(NeedleCollider needleCollider) => new PlateNeedleCollision
        {
            IsCollided = Segment.TryGetCrossPoint(needleCollider.Needle, out var crossPoint),
            Plate = this,
            Needle = needleCollider,
            CrossPoint = crossPoint,
        };
    }
}
