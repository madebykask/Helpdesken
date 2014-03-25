namespace DH.Helpdesk.Tests.Common.Tools
{
    using DH.Helpdesk.Common.Tools;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ReflectionHelperTests
    {
        private sealed class User
        {
            public string Name { get; set; }
        }

        [Test]
        public void GetPropertyValue_CorrectProperty_PropertyValue()
        {
            // Arrange
            var user = new User { Name = "Rustam Singatov" };

            // Act
            var name = ReflectionHelper.GetPropertyValue<string>(user, "Name");

            // Assert
            Assert.AreEqual(name, "Rustam Singatov");
        }
    }
}