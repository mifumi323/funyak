using MifuminSoft.funyak.MapObject;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public class MapObjectReader
    {
        public static IMapObject FromString(string data)
        {
            return FromDynamic(JsonConvert.DeserializeObject(data));
        }

        public static IMapObject FromDynamic(dynamic data)
        {
            if (data.type == "funya")
            {
                return GenerateMainMapObject(data);
            }
            return null;
        }

        private static IMapObject GenerateMainMapObject(dynamic data)
        {
            var mainMapObject = new MainMapObject((double)(data.x ?? 0.0), (double)(data.y ?? 0.0));
            return mainMapObject;
        }
    }
}
