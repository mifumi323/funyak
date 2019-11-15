using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.View.MapObject
{
    public sealed class RegionMapObjectView : IMapObjectView
    {
        public RegionMapObject MapObject { get; private set; }

        private Canvas canvas = null;
        private Shape shape = null;
        private bool registered = false;
        private string color;

        public Canvas Canvas
        {
            get => canvas;
            set
            {
                if (canvas != null && shape != null && registered)
                {
                    canvas.Children.Remove(shape);
                }
                canvas = value;
                registered = false;
            }
        }

        public RegionMapObjectView(RegionMapObject mapObject) => MapObject = mapObject;

        public void Update(MapObjectViewUpdateArgs args)
        {
            var collider = MapObject.Collider;
            if (collider == null
                || !args.Area.IntersectsWith(new Rect(collider.Left, collider.Top, collider.Right - collider.Left, collider.Bottom - collider.Top))
                || string.IsNullOrWhiteSpace(MapObject.Color))
            {
                RemoveFromCanvas();
                return;
            }
            if (shape == null)
            {
                shape =
                    collider is RectangleCollider ? (Shape)new Rectangle() :
                    collider is EllipseCollider ? new Ellipse() :
                    throw new Exception($"RegionMapObjectViewに渡されたCollideの型「{collider.GetType().Name}」がおかしいぞ");
            }
            if (color != MapObject.Color)
            {
                color = MapObject.Color;
                try
                {
                    shape.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }
                catch
                {
                    shape.Fill = null;
                }
            }
            Canvas.SetLeft(shape, args.TranslateX(collider.Left));
            Canvas.SetTop(shape, args.TranslateY(collider.Top));
            shape.Width = (collider.Right - collider.Left) * args.Scale;
            shape.Height = (collider.Bottom - collider.Top) * args.Scale;
            AddToCanvas();
        }

        private void AddToCanvas()
        {
            if (registered || shape == null || canvas == null) return;
            canvas.Children.Add(shape);
            Panel.SetZIndex(shape, -1);
            registered = true;
        }

        private void RemoveFromCanvas()
        {
            if (!registered || shape == null || canvas == null) return;
            canvas.Children.Remove(shape);
            registered = false;
        }
    }
}
