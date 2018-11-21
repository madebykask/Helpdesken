
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Mappers;
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
        private readonly IEntityToBusinessModelMapper<CaseLockEntity, CaseLock> _caseLockToBusinessModelMapper;

        #region ctor()

        public CaseLockService(ICaseLockRepository caseLockRepository, 
                              IGlobalSettingRepository globalSettingRepository,
            IEntityToBusinessModelMapper<CaseLockEntity, CaseLock> caseLockToBusinessModelMapper)
        {
            _globalSettingRepository = globalSettingRepository;
            _caseLockRepository = caseLockRepository;
            _caseLockToBusinessModelMapper = caseLockToBusinessModelMapper;
        }

        #endregion

        public CaseLockSettings GetCaseLockSettings()
        {
            var globalSettings = _globalSettingRepository.Get();
            return GetCaseLockSettingsModel(globalSettings);
        }
        
        public async Task<CaseLockSettings> GetCaseLockSettingsAsync()
        {
            var globalSettings = await _globalSettingRepository.GetAsync();
            return GetCaseLockSettingsModel(globalSettings);
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
            var caseLockEntity = _caseLockRepository.GetCaseLockByGUID(lockGUID);
            return caseLockEntity == null ? null : _caseLockToBusinessModelMapper.Map(caseLockEntity);
        }

        public CaseLock GetCaseLock(int caseId)
        {
            var caseLockEntity = _caseLockRepository.GetCaseLockByCaseId(caseId);
            return caseLockEntity == null ? null : _caseLockToBusinessModelMapper.Map(caseLockEntity);
        }

        public async Task<CaseLock> GetCaseLockAsync(int caseId)
        {
            var caseLockEntity = await _caseLockRepository.GetCaseLockByCaseIdAsync(caseId);
            return caseLockEntity == null ? null : _caseLockToBusinessModelMapper.Map(caseLockEntity);
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

        public async Task<CaseLockInfo> TryAcquireCaseLockAsync(int caseId, int userId, string sessionId)
        {
            var now = DateTime.Now;
            var caseLockSettings = await GetCaseLockSettingsAsync();

            var extendedSec = caseLockSettings.CaseLockExtendTime;
            var bufferTime = caseLockSettings.CaseLockBufferTime;

            var isCaseLocked = false;

            var caseLock = await GetCaseLockAsync(caseId);
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
                    var caseLockEntity = await _caseLockRepository.GetCaseLockByCaseIdAsync(caseId);
                    _caseLockRepository.DeleteCaseLock(caseLockEntity);
                    isCaseLocked = false;
                }
            }

            // lock case if it has not been locked yet
            if (!isCaseLocked)
            {
                caseLock = new CaseLock(caseId, userId, Guid.NewGuid(), sessionId, now, now.AddSeconds(extendedSec));
                _caseLockRepository.LockCase(caseLock);
            }

            //return true if case has been locked, false if it was already locked.
            return new CaseLockInfo()
            {
                IsLocked = isCaseLocked,
                CaseId = caseId,
                UserId = caseLock.UserId,
                LockGuid = caseLock.LockGUID.ToString(),
                ExtendValue = caseLockSettings.CaseLockExtendTime,
                ExtendedTime = caseLock.ExtendedTime,
                TimerInterval = caseLockSettings.CaseLockTimer,
                BrowserSession = caseLock.BrowserSession ?? "",
                CreatedTime = caseLock.CreatedTime,
                UserFullName = caseLock.User != null
                    ? $"{caseLock.User.FirstName} {caseLock.User.SurName}".Trim()
                    : string.Empty,
            };
        }

        public void LockCase(CaseLock caseLock)
        {
            this._caseLockRepository.LockCase(caseLock);
        }

        public bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond)
        {
            var existingLock = _caseLockRepository.GetCaseLockByGUID(lockGUID);
            return ReExtendLockCase(extendedTimeInSecond, existingLock);
        }

        public async Task<bool> ReExtendLockCaseAsync(Guid lockGUID, int extendedTimeInSecond)
        {
            var existingLock = await _caseLockRepository.GetCaseLockByGUIDAsync(lockGUID);
            return ReExtendLockCase(extendedTimeInSecond, existingLock);
        }

        public void UnlockCaseByCaseId(int caseId)
        {
            var caseLockEntity = _caseLockRepository.GetCaseLockByCaseId(caseId);
            _caseLockRepository.DeleteCaseLock(caseLockEntity);
        }

        public bool UnlockCaseByGUID(Guid lockGUID)
        {
            var caseLockEntity = _caseLockRepository.GetCaseLockByGUID(lockGUID);
            return _caseLockRepository.DeleteCaseLock(caseLockEntity);
        }

        public async Task<bool> UnlockCaseByGUIDAsync(Guid lockGUID)
        {
            var caseLockEntity = await _caseLockRepository.GetCaseLockByGUIDAsync(lockGUID);
            return _caseLockRepository.DeleteCaseLock(caseLockEntity);
        }

        public void DeleteCaseLockByCaseId(int caseId)
        {
            this._caseLockRepository.DeleteCaseLockByCaseId(caseId);
        }

        private CaseLockSettings GetCaseLockSettingsModel(GlobalSetting globalSettings)
        {
            var extendedSec = globalSettings.CaseLockExtendTime > 0 ? globalSettings.CaseLockExtendTime : DefaultExtendCaseLockTime;
            var bufferTime = globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : DefaultCaseLockBufferTime;
            var timerInterval = globalSettings.CaseLockTimer;

            return new CaseLockSettings(timerInterval, bufferTime, extendedSec);
        }

        private bool ReExtendLockCase(int extendedTimeInSecond, CaseLockEntity existingLock)
        {
            if (existingLock != null)
            {
                existingLock.ExtendedTime = DateTime.Now.AddSeconds(extendedTimeInSecond);
                _caseLockRepository.Update(existingLock);
                _caseLockRepository.Commit();
                return true;
            }

            return false;
        }

    }
}