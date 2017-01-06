using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    public class TileChipResource
    {
        public Sprite ImageResource;
        public string Key;

        public TileChipResource() { }

        public TileChipResource(Sprite imageResource, string key)
        {
            ImageResource = imageResource;
            Key = key;
        }
    }

    public class TileMapObjectView : IMapObjectView
    {
        public TileMapObject MapObject { get; protected set; }

        private Canvas canvas = null;
        private Rectangle[] rectangles = null;

        public Canvas Canvas
        {
            get
            {
                return canvas;
            }

            set
            {
                if (canvas != null)
                {
                    for (var i = 0; i < rectangles.Length; i++)
                    {
                        var rect = rectangles[i];
                        if (rect == null) continue;
                        canvas.Children.Remove(rect);
                        rectangles[i] = null;
                    }
                }
                canvas = value;
            }
        }

        public TileMapObjectView(TileMapObject mapObject)
        {
            MapObject = mapObject;
            rectangles = new Rectangle[mapObject.TileCountX * mapObject.TileCountY];
        }

        public void Update(MapObjectViewUpdateArgs args)
        {
            var startX = Math.Max((int)((args.Area.Left - MapObject.Left) / MapObject.TileWidth), 0);
            var endX = Math.Min((int)((args.Area.Right - MapObject.Left) / MapObject.TileWidth), MapObject.TileCountX - 1);
            var startY = Math.Max((int)((args.Area.Top - MapObject.Top) / MapObject.TileHeight), 0);
            var endY = Math.Min((int)((args.Area.Bottom - MapObject.Top) / MapObject.TileHeight), MapObject.TileCountY - 1);
            var i = 0;
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    var chip = MapObject[x, y];
                    if (chip != null)
                    {
                        var resource = chip.Resource as TileChipResource;
                        if (resource != null)
                        {
                            var rect = rectangles[i];
                            if (rect == null)
                            {
                                rect = rectangles[i] = new Rectangle();
                                canvas.Children.Add(rect);
                                Panel.SetZIndex(rect, -2);
                            }
                            resource.ImageResource.SetToRectangle(
                                rect,
                                resource.Key,
                                args.Frame - 1,
                                args.TranslateX(MapObject.X + (x + 0.5) * MapObject.TileWidth),
                                args.TranslateY(MapObject.Y + (y + 0.5) * MapObject.TileHeight),
                                args.Scale);
                            i++;
                        }
                    }
                }
            }

            // 再利用されなかったRectangleを破棄
            for (int r = i; r < rectangles.Length; r++)
            {
                var rect = rectangles[r];
                if (rect != null)
                {
                    rectangles[r] = null;
                    canvas.Children.Remove(rect);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
