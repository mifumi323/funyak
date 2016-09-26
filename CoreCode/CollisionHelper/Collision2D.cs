namespace MifuminSoft.funyak.CollisionHelper
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
        public static bool SegmentSegment(Segment2D s1, Segment2D s2)
        {
            var v1 = s1.End - s1.Start;
            var v2 = s2.End - s2.Start;
            var crs_v1_v2 = v1.Cross(v2);

            // 平行
            if (crs_v1_v2 == 0)
            {
                return false;
            }

            var v = s2.Start - s1.Start;
            var crs_v_v1 = v.Cross(v1);
            var crs_v_v2 = v.Cross(v2);

            var t1 = crs_v_v2 / crs_v1_v2;
            var t2 = crs_v_v1 / crs_v1_v2;

            return -DELTA <= t1 && t1 <= 1 + DELTA && -DELTA <= t2 && t2 <= 1 + DELTA;
        }

        /// <summary>
        /// 線分と線分の交差判定を行います。
        /// 参考：http://marupeke296.com/COL_2D_No10_SegmentAndSegment.html
        /// </summary>
        /// <param name="s1">線分1</param>
        /// <param name="s2">線分2</param>
        /// <returns>交差していれば交差点の位置ベクトル、していなければnull</returns>
        public static Vector2D SegmentSegmentCrossPoint(Segment2D s1, Segment2D s2)
        {
            var v1 = s1.End - s1.Start;
            var v2 = s2.End - s2.Start;
            var crs_v1_v2 = v1.Cross(v2);

            // 平行
            if (crs_v1_v2 == 0)
            {
                return null;
            }

            var v = s2.Start - s1.Start;
            var crs_v_v1 = v.Cross(v1);
            var crs_v_v2 = v.Cross(v2);

            var t1 = crs_v_v2 / crs_v1_v2;
            var t2 = crs_v_v1 / crs_v1_v2;

            return -DELTA <= t1 && t1 <= 1 + DELTA && -DELTA <= t2 && t2 <= 1 + DELTA ?
                s1.Start + v1 * t1:
                null;
        }
    }
}
