using System;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    public class MapObjectViewFactory
    {
        public Func<int, Sprite> MainMapObjectResourceSelector { get; set; }

        public IMapObjectView Create(IMapObject mapObject)
        {
            if (mapObject is LineMapObject)
            {
                return new LineMapObjectView((LineMapObject)mapObject);
            }
            if (mapObject is MainMapObject)
            {
                return new MainMapObjectView((MainMapObject)mapObject)
                {
                    ImageResource = MainMapObjectResourceSelector?.Invoke(((MainMapObject)mapObject).Appearance),
                };
            }
            if (mapObject is TileGridMapObject)
            {
                return new TileGridMapObjectView((TileGridMapObject)mapObject);
            }
            return null;
        }
    }
}
