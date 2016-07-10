using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public static class MapReader
    {
        public static Map FromString(string data)
        {
            return FromDynamic(JsonConvert.DeserializeObject(data));
        }

        public static Map FromDynamic(dynamic data)
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
                    var mapObject = MapObjectReader.FromDynamic(mapObjectData);
                    if (mapObject != null) map.AddMapObject(mapObject);
                }
            }
            return map;
        }
    }
}
