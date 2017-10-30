using System;
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
        CaseLock GetCaseLockByGUID(Guid lockGUID);

        ICaseLockOverview GetCaseLockByCaseId(int caseId);

        IDictionary<int, ICaseLockOverview> GetCasesLock(int[] caseIds);

        void CaseLockCleanUp();
        
        void LockCase(CaseLock caseLock);

        bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond);

        void UnlockCaseByCaseId(int caseId);

        void UnlockCaseByGUID(Guid lockGUID);

        void DeleteCaseLockByCaseId(int caseId);

        int GetCaseUnlockPermission(int userId);
    }
}