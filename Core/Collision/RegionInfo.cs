namespace MifuminSoft.funyak.Collision
{
    public sealed class RegionInfo
    {
        public RegionFlags Flags { get; set; }
        public double Gravity { get; set; }
        public double Wind { get; set; }

        public bool HasFlag(RegionFlags flag) => (Flags & flag) != RegionFlags.None;
        public void SetFlag(RegionFlags flag, bool value) => Flags = value ? (Flags | flag) : (Flags & ~flag);
    }
}
