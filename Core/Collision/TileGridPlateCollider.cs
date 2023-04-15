using System;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Collision
{
    public sealed class TileGridPlateCollider : PlateCollider
    {
        private readonly TileGridMapObject tileGridMapObject;
        public TileGridPlateCollider(TileGridMapObject owner) : base(owner)
        {
            tileGridMapObject = owner;
            PlateInfo.Flags = (PlateAttributeFlag)ulong.MaxValue;
            UpdatePosition();
        }

        public void UpdatePosition() => UpdatePosition(tileGridMapObject);

        public override bool IsCollided(NeedleCollider needleCollider, out PlateNeedleCollision collision)
        {
            var startPoint = needleCollider.StartPoint;
            var startTIX = tileGridMapObject.ToTileIndexX(startPoint.X);
            var startTIY = tileGridMapObject.ToTileIndexY(startPoint.Y);
            var endPoint = needleCollider.EndPoint;
            var endTIX = tileGridMapObject.ToTileIndexX(endPoint.X);
            var endTIY = tileGridMapObject.ToTileIndexY(endPoint.Y);

            // 頻出の単純なパターン
            if (startTIX == endTIX && startTIY == endTIY)
            {
                // 境界跨がず
                collision = default;
                return false;
            }
            if (startPoint.X == endPoint.X)
            {
                // まっすぐ上下
                if (startTIY < endTIY)
                {
                    // まっすぐ下
                    return IsCollidedAxisAlignedBelow(needleCollider, out collision, startPoint, startTIX, startTIY, endTIY);
                }
                else
                {
                    // まっすぐ上
                    return IsCollidedAxisAlignedUpper(needleCollider, out collision, startPoint, startTIX, startTIY, endTIY);
                }
            }
            if (startPoint.Y == endPoint.Y)
            {
                // まっすぐ左右
                if (startTIX < endTIX)
                {
                    // まっすぐ右
                    return IsCollidedAxisAlignedRight(needleCollider, out collision, startPoint, startTIY, startTIX, endTIX);
                }
                else
                {
                    // まっすぐ左
                    return IsCollidedAxisAlignedLeft(needleCollider, out collision, startPoint, startTIY, startTIX, endTIX);
                }
            }

            // http://marupeke296.com/COL_3D_No23_intcoord.html を参考に作ろう
            throw new NotImplementedException(); // TODO: 実装( #52 )
        }

        private bool IsCollidedAxisAlignedBelow(NeedleCollider needleCollider, out PlateNeedleCollision collision, Vector2D startPoint, int tix, int startTIY, int endTIY)
        {
            var start = Math.Max(startTIY + 1, 0);
            var end = Math.Min(endTIY, tileGridMapObject.TileCountY - 1);
            for (var tiy = start; tiy <= end; tiy++)
            {
                var tile = tileGridMapObject[tix, tiy];
                if (tile != null && tile.HitUpper)
                {
                    var y = tileGridMapObject.FromTileY(tiy);
                    var segment = new Segment2D(tileGridMapObject.FromTileX(tix), y, tileGridMapObject.FromTileX(tix + 1), y);
                    var crossPoint = new Vector2D(startPoint.X, y);
                    collision = new PlateNeedleCollision(this, needleCollider, crossPoint, tile.PlateInfo, segment);
                    return true;
                }
            }
            collision = default;
            return false;
        }

        private bool IsCollidedAxisAlignedUpper(NeedleCollider needleCollider, out PlateNeedleCollision collision, Vector2D startPoint, int tix, int startTIY, int endTIY)
        {
            var start = Math.Min(startTIY - 1, tileGridMapObject.TileCountY - 1);
            var end = Math.Max(endTIY, 0);
            for (var tiy = start; tiy >= end; tiy--)
            {
                var tile = tileGridMapObject[tix, tiy];
                if (tile != null && tile.HitBelow)
                {
                    var y = tileGridMapObject.FromTileY(tiy + 1);
                    var segment = new Segment2D(tileGridMapObject.FromTileX(tix), y, tileGridMapObject.FromTileX(tix + 1), y);
                    var crossPoint = new Vector2D(startPoint.X, y);
                    collision = new PlateNeedleCollision(this, needleCollider, crossPoint, tile.PlateInfo, segment);
                    return true;
                }
            }
            collision = default;
            return false;
        }

        private bool IsCollidedAxisAlignedRight(NeedleCollider needleCollider, out PlateNeedleCollision collision, Vector2D startPoint, int tiy, int startTIX, int endTIX)
        {
            var start = Math.Max(startTIX + 1, 0);
            var end = Math.Min(endTIX, tileGridMapObject.TileCountX - 1);
            for (var tix = start; tix <= end; tix++)
            {
                var tile = tileGridMapObject[tix, tiy];
                if (tile != null && tile.HitLeft)
                {
                    var x = tileGridMapObject.FromTileX(tix);
                    var segment = new Segment2D(x, tileGridMapObject.FromTileY(tiy), x, tileGridMapObject.FromTileY(tiy + 1));
                    var crossPoint = new Vector2D(x, startPoint.Y);
                    collision = new PlateNeedleCollision(this, needleCollider, crossPoint, tile.PlateInfo, segment);
                    return true;
                }
            }
            collision = default;
            return false;
        }

        private bool IsCollidedAxisAlignedLeft(NeedleCollider needleCollider, out PlateNeedleCollision collision, Vector2D startPoint, int tiy, int startTIX, int endTIX)
        {
            var start = Math.Min(startTIX - 1, tileGridMapObject.TileCountX - 1);
            var end = Math.Max(endTIX, 0);
            for (var tix = start; tix >= end; tix--)
            {
                var tile = tileGridMapObject[tix, tiy];
                if (tile != null && tile.HitRight)
                {
                    var x = tileGridMapObject.FromTileX(tix + 1);
                    var segment = new Segment2D(x, tileGridMapObject.FromTileY(tiy), x, tileGridMapObject.FromTileY(tiy + 1));
                    var crossPoint = new Vector2D(x, startPoint.Y);
                    collision = new PlateNeedleCollision(this, needleCollider, crossPoint, tile.PlateInfo, segment);
                    return true;
                }
            }
            collision = default;
            return false;
        }

        public override void Shift(double dx, double dy)
        {
            // REVIEW: そもそもShiftメソッド自体必要か？( #59 )
            throw new NotImplementedException();
        }
    }
}
