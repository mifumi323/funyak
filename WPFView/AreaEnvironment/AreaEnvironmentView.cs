using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MifuminSoft.funyak.View.AreaEnvironment
{
    public class AreaEnvironmentView : IAreaEnvironmentView
    {
        private MapEnvironment.AreaEnvironment areaEnvironment;

        private Canvas canvas = null;
        private Rectangle rectangle = null;
        private bool registered = false;
        private string color;

        public Canvas Canvas
        {
            get
            {
                return canvas;
            }

            set
            {
                if (canvas != null && rectangle != null && registered)
                {
                    canvas.Children.Remove(rectangle);
                }
                canvas = value;
                registered = false;
            }
        }

        public AreaEnvironmentView(MapEnvironment.AreaEnvironment areaEnvironment)
        {
            this.areaEnvironment = areaEnvironment;
        }

        public void Update(AreaEnvironmentViewUpdateArgs args)
        {
            if (!args.Area.IntersectsWith(new Rect(areaEnvironment.Left, areaEnvironment.Top, areaEnvironment.Right - areaEnvironment.Left, areaEnvironment.Bottom - areaEnvironment.Top)) || string.IsNullOrWhiteSpace(areaEnvironment.BackgroundColor))
            {
                RemoveFromCanvas();
                return;
            }
            if (rectangle == null)
            {
                rectangle = new Rectangle();
            }
            if (color != areaEnvironment.BackgroundColor)
            {
                color = areaEnvironment.BackgroundColor;
                try
                {
                    rectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }
                catch
                {
                    rectangle.Fill = null;
                }
            }
            rectangle.Width = (areaEnvironment.Right - areaEnvironment.Left) * args.Scale;
            rectangle.Height = (areaEnvironment.Bottom - areaEnvironment.Top) * args.Scale;
            Canvas.SetLeft(rectangle, args.TranslateX(areaEnvironment.Left));
            Canvas.SetTop(rectangle, args.TranslateY(areaEnvironment.Top));
            AddToCanvas();
        }

        private void AddToCanvas()
        {
            if (registered || rectangle == null || canvas == null) return;
            canvas.Children.Add(rectangle);
            Panel.SetZIndex(rectangle, 1);
            registered = true;
        }

        private void RemoveFromCanvas()
        {
            if (!registered || rectangle == null || canvas == null) return;
            canvas.Children.Remove(rectangle);
            registered = false;
        }
    }
}
