namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public interface INotifierRepository : IRepository<ComputerUser>
    {
        void DeleteById(int notifierId);

        ExistingNotifierDto FindExistingNotifierById(int notifierId);

        NotifierDetailsDto FindNotifierDetailsById(int notifierId);

        void AddNotifier(NewNotifierDto notifier);

        void UpdateNotifier(UpdatedNotifierDto notifier);

        List<NotifierDetailedOverviewDto> FindDetailedOverviewsByCustomerIdOrderedByUserIdAndFirstNameAndLastName(int customerId);

        List<ItemOverview> FindOverviewsByCustomerId(int customerId);

        IList<UserSearchResults> Search(int customerId, string searchFor);

        SearchResultDto SearchDetailedOverviewsOrderedByUserIdAndFirstNameAndLastName(
            int customerId,
            int? domainId,
            int? departmentId,
            int? divisionId,
            string pharse,
            EntityStatus status,
            int selectCount);
    }
}
