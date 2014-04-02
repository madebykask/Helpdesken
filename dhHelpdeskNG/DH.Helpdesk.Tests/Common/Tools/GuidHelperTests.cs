namespace DH.Helpdesk.Tests.Common.Tools
{
    using System;

    using DH.Helpdesk.Common.Tools;

    using NUnit.Framework;

    [TestFixture]
    public sealed class GuidHelperTests
    {
        [Test]
        public void IsGuidShouldReturnTrueOnCorrectGuidString()
        {
            const string Guid = "4fee9019-7bd1-4b51-bead-df9fa7de4c4e";
            var isGuid = GuidHelper.IsGuid(Guid);
            Assert.IsTrue(isGuid);
        }

        [Test]
        public void IsGuidShouldReturnFalseOnIncorrectGuidString()
        {
            const string NotGuid = "Not guid.";
            var isGuid = GuidHelper.IsGuid(NotGuid);
            Assert.IsFalse(isGuid);
        }
    }
}