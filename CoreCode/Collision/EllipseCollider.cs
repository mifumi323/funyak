using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public class EllipseCollider : RegionCollider
    {
        public EllipseCollider(MapObjectBase owner) : base(owner) { }

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
            //var centerX = (Left + Right) * 0.5;
            //var centerY = (Top + Bottom) * 0.5;
            //var radiusX = (Right - Left) * 0.5;
            //var radiusY = (Bottom - Top) * 0.5;
            //var normalizedPointX = (x - centerX) / radiusX;
            //var normalizedPointY = (y - centerY) / radiusY;
            // 上式をまとめて簡略化
            var normalizedPointX = (x * 2.0 - (Left + Right)) / (Right - Left);
            var normalizedPointY = (y * 2.0 - (Top + Bottom)) / (Bottom - Top);
            var normalizedDistanceSquare = normalizedPointX * normalizedPointX + normalizedPointY * normalizedPointY;

            return normalizedDistanceSquare <= 1;
        }
    }
}
