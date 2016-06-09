using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.View.MapObject
{
    public class MapObjectViewFactory
    {
        public IMapObjectView Create(IMapObject mapObject)
        {
            if (mapObject is LineMapObject)
            {
                return new LineMapObjectView((LineMapObject)mapObject);
            }
            if (mapObject is MainMapObject)
            {
                return new MainMapObjectView((MainMapObject)mapObject);
            }
            return null;
        }
    }
}
