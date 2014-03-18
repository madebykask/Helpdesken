namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public interface INotifierRepository : IRepository<ComputerUser>
    {
        void DeleteById(int notifierId);

        ExistingNotifier FindExistingNotifierById(int notifierId);

        NotifierDetails FindNotifierDetailsById(int notifierId);

        void AddNotifier(NewNotifier notifier);

        void UpdateNotifier(UpdatedNotifier notifier);

        List<NotifierDetailedOverview> FindDetailedOverviewsByCustomerIdOrderedByUserIdAndFirstNameAndLastName(int customerId);

        List<ItemOverview> FindOverviewsByCustomerId(int customerId);

        IList<UserSearchResults> Search(int customerId, string searchFor);

        SearchResult Search(SearchParameters parameters);
    }
}
