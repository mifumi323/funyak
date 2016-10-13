namespace MifuminSoft.funyak.MapEnvironment
{
    /// <summary>
    /// マップ中の局所的な環境を表します。
    /// </summary>
    public class AreaEnvironment : IMapEnvironment, IBounds
    {
        /// <summary>
        /// 環境の適用範囲の左端を取得または設定します。
        /// </summary>
        public double Left { get; set; } = double.NegativeInfinity;

        /// <summary>
        /// 環境の適用範囲の上端を取得または設定します。
        /// </summary>
        public double Top { get; set; } = double.NegativeInfinity;

        /// <summary>
        /// 環境の適用範囲の右端を取得または設定します。
        /// </summary>
        public double Right { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// 環境の適用範囲の下端を取得または設定します。
        /// </summary>
        public double Bottom { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// 重力を取得または設定します。
        /// </summary>
        public double Gravity { get; set; } = double.NaN;

        /// <summary>
        /// 風速を取得または設定します。
        /// </summary>
        public double Wind { get; set; } = double.NaN;

        /// <summary>
        /// 背景色を取得または設定します。
        /// (未指定または無効な色の場合透明)
        /// </summary>
        public string BackgroundColor { get; set; } = null;
    }
}
