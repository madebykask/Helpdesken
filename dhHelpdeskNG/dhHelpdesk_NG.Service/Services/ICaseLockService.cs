namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Dal.Dal;
    using System.Linq;

    public interface ICaseLockService
    {

        List<LockedCaseOverview> GetLockedCases(int? customerId);

        List<LockedCaseOverview> GetLockedCases(int? customerId, decimal caseNumber);

        List<LockedCaseOverview> GetLockedCases(int? customerId, string searchText);

        CaseLock GetCaseLockByGUID(Guid lockGUID);

        ICaseLockOverview GetCaseLockByCaseId(int caseId);

        IDictionary<int, ICaseLockOverview> GetCasesLocks(int[] caseIds);

        IQueryable<ICaseLockOverview> GetLockedCasesToOverView(int[] caseIds, GlobalSetting globalSettings, int defaultCaseLockBufferTime);

        void CaseLockCleanUp();

        void LockCase(CaseLock caseLock);

        bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond);

        void UnlockCaseByCaseId(int caseId);

        void UnlockCaseByGUID(Guid lockGUID);

        void DeleteCaseLockByCaseId(int caseId);
    }
}