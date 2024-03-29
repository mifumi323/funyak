﻿using System.Collections.Generic;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak
{
    /// <summary>
    /// マップオブジェクトの当たり判定の引数を格納します。
    /// </summary>
    public class CheckMapObjectCollisionArgs
    {
        private readonly Map map;

        public CheckMapObjectCollisionArgs(Map map) => this.map = map;

        /// <summary>
        /// 登録されているマップオブジェクトを取得します。
        /// </summary>
        /// <param name="bounds">
        /// マップオブジェクトの存在範囲
        /// この範囲に少なくとも一部が含まれるマップオブジェクトが返される
        /// </param>
        /// <returns>マップオブジェクトの集合</returns>
        public IEnumerable<MapObjectBase> GetMapObjects(IBounds bounds) => map.GetMapObjects(bounds);
    }
}
