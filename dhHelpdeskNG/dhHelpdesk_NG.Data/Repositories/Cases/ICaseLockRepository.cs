using System;
namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.Dal.Dal;

    public interface ICaseLockRepository : INewRepository
    {
        List<LockedCaseOverview> GetLockedCases(int? customerId);

        List<LockedCaseOverview> GetLockedCases(int? customerId, decimal caseNumber);

        List<LockedCaseOverview> GetLockedCases(int? customerId, string searchText);

        CaseLock GetCaseLockByGUID(Guid lockGUID);

        CaseLock GetCaseLockByCaseId(int caseId);
        
        void LockCase(CaseLock caseLock);

        bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond);

        void UnlockCaseByCaseId(int caseId);

        void UnlockCaseByGUID(Guid lockGUID);

        void DeleteCaseLockByCaseId(int caseId);

    }
}