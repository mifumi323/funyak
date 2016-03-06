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
    }

    public interface IStaticMapObject : IMapObject
    {
    }
}
