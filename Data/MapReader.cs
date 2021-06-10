using System;
using System.IO;
using MifuminSoft.funyak.Input;
using Newtonsoft.Json;
using YamlDotNet.RepresentationModel;

namespace MifuminSoft.funyak.Data
{
    public class MapReaderOption
    {
        public IInput Input = NullInput.Instance;
    }

    public static class MapReader
    {
        public static Map FromString(string data, MapReaderOption option)
        {
            var input = new StringReader(data);
            var yaml = new YamlStream();
            yaml.Load(input);
            var yamlNode = yaml.Documents[0].RootNode;
            return FromYamlNode(yamlNode, option);

            var deserialized = JsonConvert.DeserializeObject(data);
            if (deserialized == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return FromDynamic(deserialized, option);
        }

        private static Map FromYamlNode(YamlNode yamlNode, MapReaderOption option)
        {
            var mappingNode = yamlNode as YamlMappingNode;
            if (mappingNode is null)
            {
                throw new ArgumentException("mapのデータ構造はマッピングノードである必要があります。", nameof(yamlNode));
            }
            var map = new Map(mappingNode.GetDouble("width", 100.0), mappingNode.GetDouble("height", 100.0))
            {
                BackgroundColor = mappingNode.GetString("color", "Transparent"),
                Gravity = mappingNode.GetDouble("gravity", 1.0),
                Wind = mappingNode.GetDouble("wind", 0.0),
            };
            if (mappingNode.Children.ContainsKey("objects"))
            {
                if (!(mappingNode.Children["objects"] is YamlSequenceNode objectsNode))
                {
                    throw new ArgumentException("objectsのデータ構造はシーケンスノードである必要があります。", nameof(yamlNode));
                }
                foreach (var objectNode in objectsNode)
                {
                    var mapObject = MapObjectReader.FromYamlNode(objectNode, option);
                    if (mapObject != null) map.AddMapObject(mapObject);
                }
            }
            if (mappingNode.Children.ContainsKey("areas"))
            {
                if (!(mappingNode.Children["areas"] is YamlSequenceNode areasNode))
                {
                    throw new ArgumentException("areasのデータ構造はシーケンスノードである必要があります。", nameof(yamlNode));
                }
                foreach (var areaNode in areasNode)
                {
                    var areaEnvironment = AreaEnvironmentReader.FromYamlNode(areaNode, option);
                    if (areaEnvironment != null) map.AddAreaEnvironment(areaEnvironment);
                }
            }
            return map;
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
