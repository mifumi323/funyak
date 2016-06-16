using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    /// <summary>
    /// 画像チップ情報
    /// </summary>
    public class ImageChipInfo
    {
        /// <summary>
        /// 元画像から切り抜く範囲の左端のX座標(画像全体の左上を原点とする)
        /// </summary>
        [JsonProperty("sl")]
        public double SourceLeft { get; set; }
        /// <summary>
        /// 元画像から切り抜く範囲の上端のY座標(画像全体の左上を原点とする)
        /// </summary>
        [JsonProperty("st")]
        public double SourceTop { get; set; }
        /// <summary>
        /// 元画像から切り抜く範囲の幅
        /// </summary>
        [JsonProperty("sw")]
        public double SourceWidth { get; set; }
        /// <summary>
        /// 元画像から切り抜く範囲の高さ
        /// </summary>
        [JsonProperty("sh")]
        public double SourceHeight { get; set; }
        /// <summary>
        /// 表示の中心となる点の元画像におけるX座標(画像全体の左上を原点とする)
        /// </summary>
        [JsonProperty("ox")]
        public double SourceOriginX { get; set; }
        /// <summary>
        /// 表示の中心となる点の元画像におけるY座標(画像全体の左上を原点とする)
        /// </summary>
        [JsonProperty("oy")]
        public double SourceOriginY { get; set; }

        /// <summary>
        /// 表示先における画像チップの幅
        /// </summary>
        [JsonProperty("dw")]
        public double DestinationWidth { get; set; }
        /// <summary>
        /// 表示先における画像チップの高さ
        /// </summary>
        [JsonProperty("dh")]
        public double DestinationHeight { get; set; }
    }
}
