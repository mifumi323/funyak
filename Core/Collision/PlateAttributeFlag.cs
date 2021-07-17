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
}
