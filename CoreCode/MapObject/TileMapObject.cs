namespace MifuminSoft.funyak.MapObject
{
    public class TileChip
    {
        public object Resource;
        public bool HitUpper;
        public bool HitBelow;
        public bool HitLeft;
        public bool HitRight;
    }

    public class TileMapObject : IStaticMapObject
    {
        public string Name { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Left { get { return X; } }

        public double Top { get { return Y; } }

        public double Right
        {
            get
            {
                return X + TileWidth * TileCountX;
            }
        }

        public double Bottom
        {
            get
            {
                return Y + TileHeight * TileCountY;
            }
        }

        /// <summary>
        /// タイル一つの幅
        /// </summary>
        public double TileWidth { get; set; }

        /// <summary>
        /// タイル一つの高さ
        /// </summary>
        public double TileHeight { get; set; }

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
    }
}
