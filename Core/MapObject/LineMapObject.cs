using System;

namespace MifuminSoft.funyak.Core.MapObject
{
    public class LineMapObject : IStaticMapObject
    {
        public LineMapObject(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public double X1 { get; protected set; }
        public double Y1 { get; protected set; }
        public double X2 { get; protected set; }
        public double Y2 { get; protected set; }

        public double X { get { return (X1 + X2) / 2; } }
        public double Y { get { return (Y1 + Y2) / 2; } }
        public double Left { get { return Math.Min(X1, X2); } }
        public double Right { get { return Math.Max(X1, X2); } }
        public double Top { get { return Math.Min(Y1, Y2); } }
        public double Bottom { get { return Math.Max(Y1, Y2); } }
    }
}
