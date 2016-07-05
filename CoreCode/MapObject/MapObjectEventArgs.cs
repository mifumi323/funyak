using System;

namespace MifuminSoft.funyak.MapObject
{
    /// <summary>
    /// マップオブジェクトに関するイベントの引数を格納します。
    /// </summary>
    public class MapObjectEventArgs : EventArgs
    {
        /// <summary>
        /// 追加されたマップオブジェクト
        /// </summary>
        public IMapObject MapObject { get; private set; }

        public MapObjectEventArgs(IMapObject mapObject)
        {
            MapObject = mapObject;
        }
    }
}
