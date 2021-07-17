using System;

namespace MifuminSoft.funyak.Collision
{
    [Flags]
    public enum RegionAttributeFlag
    {
        None = 0,
        Active = 1 << 0,
        Gravity = 1 << 1,
        Wind = 1 << 2,
    }
}
