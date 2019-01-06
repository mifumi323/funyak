using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class SegmentPlateCollider : PlateCollider
    {
        public Segment2D Segment { get; private set; }
        public PlateInfo PlateInfo { get; set; }

        public SegmentPlateCollider(MapObjectBase owner) : base(owner) { }

        public void SetSegment(Segment2D segment) => UpdatePosition(Segment = segment);

        public override bool IsCollided(NeedleCollider needleCollider, out PlateNeedleCollision collision)
        {
            var isCollided = Segment.TryGetCrossPoint(needleCollider.Needle, out var crossPoint);
            collision = isCollided ? new PlateNeedleCollision
            {
                Plate = this,
                Needle = needleCollider,
                CrossPoint = crossPoint,
            } : default;
            return isCollided;
        }
    }
}
