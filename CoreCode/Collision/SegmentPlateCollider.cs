using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class SegmentPlateCollider : PlateCollider
    {
        public Segment2D Segment { get; private set; }

        public SegmentPlateCollider(IMapObject owner) : base(owner) { }

        public void SetSegment(Segment2D segment)
        {
            Segment = segment;
            double left, top, right, bottom;
            if (segment.Start.X <= segment.End.X)
            {
                left = segment.Start.X;
                right = segment.End.X;
            }
            else
            {
                left = segment.End.X;
                right = segment.Start.X;
            }
            if (segment.Start.Y <= segment.End.Y)
            {
                top = segment.Start.Y;
                bottom = segment.End.Y;
            }
            else
            {
                top = segment.End.Y;
                bottom = segment.Start.Y;
            }
            UpdatePosition(left, top, right, bottom);
        }
    }
}
