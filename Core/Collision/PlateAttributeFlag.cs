using System;

namespace MifuminSoft.funyak.Collision
{
    [Flags]
    public enum PlateAttributeFlag : ulong
    {
        None = 0,
        Active = 1 << 0,
        HitUpper = 1 << 1,
        HitBelow = 1 << 2,
        HitLeft = 1 << 3,
        HitRight = 1 << 4,
    }
}
