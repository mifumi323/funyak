namespace MifuminSoft.funyak.Core
{
    /// <summary>
    /// 範囲を表します。
    /// </summary>
    public interface IBounds
    {
        /// <summary>
        /// 左端の座標
        /// </summary>
        double Left { get; }
        /// <summary>
        /// 上端の座標
        /// </summary>
        double Top { get; }
        /// <summary>
        /// 右端の座標
        /// </summary>
        double Right { get; }
        /// <summary>
        /// 下端の座標
        /// </summary>
        double Bottom { get; }
    }
}
