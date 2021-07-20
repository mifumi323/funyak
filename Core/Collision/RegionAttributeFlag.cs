using System;

namespace MifuminSoft.funyak.Collision
{
    [Flags]
    public enum RegionAttributeFlag
    {
        None = 0,
        Gravity = 1 << 0,
        Wind = 1 << 1,
    }

    public static class RegionAttributeFlagUtil
    {
        public static bool Has(this RegionAttributeFlag source, RegionAttributeFlag flag) => (source & flag) != RegionAttributeFlag.None;
        public static RegionAttributeFlag Set(this RegionAttributeFlag source, RegionAttributeFlag flag) => source | flag;
        public static RegionAttributeFlag Reset(this RegionAttributeFlag source, RegionAttributeFlag flag) => source & ~flag;
        public static RegionAttributeFlag SetTo(this RegionAttributeFlag source, RegionAttributeFlag flag, bool value) => value ? (source | flag) : (source & ~flag);
    }
}
