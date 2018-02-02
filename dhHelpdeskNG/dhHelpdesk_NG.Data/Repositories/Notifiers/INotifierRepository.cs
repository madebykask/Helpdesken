namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public interface INotifierRepository : IRepository<ComputerUser>
    {
        void DeleteById(int notifierId);

        Notifier FindExistingNotifierById(int notifierId);

        NotifierDetails FindNotifierDetailsById(int notifierId);

        void AddNotifier(Notifier notifier);

        void UpdateNotifier(Notifier notifier);

        List<NotifierDetailedOverview> FindDetailedOverviewsByCustomerIdOrderedByUserIdAndFirstNameAndLastName(int customerId);

        List<ItemOverview> FindOverviewsByCustomerId(int customerId);

		IList<UserSearchResults> Search(int customerId, string searchFor, int? categoryID = null);

		SearchResult Search(SearchParameters parameters);

        void UpdateCaseNotifier(CaseNotifier caseNotifier);

        Notifier GetInitiatorByUserId(string userId, int customerId, bool activeOnly);

        bool IsInitiatorUserIdUnique(string userId, int initiatorId, int customerId, bool activeOnly);
    }
}
