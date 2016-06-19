using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MifuminSoft.funyak.View.Resource
{
    public abstract class Sprite
    {
        public Dictionary<string, SpriteChipInfo> Chip { get; set; }
        public Dictionary<string, SpriteChipInfo> Metadata { get; set; }

        public void SetToRectangle(Rectangle rectangle, string key, double x, double y, double scale)
        {
            SetToRectangle(rectangle, key, x, y, scale, Transform.Identity);
        }

        public void SetToRectangle(Rectangle rectangle, string key, double x, double y, double scale, double angle)
        {
            SetToRectangle(rectangle, key, x, y, scale, new RotateTransform(angle));
        }

        public abstract Brush GetBrush(string key);
        public abstract void SetToRectangle(Rectangle rectangle, string key, double x, double y, double scale, Transform transform);
    }
}
