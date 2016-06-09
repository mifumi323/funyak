using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using MifuminSoft.funyak.Core.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    class MainMapObjectView : IMapObjectView
    {
        public MainMapObject MapObject { get; protected set; }
        public ImageResource ImageResource { get; set; }

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

        public void Update(Point offset, double scale, Rect area)
        {
            // TODO: 描画
            throw new NotImplementedException();
        }
    }
}
