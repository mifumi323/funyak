using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MifuminSoft.funyak.View.Resource
{
    public abstract class ImageResource : IDisposable
    {
        public bool IsUserResource { get; private set; }

        public Dictionary<string, ImageChipInfo> Chip { get; set; }

        public ImageResource(bool isUserResource)
        {
            IsUserResource = isUserResource;
        }

        public abstract Brush GetBrush(string key);
        public abstract void SetToRectangle(Rectangle rectangle, string key, double x, double y, double scale, double angle);
        public abstract void Dispose();
    }
}
