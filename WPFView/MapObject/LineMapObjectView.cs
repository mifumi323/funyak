using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.View.MapObject
{
    public class LineMapObjectView : IMapObjectView
    {
        public LineMapObject MapObject { get; protected set; }

        private Canvas canvas = null;
        private Line line = null;
        private bool registered = false;

        public Canvas Canvas
        {
            get
            {
                return canvas;
            }

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

        public LineMapObjectView(LineMapObject mapObject)
        {
            MapObject = mapObject;
        }

        public void Update(Point offset, double scale, Rect area)
        {
            if (!area.IntersectsWith(new Rect(MapObject.Left, MapObject.Top, MapObject.Right - MapObject.Left, MapObject.Bottom - MapObject.Top)))
            {
                RemoveFromCanvas();
                return;
            }
            if (line == null)
            {
                line = new Line();
            }
            line.X1 = (MapObject.X1 - area.X) * scale + offset.X;
            line.Y1 = (MapObject.Y1 - area.Y) * scale + offset.Y;
            line.X2 = (MapObject.X2 - area.X) * scale + offset.X;
            line.Y2 = (MapObject.Y2 - area.Y) * scale + offset.Y;
            line.StrokeThickness = scale;
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
