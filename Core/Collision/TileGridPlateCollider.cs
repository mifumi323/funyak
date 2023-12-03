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

            // 斜めは真面目に計算する
            return IsCollidedDiagonal(needleCollider, out collision);
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

        private bool IsCollidedDiagonal(NeedleCollider needleCollider, out PlateNeedleCollision collision)
        {
            // 参考：http://marupeke296.com/COL_3D_No23_intcoord.html
            // 参考記事に合わせるため、計算中はタイルが整数区画になるようにタイル座標系を使う

            // レイの始点(タイル座標)
            var sp = tileGridMapObject.ToTilePosition(needleCollider.StartPoint);

            // 初期インデックス
            var initX = sp.X < 0.0 ? (int)sp.X - 1 : (int)sp.X;
            var initY = sp.Y < 0.0 ? (int)sp.Y - 1 : (int)sp.Y;

            // レイベクトル算出
            var v = tileGridMapObject.ToTileVector(needleCollider.DirectedLength);

            // 調整したレイの始点
            var s = sp;

            // レイの方向ベクトルVの成分の符号をプラス化
            int signX = 1, signY = 1;
            int adjX = 0, adjY = 0;
            PlateAttributeFlag vFlag = PlateAttributeFlag.HitUpper, hFlag = PlateAttributeFlag.HitLeft;

            if (v.X < 0.0)
            {
                s.X *= -1.0;
                v.X *= -1;
                signX = -1;
                adjX = 1;
                hFlag = PlateAttributeFlag.HitRight;
            }
            if (v.Y < 0.0)
            {
                s.Y *= -1.0;
                v.Y *= -1;
                signY = -1;
                adjY = 1;
                vFlag = PlateAttributeFlag.HitBelow;
            }

            // 始点を含む領域が初期領域
            var cx = signX * (initX + adjX);
            var cy = signY * (initY + adjY);

            // レイが飛んでない場合はこのメソッドに来ない
            //if (v.X == 0.0 && v.Y == 0.0)
            //    return true; // レイが飛んでいないので終了

            // 衝突探索
            while (true)
            {
                // レイが斜めの場合に来るので、XもYも非0保証
                //var ax = v.X != 0.0 ? (cx + 1 - s.X) / v.X : double.MaxValue;
                //var ay = v.Y != 0.0 ? (cy + 1 - s.Y) / v.Y : double.MaxValue;
                var ax = (cx + 1 - s.X) / v.X;
                var ay = (cy + 1 - s.Y) / v.Y;

                if (ax > 1.0 && ay > 1.0)
                    break; // レイを越えたのでおしまい

                if (ax < ay)
                {
                    cx += 1; // X-Side
                }
                else if (ay < ax)
                {
                    cy += 1; // Y-Side
                }
                else
                {
                    cx += 1;
                    cy += 1; // 角
                }

                // 見つけた区画
                var newX = signX * (cx + adjX);
                var newY = signY * (cy + adjY);

                if (ax == ay)
                {
                    // 角の場合の特殊対応
                    // TODO: 複数衝突している場合は合成する(#71)
                    throw new NotImplementedException();
                }
                else
                {
                    var tile = tileGridMapObject.GetTileOrNull(newX, newY);
                    if (tile != null)
                    {
                        if (ax < ay)
                        {
                            // X-Side
                            if ((tile.PlateInfo.Flags & hFlag) == hFlag) // REVIEW: needle側の当たり判定も見る必要があると思う
                            {
                                var crossPoint = needleCollider.StartPoint + needleCollider.DirectedLength * ax;
                                var plateSegment = tileGridMapObject.GetBoundSegment(newX, newY, hFlag);
                                collision = new PlateNeedleCollision(this, needleCollider, crossPoint, tile.PlateInfo, plateSegment);
                                return true;
                            }
                        }
                        else
                        {
                            // Y-Side
                            if ((tile.PlateInfo.Flags & vFlag) == vFlag) // REVIEW: needle側の当たり判定も見る必要があると思う
                            {
                                var crossPoint = needleCollider.StartPoint + needleCollider.DirectedLength * ay;
                                var plateSegment = tileGridMapObject.GetBoundSegment(newX, newY, vFlag);
                                collision = new PlateNeedleCollision(this, needleCollider, crossPoint, tile.PlateInfo, plateSegment);
                                return true;
                            }
                        }
                    }
                }
            }

            collision = default;
            return false;
        }
    }
}
