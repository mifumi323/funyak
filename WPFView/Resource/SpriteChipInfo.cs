using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.View.Resource
{
    /// <summary>
    /// 画像チップ情報
    /// </summary>
    public class SpriteChipInfo
    {
        /// <summary>
        /// 元画像から切り抜く範囲の左端のX座標を取得または設定します。
        /// 画像全体の左上を原点とします。
        /// </summary>
        [JsonProperty("sl")]
        [DefaultValue(0.0)]
        public double SourceLeft { get; set; }
        /// <summary>
        /// 元画像から切り抜く範囲の上端のY座標を取得または設定します。
        /// 画像全体の左上を原点とします。
        /// </summary>
        [JsonProperty("st")]
        [DefaultValue(0.0)]
        public double SourceTop { get; set; }
        /// <summary>
        /// 元画像から切り抜く範囲の幅を取得または設定します。
        /// </summary>
        [JsonProperty("sw")]
        [DefaultValue(0.0)]
        public double SourceWidth { get; set; }
        /// <summary>
        /// 元画像から切り抜く範囲の高さを取得または設定します。
        /// </summary>
        [JsonProperty("sh")]
        [DefaultValue(0.0)]
        public double SourceHeight { get; set; }

        /// <summary>
        /// 表示の中心となる点の元画像におけるX座標を取得または設定します。
        /// 画像全体の左上を原点とします。
        /// NaNの場合、切り抜き範囲の中央が使用されます。
        /// </summary>
        [JsonProperty("ox")]
        [DefaultValue(double.NaN)]
        public double SourceOriginX { get; set; } = double.NaN;
        /// <summary>
        /// 表示の中心となる点の元画像におけるY座標を取得または設定します。
        /// 画像全体の左上を原点とします。
        /// NaNの場合、切り抜き範囲の中央が使用されます。
        /// </summary>
        [JsonProperty("oy")]
        [DefaultValue(double.NaN)]
        public double SourceOriginY { get; set; } = double.NaN;

        /// <summary>
        /// 表示先における画像チップの幅を取得または設定します。
        /// NaNの場合、切り抜き範囲の幅が使用されます。
        /// </summary>
        [JsonProperty("dw")]
        [DefaultValue(double.NaN)]
        public double DestinationWidth { get; set; } = double.NaN;
        /// <summary>
        /// 表示先における画像チップの高さを取得または設定します。
        /// NaNの場合、切り抜き範囲の高さが使用されます。
        /// </summary>
        [JsonProperty("dh")]
        [DefaultValue(double.NaN)]
        public double DestinationHeight { get; set; } = double.NaN;

        /// <summary>
        /// 表示の中心となる点の元画像におけるX座標の実際の値を取得します。
        /// 画像全体の左上を原点とします。
        /// </summary>
        [JsonIgnore]
        public double ActualSourceOriginX => double.IsNaN(SourceOriginX) ? (SourceLeft + SourceWidth / 2) : SourceOriginX;
        /// <summary>
        /// 表示の中心となる点の元画像におけるY座標の実際の値を取得します。
        /// 画像全体の左上を原点とします。
        /// </summary>
        [JsonIgnore]
        public double ActualSourceOriginY => double.IsNaN(SourceOriginY) ? (SourceTop + SourceHeight / 2) : SourceOriginY;

        /// <summary>
        /// 表示先における画像チップの幅の実際の値を取得します。
        /// </summary>
        [JsonIgnore]
        public double ActualDestinationWidth => double.IsNaN(DestinationWidth) ? SourceWidth : DestinationWidth;
        /// <summary>
        /// 表示先における画像チップの高さの実際の値を取得します。
        /// </summary>
        [JsonIgnore]
        public double ActualDestinationHeight => double.IsNaN(DestinationHeight) ? SourceHeight : DestinationHeight;

        /// <summary>
        /// 表示と直接かかわりのないデータを文字列で保持します。
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
