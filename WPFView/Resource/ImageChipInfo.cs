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
        double SourceLeft;
        /// <summary>
        /// 元画像から切り抜く範囲の上端のY座標(画像全体の左上を原点とする)
        /// </summary>
        double SourceTop;
        /// <summary>
        /// 元画像から切り抜く範囲の幅
        /// </summary>
        double SourceWidth;
        /// <summary>
        /// 元画像から切り抜く範囲の高さ
        /// </summary>
        double SourceHeight;
        /// <summary>
        /// 表示の中心となる点の元画像におけるX座標(画像全体の左上を原点とする)
        /// </summary>
        double SourceOriginX;
        /// <summary>
        /// 表示の中心となる点の元画像におけるY座標(画像全体の左上を原点とする)
        /// </summary>
        double SourceOriginY;

        /// <summary>
        /// 表示先における画像チップの幅
        /// </summary>
        double DestinationWidth;
        /// <summary>
        /// 表示先における画像チップの高さ
        /// </summary>
        double DestinationHeight;
    }
}
