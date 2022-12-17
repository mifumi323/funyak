using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.View.MapObject
{
    public class LineMapObjectView : IMapObjectView
    {
        public SegmentMapObject MapObject { get; protected set; }

        private Canvas canvas = null;
        private Line line = null;
        private bool registered = false;
        private string color;

        public Canvas Canvas
        {
            get => canvas;
            set
            {
                if (canvas != null && line != null && registered)
                {
                    canvas.Children.Remove(line);
                }
                canvas = value;
                registered = false;
            }
        }

        public LineMapObjectView(SegmentMapObject mapObject) => MapObject = mapObject;

        public void Update(MapObjectViewUpdateArgs args)
        {
            if (!args.Area.IntersectsWith(new Rect(MapObject.Left, MapObject.Top, MapObject.Right - MapObject.Left, MapObject.Bottom - MapObject.Top)) || string.IsNullOrWhiteSpace(MapObject.Color))
            {
                RemoveFromCanvas();
                return;
            }
            if (line == null)
            {
                line = new Line();
            }
            if (color != MapObject.Color)
            {
                color = MapObject.Color;
                try
                {
                    line.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }
                catch
                {
                    line.Stroke = null;
                }
            }
            line.X1 = args.TranslateX(MapObject.X1);
            line.Y1 = args.TranslateY(MapObject.Y1);
            line.X2 = args.TranslateX(MapObject.X2);
            line.Y2 = args.TranslateY(MapObject.Y2);
            line.StrokeThickness = args.Scale;
            AddToCanvas();
        }

        private void AddToCanvas()
        {
            if (registered || line == null || canvas == null) return;
            canvas.Children.Add(line);
            Panel.SetZIndex(line, -1);
            registered = true;
        }

        private void RemoveFromCanvas()
        {
            if (!registered || line == null || canvas == null) return;
            canvas.Children.Remove(line);
            registered = false;
        }
    }
}
