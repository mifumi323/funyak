namespace MifuminSoft.funyak.Collision
{
    public sealed class RegionInfo
    {
        public RegionAttributeFlag Flags { get; set; }
        public double Gravity { get; set; }
        public double Wind { get; set; }

        public bool HasFlag(RegionAttributeFlag flag) => (Flags & flag) != RegionAttributeFlag.None;
        public void SetFlag(RegionAttributeFlag flag, bool value) => Flags = value ? (Flags | flag) : (Flags & ~flag);
    }
}
