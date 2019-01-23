using System;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Cases;
    using System.Linq;

    public interface ICaseLockRepository : IRepository<CaseLockEntity>
    {
        List<LockedCaseOverview> GetLockedCases(int? customerId);

        List<LockedCaseOverview> GetLockedCases(int? customerId, decimal caseNumber);

        List<LockedCaseOverview> GetLockedCases(int? customerId, string searchText);
        IQueryable<ICaseLockOverview> GetLockedCases(int[] caseIds, int bufferTime);
        CaseLockEntity GetCaseLockByGUID(Guid lockGUID);
        Task<CaseLockEntity> GetCaseLockByGUIDAsync(Guid lockGUID);

        ICaseLockOverview GetCaseLockOverviewByCaseId(int caseId);

        IDictionary<int, ICaseLockOverview> GetCasesLock(int[] caseIds);

        CaseLockEntity GetCaseLockByCaseId(int caseId);
        Task<CaseLockEntity> GetCaseLockByCaseIdAsync(int caseId);

        void CaseLockCleanUp();
        
        void LockCase(CaseLock caseLock);

        bool DeleteCaseLock(CaseLockEntity caseLock);

        void DeleteCaseLockByCaseId(int caseId);
    }
}