using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MifuminSoft.funyak.Core;
using MifuminSoft.funyak.Core.MapObject;
using MifuminSoft.funyak.View.MapObject;

namespace MifuminSoft.funyak.View
{
    public class MapView
    {
        protected Map Map { get; private set; }
        protected List<IMapObjectView> MapObjectViewCollection { get; private set; }
        protected bool MapObjectViewCollentionDirty { get; private set; }
        protected MapObjectViewFactory MapObjectViewFactory { get; private set; }
        private Canvas canvas = null;
        public Canvas Canvas
        {
            get
            {
                return canvas;
            }
            set
            {
                canvas = value;
                foreach (var mapObjectView in MapObjectViewCollection)
                {
                    mapObjectView.Canvas = canvas;
                }
            }
        }
        private Rectangle background;
        private string backgroundColor;

        /// <summary>
        /// 注視対象のマップオブジェクト
        /// </summary>
        public IMapObject FocusTo { get; set; }

        /// <summary>
        /// 注視点が注視対象の座標からどの程度ずれるか
        /// </summary>
        public Point FocusOffset { get; set; }

        public MapView(Map map, MapObjectViewFactory mapObjectViewFactory = null)
        {
            Map = map;
            Map.MapObjectAdded += Map_MapObjectAdded;
            MapObjectViewCollection = new List<IMapObjectView>();
            MapObjectViewCollentionDirty = false;
            MapObjectViewFactory = mapObjectViewFactory != null ? mapObjectViewFactory : new MapObjectViewFactory();
            foreach (var mapObject in Map.GetMapObjects())
            {
                AddMapObject(mapObject);
            }
        }

        private void Map_MapObjectAdded(object sender, MapObjectAddedEventArgs e)
        {
            AddMapObject(e.MapObject);
        }

        private void AddMapObject(IMapObject mapObject)
        {
            var mapObjectiew = MapObjectViewFactory.Create(mapObject);
            if (mapObjectiew != null) AddMapObjectView(mapObjectiew);
        }

        private void AddMapObjectView(IMapObjectView mapObjectiew)
        {
            MapObjectViewCollection.Add(mapObjectiew);
            MapObjectViewCollentionDirty = true;
        }

        public void Update(double scale)
        {
            if (canvas == null) return;
            if (!(scale > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(scale), scale, nameof(scale) + "には正の数を指定する必要がありますが、" + scale + "が指定されました。");
            }

            // 表示領域を計算
            double focusX = (FocusTo != null ? FocusTo.X : 0) + (FocusOffset != null ? FocusOffset.X : 0);
            double focusY = (FocusTo != null ? FocusTo.Y : 0) + (FocusOffset != null ? FocusOffset.Y : 0);
            double width = Math.Min(canvas.ActualWidth / scale, Map.Width);
            double height = Math.Min(canvas.ActualHeight / scale, Map.Height);
            double actualWidth = width * scale;
            double actualHeight = height * scale;

            var offset = new Point((canvas.ActualWidth - actualWidth) * 0.5, (canvas.ActualHeight - actualHeight) * 0.5);
            var area = new Rect(Math.Min(Math.Max(0, focusX - width * 0.5), Map.Width - width), Math.Min(Math.Max(0, focusY - height * 0.5), Map.Height - height), width, height);
            var actualArea = new Rect(offset.X, offset.Y, actualWidth, actualHeight);

            // 背景の設定
            canvas.Clip = new RectangleGeometry(actualArea);
            if (backgroundColor != Map.BackgroundColor)
            {
                backgroundColor = Map.BackgroundColor;
                if (background == null)
                {
                    background = new Rectangle();
                    canvas.Children.Add(background);
                    Panel.SetZIndex(background, int.MinValue);
                }
                try
                {
                    background.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor));
                }
                catch
                {
                    background.Fill = null;
                }
            }
            if (background != null)
            {
                background.Width = actualWidth;
                background.Height = actualHeight;
                Canvas.SetLeft(background, offset.X);
                Canvas.SetTop(background, offset.Y);
            }

            // マップオブジェクトの状態を更新
            var args = new MapObjectViewUpdateArgs(offset, scale, area);
            foreach (var mapObjectView in MapObjectViewCollection)
            {
                mapObjectView.Update(args);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // マネージ状態を破棄します (マネージ オブジェクト)。
                    Map.MapObjectAdded -= Map_MapObjectAdded;
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~MapView() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
