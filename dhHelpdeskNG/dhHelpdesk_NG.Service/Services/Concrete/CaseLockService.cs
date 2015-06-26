namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Domain;

    
    public sealed class CaseLockService : ICaseLockService
    {
        private readonly ICaseLockRepository _caseLockRepository;

        public CaseLockService(ICaseLockRepository caseLockRepository)
        {
            this._caseLockRepository = caseLockRepository;
        }

        public IEnumerable<CaseLock> GetAllLockedCases()
        {
            return this._caseLockRepository.GetAllLockedCases();
        }
        
        public CaseLock GetCaseLockByGUID(Guid lockGUID)
        {
            return this._caseLockRepository.GetCaseLockByGUID(lockGUID);
        }

        public CaseLock GetCaseLockByCaseId(int caseId)
        {
            return this._caseLockRepository.GetCaseLockByCaseId(caseId);
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

    }
}