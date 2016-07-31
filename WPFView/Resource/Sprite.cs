using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MifuminSoft.funyak.View.Resource
{
    public abstract class Sprite
    {
        public Dictionary<string, SpriteChipInfo> Chip { get; set; } = new Dictionary<string, SpriteChipInfo>();
        public Dictionary<string, string[]> Animation { get; set; } = new Dictionary<string, string[]>();
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public void SetToRectangle(Rectangle rectangle, string key, int frame, double x, double y, double scale)
        {
            SetToRectangle(rectangle, key, frame, x, y, scale, Transform.Identity);
        }

        public void SetToRectangle(Rectangle rectangle, string key, int frame, double x, double y, double scale, double angle)
        {
            SetToRectangle(rectangle, key, frame, x, y, scale, new RotateTransform(angle));
        }

        public abstract Brush GetBrush(string key, int frame);
        public abstract void SetToRectangle(Rectangle rectangle, string key, int frame, double x, double y, double scale, Transform transform);
    }
}
