using System;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.Collision
{
    public readonly struct PlateNeedleCollision
    {
        public delegate void Listener(ref PlateNeedleCollision collision);

        public PlateCollider Plate { get; }
        public NeedleCollider Needle { get; }
        public Vector2D CrossPoint { get; }
        public PlateInfo PlateInfo { get; }
        public Segment2D PlateSegment { get; }

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
