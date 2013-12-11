namespace dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public sealed class NotifierGroupsRepository : RepositoryBase<ComputerUserGroup>, INotifierGroupsRepository
    {
        public NotifierGroupsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<NotifierGroupOverviewDto> FindOverviewByCustomerId(int customerId)
        {
            return
                this.DataContext.ComputerUserGroups.Where(g => g.Customer_Id == customerId)
                    .Select(g => new NotifierGroupOverviewDto { Id = g.Id, Name = g.Name })
                    .ToList();
        }
    }
}
