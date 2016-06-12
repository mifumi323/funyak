﻿using System;

namespace MifuminSoft.funyak.Core.MapObject
{
    public interface IMapObject : IBounds
    {
        double X { get; }
        double Y { get; }
    }

    public interface IDynamicMapObject : IMapObject
    {
        void UpdateSelf();
        Action CheckCollision();
    }

    public interface IStaticMapObject : IMapObject
    {
    }
}