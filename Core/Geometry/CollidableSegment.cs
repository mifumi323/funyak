namespace MifuminSoft.funyak.Geometry
{
    public struct CollidableSegment
    {
        public Segment2D Segment;
        public bool HitUpper;
        public bool HitBelow;
        public bool HitLeft;
        public bool HitRight;
        public double Friction;
    }
}
