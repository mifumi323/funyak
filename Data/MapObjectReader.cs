using System;
using MifuminSoft.funyak.MapObject;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public class MapObjectReader
    {
        public static IMapObject FromString(string data, MapReaderOption option)
        {
            return FromDynamic(JsonConvert.DeserializeObject(data), option);
        }

        public static IMapObject FromDynamic(dynamic data, MapReaderOption option)
        {
            return
                data.type == "funya" ? GenerateMainMapObject(data, option) :
                data.type == "line" ? GenerateLineMapObject(data, option) :
                null;
        }

        private static IMapObject GenerateMainMapObject(dynamic data, MapReaderOption option)
        {
            var mainMapObject = new MainMapObject((double)(data.x ?? 0.0), (double)(data.y ?? 0.0))
            {
                Input = data.i != null ? InputReader.FromDynamic(data.i, option) : option.Input,

                Name = data.n,

                VelocityX = data.vx ?? 0.0,
                VelocityY = data.vy ?? 0.0,
                Angle = data.a ?? 0.0,
                AngularVelocity = data.av ?? 0.0,
                Direction = (Direction)Enum.Parse(typeof(Direction), (string)data.dr ?? nameof(Direction.Front)),

                State = (MainMapObjectState)Enum.Parse(typeof(MainMapObjectState), (string)data.st ?? nameof(MainMapObjectState.Float)),
                StateCounter = data.sc ?? 0,

                GroundNormalX = data.gx ?? 0.0,
                GroundNormalY = data.gy ?? 0.0,
            };
            mainMapObject.PreviousX = data.px ?? mainMapObject.X;
            mainMapObject.PreviousY = data.py ?? mainMapObject.Y;
            mainMapObject.PreviousVelocityX = data.pvx ?? mainMapObject.VelocityX;
            mainMapObject.PreviousVelocityY = data.pvy ?? mainMapObject.VelocityY;
            return mainMapObject;
        }

        private static IMapObject GenerateLineMapObject(dynamic data, MapReaderOption option)
        {
            var lineMapObject = new LineMapObject((double)(data.x1 ?? 0.0), (double)(data.y1 ?? 0.0), (double)(data.x2 ?? 0.0), (double)(data.y2 ?? 0.0));
            lineMapObject.Name = data.n;
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
