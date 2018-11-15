using System;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 当たり判定を行うマップオブジェクトを表します。
    /// </summary>
    public interface ICollidableMapObject : IMapObject
    {
        /// <summary>
        /// 他のマップオブジェクトとの当たり判定を行います。
        /// このメソッド内では他のマップオブジェクトや自分自身の状態の更新を行いません。
        /// 戻り値にて、当たり判定によって生じる自分自身の状態の変化を反映させるActionを返します。
        /// </summary>
        /// <returns></returns>
        Action CheckCollision(CheckMapObjectCollisionArgs args);
    }
}
