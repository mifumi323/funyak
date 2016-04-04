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

    public delegate void MapObjectAddedHandler(object sender, MapObjectAddedEventArgs e);

    public class Map
    {
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        public event MapObjectAddedHandler MapObjectAdded;

        private ICollection<IMapObject> mapObjectCollection;
        private ICollection<IDynamicMapObject> dynamicMapObjectCollection;

        public Map(double width, double height)
        {
            Width = width;
            Height = height;

            mapObjectCollection = new List<IMapObject>();
            dynamicMapObjectCollection = new List<IDynamicMapObject>();
        }

        public void AddMapObject(IMapObject mapObject)
        {
            mapObjectCollection.Add(mapObject);
            if (mapObject is IDynamicMapObject) dynamicMapObjectCollection.Add((IDynamicMapObject)mapObject);

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

        public IEnumerable<IMapObject> GetNeighborMapObjects(IBounds bounds)
        {
            if (bounds== null) return mapObjectCollection;
            // TODO: 範囲内のマップオブジェクトだけ返す
            return mapObjectCollection;
        }
    }
}
