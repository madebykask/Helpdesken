namespace DH.Helpdesk.Tests.Common.Extensions.Boolean
{
    using DH.Helpdesk.Common.Extensions.Boolean;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ToIntExtensionTests
    {
        [Test]
        public void ToIntShouldConvertTrueToOne()
        {
            const int ExpectedResult = 1;
            var convertedValue = true.ToInt();
            Assert.AreEqual(convertedValue, ExpectedResult);
        }

        [Test]
        public void ToIntShouldConvertFalseToZero()
        {
            const int ExpectedResult = 0;
            var convertedValue = false.ToInt();
            Assert.AreEqual(convertedValue, ExpectedResult);
        }
    }
}