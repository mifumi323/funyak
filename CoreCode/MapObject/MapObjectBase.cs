using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

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
        public string Name { get; set; }

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

        public virtual void OnCollided(RegionCollider myCollider, PointCollider theirCollider) { }
        public virtual void OnCollided(PointCollider myCollider, RegionCollider theirCollider) { }
        public virtual void OnCollided(PlateCollider myCollider, NeedleCollider theirCollider, Vector2D crossPoint) { }
        public virtual void OnCollided(NeedleCollider myCollider, PlateCollider theirCollider, Vector2D crossPoint) { }

        /// <summary>
        /// CheckCollisionによって生じた変化を自分自身の状態に反映します。
        /// </summary>
        public virtual void RealizeCollision() { }
    }
}
