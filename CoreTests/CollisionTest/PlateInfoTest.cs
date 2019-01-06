using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.Core.Tests.CollisionTest
{
    [TestClass]
    public class PlateInfoTest
    {
        [TestMethod]
        public void HasFlagTest()
        {
            var plateInfo = new PlateInfo
            {
                Flags = PlateFlags.Active | PlateFlags.HitUpper
            };
            Assert.IsTrue(plateInfo.HasFlag(PlateFlags.Active));
            Assert.IsTrue(plateInfo.HasFlag(PlateFlags.HitUpper));
            Assert.IsFalse(plateInfo.HasFlag(PlateFlags.HitBelow));
        }

        [TestMethod]
        public void SetFlagTest()
        {
            var plateInfo = new PlateInfo
            {
                Flags = PlateFlags.Active | PlateFlags.HitUpper
            };
            plateInfo.SetFlag(PlateFlags.HitUpper, false);
            plateInfo.SetFlag(PlateFlags.HitBelow, true);
            Assert.IsTrue(plateInfo.HasFlag(PlateFlags.Active), "関係ないフラグが変化");
            Assert.IsFalse(plateInfo.HasFlag(PlateFlags.HitUpper), "フラグをOFFにできていない");
            Assert.IsTrue(plateInfo.HasFlag(PlateFlags.HitBelow), "フラグをONにできていない");
        }
    }
}
