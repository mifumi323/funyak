using System;
using System.Collections.Generic;
using MifuminSoft.funyak.CollisionHelper;

namespace MifuminSoft.funyak.MapObject
{
    public class TileChip
    {
        public object Resource;
        public bool HitUpper;
        public bool HitBelow;
        public bool HitLeft;
        public bool HitRight;
        public double Friction;
    }

    public class TileMapObject : IStaticMapObject, IBounds
    {
        public string Name { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Left { get { return X; } }

        public double Top { get { return Y; } }

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

        private TileChip[,] tiles;

        public TileMapObject(double x, double y, int tileCountX, int tileCountY)
        {
            X = x;
            Y = y;
            TileCountX = tileCountX;
            TileCountY = tileCountY;
            tiles = new TileChip[tileCountX, tileCountY];
        }

        public TileChip this[int x, int y]
        {
            get { return tiles[x, y]; }
            set { tiles[x, y] = value; }
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
    }
}
