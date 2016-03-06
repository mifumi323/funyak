using System;
using System.Collections.Generic;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.Core
{
    public class Map
    {
        public double Width { get; protected set; }
        public double Height { get; protected set; }

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
    }
}
