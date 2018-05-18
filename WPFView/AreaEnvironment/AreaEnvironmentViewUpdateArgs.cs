using System.Windows;

namespace MifuminSoft.funyak.View.AreaEnvironment
{
    public sealed class AreaEnvironmentViewUpdateArgs
    {
        /// <summary>
        /// 表示領域での位置オフセット量
        /// </summary>
        public Point Offset { get; private set; }

        /// <summary>
        /// 拡大率
        /// </summary>
        public double Scale { get; private set; }

        /// <summary>
        /// マップ領域での表示範囲
        /// </summary>
        public Rect Area { get; private set; }

        public AreaEnvironmentViewUpdateArgs(Point offset, double scale, Rect area)
        {
            Offset = offset;
            Scale = scale;
            Area = area;
        }

        public double TranslateX(double x) => (x - Area.X) * Scale + Offset.X;

        public double TranslateY(double y) => (y - Area.Y) * Scale + Offset.Y;

        public Point Translate(Point point) => new Point(TranslateX(point.X), TranslateY(point.Y));
    }
}