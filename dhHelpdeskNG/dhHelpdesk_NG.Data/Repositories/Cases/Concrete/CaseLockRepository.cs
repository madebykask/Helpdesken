using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Domain;

    public sealed class CaseLockRepository : RepositoryBase<CaseLockEntity>, ICaseLockRepository
    {
        public class TempLockInfo
        {
            public TempLockInfo()
            {
            }

            public Case CaseEntity { get; set; }

            public CaseLockEntity LockEntity { get; set; }
        }

        private readonly IBusinessModelToEntityMapper<CaseLock, CaseLockEntity> _caseLockToEntityMapper;

        #region ctor()

        public CaseLockRepository(
                IDatabaseFactory databaseFactory,
                IBusinessModelToEntityMapper<CaseLock, CaseLockEntity> caseLockToEntityMapper)
                : base(databaseFactory)
        {
            this._caseLockToEntityMapper = caseLockToEntityMapper;
        }

        #endregion

        public CaseLockEntity GetCaseLockByCaseId(int caseId)
        {
            return GetCaseLockByCaseIdQuery(caseId).FirstOrDefault();
        }

        public Task<CaseLockEntity> GetCaseLockByCaseIdAsync(int caseId)
        {
            return GetCaseLockByCaseIdQuery(caseId).FirstOrDefaultAsync();
        }

        public CaseLockEntity GetCaseLockByGUID(Guid lockGUID)
        {
            return GetCaseByGUIDQuery(lockGUID).FirstOrDefault();
        }

        public Task<CaseLockEntity> GetCaseLockByGUIDAsync(Guid lockGUID)
        {
            return GetCaseByGUIDQuery(lockGUID).FirstOrDefaultAsync();
        }

        public void CaseLockCleanUp()
        {
            var affectDate = DateTime.Now.AddDays(-1);

            var recordsForCleaning = Table.Where(l => l.ExtendedTime < affectDate).ToList();

            if (recordsForCleaning.Any())
            {
                this.DataContext.CaseLock.RemoveRange(recordsForCleaning);
                this.Commit();
            }
        }

        public IDictionary<int, ICaseLockOverview> GetCasesLock(int[] caseIds)
        {
            var resultMap = new Dictionary<int, ICaseLockOverview>();

            var items =
                Table.Where(l => caseIds.Contains(l.Case_Id))
                     .Select(ProjectToCaseLockOverview())
                     .ToList();

            foreach (var caseLockItem in items)
            {
                if (!resultMap.ContainsKey(caseLockItem.CaseId))
                {
                    resultMap.Add(caseLockItem.CaseId, caseLockItem);
                }
            }

            return resultMap;
        }

        public ICaseLockOverview GetCaseLockOverviewByCaseId(int caseId)
        {
            var item = Table.Where(l => l.Case_Id == caseId)
                            .Select(ProjectToCaseLockOverview())
                            .FirstOrDefault();
            return item;
        }

        private Expression<Func<CaseLockEntity, CaseLockOverview>> ProjectToCaseLockOverview()
        {
            Expression<Func<CaseLockEntity, CaseLockOverview>>  exp = 
                l => new CaseLockOverview
                {
                    Id = l.Id,
                    CaseId =  l.Case_Id,
                    UserId = l.User_Id,
                    LockGUID = l.LockGUID,
                    BrowserSession = l.BrowserSession,
                    CreatedTime = l.CreatedTime,
                    ExtendedTime = l.ExtendedTime,
                    User = new CaseLockUserInfo
                    {
                        Id = l.Id,
                        UserId = l.User.UserID,
                        FirstName = l.User.FirstName,
                        LastName = l.User.SurName
                    }
                };

            return exp;
        }

        public void LockCase(CaseLock caseLock)
        {
            var entity = new CaseLockEntity();
            this._caseLockToEntityMapper.Map(caseLock, entity);

            this.Add(entity);
            this.Commit();
        }

        public bool DeleteCaseLock(CaseLockEntity caseLock)
        {
            if (caseLock != null)
            {
                Delete(caseLock);
                Commit();
                return true;
            }

            return false;
        }

        public void DeleteCaseLockByCaseId(int caseId)
        {
            var entity = Table.Where(l => l.Case_Id == caseId).FirstOrDefault();
            if (entity != null)
            {
                this.Delete(entity);
                this.Commit();
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

        public IQueryable<ICaseLockOverview> GetLockedCases(int[] caseIds, int bufferTime)
        {
            //Reason for adding logic here:
            //System.Data.Entity is already defined here so that we can use DbFunctions.
            var items =
                Table.Where(l => caseIds.Contains(l.Case_Id) && DbFunctions.AddSeconds(l.ExtendedTime, bufferTime) > DateTime.Now)
                     .Select(ProjectToCaseLockOverview())
                     .OrderByDescending(x => x.ExtendedTime) //If there are several, it needs to be orded this way
                    ;
            return items;
        }

        private List<TempLockInfo> GetAlllockedCaseEntities()
        {
            // Get cases which were locked at least at 3 seconds ago
            var curTime = DateTime.Now.AddSeconds(-3);
            return (from cl in this.DataContext.CaseLock
                    join c in this.DataContext.Cases on cl.Case_Id equals c.Id
                    where (cl.ExtendedTime > curTime)
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

        private IQueryable<CaseLockEntity> GetCaseLockByCaseIdQuery(int caseId)
        {
            return Table.Where(l => l.Case_Id == caseId);
        }

        private IQueryable<CaseLockEntity> GetCaseByGUIDQuery(Guid lockGUID)
        {
            return Table.Where(l => l.LockGUID == lockGUID);
        }
    }
}
