
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain.Cases;

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
        public const int DefaultCaseLockBufferTime = 30;
        public const int DefaultExtendCaseLockTime = 60;

        private readonly ICaseLockRepository _caseLockRepository;
        private readonly IGlobalSettingRepository _globalSettingRepository;

        #region ctor()

        public CaseLockService(ICaseLockRepository caseLockRepository, 
                              IGlobalSettingRepository globalSettingRepository)
        {
            _globalSettingRepository = globalSettingRepository;
            _caseLockRepository = caseLockRepository;
        }

        #endregion

        public CaseLockSettings GetCaseLockSettings()
        {
            var globalSettings = _globalSettingRepository.Get();
            var extendedSec = globalSettings.CaseLockExtendTime > 0 ? globalSettings.CaseLockExtendTime : DefaultExtendCaseLockTime;
            var bufferTime = globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : DefaultCaseLockBufferTime;
            var timerInterval = globalSettings.CaseLockTimer;

            return new CaseLockSettings(timerInterval, bufferTime, extendedSec);
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

        public CaseLock GetCaseLock(int caseId)
        {
            return this._caseLockRepository.GetCaseLockByCaseId(caseId);
        }

        public ICaseLockOverview GetCaseLockOverviewByCaseId(int caseId)
        {
            return this._caseLockRepository.GetCaseLockOverviewByCaseId(caseId);
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

        public bool TryAcquireCaseLock(int caseId, int userId, string sessionId, out Guid caseLockGuid)
        {
            var now = DateTime.Now;
            var caseLockSettings = GetCaseLockSettings();
            
            var extendedSec = caseLockSettings.CaseLockExtendTime;
            var bufferTime = caseLockSettings.CaseLockBufferTime;

            var isCaseLocked = false;

            var caseLock = _caseLockRepository.GetCaseLockByCaseId(caseId); 
            if (caseLock != null)
            {
                isCaseLocked = true;

                //check if case can be unlocked 
                if ((caseLock.ExtendedTime.AddSeconds(bufferTime) < now) ||
                    (caseLock.ExtendedTime.AddSeconds(bufferTime) >= now &&
                     caseLock.UserId == userId && caseLock.BrowserSession == sessionId)) //todo: session check
                {
                    // Unlock case because user left the Case in abnormal way (Close browser/reset computer)
                    // Unlock case because it was open by current user last time / recently
                    _caseLockRepository.UnlockCaseByCaseId(caseId); 
                    isCaseLocked = false;
                }
            }

            // lock case if it has not been locked yet
            if (!isCaseLocked)
            {
                caseLock = new CaseLock(caseId, userId, Guid.NewGuid(), sessionId, now, now.AddSeconds(extendedSec));
                _caseLockRepository.LockCase(caseLock);
            }

            caseLockGuid = caseLock.LockGUID;

            //return true if case has been locked, false if it was already locked.
            return !isCaseLocked;
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

        public bool UnlockCaseByGUID(Guid lockGUID)
        {
            return _caseLockRepository.UnlockCaseByGUID(lockGUID);
        }

        public void DeleteCaseLockByCaseId(int caseId)
        {
            this._caseLockRepository.DeleteCaseLockByCaseId(caseId);
        }
    }
}