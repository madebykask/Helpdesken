namespace dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public sealed class NotifierGroupsRepository : RepositoryBase<ComputerUserGroup>, INotifierGroupRepository
    {
        public NotifierGroupsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId)
        {
            var notifierGroupOverviews =
                this.DataContext.ComputerUserGroups.Where(g => g.Customer_Id == customerId)
                    .Select(g => new { g.Id, g.Name })
                    .ToList();

            return
                notifierGroupOverviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Id.ToString(CultureInfo.InvariantCulture) })
                                      .ToList();
        }
    }
}
