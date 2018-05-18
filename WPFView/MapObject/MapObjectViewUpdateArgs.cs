using System.Windows;

namespace MifuminSoft.funyak.View.MapObject
{
    public sealed class MapObjectViewUpdateArgs
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

        /// <summary>
        /// 経過フレーム数
        /// </summary>
        public int Frame { get; private set; }

        public MapObjectViewUpdateArgs(Point offset, double scale, Rect area, int frame)
        {
            Offset = offset;
            Scale = scale;
            Area = area;
            Frame = frame;
        }

        public double TranslateX(double x) => (x - Area.X) * Scale + Offset.X;

        public double TranslateY(double y) => (y - Area.Y) * Scale + Offset.Y;

        public Point Translate(Point point) => new Point(TranslateX(point.X), TranslateY(point.Y));
    }
}
