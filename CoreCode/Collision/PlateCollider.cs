﻿using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public abstract class PlateCollider : ColliderBase
    {
        public PlateCollider(IMapObject owner) : base(owner) { }

        public abstract PlateNeedleCollision GetCollision(NeedleCollider needleCollider);
    }
}
