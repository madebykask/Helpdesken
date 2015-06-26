
namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Dal.Dal;
    

    public sealed class CaseLockRepository : Repository, ICaseLockRepository
    {
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

        public IEnumerable<CaseLock> GetAllLockedCases()
        {
            var entities = this.DbContext.CaseLock.OrderBy(l => l.User_Id)
                                                  .ThenBy(l=> l.CreatedTime)
                                                  .ToList();

            return entities
                .Select(this._caseLockToBusinessModelMapper.Map);
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

    }
}