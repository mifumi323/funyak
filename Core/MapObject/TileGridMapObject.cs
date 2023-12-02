using System;
using System.Collections.Generic;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;

namespace MifuminSoft.funyak.MapObject
{
    public class TileChip
    {
        public readonly PlateInfo PlateInfo;

        public object? Resource;
        public bool HitUpper
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitUpper);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitUpper, value);
        }
        public bool HitBelow
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitBelow);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitBelow, value);
        }
        public bool HitLeft
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitLeft);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitLeft, value);
        }
        public bool HitRight
        {
            get => PlateInfo.HasFlag(PlateAttributeFlag.HitRight);
            set => PlateInfo.SetFlag(PlateAttributeFlag.HitRight, value);
        }
        public double Friction
        {
            get => PlateInfo.Friction;
            set => PlateInfo.Friction = value;
        }

        public TileChip(PlateInfo? plateInfo = null)
        {
            PlateInfo = plateInfo ?? new PlateInfo();
        }
    }

    public class TileGridMapObject : MapObjectBase, IBounds
    {
        public override double X { get; set; }

        public override double Y { get; set; }

        public double Left => X;

        public double Top => Y;

        public double Right => X + TileWidth * TileCountX;

        public double Bottom => Y + TileHeight * TileCountY;

        /// <summary>
        /// タイル一つの幅
        /// </summary>
        public double TileWidth { get; set; } = DefaultTileWidth;

        public const double DefaultTileWidth = 32.0;

        /// <summary>
        /// タイル一つの高さ
        /// </summary>
        public double TileHeight { get; set; } = DefaultTileHeight;

        public const double DefaultTileHeight = 32.0;

        /// <summary>
        /// X方向のタイル数
        /// </summary>
        public int TileCountX { get; private set; }

        /// <summary>
        /// Y方向のタイル数
        /// </summary>
        public int TileCountY { get; private set; }

        private readonly TileChip?[,] tiles;

        private readonly TileGridPlateCollider plateCollider;

        public TileGridMapObject(double x, double y, int tileCountX, int tileCountY, IEnumerable<TileChip>? sequence = null)
        {
            X = x;
            Y = y;
            TileCountX = tileCountX;
            TileCountY = tileCountY;
            tiles = new TileChip[tileCountX, tileCountY];
            if (sequence != null)
            {
                Fill(sequence);
            }
            plateCollider = new TileGridPlateCollider(this);
        }

        public TileChip? this[int x, int y]
        {
            get => tiles[x, y];
            set => tiles[x, y] = value;
        }

        /// <summary>
        /// X方向のタイル番号を取得
        /// </summary>
        /// <param name="x">マップ座標でのX座標</param>
        /// <returns>X方向のタイル番号</returns>
        public int ToTileIndexX(double x) => (int)Math.Floor((x - Left) / TileWidth);

        /// <summary>
        /// Y方向のタイル番号を取得
        /// </summary>
        /// <param name="y">マップ座標でのY座標</param>
        /// <returns>Y方向のタイル番号</returns>
        public int ToTileIndexY(double y) => (int)Math.Floor((y - Top) / TileHeight);

        /// <summary>マップ座標からタイル座標に変換</summary>
        /// <param name="x">マップ座標でのX座標</param>
        /// <returns>タイル座標</returns>
        public double ToTileX(double x) => (x - Left) / TileWidth;

        /// <summary>マップ座標からタイル座標に変換</summary>
        /// <param name="y">マップ座標でのY座標</param>
        /// <returns>タイル座標</returns>
        public double ToTileY(double y) => (y - Top) / TileHeight;

        /// <summary>位置をマップ座標からタイル座標に変換(始点を考慮)</summary>
        /// <param name="v">マップ座標でのベクトル</param>
        /// <returns>タイル座標</returns>
        public Vector2D ToTilePosition(Vector2D v) => new Vector2D(ToTileX(v.X), ToTileY(v.Y));

        /// <summary>ベクトルをマップ座標からタイル座標に変換(始点無し)</summary>
        /// <param name="v">マップ座標でのベクトル</param>
        /// <returns>タイル座標</returns>
        public Vector2D ToTileVector(Vector2D v) => new Vector2D(v.X / TileWidth, v.Y / TileHeight);

        /// <summary>タイル座標からマップ座標に変換</summary>
        /// <param name="x">タイル座標</param>
        /// <returns>マップ座標</returns>
        public double FromTileX(double x) => Left + TileWidth * x;

        /// <summary>タイル座標からマップ座標に変換</summary>
        /// <param name="y">タイル座標</param>
        /// <returns>マップ座標</returns>
        public double FromTileY(double y) => Top + TileHeight * y;

        /// <summary>タイル座標からマップ座標に変換</summary>
        /// <param name="v">タイル座標でのベクトル</param>
        /// <returns>マップ座標</returns>
        public Vector2D FromTileVector(Vector2D v) => new Vector2D(FromTileX(v.X), FromTileY(v.Y));

        public void AddCollidableSegmentsToList(IList<CollidableSegment> list, double left, double top, double right, double bottom)
        {
            var startX = Math.Max(ToTileIndexX(left), 0);
            var endX = Math.Min(ToTileIndexX(right), TileCountX - 1);
            var startY = Math.Max(ToTileIndexY(top), 0);
            var endY = Math.Min(ToTileIndexY(bottom), TileCountY - 1);
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    var chip = tiles[x, y];
                    if (chip != null)
                    {
                        if (chip.HitUpper)
                        {
                            list.Add(new CollidableSegment()
                            {
                                Segment = new Segment2D(Left + x * TileWidth, Top + y * TileHeight, Left + (x + 1) * TileWidth, Top + y * TileHeight),
                                HitUpper = true,
                                Friction = chip.Friction,
                            });
                        }
                        if (chip.HitBelow)
                        {
                            list.Add(new CollidableSegment()
                            {
                                Segment = new Segment2D(Left + x * TileWidth, Top + (y + 1) * TileHeight, Left + (x + 1) * TileWidth, Top + (y + 1) * TileHeight),
                                HitBelow = true,
                            });
                        }
                        if (chip.HitLeft)
                        {
                            list.Add(new CollidableSegment()
                            {
                                Segment = new Segment2D(Left + x * TileWidth, Top + y * TileHeight, Left + x * TileWidth, Top + (y + 1) * TileHeight),
                                HitLeft = true,
                            });
                        }
                        if (chip.HitRight)
                        {
                            list.Add(new CollidableSegment()
                            {
                                Segment = new Segment2D(Left + (x + 1) * TileWidth, Top + y * TileHeight, Left + (x + 1) * TileWidth, Top + (y + 1) * TileHeight),
                                HitRight = true,
                            });
                        }
                    }
                }
            }

        }

        public override void OnJoin(Map map, ColliderCollection colliderCollection)
        {
            colliderCollection.Add(plateCollider);
        }

        public override void OnLeave(Map map, ColliderCollection colliderCollection)
        {
            colliderCollection.Remove(plateCollider);
        }

        public override void CheckCollision(CheckMapObjectCollisionArgs args)
        {
            base.CheckCollision(args);
            plateCollider.UpdatePosition();
        }

        /// <summary>
        /// チップを左上から充填します。
        /// 全タイル充填するか、シーケンスが終了した時点で重点を終了します。
        /// </summary>
        /// <param name="sequence">チップのシーケンス</param>
        public void Fill(IEnumerable<TileChip> sequence)
        {
            var x = 0;
            var y = 0;
            foreach (var chip in sequence)
            {
                tiles[x, y] = chip;
                x++;
                if (x == TileCountX)
                {
                    x = 0;
                    y++;
                    if (y == TileCountY)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// タイルを取得します。
        /// 範囲外の場合、null を返します。
        /// </summary>
        /// <param name="x">タイル番号X</param>
        /// <param name="y">タイル番号Y</param>
        /// <returns></returns>
        public TileChip? GetTileOrNull(int x, int y)
        {
            if (x < 0 || TileCountX <= x || y < 0 || TileCountY <= y)
            {
                return null;
            }
            return tiles[x, y];
        }

        /// <summary>タイルの境界線を取得します。</summary>
        /// <param name="x">タイル番号X</param>
        /// <param name="y">タイル番号Y</param>
        /// <param name="flag">上下左右のいずれか一つを表す属性</param>
        /// <returns>境界線</returns>
        public Segment2D GetBoundSegment(int x, int y, PlateAttributeFlag flag)
        {
            return flag switch
            {
                PlateAttributeFlag.HitUpper => new Segment2D(FromTileX(x), FromTileY(y), FromTileX(x + 1), FromTileY(y)),
                PlateAttributeFlag.HitBelow => new Segment2D(FromTileX(x), FromTileY(y + 1), FromTileX(x + 1), FromTileY(y + 1)),
                PlateAttributeFlag.HitLeft => new Segment2D(FromTileX(x), FromTileY(y), FromTileX(x), FromTileY(y + 1)),
                PlateAttributeFlag.HitRight => new Segment2D(FromTileX(x + 1), FromTileY(y), FromTileX(x + 1), FromTileY(y + 1)),
                _ => throw new ArgumentException($"{nameof(flag)} は、上下左右のどれか一つである必要があります。", nameof(flag)),
            };
        }
    }
}
