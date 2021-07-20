namespace MifuminSoft.funyak.Collision
{
    public class PlateInfo
    {
        public PlateAttributeFlag Flags { get; set; }
        public double Friction { get; set; }

        public bool HasFlag(PlateAttributeFlag flag) => Flags.Has(flag);
        public void SetFlag(PlateAttributeFlag flag, bool value) => Flags = Flags.SetTo(flag, value);
    }
}
