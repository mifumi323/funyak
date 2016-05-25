using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public IMapObject FocusTo { get; set; }

        public MapView(Map map)
        {
            Map = map;
            Map.MapObjectAdded += Map_MapObjectAdded;
            MapObjectViewCollection = new List<IMapObjectView>();
            MapObjectViewCollentionDirty = false;
            MapObjectViewFactory = new MapObjectViewFactory();
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
            // 表示領域を計算
            // TODO: 本当の表示領域を計算
            var offset = new Point(0, 0);
            var area = new Rect(0, 0, Map.Width, Map.Height);

            // マップオブジェクトの状態を更新
            foreach (var mapObjectView in MapObjectViewCollection)
            {
                mapObjectView.Update(offset, scale, area);
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
