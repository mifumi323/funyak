using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class EllipseCollider : RegionCollider
    {
        public EllipseCollider(IMapObject owner) : base(owner) { }

        public void SetPosition(double left, double top, double right, double bottom) => UpdatePosition(left, top, right, bottom);

        public override bool Contains(double x, double y)
        {
            // 特殊なケース
            if (!ContainsInAABB(x, y))
            {
                return false;
            }
            if (Left == Right)
            {
                return Top <= y && y <= Bottom;
            }
            if (Top == Bottom)
            {
                return Left <= x && x <= Right;
            }

            // 楕円侵入判定をまじめにやるべきケース
            var centerX = (Left + Right) * 0.5;
            var centerY = (Top + Bottom) * 0.5;
            var radiusX = (Right - Left) * 0.5;
            var radiusY = (Bottom - Top) * 0.5;
            var normalizedPointX = (x - centerX) / radiusX;
            var normalizedPointY = (y - centerY) / radiusY;
            var normalizedDistanceSquare = normalizedPointX * normalizedPointX + normalizedPointY * normalizedPointY;

            return normalizedDistanceSquare <= 1;
        }
    }
}
