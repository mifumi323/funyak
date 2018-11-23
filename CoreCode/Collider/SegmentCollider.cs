using MifuminSoft.funyak.CollisionHelper;

namespace MifuminSoft.funyak.Collider
{
    public class SegmentCollider : ColliderBase
    {
        public Segment2D Segment { get; private set; }

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
