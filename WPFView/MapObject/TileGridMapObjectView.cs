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

    public class TileGridMapObjectView : IMapObjectView
    {
        public TileGridMapObject MapObject { get; protected set; }

        private Canvas canvas = null;
        private readonly Rectangle[] rectangles = null;

        public Canvas Canvas
        {
            get => canvas;
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

        public TileGridMapObjectView(TileGridMapObject mapObject)
        {
            MapObject = mapObject;
            rectangles = new Rectangle[mapObject.TileCountX * mapObject.TileCountY];
        }

        public void Update(MapObjectViewUpdateArgs args)
        {
            var startX = Math.Max(MapObject.ToTileIndexX(args.Area.Left), 0);
            var endX = Math.Min(MapObject.ToTileIndexX(args.Area.Right), MapObject.TileCountX - 1);
            var startY = Math.Max(MapObject.ToTileIndexY(args.Area.Top), 0);
            var endY = Math.Min(MapObject.ToTileIndexY(args.Area.Bottom), MapObject.TileCountY - 1);
            var i = 0;
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    var chip = MapObject[x, y];
                    if (chip != null)
                    {
                        if (chip.Resource is TileChipResource resource)
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
