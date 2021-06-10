using YamlDotNet.RepresentationModel;

namespace MifuminSoft.funyak.Data
{
    public static class YamlHelper
    {
        public static bool GetBoolean(this YamlMappingNode mappingNode, YamlNode key, bool defaultValue)
        {
            return mappingNode.Children.ContainsKey(key) &&
                mappingNode.Children[key] is YamlScalarNode valueNode &&
                bool.TryParse(valueNode.Value, out var value) ?
                value : defaultValue;
        }

        public static double GetDouble(this YamlMappingNode mappingNode, YamlNode key, double defaultValue)
        {
            return mappingNode.Children.ContainsKey(key) &&
                mappingNode.Children[key] is YamlScalarNode valueNode &&
                double.TryParse(valueNode.Value, out var value) ?
                value : defaultValue;
        }

        public static int GetInt32(this YamlMappingNode mappingNode, YamlNode key, int defaultValue)
        {
            return mappingNode.Children.ContainsKey(key) &&
                mappingNode.Children[key] is YamlScalarNode valueNode &&
                int.TryParse(valueNode.Value, out var value) ?
                value : defaultValue;
        }

        public static string GetString(this YamlMappingNode mappingNode, YamlNode key, string defaultValue)
        {
            return mappingNode.Children.ContainsKey(key) &&
                mappingNode.Children[key] is YamlScalarNode valueNode  ?
                valueNode.Value ?? defaultValue : defaultValue;
        }

        public static string? GetStringOrNull(this YamlMappingNode mappingNode, YamlNode key)
        {
            return mappingNode.Children.ContainsKey(key) &&
                mappingNode.Children[key] is YamlScalarNode valueNode  ?
                valueNode.Value : null;
        }
    }
}
