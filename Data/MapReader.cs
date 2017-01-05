using System.Collections.Generic;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapObject;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public class MapReaderOption
    {
        public IInput Input = null;
        public IList<TileChip> ChipList = null;
    }

    public static class MapReader
    {
        public static Map FromString(string data, MapReaderOption option)
        {
            return FromDynamic(JsonConvert.DeserializeObject(data), option);
        }

        public static Map FromDynamic(dynamic data, MapReaderOption option)
        {
            var map = new Map((double)(data.width ?? 100.0), (double)(data.height ?? 100.0))
            {
                BackgroundColor = (string)(data.color ?? "Transparent"),
                Gravity = (double)(data.gravity ?? 1.0),
                Wind = (double)(data.wind ?? 0.0),
            };
            if (data.objects != null)
            {
                foreach (var mapObjectData in data.objects)
                {
                    var mapObject = MapObjectReader.FromDynamic(mapObjectData, option);
                    if (mapObject != null) map.AddMapObject(mapObject);
                }
            }
            if (data.areas != null)
            {
                foreach (var areaEnvironmentData in data.areas)
                {
                    var areaEnvironment = AreaEnvironmentReader.FromDynamic(areaEnvironmentData, option);
                    if (areaEnvironment != null) map.AddAreaEnvironment(areaEnvironment);
                }
            }
            return map;
        }
    }
}
