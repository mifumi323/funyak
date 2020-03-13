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
    public class BitmapSprite : Sprite
    {
        readonly BitmapSource bitmapSource = null;
        readonly Dictionary<string, Brush> knownBrush = new Dictionary<string, Brush>();

        public BitmapSprite(Stream bitmapStream)
        {
            using var mi = new MagickImage(bitmapStream);
            bitmapSource = mi.ToBitmapSource();
            bitmapSource.Freeze();
        }

        public override Brush GetBrush(string key, int frame)
        {
            if (Animation.ContainsKey(key))
            {
                var animation = Animation[key];
                return GetStaticBrush(animation[frame % animation.Length]);
            }
            return GetStaticBrush(key);
        }

        public Brush GetStaticBrush(string key)
        {
            if (knownBrush.ContainsKey(key)) return knownBrush[key];
            SpriteChipInfo info;
            try
            {
                info = GetChipInfo(key);
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }
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

        public override void SetToRectangle(Rectangle rectangle, string key, int frame, double x, double y, double scale, Transform transform)
        {
            SpriteChipInfo info;
            try
            {
                info = GetChipInfo(key, frame);
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }
            var sourceOriginX = info.ActualSourceOriginX;
            var sourceOriginY = info.ActualSourceOriginY;
            var destinationWidth = info.ActualDestinationWidth;
            var destinationHeight = info.ActualDestinationHeight;
            rectangle.Fill = GetBrush(key, frame);
            rectangle.Width = destinationWidth * scale;
            rectangle.Height = destinationHeight * scale;
            rectangle.RenderTransformOrigin = new Point(
                (sourceOriginX - info.SourceLeft) / info.SourceWidth,
                (sourceOriginY - info.SourceTop) / info.SourceHeight);
            rectangle.RenderTransform = transform;
            Canvas.SetLeft(rectangle, x - destinationWidth * scale * (sourceOriginX - info.SourceLeft) / info.SourceWidth);
            Canvas.SetTop(rectangle, y - destinationHeight * scale * (sourceOriginY - info.SourceTop) / info.SourceHeight);
        }

        private SpriteChipInfo GetChipInfo(string key, int frame)
        {
            if (Animation.ContainsKey(key))
            {
                var animation = Animation[key];
                return GetChipInfo(animation[frame % animation.Length]);
            }
            return GetChipInfo(key);
        }

        private SpriteChipInfo GetChipInfo(string key)
        {
            if (key == "")
            {
                return new SpriteChipInfo()
                {
                    SourceLeft = 0,
                    SourceTop = 0,
                    SourceWidth = bitmapSource.PixelWidth,
                    SourceHeight = bitmapSource.PixelHeight,
                    SourceOriginX = 0,
                    SourceOriginY = 0,
                    DestinationWidth = bitmapSource.PixelWidth,
                    DestinationHeight = bitmapSource.PixelHeight,
                };
            }
            else
            {
                return Chip[key];
            }
        }
    }
}
