namespace DH.Helpdesk.Dal.Repositories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public sealed class NotifierGroupRepository : RepositoryBase<ComputerUserGroup>, INotifierGroupRepository
    {
        public NotifierGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId)
        {
            var overviews =
                this.DataContext.ComputerUserGroups.Where(g => g.Customer_Id == customerId)
                    .Select(g => new { g.Id, g.Name })
                    .ToList();

            return
                overviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Id.ToString(CultureInfo.InvariantCulture) })
                                      .ToList();
        }
    }
}
