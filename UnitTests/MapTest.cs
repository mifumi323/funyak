using System.Linq;
using NUnit.Framework;
using MifuminSoft.funyak.MapEnvironment;
using MifuminSoft.funyak.MapObject;

namespace MifuminSoft.funyak.UnitTests
{
    public class MapTest
    {
        [Test]
        public void InitializeTest()
        {
            var map = new Map(320, 224);
            Assert.AreEqual(320, map.Width);
            Assert.AreEqual(224, map.Height);
        }

        [Test]
        public void AddMapObjectTest()
        {
            var map = new Map(320, 224);
            var mapObject = new FunyaMapObject(80, 112);
            map.AddMapObject(mapObject);
            var mapObjects = map.EnumerateAllMapObjects();

            Assert.AreEqual(1, mapObjects.Count());
        }

        [Test]
        public void RemoveMapObjectTest()
        {
            var map = new Map(320, 224);
            var mapObject = new FunyaMapObject(80, 112);
            map.AddMapObject(mapObject);
            map.RemoveMapObject(mapObject);
            var mapObjects = map.EnumerateAllMapObjects();

            Assert.AreEqual(0, mapObjects.Count());
        }

        [Test]
        public void FindMapObjectTest()
        {
            var map = new Map(320, 224);
            var namedMapObject = new FunyaMapObject(80, 112)
            {
                Name = "knownName",
            };
            map.AddMapObject(namedMapObject);
            var unnamedMapObject = new FunyaMapObject(240, 112)
            {
            };
            map.AddMapObject(unnamedMapObject);

            Assert.AreEqual(namedMapObject, map.FindMapObject("knownName"), "既知の名前によるマップオブジェクトの検索に失敗");
            Assert.AreEqual(null, map.FindMapObject("unknownName"), "未知の名前によるマップオブジェクトの検索に失敗");
        }

        [Test]
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
                    map.AddMapObject(new FunyaMapObject(x * cellWidth + cellWidth / 2, y * cellHeight + cellHeight / 2));
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
