using System;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    public class MapObjectViewFactory
    {
        public Func<int, Sprite> FunyaMapObjectResourceSelector { get; set; }

        public IMapObjectView Create(MapObjectBase mapObject)
        {
            if (mapObject is LineMapObject lineMapObject)
            {
                return new LineMapObjectView(lineMapObject);
            }
            if (mapObject is FunyaMapObject funyaMapObject)
            {
                return new FunyaMapObjectView(funyaMapObject)
                {
                    ImageResource = FunyaMapObjectResourceSelector?.Invoke(funyaMapObject.Appearance),
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
