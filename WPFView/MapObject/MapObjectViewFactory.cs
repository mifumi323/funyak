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
            if (mapObject is FunyaMapObject mainMapObject)
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
            if (mapObject is RegionMapObject regionGridMapObject)
            {
                return new RegionMapObjectView(regionGridMapObject);
            }
            throw new ArgumentException($"非対応の型「{mapObject.GetType().Name}」です。", nameof(mapObject));
        }
    }
}
