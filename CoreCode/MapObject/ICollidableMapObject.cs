using System;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// 当たり判定を行うマップオブジェクトを表します。
    /// </summary>
    public interface ICollidableMapObject
    {
        /// <summary>
        /// 他のマップオブジェクトとの当たり判定を行います。
        /// このメソッド内では他のマップオブジェクトや自分自身の状態の更新を行いません。
        /// RealizeCollisionにて、当たり判定によって生じる自分自身の状態の変化を反映させます。
        /// </summary>
        /// <param name="args"></param>
        void CheckCollision(CheckMapObjectCollisionArgs args);

        /// <summary>
        /// CheckCollisionによって生じた変化を自分自身の状態に反映します。
        /// </summary>
        void RealizeCollision();
    }
}
