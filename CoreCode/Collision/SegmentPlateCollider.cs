using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class SegmentPlateCollider : PlateCollider
    {
        public Segment2D Segment { get; private set; }

        public SegmentPlateCollider(IMapObject owner) : base(owner) { }

        public void SetSegment(Segment2D segment) => UpdatePosition(Segment = segment);
    }
}
