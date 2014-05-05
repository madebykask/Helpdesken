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
        public void GetPropertyValueShouldReturnCorrectPropertyValueFromObject()
        {
            var user = new User { Name = "Rustam Singatov" };
            var name = ReflectionHelper.GetInstancePropertyValue<string>(user, "Name");
            Assert.AreEqual(name, "Rustam Singatov");
        }
    }
}