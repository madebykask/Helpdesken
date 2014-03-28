namespace DH.Helpdesk.Tests.Common.Tools
{
    using DH.Helpdesk.Common.Tools;

    using NUnit.Framework;

    [TestFixture]
    public sealed class GuidHelperTests
    {
        [Test]
        public void IsGuid_CorrectGuid_True()
        {
            // Arrange
            const string guid = "4fee9019-7bd1-4b51-bead-df9fa7de4c4e";

            // Act
            var isGuid = GuidHelper.IsGuid(guid);

            // Assert
            Assert.IsTrue(isGuid);
        }

        [Test]
        public void IsGuid_IncorrectGuid_False()
        {
            // Arrange
            const string notGuid = "Not guid.";

            // Act
            var isGuid = GuidHelper.IsGuid(notGuid);

            // Assert
            Assert.IsFalse(isGuid);
        }
    }
}