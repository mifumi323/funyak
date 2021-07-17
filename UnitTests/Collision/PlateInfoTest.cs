using NUnit.Framework;
using MifuminSoft.funyak.Collision;

namespace MifuminSoft.funyak.UnitTests.Collision
{
    public class PlateInfoTest
    {
        [Test]
        public void HasFlagTest()
        {
            var plateInfo = new PlateInfo
            {
                Flags = PlateAttributeFlag.HitUpper
            };
            Assert.IsTrue(plateInfo.HasFlag(PlateAttributeFlag.HitUpper));
            Assert.IsFalse(plateInfo.HasFlag(PlateAttributeFlag.HitBelow));
            Assert.IsFalse(plateInfo.HasFlag(PlateAttributeFlag.HitLeft));
        }

        [Test]
        public void SetFlagTest()
        {
            var plateInfo = new PlateInfo
            {
                Flags = PlateAttributeFlag.HitUpper
            };
            plateInfo.SetFlag(PlateAttributeFlag.HitUpper, false);
            plateInfo.SetFlag(PlateAttributeFlag.HitBelow, true);
            Assert.IsFalse(plateInfo.HasFlag(PlateAttributeFlag.HitUpper), "フラグをOFFにできていない");
            Assert.IsTrue(plateInfo.HasFlag(PlateAttributeFlag.HitBelow), "フラグをONにできていない");
            Assert.IsFalse(plateInfo.HasFlag(PlateAttributeFlag.HitLeft), "関係ないフラグが変化");
        }
    }
}
