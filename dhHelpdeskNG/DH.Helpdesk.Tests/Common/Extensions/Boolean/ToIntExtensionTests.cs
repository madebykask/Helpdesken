namespace DH.Helpdesk.Tests.Common.Extensions.Boolean
{
    using DH.Helpdesk.Common.Extensions.Boolean;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ToIntExtensionTests
    {
        [Test]
        public void ToInt_True_1()
        {
            // Act
            var convertedValue = true.ToInt();

            // Assert
            Assert.AreEqual(convertedValue, 1);
        }

        [Test]
        public void ToInt_False_0()
        {
            // Act
            var convertedValue = false.ToInt();

            // Assert
            Assert.AreEqual(convertedValue, 0);
        }
    }
}