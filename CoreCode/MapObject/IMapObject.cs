using System;

namespace MifuminSoft.funyak.Core.MapObject
{
    /// <summary>
    /// マップオブジェクトを表します。
    /// </summary>
    public interface IMapObject : IBounds
    {
        /// <summary>
        /// マップ中のX座標
        /// </summary>
        double X { get; }

        /// <summary>
        /// マップ中のY座標
        /// </summary>
        double Y { get; }
    }

    /// <summary>
    /// 能動的な動きのあるマップオブジェクトを表します。
    /// </summary>
    public interface IDynamicMapObject : IMapObject
    {
        /// <summary>
        /// 自分自身の状態を更新します。
        /// この時点では当たり判定を行いません。
        /// </summary>
        void UpdateSelf(UpdateMapObjectArgs args);

        /// <summary>
        /// 他のマップオブジェクトとの当たり判定を行います。
        /// このメソッド内では他のマップオブジェクトや自分自身の状態の更新を行いません。
        /// 戻り値にて、当たり判定によって生じる自分自身の状態の変化を反映させるActionを返します。
        /// </summary>
        /// <returns></returns>
        Action CheckCollision(CheckMapObjectCollisionArgs args);
    }

    /// <summary>
    /// 能動的な動きのないマップオブジェクトを表します。
    /// </summary>
    public interface IStaticMapObject : IMapObject
    {
    }
}
