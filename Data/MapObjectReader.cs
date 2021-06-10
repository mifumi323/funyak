using System;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.MapObject;
using YamlDotNet.RepresentationModel;

namespace MifuminSoft.funyak.Data
{
    public class MapObjectReader
    {
        public static MapObjectBase FromDynamic(dynamic data, MapReaderOption option) => (string)data.type switch
        {
            "funya" => GenerateFunyaMapObject(data, option),
            "line" => GenerateLineMapObject(data, option),
            "tile" => GenerateTileMapObject(data, option),
            "rect" => GenerateRectangleMapObject(data, option),
            "ellipse" => GenerateEllipseMapObject(data, option),
            _ => throw new ArgumentException($"不明なdata.type\"{data.type}\"です。")
        };

        private static MapObjectBase GenerateFunyaMapObject(dynamic data, MapReaderOption option)
        {
            var funyaMapObject = new FunyaMapObject((double)(data.x ?? 0.0), (double)(data.y ?? 0.0))
            {
                Input = data.i != null ? InputReader.FromDynamic(data.i, option) : option.Input,

                Name = data.n,

                VelocityX = data.vx ?? 0.0,
                VelocityY = data.vy ?? 0.0,
                Angle = data.a ?? 0.0,
                AngularVelocity = data.av ?? 0.0,
                Direction = (Direction)Enum.Parse(typeof(Direction), (string)data.dr ?? nameof(Direction.Front)),

                State = (FunyaMapObjectState)Enum.Parse(typeof(FunyaMapObjectState), (string)data.st ?? nameof(FunyaMapObjectState.Float)),
                StateCounter = data.sc ?? 0,

                GroundNormalX = data.gx ?? 0.0,
                GroundNormalY = data.gy ?? -1.0,
                GroundFriction = data.f ?? 1.0,

                Size = data.size ?? 28.0,
            };
            funyaMapObject.PreviousX = data.px ?? funyaMapObject.X;
            funyaMapObject.PreviousY = data.py ?? funyaMapObject.Y;
            funyaMapObject.PreviousVelocityX = data.pvx ?? funyaMapObject.VelocityX;
            funyaMapObject.PreviousVelocityY = data.pvy ?? funyaMapObject.VelocityY;
            return funyaMapObject;
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
            collider.RegionInfo.Gravity = data.g ?? double.NaN;
            collider.RegionInfo.Wind = data.w ?? double.NaN;
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Active, true);
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
            collider.RegionInfo.Gravity = data.g ?? double.NaN;
            collider.RegionInfo.Wind = data.w ?? double.NaN;
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Active, true);
            ellipseMapObject.Collider = collider;
            return ellipseMapObject;
        }

        public static MapObjectBase? FromYamlNode(YamlNode objectNode, MapReaderOption option)
        {
            if (!(objectNode is YamlMappingNode mappingNode))
            {
                throw new ArgumentException("objectのデータ構造はマッピングノードである必要があります。", nameof(objectNode));
            }
            var name = mappingNode.GetStringOrNull("n");
            var type = mappingNode.GetStringOrNull("type");
            return type switch
            {
                "funya" => GenerateFunyaMapObject(mappingNode, option, name),
                "line" => GenerateLineMapObject(mappingNode, option, name),
                "tile" => GenerateTileMapObject(mappingNode, option, name),
                "rect" => GenerateRectangleMapObject(mappingNode, option, name),
                "ellipse" => GenerateEllipseMapObject(mappingNode, option, name),
                _ => throw new ArgumentException($"不明なobject.type\"{type}\"です。")
            };
        }

        private static MapObjectBase GenerateFunyaMapObject(YamlMappingNode objectNode, MapReaderOption option, string? name)
        {
            var inputNode = objectNode.Children.ContainsKey("i") ? objectNode.Children["i"] : null;
            var directionName = objectNode.GetStringOrNull("dr");
            var stateName = objectNode.GetStringOrNull("st");
            var funyaMapObject = new FunyaMapObject(objectNode.GetDouble("x", 0.0), objectNode.GetDouble("y", 0.0))
            {
                Input = inputNode != null ? InputReader.FromYamlNode(inputNode, option) : option.Input,

                Name = name,

                VelocityX = objectNode.GetDouble("vx", 0.0),
                VelocityY = objectNode.GetDouble("vy", 0.0),
                Angle = objectNode.GetDouble("a", 0.0),
                AngularVelocity = objectNode.GetDouble("av", 0.0),
                Direction = directionName is null ? Direction.Front : (Direction)Enum.Parse(typeof(Direction), directionName),

                State = stateName is null ? FunyaMapObjectState.Float : (FunyaMapObjectState)Enum.Parse(typeof(FunyaMapObjectState), stateName),
                StateCounter = objectNode.GetInt32("sc", 0),

                GroundNormalX = objectNode.GetDouble("gx", 0.0),
                GroundNormalY = objectNode.GetDouble("gy", -1.0),
                GroundFriction = objectNode.GetDouble("f", 1.0),

                Size = objectNode.GetDouble("size", 28.0),
            };
            funyaMapObject.PreviousX = objectNode.GetDouble("px", funyaMapObject.X);
            funyaMapObject.PreviousY = objectNode.GetDouble("py", funyaMapObject.Y);
            funyaMapObject.PreviousVelocityX = objectNode.GetDouble("pvx", funyaMapObject.VelocityX);
            funyaMapObject.PreviousVelocityY = objectNode.GetDouble("pvy", funyaMapObject.VelocityY);
            return funyaMapObject;
        }

        private static MapObjectBase GenerateLineMapObject(YamlMappingNode objectNode, MapReaderOption option, string? name)
        {
            var lineMapObject = new LineMapObject(objectNode.GetDouble("x1", 0.0), objectNode.GetDouble("y1", 0.0), objectNode.GetDouble("x2", 0.0), objectNode.GetDouble("y2", 0.0))
            {
                Name = name
            };
            lineMapObject.Color = objectNode.GetStringOrNull("color");
            lineMapObject.HitUpper = lineMapObject.HitBelow = lineMapObject.HitLeft = lineMapObject.HitRight = objectNode.GetBoolean("hit", false);
            lineMapObject.HitUpper = objectNode.GetBoolean("ht", lineMapObject.HitUpper);
            lineMapObject.HitBelow = objectNode.GetBoolean("hb", lineMapObject.HitBelow);
            lineMapObject.HitLeft = objectNode.GetBoolean("hl", lineMapObject.HitLeft);
            lineMapObject.HitRight = objectNode.GetBoolean("hr", lineMapObject.HitRight);
            lineMapObject.Friction = objectNode.GetDouble("f", lineMapObject.Friction);
            return lineMapObject;
        }

        private static MapObjectBase GenerateTileMapObject(YamlMappingNode objectNode, MapReaderOption option, string? name)
            => new TileGridMapObject(objectNode.GetDouble("x", 0.0), objectNode.GetDouble("y", 0.0), objectNode.GetInt32("w", 1), objectNode.GetInt32("h", 1))
            {
                Name = name
            };

        private static MapObjectBase GenerateRectangleMapObject(YamlMappingNode objectNode, MapReaderOption option, string? name)
        {
            var regionMapObject = new RegionMapObject()
            {
                Name = name,
                Color = objectNode.GetStringOrNull("c"),
            };
            var collider = new RectangleCollider(regionMapObject);
            collider.SetPosition(objectNode.GetDouble("l", 0.0), objectNode.GetDouble("t", 0.0), objectNode.GetDouble("r", 0.0), objectNode.GetDouble("b", 0.0));
            collider.RegionInfo.Gravity = objectNode.GetDouble("g", double.NaN);
            collider.RegionInfo.Wind = objectNode.GetDouble("w", double.NaN);
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Active, true);
            regionMapObject.Collider = collider;
            regionMapObject.Color = objectNode.GetStringOrNull("color");
            return regionMapObject;
        }

        private static MapObjectBase GenerateEllipseMapObject(YamlMappingNode objectNode, MapReaderOption option, string? name)
        {
            var ellipseMapObject = new RegionMapObject()
            {
                Name = name,
                Color = objectNode.GetStringOrNull("c"),
            };
            var collider = new EllipseCollider(ellipseMapObject);
            collider.SetPosition(objectNode.GetDouble("l", 0.0), objectNode.GetDouble("t", 0.0), objectNode.GetDouble("r", 0.0), objectNode.GetDouble("b", 0.0));
            collider.RegionInfo.Gravity = objectNode.GetDouble("g", double.NaN);
            collider.RegionInfo.Wind = objectNode.GetDouble("w", double.NaN);
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Active, true);
            ellipseMapObject.Collider = collider;
            return ellipseMapObject;
        }
    }
}
