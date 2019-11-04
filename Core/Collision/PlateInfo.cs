namespace MifuminSoft.funyak.Collision
{
    public class PlateInfo
    {
        public PlateFlags Flags { get; set; }
        public double Friction { get; set; }

        public bool HasFlag(PlateFlags flag) => (Flags & flag) != PlateFlags.None;
        public void SetFlag(PlateFlags flag, bool value) => Flags = value ? (Flags | flag) : (Flags & ~flag);
    }
}
