namespace DH.Helpdesk.Tests.Common.Extensions.Integer
{
    using DH.Helpdesk.Common.Extensions.Integer;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ToBoolExtensionTests
    {
        [Test]
        public void ToBoolShouldConvertZeroToFalse()
        {
            var convertedValue = 0.ToBool();
            Assert.IsFalse(convertedValue);
        }

        [Test]
        public void ToBoolShouldConvertOneToTrue()
        {
            var convertedValue = 1.ToBool();
            Assert.IsTrue(convertedValue);
        }
    }
}