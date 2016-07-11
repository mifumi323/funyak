using MifuminSoft.funyak.Input;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public class MapReaderOption
    {
        public IInput Input = null;
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
            return map;
        }
    }
}
