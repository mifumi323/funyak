using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.View.MapObjectView
{
    public class MapObjectViewFactory
    {
        public IMapObjectView Create(IMapObject mapObject)
        {
            if (mapObject is MainMapObject)
            {
                return new MainMapObjectView();
            }
            return null;
        }
    }
}
