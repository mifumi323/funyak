using System;
using System.Collections.Generic;
using System.Drawing;
using MifuminSoft.funyak.Core;
using MifuminSoft.funyak.View.MapObjectView;

namespace MifuminSoft.funyak.View
{
    public class MapView : IDisposable
    {
        protected Map Map { get; private set; }
        protected ICollection<IMapObjectView> MapObjectViewCollention { get; private set; }
        protected bool MapObjectViewCollentionDirty { get; private set; }
        protected MapObjectViewFactory MapObjectViewFactory { get; private set; }

        public MapView(Map map)
        {
            Map = map;
            Map.MapObjectAdded += Map_MapObjectAdded;
            MapObjectViewCollention = new List<IMapObjectView>();
            MapObjectViewCollentionDirty = false;
            MapObjectViewFactory = new MapObjectViewFactory();
        }

        private void Map_MapObjectAdded(object sender, MapObjectAddedEventArgs e)
        {
            var mapObjectiew = MapObjectViewFactory.Create(e.MapObject);
            if (mapObjectiew != null) AddMapObjectView(mapObjectiew);
        }

        private void AddMapObjectView(IMapObjectView mapObjectiew)
        {
            MapObjectViewCollention.Add(mapObjectiew);
            MapObjectViewCollentionDirty = true;
        }

        public void Display(Graphics graphics)
        {
            graphics.Clear(Color.Black);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
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
