namespace MifuminSoft.funyak.Geometry
{
    /// <summary>
    /// 2次元の始点と終点を持つ線分を表します。
    /// </summary>
    public struct Segment2D
    {
        public const double DELTA = 0.0001;

        public Vector2D Start;
        public Vector2D End;

        public Segment2D(Vector2D start, Vector2D end)
        {
            Start = start;
            End = end;
        }

        public Segment2D(double startX, double startY, double endX, double endY)
        {
            Start = new Vector2D(startX, startY);
            End = new Vector2D(endX, endY);
        }

        /// <summary>
        /// 線分と線分の交差判定を行います。
        /// 参考：http://marupeke296.com/COL_2D_No10_SegmentAndSegment.html
        /// </summary>
        /// <param name="segment">判定対象の線分</param>
        /// <param name="delta">誤差の許容範囲</param>
        /// <returns>交差していればtrue</returns>
        public bool IsCrossed(in Segment2D segment, double delta = DELTA)
        {
            var v1 = End - Start;
            var v2 = segment.End - segment.Start;
            var crs_v1_v2 = v1.Cross(v2);

            // 平行
            if (crs_v1_v2 == 0)
            {
                return false;
            }

            var v = segment.Start - Start;
            var crs_v_v1 = v.Cross(v1);
            var crs_v_v2 = v.Cross(v2);

            var t1 = crs_v_v2 / crs_v1_v2;
            var t2 = crs_v_v1 / crs_v1_v2;

            return -delta <= t1 && t1 <= 1 + delta && -delta <= t2 && t2 <= 1 + delta;
        }

        /// <summary>
        /// 線分と線分の交差判定を行い、交点の取得を試みます。
        /// 参考：http://marupeke296.com/COL_2D_No10_SegmentAndSegment.html
        /// </summary>
        /// <param name="segment">判定対象の線分</param>
        /// <param name="crossPoint">交差していた場合に交点の位置ベクトルを格納するVector2Dへの参照</param>
        /// <param name="delta">誤差の許容範囲</param>
        /// <returns>交差していればtrue</returns>
        public bool TryGetCrossPoint(in Segment2D segment, out Vector2D crossPoint, double delta = DELTA)
        {
            var v1 = End - Start;
            var v2 = segment.End - segment.Start;
            var crs_v1_v2 = v1.Cross(v2);

            // 平行
            if (crs_v1_v2 == 0)
            {
                crossPoint = default(Vector2D);
                return false;
            }

            var v = segment.Start - Start;
            var crs_v_v1 = v.Cross(v1);
            var crs_v_v2 = v.Cross(v2);

            var t1 = crs_v_v2 / crs_v1_v2;
            var t2 = crs_v_v1 / crs_v1_v2;

            if (-delta <= t1 && t1 <= 1 + delta && -delta <= t2 && t2 <= 1 + delta)
            {
                crossPoint = Start + v1 * t1;
                return true;
            }
            else
            {
                crossPoint = default(Vector2D);
                return false;
            }
        }
    }
}
