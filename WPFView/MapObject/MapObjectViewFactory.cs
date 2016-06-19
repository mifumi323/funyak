using System;
using MifuminSoft.funyak.Core.MapObject;
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
            return null;
        }
    }
}
