
namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Domain;
    using System.Linq;


    public sealed class CaseLockService : ICaseLockService
    {
        private readonly ICaseLockRepository _caseLockRepository;

        public CaseLockService(ICaseLockRepository caseLockRepository)
        {
            this._caseLockRepository = caseLockRepository;
        }

        public List<LockedCaseOverview> GetLockedCases(int? customerId)
        {
            return this._caseLockRepository.GetLockedCases(customerId);
        }

        public List<LockedCaseOverview> GetLockedCases(int? customerId, decimal caseNumber)
        {
            return this._caseLockRepository.GetLockedCases(customerId, caseNumber);
        }

        public List<LockedCaseOverview> GetLockedCases(int? customerId, string searchText)
        {
            return this._caseLockRepository.GetLockedCases(customerId, searchText);
        }
        
        public CaseLock GetCaseLockByGUID(Guid lockGUID)
        {
            return this._caseLockRepository.GetCaseLockByGUID(lockGUID);
        }

        public ICaseLockOverview GetCaseLockByCaseId(int caseId)
        {
            return this._caseLockRepository.GetCaseLockByCaseId(caseId);
        }

        public IDictionary<int, ICaseLockOverview> GetCasesLocks(int[] caseIds)
        {
            return _caseLockRepository.GetCasesLock(caseIds);
        }

       public IQueryable<ICaseLockOverview> GetLockedCasesToOverView(int[] caseIds, GlobalSetting globalSettings, int defaultCaseLockBufferTime)
        {
            var bufferTime = (globalSettings != null && globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : defaultCaseLockBufferTime);
            var caseLocks = _caseLockRepository.GetLockedCases(caseIds, bufferTime);
            
            return caseLocks;
        }

        public void CaseLockCleanUp()
        {
            this._caseLockRepository.CaseLockCleanUp();
        }

        public void LockCase(CaseLock caseLock)
        {
            this._caseLockRepository.LockCase(caseLock);            
        }

        public bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond)
        {
            return this._caseLockRepository.ReExtendLockCase(lockGUID, extendedTimeInSecond);
        }

        public void UnlockCaseByCaseId(int caseId)
        {
            this._caseLockRepository.UnlockCaseByCaseId(caseId);
        }

        public void UnlockCaseByGUID(Guid lockGUID)
        {
            this._caseLockRepository.UnlockCaseByGUID(lockGUID);
        }

        public void DeleteCaseLockByCaseId(int caseId)
        {
            this._caseLockRepository.DeleteCaseLockByCaseId(caseId);
        }

        public int GetCaseUnlockPermission(int userId)
        {
            return _caseLockRepository.GetCaseUnlockPermission(userId);
        }
    }
}