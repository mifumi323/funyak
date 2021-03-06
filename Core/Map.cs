﻿using System;
using System.Collections.Generic;
using System.Linq;
using MifuminSoft.funyak.Collision;
using MifuminSoft.funyak.MapEnvironment;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak
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
        /// 環境が追加されたときに発生します。
        /// </summary>
        public event EventHandler<AreaEnvironmentEventArgs>? AreaEnvironmentAdded;

        private readonly ICollection<AreaEnvironment> areaEnvironmentCollection;
        private readonly IDictionary<string, AreaEnvironment> namedAreaEnvironment;

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
            areaEnvironmentCollection = new List<AreaEnvironment>();
            namedAreaEnvironment = new Dictionary<string, AreaEnvironment>();
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
            mapObject.OnJoin(this, colliderCollection);

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
            mapObject.OnLeave(this, colliderCollection);

            MapObjectRemoved?.Invoke(this, new MapObjectEventArgs(mapObject));
        }

        /// <summary>
        /// 環境を追加します。
        /// </summary>
        /// <param name="mapObject">追加するマップオブジェクト</param>
        public void AddAreaEnvironment(AreaEnvironment areaEnvironment)
        {
            areaEnvironmentCollection.Add(areaEnvironment);
            if (!string.IsNullOrEmpty(areaEnvironment.Name)) namedAreaEnvironment[areaEnvironment.Name!] = areaEnvironment;
            AreaEnvironmentAdded?.Invoke(this, new AreaEnvironmentEventArgs(areaEnvironment));
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
        /// マップオブジェクトの存在範囲
        /// この範囲に少なくとも一部が含まれるマップオブジェクトが返される
        /// nullの場合は全マップオブジェクトが返される
        /// </param>
        /// <returns>マップオブジェクトの集合</returns>
        public IEnumerable<MapObjectBase> GetMapObjects(IBounds? bounds = null)
        {
            if (bounds == null) return mapObjectCollection;
            // TODO: 範囲内のマップオブジェクトだけ返す
            return mapObjectCollection;
        }

        /// <summary>
        /// 名前でマップオブジェクトを検索します。
        /// </summary>
        /// <param name="name">マップオブジェクトの名前</param>
        /// <returns></returns>
        public MapObjectBase FindMapObject(string name)
        {
            namedMapObject.TryGetValue(name, out MapObjectBase mapObject);
            return mapObject;
        }

        /// <summary>
        /// 重力を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>重力(0.0が無重力、1.0が通常)</returns>
        public double GetGravity(double x, double y) => GetEnvironment(x, y, me => !double.IsNaN(me.Gravity)).Gravity;

        /// <summary>
        /// 風速を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>風速(0.0：無風、正の数：右向きの風、負の数：左向きの風)</returns>
        public double GetWind(double x, double y) => GetEnvironment(x, y, me => !double.IsNaN(me.Wind)).Wind;

        /// <summary>
        /// 指定した位置の環境情報を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>環境</returns>
        public IMapEnvironment GetEnvironment(double x, double y)
            => (IMapEnvironment)areaEnvironmentCollection.LastOrDefault(me => me.Left <= x && x < me.Right && me.Top <= y && y < me.Bottom) ?? this;

        /// <summary>
        /// 指定した位置の環境情報を取得します。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="predicate">追加の条件</param>
        /// <returns>環境</returns>
        public IMapEnvironment GetEnvironment(double x, double y, Func<AreaEnvironment, bool> predicate)
            => (IMapEnvironment)areaEnvironmentCollection.LastOrDefault(me => me.Left <= x && x < me.Right && me.Top <= y && y < me.Bottom && predicate(me)) ?? this;

        /// <summary>
        /// 全ての局所的環境を取得します。
        /// </summary>
        /// <returns>環境</returns>
        public IEnumerable<AreaEnvironment> GetAllAreaEnvironment() => areaEnvironmentCollection;

        /// <summary>
        /// 名前で局所的環境を検索します。
        /// </summary>
        /// <param name="name">局所的環境の名前</param>
        /// <returns></returns>
        public AreaEnvironment FindAreaEnvironment(string name)
        {
            namedAreaEnvironment.TryGetValue(name, out AreaEnvironment area);
            return area;
        }
    }
}
