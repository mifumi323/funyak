using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageMagick;

namespace MifuminSoft.funyak.View.Resource
{
    class BitmapImageResource : ImageResource
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
    }
}
