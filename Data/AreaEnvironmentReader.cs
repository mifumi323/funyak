using System;
using MifuminSoft.funyak.MapEnvironment;
using YamlDotNet.RepresentationModel;

namespace MifuminSoft.funyak.Data
{
    public class AreaEnvironmentReader
    {
        public static AreaEnvironment? FromYamlNode(YamlNode areaNode, MapReaderOption option) => areaNode is YamlMappingNode mappingNode ? new AreaEnvironment()
        {
            Name = mappingNode.GetStringOrNull("n"),

            Left = mappingNode.GetDouble("l", double.NegativeInfinity),
            Top = mappingNode.GetDouble("t", double.NegativeInfinity),
            Right = mappingNode.GetDouble("r", double.PositiveInfinity),
            Bottom = mappingNode.GetDouble("b", double.PositiveInfinity),

            Gravity = mappingNode.GetDouble("g", double.NaN),
            Wind = mappingNode.GetDouble("w", double.NaN),

            BackgroundColor = mappingNode.GetStringOrNull("c"),
        } : throw new ArgumentException("areaのデータ構造はマッピングノードである必要があります。", nameof(areaNode));
    }
}
