using System;

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

    /// <summary>
    /// 能動的な動きのあるマップオブジェクトを表します。
    /// </summary>
    public interface IDynamicMapObject : IMapObject, ISelfUpdatable
    {
        /// <summary>
        /// 他のマップオブジェクトとの当たり判定を行います。
        /// このメソッド内では他のマップオブジェクトや自分自身の状態の更新を行いません。
        /// 戻り値にて、当たり判定によって生じる自分自身の状態の変化を反映させるActionを返します。
        /// </summary>
        /// <returns></returns>
        Action CheckCollision(CheckMapObjectCollisionArgs args);
    }

    /// <summary>
    /// マップオブジェクトの向きを表します。
    /// </summary>
    public enum Direction
    {
        Front,
        Left,
        Right,
    }
}
