﻿using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// マップオブジェクトを表します。
    /// </summary>
    public abstract class MapObjectBase
    {
        /// <summary>
        /// 名前を設定および取得します。MapObject以外が各MapObjectを区別するために用います。
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// マップ中のX座標を設定および取得します。MapObjectが自発的に更新しますが、MapObject以外によって特定のタイミングで変更されることもあり得ます。
        /// </summary>
        public abstract double X { get; set; }

        /// <summary>
        /// マップ中のY座標を設定および取得します。MapObjectが自発的に更新しますが、MapObject以外によって特定のタイミングで変更されることもあり得ます。
        /// </summary>
        public abstract double Y { get; set; }

        /// <summary>
        /// 他のマップオブジェクトとの当たり判定を行います。
        /// このメソッド内では他のマップオブジェクトや自分自身の状態の更新を行いません。
        /// RealizeCollisionにて、当たり判定によって生じる自分自身の状態の変化を反映させます。
        /// </summary>
        /// <param name="args"></param>
        public virtual void CheckCollision(CheckMapObjectCollisionArgs args) { }

        /// <summary>
        /// CheckCollisionによって生じた変化を自分自身の状態に反映します。
        /// </summary>
        public virtual void RealizeCollision(RealizeCollisionArgs args) { }

        /// <summary>
        /// マップに登録されるとき呼び出されます。
        /// </summary>
        /// <param name="collisionManager"></param>
        public virtual void OnJoin(ColliderCollection colliderCollection) { }

        /// <summary>
        /// マップから登録解除されるときに呼び出されます。
        /// </summary>
        /// <param name="collisionManager"></param>
        public virtual void OnLeave(ColliderCollection colliderCollection) { }
    }
}
