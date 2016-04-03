using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.View.MapObjectView
{
    public class MapObjectViewFactory
    {
        public IMapObjectView Create(IMapObject mapObject)
        {
            if (mapObject is MainMapObject)
            {
                return new MainMapObjectView((MainMapObject)mapObject);
            }
            if (mapObject is LineMapObject)
            {
                return new LineMapObjectView((LineMapObject)mapObject);
            }
            return null;
        }
    }
}
