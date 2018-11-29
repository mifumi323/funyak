using System;

namespace MifuminSoft.funyak.Geometry
{
    /// <summary>
    /// 衝突判定の共通処理を行います。
    /// </summary>
    public static class Collision2D
    {
        public const double DELTA = 0.0001;

        /// <summary>
        /// 線分と線分の交差判定を行います。
        /// 参考：http://marupeke296.com/COL_2D_No10_SegmentAndSegment.html
        /// </summary>
        /// <param name="s1">線分1</param>
        /// <param name="s2">線分2</param>
        /// <returns>交差していればtrue</returns>
        [Obsolete("Collision2D.SegmentSegmentは古い形式です。Segment2D.IsCrossedを使用してください。")]
        public static bool SegmentSegment(Segment2D s1, Segment2D s2) => s1.IsCrossed(s2);

        /// <summary>
        /// 線分と線分の交差判定を行います。
        /// 参考：http://marupeke296.com/COL_2D_No10_SegmentAndSegment.html
        /// </summary>
        /// <param name="s1">線分1</param>
        /// <param name="s2">線分2</param>
        /// <param name="crossPoint">交差していた場合に交点の位置ベクトルを格納するVector2Dへの参照</param>
        /// <returns>交差していればtrue</returns>
        [Obsolete("Collision2D.SegmentSegmentは古い形式です。Segment2D.TryGetCrossPointを使用してください。")]
        public static bool SegmentSegment(Segment2D s1, Segment2D s2, ref Vector2D crossPoint) => s1.TryGetCrossPoint(s2, out crossPoint);
    }
}
