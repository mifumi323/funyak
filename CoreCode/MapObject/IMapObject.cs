namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// マップオブジェクトを表します。
    /// </summary>
    public interface IMapObject
    {
        /// <summary>
        /// 名前
        /// </summary>
        string Name { get; }

        /// <summary>
        /// マップ中のX座標
        /// </summary>
        double X { get; }

        /// <summary>
        /// マップ中のY座標
        /// </summary>
        double Y { get; }
    }
}
