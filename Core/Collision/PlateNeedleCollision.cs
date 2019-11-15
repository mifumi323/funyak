using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Collision
{
    public readonly struct PlateNeedleCollision
    {
        public delegate void Listener(ref PlateNeedleCollision collision);

        public readonly PlateCollider Plate;
        public readonly NeedleCollider Needle;
        public readonly Vector2D CrossPoint;
        public readonly PlateInfo PlateInfo;
        public readonly Segment2D PlateSegment;

        public PlateNeedleCollision(PlateCollider plate, NeedleCollider needle, Vector2D crossPoint, PlateInfo plateInfo, in Segment2D plateSegment)
        {
            Plate = plate;
            Needle = needle;
            CrossPoint = crossPoint;
            PlateInfo = plateInfo;
            PlateSegment = plateSegment;
        }
    }
}
