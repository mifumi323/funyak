using System.Collections.Generic;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.Core
{
    public class Map
    {
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        private ICollection<IMapObject> mapObjectCollection;

        public Map(double width, double height)
        {
            Width = width;
            Height = height;

            mapObjectCollection = new List<IMapObject>();
        }

        public void AddMapObject(IMapObject mapObject)
        {
            mapObjectCollection.Add(mapObject);
        }

        public void Update()
        {
        }
    }
}
