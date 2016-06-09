using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ImageMagick;

namespace MifuminSoft.funyak.View.Resource
{
    public class BitmapImageResource : ImageResource
    {
        BitmapSource bitmapSource = null;
        Dictionary<string, Brush> knownBrush = new Dictionary<string, Brush>();

        public BitmapImageResource(Stream bitmapStream, Stream infoStream, bool isUserResource) : base(isUserResource)
        {
            using (var mi = new MagickImage(bitmapStream))
            {
                bitmapSource = mi.ToBitmapSource();
                bitmapSource.Freeze();
            }
            // TODO: infoStreamから画像チップ情報とか読み込めたらいいな
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Brush GetBrush(string key)
        {
            if (knownBrush.ContainsKey(key)) return knownBrush[key];
            if (!Chip.ContainsKey(key)) throw new ArgumentOutOfRangeException(nameof(key));
            var info = Chip[key];
            var brush = new ImageBrush(bitmapSource)
            {
                TileMode = TileMode.None,
                Stretch = Stretch.Fill,
                Viewbox = new Rect(
                    info.SourceLeft / bitmapSource.PixelWidth,
                    info.SourceTop / bitmapSource.PixelHeight,
                    info.SourceWidth / bitmapSource.PixelWidth,
                    info.SourceHeight / bitmapSource.PixelHeight
                ),
            };
            brush.Freeze();
            knownBrush[key] = brush;
            return brush;
        }

        public override void SetToRectangle(Rectangle rectangle, string key, double x, double y, double scale, Transform transform)
        {
            if (!Chip.ContainsKey(key)) throw new ArgumentOutOfRangeException(nameof(key));
            var info = Chip[key];
            rectangle.Fill = GetBrush(key);
            rectangle.Width = info.DestinationWidth * scale;
            rectangle.Height = info.DestinationHeight * scale;
            rectangle.RenderTransformOrigin = new Point(
                (info.SourceOriginX - info.SourceLeft) / info.SourceWidth,
                (info.SourceOriginY - info.SourceTop) / info.SourceHeight);
            rectangle.RenderTransform = transform;
            Canvas.SetLeft(rectangle, x - info.DestinationWidth * scale * (info.SourceOriginX - info.SourceLeft) / info.SourceWidth);
            Canvas.SetTop(rectangle, y - info.DestinationHeight * scale * (info.SourceOriginY - info.SourceTop) / info.SourceHeight);
        }
    }
}
