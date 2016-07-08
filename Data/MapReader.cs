using System.ComponentModel;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public static class MapReader
    {
        public static Map FromString(string data)
        {
            dynamic def = JsonConvert.DeserializeObject(data, new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Include,
            });
            double width = def.width ?? 100.0;
            double height = def.height ?? 100.0;
            string color = def.color ?? "Transparent";
            var map = new Map(width, height)
            {
                BackgroundColor = color,
            };
            return map;
        }
    }
}
