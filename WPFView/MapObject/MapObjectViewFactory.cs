using System;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    public class MapObjectViewFactory
    {
        public Func<int, Sprite> MainMapObjectResourceSelector { get; set; }

        public IMapObjectView Create(MapObjectBase mapObject)
        {
            if (mapObject is LineMapObject lineMapObject)
            {
                return new LineMapObjectView(lineMapObject);
            }
            if (mapObject is MainMapObject mainMapObject)
            {
                return new MainMapObjectView(mainMapObject)
                {
                    ImageResource = MainMapObjectResourceSelector?.Invoke(mainMapObject.Appearance),
                };
            }
            if (mapObject is TileGridMapObject tileGridMapObject)
            {
                return new TileGridMapObjectView(tileGridMapObject);
            }
            return null;
        }
    }
}
