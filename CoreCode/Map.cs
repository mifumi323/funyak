using System;
using System.Collections.Generic;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.Core
{
    /// <summary>
    /// マップにマップオブジェクトが追加されたときに通知するイベントの引数を格納します。
    /// </summary>
    public class MapObjectAddedEventArgs : EventArgs
    {
        /// <summary>
        /// 追加されたマップオブジェクト
        /// </summary>
        public IMapObject MapObject { get; private set; }

        public MapObjectAddedEventArgs(IMapObject mapObject)
        {
            MapObject = mapObject;
        }
    }

    /// <summary>
    /// ゲームのマップ
    /// </summary>
    public class Map
    {
        /// <summary>
        /// マップの幅(マップ空間内のピクセル単位)
        /// </summary>
        public double Width { get; protected set; }

        /// <summary>
        /// マップの高さ(マップ空間内のピクセル単位)
        /// </summary>
        public double Height { get; protected set; }

        /// <summary>
        /// 背景色(未指定または無効な色の場合透明)
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// マップオブジェクトが追加されたときに発生します。
        /// </summary>
        public event EventHandler<MapObjectAddedEventArgs> MapObjectAdded;

        private ICollection<IMapObject> mapObjectCollection;
        private ICollection<IDynamicMapObject> dynamicMapObjectCollection;

        /// <summary>
        /// ゲームのマップを初期化します。
        /// </summary>
        /// <param name="width">マップの幅(マップ空間内のピクセル単位)</param>
        /// <param name="height">マップの高さ(マップ空間内のピクセル単位)</param>
        public Map(double width, double height)
        {
            Width = width;
            Height = height;
            BackgroundColor = null;

            mapObjectCollection = new List<IMapObject>();
            dynamicMapObjectCollection = new List<IDynamicMapObject>();
        }

        /// <summary>
        /// マップオブジェクトを追加します。
        /// </summary>
        /// <param name="mapObject">追加するマップオブジェクト</param>
        public void AddMapObject(IMapObject mapObject)
        {
            mapObjectCollection.Add(mapObject);
            var dynamicMapObject = mapObject as IDynamicMapObject;
            if (dynamicMapObject != null) dynamicMapObjectCollection.Add(dynamicMapObject);

            MapObjectAdded?.Invoke(this, new MapObjectAddedEventArgs(mapObject));
        }

        /// <summary>
        /// マップの状態を更新します。
        /// </summary>
        public void Update()
        {
            UpdateMapObjects();
        }

        /// <summary>
        /// マップオブジェクトの状態を更新します。
        /// </summary>
        private void UpdateMapObjects()
        {
            foreach (var mapObject in dynamicMapObjectCollection)
            {
                mapObject.UpdateSelf();
            }
        }

        /// <summary>
        /// 登録されているマップオブジェクトを取得します。
        /// </summary>
        /// <param name="bounds">
        /// マップオブジェクトの存在範囲
        /// この範囲に少なくとも一部が含まれるマップオブジェクトが返される
        /// nullの場合は全マップオブジェクトが返される
        /// </param>
        /// <returns>マップオブジェクトの集合</returns>
        public IEnumerable<IMapObject> GetMapObjects(IBounds bounds = null)
        {
            if (bounds== null) return mapObjectCollection;
            // TODO: 範囲内のマップオブジェクトだけ返す
            return mapObjectCollection;
        }
    }
}
