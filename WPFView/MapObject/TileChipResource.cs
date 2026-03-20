using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.View.MapObject
{
    public class TileChipResource
    {
        public Sprite ImageResource;
        public string Key;

        public TileChipResource() { }

        public TileChipResource(Sprite imageResource, string key)
        {
            ImageResource = imageResource;
            Key = key;
        }
    }
}
