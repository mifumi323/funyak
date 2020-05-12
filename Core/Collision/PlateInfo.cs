namespace MifuminSoft.funyak.Collision
{
    public class PlateInfo
    {
        public PlateAttributeFlag Flags { get; set; }
        public double Friction { get; set; }

        public bool HasFlag(PlateAttributeFlag flag) => (Flags & flag) != PlateAttributeFlag.None;
        public void SetFlag(PlateAttributeFlag flag, bool value) => Flags = value ? (Flags | flag) : (Flags & ~flag);
    }
}
