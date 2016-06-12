using System;
using System.Collections.Generic;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.Core
{
    public class MapObjectAddedEventArgs : EventArgs
    {
        public IMapObject MapObject { get; private set; }

        public MapObjectAddedEventArgs(IMapObject mapObject)
        {
            MapObject = mapObject;
        }
    }

    public class Map
    {
        public double Width { get; protected set; }
        public double Height { get; protected set; }
        public string BackgroundColor { get; set; }

        public event EventHandler<MapObjectAddedEventArgs> MapObjectAdded;

        private ICollection<IMapObject> mapObjectCollection;
        private ICollection<IDynamicMapObject> dynamicMapObjectCollection;

        public Map(double width, double height)
        {
            Width = width;
            Height = height;
            BackgroundColor = null;

            mapObjectCollection = new List<IMapObject>();
            dynamicMapObjectCollection = new List<IDynamicMapObject>();
        }

        public void AddMapObject(IMapObject mapObject)
        {
            mapObjectCollection.Add(mapObject);
            var dynamicMapObject = mapObject as IDynamicMapObject;
            if (dynamicMapObject != null) dynamicMapObjectCollection.Add(dynamicMapObject);

            MapObjectAdded?.Invoke(this, new MapObjectAddedEventArgs(mapObject));
        }

        public void Update()
        {
            UpdateMapObjects();
        }

        private void UpdateMapObjects()
        {
            foreach (var mapObject in dynamicMapObjectCollection)
            {
                mapObject.UpdateSelf();
            }
        }

        /// <summary>
        /// 登録されているマップオブジェクトを取得
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
