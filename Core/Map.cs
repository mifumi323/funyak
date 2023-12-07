using System;
using System.Collections.Generic;
using System.Linq;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.Geometry;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak
{
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
        public string? BackgroundColor { get; set; }

        /// <summary>
        /// マップオブジェクトが追加されたときに発生します。
        /// </summary>
        public event EventHandler<MapObjectEventArgs>? MapObjectAdded;

        /// <summary>
        /// マップオブジェクトが削除されたときに発生します。
        /// </summary>
        public event EventHandler<MapObjectEventArgs>? MapObjectRemoved;

        private readonly ICollection<MapObjectBase> mapObjectCollection;
        private readonly ICollection<IUpdatableMapObject> selfUpdatableMapObjectCollection;
        private readonly IDictionary<string, MapObjectBase> namedMapObject;
        private readonly ColliderCollection colliderCollection;

        /// <summary>
        /// 経過フレーム数を取得します。
        /// </summary>
        public int FrameCount { get; set; }

        #region EventArgs
        private readonly RealizeCollisionArgs realizeCollisionArgs;
        #endregion

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
            FrameCount = 0;

            mapObjectCollection = new List<MapObjectBase>();
            selfUpdatableMapObjectCollection = new List<IUpdatableMapObject>();
            namedMapObject = new Dictionary<string, MapObjectBase>();
            colliderCollection = new ColliderCollection();

            realizeCollisionArgs = new RealizeCollisionArgs(this);
        }

        /// <summary>
        /// マップオブジェクトを追加します。
        /// </summary>
        /// <param name="mapObject">追加するマップオブジェクト</param>
        public void AddMapObject(MapObjectBase mapObject)
        {
            mapObjectCollection.Add(mapObject);
            if (mapObject is IUpdatableMapObject selfUpdatableMapObject) selfUpdatableMapObjectCollection.Add(selfUpdatableMapObject);
            if (!string.IsNullOrEmpty(mapObject.Name)) namedMapObject[mapObject.Name!] = mapObject;
            mapObject.OnJoin(colliderCollection);

            MapObjectAdded?.Invoke(this, new MapObjectEventArgs(mapObject));
        }

        /// <summary>
        /// マップオブジェクトを削除します。
        /// </summary>
        /// <param name="mapObject">削除するマップオブジェクト</param>
        public void RemoveMapObject(MapObjectBase mapObject)
        {
            mapObjectCollection.Remove(mapObject);
            if (mapObject is IUpdatableMapObject selfUpdatableMapObject) selfUpdatableMapObjectCollection.Remove(selfUpdatableMapObject);
            if (!string.IsNullOrEmpty(mapObject.Name)) namedMapObject.Remove(mapObject.Name!);
            mapObject.OnLeave(colliderCollection);

            MapObjectRemoved?.Invoke(this, new MapObjectEventArgs(mapObject));
        }

        /// <summary>
        /// マップの状態を更新します。
        /// </summary>
        public void Update()
        {
            UpdateMapObjects();
            CheckMapObjectsCollision();
            colliderCollection.Collide();
            RealizeMapObjectsCollision();
            FrameCount++;
        }

        /// <summary>
        /// マップオブジェクトの状態を更新します。
        /// </summary>
        private void UpdateMapObjects()
        {
            var args = new UpdateMapObjectArgs(this);
            foreach (var mapObject in selfUpdatableMapObjectCollection)
            {
                mapObject.UpdateSelf(args);
            }
        }

        private void CheckMapObjectsCollision()
        {
            var args = new CheckMapObjectCollisionArgs(this);
            foreach (var mapObject in mapObjectCollection)
            {
                mapObject.CheckCollision(args);
            }
        }

        private void RealizeMapObjectsCollision()
        {
            foreach (var mapObject in mapObjectCollection)
            {
                mapObject.RealizeCollision(realizeCollisionArgs);
            }
        }

        /// <summary>
        /// 登録されているマップオブジェクトを取得します。
        /// </summary>
        /// <param name="bounds">
        /// マップオブジェクトの存在範囲。
        /// この範囲に少なくとも一部が含まれるマップオブジェクトが返される。
        /// </param>
        /// <returns>マップオブジェクトの集合</returns>
        public IEnumerable<MapObjectBase> GetMapObjects(IBounds bounds)
        {
            // TODO: 範囲内のマップオブジェクトだけ返す
            return mapObjectCollection;
        }

        /// <summary>
        /// 登録されているマップオブジェクトを取得します。
        /// </summary>
        /// <returns>マップオブジェクトの集合</returns>
        public IEnumerable<MapObjectBase> EnumerateAllMapObjects()
        {
            return mapObjectCollection;
        }

        /// <summary>
        /// 名前でマップオブジェクトを検索します。
        /// </summary>
        /// <param name="name">マップオブジェクトの名前</param>
        /// <returns>マップオブジェクト。なければ null。</returns>
        public MapObjectBase? FindMapObject(string name)
        {
            return namedMapObject.TryGetValue(name, out MapObjectBase mapObject) ? mapObject : null;
        }
    }
}
