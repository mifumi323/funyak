using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    class FunyaMapObjectView : IMapObjectView
    {
        public FunyaMapObject MapObject { get; protected set; }
        public Sprite ImageResource { get; set; }

        private Canvas canvas = null;
        private Rectangle rectangle = null;
        private bool registered = false;

        public Canvas Canvas
        {
            get => canvas;
            set
            {
                if (canvas != null && rectangle != null && registered)
                {
                    canvas.Children.Remove(rectangle);
                }
                canvas = value;
                registered = false;
            }
        }

        public FunyaMapObjectView(FunyaMapObject mapObject) => MapObject = mapObject;

        public void Update(MapObjectViewUpdateArgs args)
        {
            if (ImageResource == null) return;
            if (rectangle == null)
            {
                rectangle = new Rectangle();
            }
            AddToCanvas();
            var imageKey = MapObject.State switch
            {
                FunyaMapObjectState.Stand => "Stand", // TODO: まばたきと睡眠と笑顔にも対応させよう
                FunyaMapObjectState.Run => "Run",
                FunyaMapObjectState.Walk => "Sit",
                FunyaMapObjectState.Charge => "Sit",
                FunyaMapObjectState.Jump => "Jump",
                FunyaMapObjectState.Fall => "Fall",
                FunyaMapObjectState.Float => "Fall",
                FunyaMapObjectState.BreatheIn => "BreatheIn1", // TODO: 段階で絵を変えよう
                FunyaMapObjectState.BreatheOut => "BreatheOut",
                FunyaMapObjectState.Tired => "Tired",
                FunyaMapObjectState.Frozen => "Freeze",
                FunyaMapObjectState.Damaged => "Die",
                FunyaMapObjectState.Die => "Die",
                _ => throw new Exception("FunyaMapObjectのStateがおかしいぞ。"),
            };
            imageKey +=
                MapObject.Direction == Direction.Left ? ".L" :
                MapObject.Direction == Direction.Right ? ".R" :
                ".F";
            ImageResource.SetToRectangle(
                rectangle,
                imageKey,
                MapObject.StateCounter - 1,
                args.TranslateX(MapObject.X),
                args.TranslateY(MapObject.Y),
                args.Scale * MapObject.Size / 28.0,
                MapObject.Angle);
        }

        private void AddToCanvas()
        {
            if (registered || rectangle == null || canvas == null) return;
            canvas.Children.Add(rectangle);
            Panel.SetZIndex(rectangle, 0);
            registered = true;
        }

        private void RemoveFromCanvas()
        {
            if (!registered || rectangle == null || canvas == null) return;
            canvas.Children.Remove(rectangle);
            registered = false;
        }
    }
}
