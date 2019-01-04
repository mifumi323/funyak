using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.MapEnvironment;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.Core.Tests
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            var map = new Map(320, 224);
            Assert.AreEqual(320, map.Width);
            Assert.AreEqual(224, map.Height);
        }

        [TestMethod]
        public void AddMapObjectTest()
        {
            var map = new Map(320, 224);
            var mapObject = new MainMapObject(80, 112);
            map.AddMapObject(mapObject);
            var mapObjects = map.GetMapObjects();

            Assert.AreEqual(1, mapObjects.Count());
        }

        [TestMethod]
        public void RemoveMapObjectTest()
        {
            var map = new Map(320, 224);
            var mapObject = new MainMapObject(80, 112);
            map.AddMapObject(mapObject);
            map.RemoveMapObject(mapObject);
            var mapObjects = map.GetMapObjects();

            Assert.AreEqual(0, mapObjects.Count());
        }

        [TestMethod]
        public void FindMapObjectTest()
        {
            var map = new Map(320, 224);
            var namedMapObject = new MainMapObject(80, 112)
            {
                Name = "knownName",
            };
            map.AddMapObject(namedMapObject);
            var unnamedMapObject = new MainMapObject(240, 112)
            {
            };
            map.AddMapObject(unnamedMapObject);

            Assert.AreEqual(namedMapObject, map.FindMapObject("knownName"), "既知の名前によるマップオブジェクトの検索に失敗");
            Assert.AreEqual(null, map.FindAreaEnvironment("unknownName"), "未知の名前によるマップオブジェクトの検索に失敗");
        }

        [TestMethod]
        public void FindAreaEnvironmentTest()
        {
            var map = new Map(320, 224);
            var namedArea = new AreaEnvironment()
            {
                Name = "knownName",
            };
            map.AddAreaEnvironment(namedArea);
            var unnamedArea = new AreaEnvironment()
            {
            };
            map.AddAreaEnvironment(unnamedArea);

            Assert.AreEqual(namedArea, map.FindAreaEnvironment("knownName"), "既知の名前による局所的環境の検索に失敗");
            Assert.AreEqual(null, map.FindAreaEnvironment("unknownName"), "未知の名前による局所的環境の検索に失敗");
        }

        [TestMethod]
        public void ManyMapObjectsTest()
        {
            var cellWidth = 50;
            var cellHeight = 50;
            var cellCountX = 10;
            var cellCountY = 10;
            var frameCount = 600;
            var map = new Map(cellCountX * cellWidth, cellCountY * cellHeight);

            // たくさんの壁を作る
            for (int x = 0; x <= cellCountX; x++)
            {
                map.AddMapObject(new LineMapObject(x * cellWidth, 0, x * cellWidth, cellCountY * cellHeight)
                {
                    HitLeft = true,
                    HitRight = true
                });
            }
            for (int y = 0; y <= cellCountY; y++)
            {
                map.AddMapObject(new LineMapObject(0, y * cellHeight, cellCountX * cellWidth, y * cellHeight)
                {
                    HitUpper = true,
                    HitBelow = true
                });
            }

            // たくさんのふにゃを呼ぶ
            for (int x = 0; x < cellCountX; x++)
            {
                for (int y = 0; y < cellCountY; y++)
                {
                    map.AddMapObject(new MainMapObject(x * cellWidth + cellWidth / 2, y * cellHeight + cellHeight / 2));
                }
            }

            // 動かす
            for (int f = 0; f < frameCount; f++)
            {
                map.Update();
            }
        }
    }
}
