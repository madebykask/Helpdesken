using System.Linq;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case.Input;
using DH.Helpdesk.BusinessData.Models.Notifiers;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    public interface INotifierRepository : IRepository<ComputerUser>
    {
        void DeleteById(int notifierId);

        Notifier FindExistingNotifierById(int notifierId);

        NotifierDetails FindNotifierDetailsById(int notifierId);

        void AddNotifier(Notifier notifier);

        void UpdateNotifier(Notifier notifier);

        Notifier GetInitiatorInfo(string userId, int customerId, bool activeOnly);

        List<NotifierDetailedOverview> FindDetailedOverviewsByCustomerIdOrderedByUserIdAndFirstNameAndLastName(int customerId);

        List<ItemOverview> FindOverviewsByCustomerId(int customerId);

        IQueryable<ComputerUser> Search(int customerId, string searchFor, int? categoryID = null, IList<int> departmentIds = null, bool exactSearch = false);

        SearchResult Search(SearchParameters parameters);

        void UpdateCaseNotifier(CaseNotifier caseNotifier);

        Notifier GetInitiatorByUserId(string userId, int customerId, bool activeOnly);

        bool IsInitiatorUserIdUnique(string userId, int initiatorId, int customerId, bool activeOnly);

        int GetExistingNotifierIdByUserId(string userId, int customerId);
    }
}
