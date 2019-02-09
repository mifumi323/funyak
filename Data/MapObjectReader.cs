using System;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.MapObject;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public class MapObjectReader
    {
        public static MapObjectBase FromString(string data, MapReaderOption option) => FromDynamic(JsonConvert.DeserializeObject(data), option);

        public static MapObjectBase FromDynamic(dynamic data, MapReaderOption option)
            // VS2019になったらswitch式できれいに書けそうだ
            => data.type == "funya" ? GenerateMainMapObject(data, option) :
                data.type == "line" ? GenerateLineMapObject(data, option) :
                data.type == "tile" ? GenerateTileMapObject(data, option) :
                data.type == "rect" ? GenerateRectangleMapObject(data, option) :
                data.type == "ellipse" ? GenerateEllipseMapObject(data, option) :
                null;

        private static MapObjectBase GenerateMainMapObject(dynamic data, MapReaderOption option)
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
                GroundNormalY = data.gy ?? -1.0,
                GroundFriction = data.f ?? 1.0,

                Size = data.size ?? 28.0,
            };
            mainMapObject.PreviousX = data.px ?? mainMapObject.X;
            mainMapObject.PreviousY = data.py ?? mainMapObject.Y;
            mainMapObject.PreviousVelocityX = data.pvx ?? mainMapObject.VelocityX;
            mainMapObject.PreviousVelocityY = data.pvy ?? mainMapObject.VelocityY;
            return mainMapObject;
        }

        private static MapObjectBase GenerateLineMapObject(dynamic data, MapReaderOption option)
        {
            var lineMapObject = new LineMapObject((double)(data.x1 ?? 0.0), (double)(data.y1 ?? 0.0), (double)(data.x2 ?? 0.0), (double)(data.y2 ?? 0.0))
            {
                Name = data.n
            };
            if (data.color != null) lineMapObject.Color = (string)data.color;
            if (data.hit != null) lineMapObject.HitUpper = lineMapObject.HitBelow = lineMapObject.HitLeft = lineMapObject.HitRight = (bool)data.hit;
            if (data.ht != null) lineMapObject.HitUpper = (bool)data.ht;
            if (data.hb != null) lineMapObject.HitBelow = (bool)data.hb;
            if (data.hl != null) lineMapObject.HitLeft = (bool)data.hl;
            if (data.hr != null) lineMapObject.HitRight = (bool)data.hr;
            if (data.f != null) lineMapObject.Friction = (double)data.f;
            return lineMapObject;
        }

        private static MapObjectBase GenerateTileMapObject(dynamic data, MapReaderOption option)
            => new TileGridMapObject((double)(data.x ?? 0.0), (double)(data.y ?? 0.0), (int)data.w, (int)data.h)
            {
                Name = data.n
            };

        private static MapObjectBase GenerateRectangleMapObject(dynamic data, MapReaderOption option)
        {
            var regionMapObject = new RegionMapObject()
            {
                Name = data.n,
                Color = data.c,
            };
            var collider = new RectangleCollider(regionMapObject);
            collider.SetPosition((double)(data.l ?? 0.0), (double)(data.t ?? 0.0), (double)(data.r ?? 0.0), (double)(data.b ?? 0.0));
            collider.RegionInfo = new RegionInfo()
            {
                Gravity = data.g ?? double.NaN,
                Wind = data.w ?? double.NaN,
            };
            collider.RegionInfo.SetFlag(RegionFlags.Active, true);
            regionMapObject.Collider = collider;
            if (data.color != null) regionMapObject.Color = (string)data.color;
            return regionMapObject;
        }

        private static MapObjectBase GenerateEllipseMapObject(dynamic data, MapReaderOption option)
        {
            var ellipseMapObject = new RegionMapObject()
            {
                Name = data.n,
                Color = data.c,
            };
            var collider = new EllipseCollider(ellipseMapObject);
            collider.SetPosition((double)(data.l ?? 0.0), (double)(data.t ?? 0.0), (double)(data.r ?? 0.0), (double)(data.b ?? 0.0));
            collider.RegionInfo = new RegionInfo()
            {
                Gravity = data.g ?? double.NaN,
                Wind = data.w ?? double.NaN,
            };
            collider.RegionInfo.SetFlag(RegionFlags.Active, true);
            ellipseMapObject.Collider = collider;
            return ellipseMapObject;
        }
    }
}
