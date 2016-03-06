using System;

namespace MifuminSoft.funyak.Core.MapObject
{
    public interface IMapObject
    {
        double X { get; }
        double Y { get; }
        double Left { get; }
        double Top { get; }
        double Right { get; }
        double Bottom { get; }
    }

    public interface IDynamicMapObject : IMapObject
    {
        void UpdateSelf();
    }
}
