using System;
using MifuminSoft.funyak.Input;
using YamlDotNet.RepresentationModel;

namespace MifuminSoft.funyak.Data
{
    class InputReader
    {
        public static IInput FromDynamic(dynamic data, MapReaderOption option) => (string)data.type switch
        // TODO: ReplayInputも作ろう
        {
            "arrange" => GenerateArrangeInput(data, option),
            _ => throw new ArgumentException($"不明なdata.type\"{data.type}\"です。")
        };

        private static IInput GenerateArrangeInput(dynamic data, MapReaderOption option) => new ArrangeInput(option.Input)
        {
            HorizontalReverse = data.hr ?? false,
            VerticalReverse = data.vr ?? false,
        };

        public static IInput FromYamlNode(YamlNode inputNode, MapReaderOption option)
        {
            if (!(inputNode is YamlMappingNode mappingNode))
            {
                throw new ArgumentException("inputのデータ構造はマッピングノードである必要があります。", nameof(inputNode));
            }
            var type = mappingNode.GetStringOrNull("type");
            // TODO: ReplayInputも作ろう
            return type switch
            {
                "arrange" => GenerateArrangeInput(mappingNode, option),
                _ => throw new ArgumentException($"不明なarea.type\"{type}\"です。")
            };
        }

        private static IInput GenerateArrangeInput(YamlMappingNode objectNode, MapReaderOption option) => new ArrangeInput(option.Input)
        {
            HorizontalReverse = objectNode.GetBoolean("hr", false),
            VerticalReverse = objectNode.GetBoolean("vr", false),
        };
    }
}
