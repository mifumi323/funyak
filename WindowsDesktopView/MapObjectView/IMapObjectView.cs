namespace MifuminSoft.funyak.View.MapObjectView
{
    public interface IMapObjectView
    {
        /// <summary>
        /// 表示優先順位
        /// 小さいほど先に描かれ、大きいほど手前に描かれる
        /// </summary>
        int Priority { get; }
    }
}
