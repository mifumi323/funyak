using System;

namespace MifuminSoft.funyak.Collision
{
    [Flags]
    public enum PlateAttributeFlag : ulong
    {
        None = 0,
        /// <summary>上向きのPlate(Needle側は下向き)</summary>
        HitUpper = 1 << 0,
        /// <summary>下向きのPlate(Needle側は上向き)</summary>
        HitBelow = 1 << 1,
        /// <summary>左向きのPlate(Needle側は右向き)</summary>
        HitLeft = 1 << 2,
        /// <summary>左向きのPlate(Needle側は右向き)</summary>
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
