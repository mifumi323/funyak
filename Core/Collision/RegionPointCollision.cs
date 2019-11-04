namespace MifuminSoft.funyak.Collision
{
    public readonly struct RegionPointCollision
    {
        public delegate void Listener(ref RegionPointCollision collision);

        public readonly RegionCollider Region;
        public readonly PointCollider Point;
        public readonly RegionInfo RegionInfo;

        public RegionPointCollision(RegionCollider region, PointCollider point, RegionInfo regionInfo)
        {
            Region = region;
            Point = point;
            RegionInfo = regionInfo;
        }
    }
}
