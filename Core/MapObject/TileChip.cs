using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.MapObject
{
    public class TileChip
    {
        public readonly PlateInfo PlateInfo;

        public object? Resource;
        public bool HitUpper
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitUpper);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitUpper, value);
        }
        public bool HitBelow
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitBelow);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitBelow, value);
        }
        public bool HitLeft
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitLeft);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitLeft, value);
        }
        public bool HitRight
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitRight);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitRight, value);
        }
        public double Friction
        {
            get => PlateInfo.Friction;
            set => PlateInfo.Friction = value;
        }

        public TileChip(PlateInfo? plateInfo = null)
        {
            PlateInfo = plateInfo ?? new PlateInfo();
        }
    }
}
