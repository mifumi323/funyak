using System;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public sealed class TileGridPlateCollider : PlateCollider
    {
        private readonly TileGridMapObject tileGridMapObject;
        public TileGridPlateCollider(TileGridMapObject owner) : base(owner) { tileGridMapObject = owner; }

        public override bool IsCollided(NeedleCollider needleCollider, out PlateNeedleCollision collision)
        {
            var startPoint = needleCollider.StartPoint;
            var startTIX = tileGridMapObject.ToTileIndexX(startPoint.X);
            var startTIY = tileGridMapObject.ToTileIndexY(startPoint.Y);
            var endPoint = needleCollider.EndPoint;
            var endTIX = tileGridMapObject.ToTileIndexX(endPoint.X);
            var endTIY = tileGridMapObject.ToTileIndexY(endPoint.Y);
            if (startTIX == endTIX)
            {
                // 上下向き
                if (startTIY == endTIY)
                {
                    // 境界跨がず
                    collision = default;
                    return false;
                }
                else if (startTIY < endTIY)
                {
                    // 下向き
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
                else
                {
                    // 上向き
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
            }
            else if (startTIX < endTIX)
            {
                // 右向き
                if (startTIY == endTIY)
                {
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
                else if (startTIY < endTIY)
                {
                    // 右下向き
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
                else
                {
                    // 右上向き
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
            }
            else
            {
                // 左向き
                if (startTIY == endTIY)
                {
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
                else if (startTIY < endTIY)
                {
                    // 左下向き
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
                else
                {
                    // 左上向き
                    throw new NotImplementedException(); // TODO: 実装( #52 )
                }
            }
        }

        public override void Shift(double dx, double dy)
        {
            // REVIEW: そもそもShiftメソッド自体必要か？( #59 )
            throw new NotImplementedException();
        }
    }
}
