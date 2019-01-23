using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case;

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

        CaseLock GetCaseLock(int caseId);

        Task<CaseLock> GetCaseLockAsync(int caseId);

        CaseLock GetCaseLockByGUID(Guid lockGUID);

        ICaseLockOverview GetCaseLockOverviewByCaseId(int caseId);

        IDictionary<int, ICaseLockOverview> GetCasesLocks(int[] caseIds);

        IQueryable<ICaseLockOverview> GetLockedCasesToOverView(int[] caseIds, GlobalSetting globalSettings, int defaultCaseLockBufferTime);

        void CaseLockCleanUp();

        CaseLockSettings GetCaseLockSettings();
        Task<CaseLockSettings> GetCaseLockSettingsAsync();

        Task<CaseLockInfo> TryAcquireCaseLockAsync(int caseId, int userId, string sessionId);

        void LockCase(CaseLock caseLock);

        bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond);
        Task<bool> ReExtendLockCaseAsync(Guid lockGUID, int extendedTimeInSecond);

        void UnlockCaseByCaseId(int caseId);

        bool UnlockCaseByGUID(Guid lockGUID);
        Task<bool> UnlockCaseByGUIDAsync(Guid lockGUID);

        void DeleteCaseLockByCaseId(int caseId);
    }
}