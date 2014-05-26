namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeObjectRepository : RepositoryBase<ChangeObjectEntity>, IChangeObjectRepository
    {
        public ChangeObjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var objects =
                this.DataContext.ChangeObjects.Where(o => o.Customer_Id == customerId)
                    .Select(o => new { o.Id, o.ChangeObject })
                    .ToList();

            return
                objects.Select(o => new ItemOverview(o.ChangeObject, o.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public string GetObjectName(int objectId)
        {
            return this.DataContext.ChangeObjects.Where(o => o.Id == objectId).Select(o => o.ChangeObject).FirstOrDefault();
        }
    }
}