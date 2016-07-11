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
            return
                data.type == "funya" ? GenerateMainMapObject(data) :
                data.type == "line" ? GenerateLineMapObject(data) :
                null;
        }

        private static IMapObject GenerateMainMapObject(dynamic data)
        {
            var mainMapObject = new MainMapObject((double)(data.x ?? 0.0), (double)(data.y ?? 0.0));
            return mainMapObject;
        }

        private static IMapObject GenerateLineMapObject(dynamic data)
        {
            var lineMapObject = new LineMapObject((double)(data.x1 ?? 0.0), (double)(data.y1 ?? 0.0), (double)(data.x2 ?? 0.0), (double)(data.y2 ?? 0.0));
            if (data.color != null) lineMapObject.Color = (string)data.color;
            if (data.hit != null) lineMapObject.HitUpper = lineMapObject.HitBelow = lineMapObject.HitLeft = lineMapObject.HitRight = (bool)data.hit;
            if (data.ht != null) lineMapObject.HitUpper = (bool)data.ht;
            if (data.hb != null) lineMapObject.HitBelow = (bool)data.hb;
            if (data.hl != null) lineMapObject.HitLeft = (bool)data.hl;
            if (data.hr != null) lineMapObject.HitRight = (bool)data.hr;
            return lineMapObject;
        }
    }
}
