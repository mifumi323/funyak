namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// マップオブジェクトを表します。
    /// </summary>
    public abstract class IMapObject
    {
        /// <summary>
        /// 名前を設定および取得します。MapObject以外が各MapObjectを区別するために用います。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// マップ中のX座標を設定および取得します。MapObjectが自発的に更新しますが、MapObject以外によって特定のタイミングで変更されることもあり得ます。
        /// </summary>
        public abstract double X { get; set; }

        /// <summary>
        /// マップ中のY座標を設定および取得します。MapObjectが自発的に更新しますが、MapObject以外によって特定のタイミングで変更されることもあり得ます。
        /// </summary>
        public abstract double Y { get; set; }
    }
}
