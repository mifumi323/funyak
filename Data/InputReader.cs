using System;
using MifuminSoft.funyak.Input;
using YamlDotNet.RepresentationModel;

namespace MifuminSoft.funyak.Data
{
    class InputReader
    {
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
