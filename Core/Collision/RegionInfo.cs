namespace MifuminSoft.funyak.Collision
{
    public sealed class RegionInfo
    {
        public RegionAttributeFlag Flags { get; set; }
        public double Gravity { get; set; }
        public double Wind { get; set; }

        public bool HasFlag(RegionAttributeFlag flag) => Flags.Has(flag);
        public void SetFlag(RegionAttributeFlag flag, bool value) => Flags = Flags.SetTo(flag, value);
    }
}
