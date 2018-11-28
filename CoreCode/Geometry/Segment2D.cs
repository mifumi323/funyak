namespace MifuminSoft.funyak.Geometry
{
    /// <summary>
    /// 2次元の始点と終点を持つ線分を表します。
    /// </summary>
    public struct Segment2D
    {
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
    }
}
