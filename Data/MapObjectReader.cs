using System;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.MapObject;
using YamlDotNet.RepresentationModel;

namespace MifuminSoft.funyak.Data
{
    public class MapObjectReader
    {
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

                CollidedGravity = objectNode.GetDouble("cg", double.NaN),
                CollidedWind = objectNode.GetDouble("cw", double.NaN),

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
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Gravity, !double.IsNaN(collider.RegionInfo.Gravity));
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Wind, !double.IsNaN(collider.RegionInfo.Wind));
            regionMapObject.Collider = collider;
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
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Gravity, !double.IsNaN(collider.RegionInfo.Gravity));
            collider.RegionInfo.SetFlag(RegionAttributeFlag.Wind, !double.IsNaN(collider.RegionInfo.Wind));
            ellipseMapObject.Collider = collider;
            return ellipseMapObject;
        }
    }
}
