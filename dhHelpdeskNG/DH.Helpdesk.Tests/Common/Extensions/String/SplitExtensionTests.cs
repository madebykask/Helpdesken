namespace DH.Helpdesk.Tests.Common.Extensions.String
{
    using System.Linq;

    using DH.Helpdesk.Common.Extensions.String;

    using NUnit.Framework;

    [TestFixture]
    public sealed class SplitExtensionTests
    {
        [Test]
        public void SplitShouldSplitStringForTwoPartsByCharacter()
        {
            var splittedStrings = "Rustam Singatov".Split(" ").ToList();

            Assert.AreEqual(splittedStrings.Count(), 2);
            Assert.AreEqual(splittedStrings[0], "Rustam");
            Assert.AreEqual(splittedStrings[1], "Singatov");
        }
    }
}