using System;
using System.Collections.Generic;
using System.Drawing;
using MifuminSoft.funyak.Core;
using MifuminSoft.funyak.Core.MapObject;
using MifuminSoft.funyak.View.MapObjectView;

namespace MifuminSoft.funyak.View
{
    public class MapView : IDisposable
    {
        protected Map Map { get; private set; }
        protected List<IMapObjectView> MapObjectViewCollention { get; private set; }
        protected bool MapObjectViewCollentionDirty { get; private set; }
        protected MapObjectViewFactory MapObjectViewFactory { get; private set; }

        public MapView(Map map)
        {
            Map = map;
            Map.MapObjectAdded += Map_MapObjectAdded;
            MapObjectViewCollention = new List<IMapObjectView>();
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
            MapObjectViewCollention.Add(mapObjectiew);
            MapObjectViewCollentionDirty = true;
        }

        private void CleanMapObjectViewCollention()
        {
            if (!MapObjectViewCollentionDirty) return;
            MapObjectViewCollention.Sort(CompareMapObjectView);
            MapObjectViewCollentionDirty = false;
        }

        private int CompareMapObjectView(IMapObjectView a, IMapObjectView b)
        {
            return a.Priority - b.Priority;
        }

        public void Display(Graphics graphics)
        {
            graphics.Clear(Color.Black);
            CleanMapObjectViewCollention();
            foreach (var mapObjectView in MapObjectViewCollention)
            {
                mapObjectView.Display(graphics);
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
