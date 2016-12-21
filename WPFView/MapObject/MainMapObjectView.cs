﻿using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    class MainMapObjectView : IMapObjectView
    {
        public MainMapObject MapObject { get; protected set; }
        public Sprite ImageResource { get; set; }

        private Canvas canvas = null;
        private Rectangle rectangle = null;
        private bool registered = false;

        public Canvas Canvas
        {
            get
            {
                return canvas;
            }

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

        public MainMapObjectView(MainMapObject mapObject)
        {
            MapObject = mapObject;
        }

        public void Update(MapObjectViewUpdateArgs args)
        {
            if (ImageResource == null) return;
            if (rectangle == null)
            {
                rectangle = new Rectangle();
            }
            AddToCanvas();

            var imageKey = "Fall";
            switch (MapObject.State)
            {
                case MainMapObjectState.Stand:
                    imageKey = "Stand";
                    break;
                case MainMapObjectState.Float:
                    break;
                case MainMapObjectState.Fall:
                    break;
                case MainMapObjectState.Run:
                    imageKey = "Run";
                    break;
                case MainMapObjectState.Charge:
                    imageKey = "Sit";
                    break;
                case MainMapObjectState.Jump:
                    imageKey = "Jump";
                    break;
                default:
                    throw new Exception("MainMapObjectのStateがおかしいぞ。");
            }
            imageKey +=
                MapObject.Direction == Direction.Left ? ".L" :
                MapObject.Direction == Direction.Right ? ".R" :
                ".F";
            ImageResource.SetToRectangle(rectangle, imageKey, MapObject.StateCounter - 1, args.TranslateX(MapObject.X), args.TranslateY(MapObject.Y), args.Scale, MapObject.Angle);
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
