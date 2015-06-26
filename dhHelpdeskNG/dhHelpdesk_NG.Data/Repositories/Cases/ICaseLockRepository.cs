using System;
namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Dal;

    public interface ICaseLockRepository : INewRepository
    {
        IEnumerable<CaseLock> GetAllLockedCases();

        CaseLock GetCaseLockByGUID(Guid lockGUID);

        CaseLock GetCaseLockByCaseId(int caseId);
        
        void LockCase(CaseLock caseLock);

        bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond);

        void UnlockCaseByCaseId(int caseId);

        void UnlockCaseByGUID(Guid lockGUID);

        void DeleteCaseLockByCaseId(int caseId);
    }
}