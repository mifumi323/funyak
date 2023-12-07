using System;
using System.IO;
using MifuminSoft.funyak.Input;
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
        }

        private static Map FromYamlNode(YamlNode yamlNode, MapReaderOption option)
        {
            if (!(yamlNode is YamlMappingNode mappingNode))
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
            return map;
        }
    }
}
