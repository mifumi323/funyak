namespace MifuminSoft.funyak.Collision
{
    public struct RegionPointCollision
    {
        public delegate void Listener(ref RegionPointCollision collision);

        public RegionCollider Region { get; set; }
        public PointCollider Point { get; set; }
    }
}
