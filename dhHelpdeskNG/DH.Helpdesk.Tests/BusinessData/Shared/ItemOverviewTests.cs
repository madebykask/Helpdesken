namespace DH.Helpdesk.Tests.BusinessData.Shared
{
    using DH.Helpdesk.BusinessData.Models.Shared;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ItemOverviewTests
    {
        #region Public Methods and Operators

        [Test]
        public void ConstructorMustAssignValuesCorrectly()
        {
            var name = "Name";
            var value = "Value";

            var overview = new ItemOverview("Name", "Value");

            Assert.AreSame(overview.Name, name);
            Assert.AreSame(overview.Value, value);
        }

        #endregion
    }
}