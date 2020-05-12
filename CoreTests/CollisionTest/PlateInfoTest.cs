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
                Flags = PlateAttributeFlag.Active | PlateAttributeFlag.HitUpper
            };
            Assert.IsTrue(plateInfo.HasFlag(PlateAttributeFlag.Active));
            Assert.IsTrue(plateInfo.HasFlag(PlateAttributeFlag.HitUpper));
            Assert.IsFalse(plateInfo.HasFlag(PlateAttributeFlag.HitBelow));
        }

        [TestMethod]
        public void SetFlagTest()
        {
            var plateInfo = new PlateInfo
            {
                Flags = PlateAttributeFlag.Active | PlateAttributeFlag.HitUpper
            };
            plateInfo.SetFlag(PlateAttributeFlag.HitUpper, false);
            plateInfo.SetFlag(PlateAttributeFlag.HitBelow, true);
            Assert.IsTrue(plateInfo.HasFlag(PlateAttributeFlag.Active), "関係ないフラグが変化");
            Assert.IsFalse(plateInfo.HasFlag(PlateAttributeFlag.HitUpper), "フラグをOFFにできていない");
            Assert.IsTrue(plateInfo.HasFlag(PlateAttributeFlag.HitBelow), "フラグをONにできていない");
        }
    }
}
