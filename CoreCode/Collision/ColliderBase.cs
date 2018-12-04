using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class ColliderBase : IBounds
    {
        public double Left { get; private set; }
        public double Top { get; private set; }
        public double Right { get; private set; }
        public double Bottom { get; private set; }

        public IMapObject Owner { get; private set; }

        public ColliderBase(IMapObject owner) => Owner = owner;

        protected void UpdatePosition(double left, double top, double right, double bottom, IColliderUpdatePositionListener listener)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}
