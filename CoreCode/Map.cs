using System;
using System.Collections.Generic;
using System.Linq;
using MifuminSoft.funyak.Core.MapEnvironment;
using MifuminSoft.funyak.Core.MapObject;

namespace MifuminSoft.funyak.Core
{
    /// <summary>
    /// ゲームのマップ
    /// </summary>
    public class Map : IMapEnvironment
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
        /// 重力を設定します。
        /// </summary>
        public double Gravity { get; set; }

        /// <summary>
        /// 風速を設定します。
        /// </summary>
        public double Wind { get; set; }

        /// <summary>
        /// 背景色(未指定または無効な色の場合透明)
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// マップオブジェクトが追加されたときに発生します。
        /// </summary>
        public event EventHandler<MapObjectEventArgs> MapObjectAdded;

        private ICollection<IMapObject> mapObjectCollection;
        private ICollection<IDynamicMapObject> dynamicMapObjectCollection;

        private ICollection<AreaEnvironment> areaEnvironmentCollection;

        /// <summary>
        /// ゲームのマップを初期化します。
        /// </summary>
        /// <param name="width">マップの幅(マップ空間内のピクセル単位)</param>
        /// <param name="height">マップの高さ(マップ空間内のピクセル単位)</param>
        public Map(double width, double height)
        {
            Width = width;
            Height = height;
            Gravity = 1.0;
            Wind = 0.0;
            BackgroundColor = null;

            mapObjectCollection = new List<IMapObject>();
            dynamicMapObjectCollection = new List<IDynamicMapObject>();
            areaEnvironmentCollection = new List<AreaEnvironment>();
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

            MapObjectAdded?.Invoke(this, new MapObjectEventArgs(mapObject));
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
            var args = new UpdateMapObjectArgs(this);
            foreach (var mapObject in dynamicMapObjectCollection)
            {
                mapObject.UpdateSelf(args);
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

        /// <summary>
        /// 重力を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>重力(0.0が無重力、1.0が通常)</returns>
        public double GetGravity(double x, double y)
        {
            return GetEnvironment(x, y).Gravity;
        }

        /// <summary>
        /// 風速を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>風速(0.0：無風、正の数：右向きの風、負の数：左向きの風)</returns>
        public double GetWind(double x, double y)
        {
            return GetEnvironment(x, y).Wind;
        }

        /// <summary>
        /// 指定した位置の環境情報を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>環境</returns>
        public IMapEnvironment GetEnvironment(double x, double y)
        {
            return (IMapEnvironment)areaEnvironmentCollection.LastOrDefault(me => me.Left <= x && x < me.Right && me.Top <= y && y < me.Bottom) ?? this;
        }
    }
}
