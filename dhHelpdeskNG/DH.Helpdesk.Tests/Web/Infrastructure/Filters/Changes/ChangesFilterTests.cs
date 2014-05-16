namespace DH.Helpdesk.Tests.Web.Infrastructure.Filters.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ChangesFilterTests
    {
        #region Public Methods and Operators

        [Test]
        public void ConstructorMustAssignValuesCorrectly()
        {
            var statusIds = new List<int> { 1, 2 };
            var objectIds = new List<int> { 3, 4 };
            var ownerIds = new List<int> { 5, 6 };
            var affectedProcessIds = new List<int> { 7, 8 };
            var workingGroupIds = new List<int> { 9, 10 };
            var administratorIds = new List<int> { 11, 12 };
            var pharse = "Search pharse";
            var status = ChangeStatus.Finished;
            var recordsOnPage = 50;
            var sortField = new SortField("Name", SortBy.Descending);

            var filter = new ChangesFilter(
                statusIds,
                objectIds,
                ownerIds,
                affectedProcessIds,
                workingGroupIds,
                administratorIds,
                pharse,
                status,
                recordsOnPage,
                sortField);

            Assert.AreSame(filter.StatusIds, statusIds);
            Assert.AreSame(filter.ObjectIds, objectIds);
            Assert.AreSame(filter.OwnerIds, ownerIds);
            Assert.AreSame(filter.AffectedProcessIds, affectedProcessIds);
            Assert.AreSame(filter.WorkingGroupIds, workingGroupIds);
            Assert.AreSame(filter.AdministratorIds, administratorIds);
            Assert.AreSame(filter.Pharse, pharse);
            Assert.AreEqual(filter.Status, status);
            Assert.AreEqual(filter.RecordsOnPage, recordsOnPage);
            Assert.AreSame(filter.SortField, sortField);
        }

        [Test]
        public void DefaultFilterMustBeInitializedWithCorrectValues()
        {
            var defaultFilter = ChangesFilter.CreateDefault();

            Assert.AreEqual(defaultFilter.Status, ChangeStatus.Active);
            Assert.AreEqual(defaultFilter.RecordsOnPage, 100);
        }

        #endregion
    }
}