namespace DH.Helpdesk.Tests.Common.Extensions.Integer
{
    using DH.Helpdesk.Common.Extensions.Integer;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ToBoolExtensionTests
    {
        [Test]
        public void ToBool_0_False()
        {
            // Act
            var convertedValue = 0.ToBool();

            // Assert
            Assert.IsFalse(convertedValue);
        }

        [Test]
        public void ToBool_1_True()
        {
            // Act
            var convertedValue = 1.ToBool();

            // Assert
            Assert.IsTrue(convertedValue);
        }
    }
}