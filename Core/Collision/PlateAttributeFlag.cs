using System;

namespace MifuminSoft.funyak.Collision
{
    [Flags]
    public enum PlateAttributeFlag : ulong
    {
        None = 0,
        HitUpper = 1 << 0,
        HitBelow = 1 << 1,
        HitLeft = 1 << 2,
        HitRight = 1 << 3,
    }

    public static class PlateAttributeFlagUtil
    {
        public static bool Has(this PlateAttributeFlag source, PlateAttributeFlag flag) => (source & flag) != PlateAttributeFlag.None;
        public static PlateAttributeFlag Set(this PlateAttributeFlag source, PlateAttributeFlag flag) => source | flag;
        public static PlateAttributeFlag Reset(this PlateAttributeFlag source, PlateAttributeFlag flag) => source & ~flag;
        public static PlateAttributeFlag SetTo(this PlateAttributeFlag source, PlateAttributeFlag flag, bool value) => value ? (source | flag) : (source & ~flag);
    }
}
