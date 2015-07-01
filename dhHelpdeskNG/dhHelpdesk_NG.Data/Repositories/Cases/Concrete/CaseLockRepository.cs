
namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;    
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Domain;
       
    
    public sealed class CaseLockRepository : Repository, ICaseLockRepository
    {
        public class TempLockInfo
        {
            public TempLockInfo()
            {
            }

            public Case CaseEntity { get; set; }

            public CaseLockEntity LockEntity { get; set; } 
        }

        private readonly IEntityToBusinessModelMapper<CaseLockEntity, CaseLock> _caseLockToBusinessModelMapper;

        private readonly IBusinessModelToEntityMapper<CaseLock, CaseLockEntity> _caseLockToEntityMapper;
        
        public CaseLockRepository(
            IDatabaseFactory databaseFactory, 
            IEntityToBusinessModelMapper<CaseLockEntity, CaseLock> caseLockToBusinessModelMapper, 
            IBusinessModelToEntityMapper<CaseLock, CaseLockEntity> caseLockToEntityMapper)
            : base(databaseFactory)
        {
            this._caseLockToBusinessModelMapper = caseLockToBusinessModelMapper;
            this._caseLockToEntityMapper = caseLockToEntityMapper;
        }

    
        public CaseLock GetCaseLockByGUID(Guid lockGUID)
        {
            var entity = this.DbContext.CaseLock.Where(l => l.LockGUID == lockGUID)
                                                .FirstOrDefault();

            if (entity == null)
                return null;
            else
                return this._caseLockToBusinessModelMapper.Map(entity);
        }

        public CaseLock GetCaseLockByCaseId(int caseId)
        {
            var entity = this.DbContext.CaseLock.Where(l => l.Case_Id == caseId)
                                                .FirstOrDefault();

            if (entity == null)
                return null;
            else
                return this._caseLockToBusinessModelMapper.Map(entity);
        }

        public void LockCase(CaseLock caseLock)
        {
            var entity = new CaseLockEntity();
            this._caseLockToEntityMapper.Map(caseLock, entity);

            this.DbContext.CaseLock.Add(entity);
            this.DbContext.Commit();            
        }

        public bool ReExtendLockCase(Guid lockGUID, int extendedTimeInSecond)
        {
            var ret = false;
            var existingLock = this.DbContext.CaseLock.Where(cl => cl.LockGUID == lockGUID)
                                                      .FirstOrDefault();

            if (existingLock != null)
            {
                this.DbContext.CaseLock.Remove(existingLock);
                var entity = new CaseLockEntity { 
                                                    Case_Id = existingLock.Case_Id,
                                                    User_Id = existingLock.User_Id,
                                                    LockGUID = existingLock.LockGUID,
                                                    BrowserSession = existingLock.BrowserSession,
                                                    CreatedTime = existingLock.CreatedTime,
                                                    ExtendedTime = existingLock.ExtendedTime.AddSeconds(extendedTimeInSecond)
                                                };

                this.DbContext.CaseLock.Add(entity);
                this.DbContext.Commit();
                ret = true;
            }

            return ret;
        }        
 
        public void UnlockCaseByCaseId(int caseId)
        {
            var entity = this.DbContext.CaseLock.Where(l => l.Case_Id == caseId)
                                                .FirstOrDefault();
            if (entity != null)
            {
                this.DbContext.CaseLock.Remove(entity);
                this.DbContext.Commit();
            }
        }

        public void UnlockCaseByGUID(Guid lockGUID)
        {
            var entity = this.DbContext.CaseLock.Where(l => l.LockGUID == lockGUID)
                                                .FirstOrDefault();
            if (entity != null)
            {
                this.DbContext.CaseLock.Remove(entity);
                this.DbContext.Commit();
            }
        }

        public void DeleteCaseLockByCaseId(int caseId)
        {
            var entity = this.DbContext.CaseLock.Where(l => l.Case_Id == caseId)
                                                .FirstOrDefault();
            if (entity != null)
            {
                this.DbContext.CaseLock.Remove(entity);
                this.DbContext.Commit();
            }
        }

        public List<LockedCaseOverview> GetLockedCases(int? customerId)
        {            
            var lockedCases = GetAlllockedCaseEntities();
            if (customerId.HasValue)
                lockedCases = lockedCases.Where(l => l.CaseEntity.Customer_Id == customerId.Value).ToList();
          
            return MapEntityToOverview(lockedCases);
        }

        public List<LockedCaseOverview> GetLockedCases(int? customerId, decimal caseNumber)
        {          
            var lockedCases = GetAlllockedCaseEntities();
            if (customerId.HasValue)
                lockedCases = lockedCases.Where(l => l.CaseEntity.Customer_Id == customerId.Value).ToList();

            if (caseNumber > 0)
                lockedCases = lockedCases.Where(l => l.CaseEntity.CaseNumber == caseNumber).ToList();

            return MapEntityToOverview(lockedCases);
        }

        public List<LockedCaseOverview> GetLockedCases(int? customerId, string searchText)
        {
            searchText = searchText.ToLower();
            var lockedCases = GetAlllockedCaseEntities();            
            var ret = new List<LockedCaseOverview>();
            if (customerId.HasValue)
                lockedCases = lockedCases.Where(l => l.CaseEntity.Customer_Id == customerId.Value).ToList();

            if (!string.IsNullOrEmpty(searchText))
                lockedCases = lockedCases.Where(l => l.LockEntity.User.FirstName.ToLower().Contains(searchText) ||
                                                     l.LockEntity.User.SurName.ToLower().Contains(searchText) ||
                                                     l.LockEntity.User.UserID.ToLower().Contains(searchText)).ToList();

            return MapEntityToOverview(lockedCases);
        }

        private List<TempLockInfo> GetAlllockedCaseEntities()
        {
           return (from cl in this.DbContext.CaseLock
                   join c in this.DbContext.Cases on cl.Case_Id equals c.Id
                   where (cl.ExtendedTime > DateTime.Now)
                   select new TempLockInfo
                    {
                        LockEntity = cl,
                        CaseEntity = c
                    }).ToList();
        }

        private List<LockedCaseOverview> MapEntityToOverview(List<TempLockInfo> lockedCases)
        {
            var ret = new List<LockedCaseOverview>();
            var existUsers = new List<int>();

            foreach (var lc in lockedCases)
            {
                if (!existUsers.Contains(lc.LockEntity.User_Id))
                {
                    ret.Add(new LockedCaseOverview(lc.LockEntity.User,
                                                   lockedCases.Where(l => l.LockEntity.User_Id == lc.LockEntity.User_Id)
                                                              .Select(l => new LockInfo(
                                                                               l.LockEntity.Case_Id,
                                                                               l.CaseEntity.CaseNumber,
                                                                               l.CaseEntity.Customer_Id,
                                                                               l.CaseEntity.Customer.Name,
                                                                               l.LockEntity.CreatedTime)
                                                                      )
                                                            .ToList()));
                    existUsers.Add(lc.LockEntity.User_Id);
                }
            }           

            return ret;
        }
    }
}