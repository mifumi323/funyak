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
}
