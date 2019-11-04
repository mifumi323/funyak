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
        public MapObjectBase MapObject { get; private set; }

        public MapObjectEventArgs(MapObjectBase mapObject) => MapObject = mapObject;
    }
}
