namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 自分自身の状態を他者によらず更新できるマップオブジェクトを表します。
    /// </summary>
    public interface ISelfUpdatable
    {
        /// <summary>
        /// 自分自身の状態を更新します。
        /// この時点では当たり判定を行いません。
        /// </summary>
        void UpdateSelf(UpdateMapObjectArgs args);
    }
}
