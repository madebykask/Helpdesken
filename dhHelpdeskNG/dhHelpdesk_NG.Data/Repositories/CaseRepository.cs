using System.Data.Entity;

namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
    using DH.Helpdesk.Domain;
    using Mappers;
    using System.Linq.Expressions;
    using System.Data.Linq.SqlClient;

    public interface ICaseRepository : IRepository<Case>
    {
        Case GetCaseById(int id, bool markCaseAsRead = false);
        Case GetCaseByGUID(Guid GUID);
        Case GetCaseByEmailGUID(Guid GUID);
        Case GetDetachedCaseById(int id);
        Case GetDetachedCaseIncludesById(int id);
        DynamicCase GetDynamicCase(int id);
        IList<Case> GetProjectCases(int customerId, int projectId);
        IList<Case> GetProblemCases(int customerId, int problemId);
        void SetNullProblemByProblemId(int problemId);
        void UpdateFinishedDate(int problemId, DateTime? time);
        void UpdateFollowUpDate(int caseId, DateTime? time);
        void Activate(int caseId);
        void MarkCaseAsUnread(int id);
        void MarkCaseAsRead(int id);
        IEnumerable<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user);
        IEnumerable<CaseOverview> GetCaseOverviews(int[] customers);
        int LookupLanguage(int custid, string notid, int regid, int depid, string notifierid);
        List<DynamicCase> GetAllDynamicCases(int customerId, int[] caseIds);



        Case GetCaseIncluding(int id);

        CaseModel GetCase(int id);

        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        CaseOverview GetCaseOverview(int caseId);

        MyCase[] GetMyCases(int userId, int? count = null);
        IList<Case> GetProblemCases(int problemId);
        IList<int> GetCasesIdsByType(int caseTypeId);

        List<Case> GetTop100CasesToTest();
        Case GetCaseQuickOpen(UserOverview user, Expression<Func<Case, bool>> casePermissionFilter, string searchFor);
    }

    public class CaseRepository : RepositoryBase<Case>, ICaseRepository
    {
        private readonly IWorkContext workContext;
        private readonly IEntityToBusinessModelMapper<Case, CaseModel> _caseToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<CaseModel, Case> _caseModelToEntityMapper;

        public CaseRepository(
            IDatabaseFactory databaseFactory,
            IWorkContext workContext, 
            IEntityToBusinessModelMapper<Case, CaseModel> caseToBusinessModelMapper,
            IBusinessModelToEntityMapper<CaseModel, Case> caseModelToEntityMapper)
            : base(databaseFactory)
        {
            this.workContext = workContext;
            _caseModelToEntityMapper = caseModelToEntityMapper;
            _caseToBusinessModelMapper = caseToBusinessModelMapper;
        }

        public Case GetCaseById(int id, bool markCaseAsRead = false)
        {
            if (markCaseAsRead)
                MarkCaseAsRead(id);

            return (from w in this.DataContext.Cases
                    where w.Id == id
                    select w).FirstOrDefault();
        }

        public List<DynamicCase> GetAllDynamicCases(int customerId, int[] caseIds)
        {
            var query = from f in this.DataContext.Forms
                        join ff in this.DataContext.FormField on f.Id equals ff.Form_Id
                        join ffv in this.DataContext.FormFieldValue on ff.Id equals ffv.FormField_Id
                        where f.Customer_Id == customerId && f.ExternalPage == 1 && caseIds.Contains(ffv.Case_Id)
                        select new DynamicCase
                        {
                            CaseId = ffv.Case_Id,
                            FormPath = f.FormPath
                        };

            return query.Distinct().ToList();
        }

        public IList<Case> GetProjectCases(int customerId, int projectId)
        {
            var cases = this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.Project_Id == projectId).ToList();
            return cases;
        }

        public IList<Case> GetProblemCases(int customerId, int problemId)
        {
            var cases = this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.Problem_Id == problemId).ToList();
            return cases;
        }

        public DynamicCase GetDynamicCase(int id)
        {
            var query = from f in this.DataContext.Forms
                        join ffu in this.DataContext.FormUrls on f.FormUrl_Id equals ffu.Id
                        join ff in this.DataContext.FormField on f.Id equals ff.Form_Id
                        join ffv in this.DataContext.FormFieldValue on ff.Id equals ffv.FormField_Id
                        where ffv.Case_Id == id
                        select new DynamicCase
                        {
                            CaseId = ffv.Case_Id,
                            FormPath = f.FormPath,
                            FormName = f.FormName,
                            ViewMode = f.ViewMode,
                            ExternalPage = f.ExternalPage == 1 ? true : false,
                            Scheme = ffu.Scheme,
                            Host = ffu.Host,
                            ExternalSite = ffu.ExternalSite
                        };

            return query.FirstOrDefault();
        }

        public Case GetCaseByGUID(Guid GUID)
        {
            var caseEntity = DataContext.Cases.Where(c => c.CaseGUID == GUID)
                                              .FirstOrDefault();

            return caseEntity;
        }

        public Case GetCaseByEmailGUID(Guid GUID)
        {
            Case ret = null;
            var caseHistoryId = DataContext.EmailLogs.Where(e => e.EmailLogGUID == GUID && e.CaseHistory_Id != null)
                                                     .Select(e => e.CaseHistory_Id)
                                                     .FirstOrDefault();
            if (caseHistoryId != null)
            {
                var caseId = DataContext.CaseHistories.Where(h => h.Id == caseHistoryId).Select(h => h.Case_Id).FirstOrDefault();

                if (caseId != null)
                    ret = DataContext.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            }

            return ret;
        }

        public Case GetDetachedCaseById(int id)
        {
            return (from w in this.DataContext.Set<Case>().AsNoTracking()
                    where w.Id == id
                    select w).FirstOrDefault();
        }

        public Case GetDetachedCaseIncludesById(int id)
        {
            var result = DataContext.Cases.AsNoTracking()
            .Include(x => x.CaseFollowers)
            .Include(x => x.IsAbout)
            .FirstOrDefault(x => x.Id == id);
            return result;
        }

        public void SetNullProblemByProblemId(int problemId)
        {
            var cases =
                this.DataContext.Cases.Where(x => x.Problem_Id == problemId).ToList();

            foreach (var item in cases)
            {
                item.Problem_Id = null;
            }
        }

        public void Activate(int caseId)
        {
            var cases = this.DataContext.Cases.Where(x => x.Id == caseId).FirstOrDefault();
            if (cases != null)
            {
                cases.FinishingDate = null;
                cases.ApprovedBy_User_Id = null;
                cases.ApprovedDate = null;
                cases.LeadTime = 0;
                cases.ChangeTime = DateTime.UtcNow;

                foreach (var log in cases.Logs)
                {
                    log.FinishingType = null;
                }

                this.Update(cases);
                this.Commit();
            }
        }

        public void UpdateFinishedDate(int problemId, DateTime? time)
        {
            var cases = this.DataContext.Cases.Where(x => x.Problem_Id == problemId).ToList();

            foreach (var item in cases)
            {
                item.FinishingDate = time;
            }
        }

        public void UpdateFollowUpDate(int caseId, DateTime? time)
        {
            var cases = this.DataContext.Cases.Where(x => x.Id == caseId).FirstOrDefault();
            if (cases != null)
            {
                cases.FollowUpDate = time;
                this.Update(cases);
                this.Commit();
            }
        }

        public void SetNullProjectByProjectId(int projectId)
        {
            var cases = this.DataContext.Cases.Where(x => x.Project_Id == projectId).ToList();

            foreach (var item in cases)
            {
                item.Project_Id = null;
            }
        }

        public void MarkCaseAsUnread(int id)
        {
            SetCaseUnreadFlag(id, 1);
        }

        public IEnumerable<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user)
        {
            var query = from c in this.DataContext.Cases
                        where c.Customer_Id == customerId
                        && c.Id != id
                        && c.ReportedBy.ToLower() == reportedBy.ToLower()
                        select c;
            //handläggare
            if (user.RestrictedCasePermission == 1 && user.UserGroupId == 2)
                query = query.Where(c => c.Performer_User_Id == user.Id || c.CaseResponsibleUser_Id == user.Id);

            //anmälare
            if (user.RestrictedCasePermission == 1 && user.UserGroupId == 1)
                query = query.Where(c => c.ReportedBy.ToLower() == user.UserId.ToLower());

            return query.Select(c => new CaseRelation()
            {
                Id = c.Id,
                Caption = c.Caption,
                Description = c.Description,
                CaseNumber = c.CaseNumber,
                FinishingDate = c.FinishingDate,
                Regtime = c.RegTime
            });
        }

        public IEnumerable<CaseOverview> GetCaseOverviews(int[] customers)
        {
            return DataContext.Cases
                .Where(c => customers.Contains(c.Customer_Id))
                .Select(c => new CaseOverview()
                {
                    CustomerId = c.Customer_Id,
                    Deleted = c.Deleted,
                    FinishingDate = c.FinishingDate,
                    StatusId = c.Status_Id,
                    UserId = c.User_Id
                });
        }

        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        public CaseOverview GetCaseOverview(int caseId)
        {
            var query = (from _case in DataContext.Cases
                         from caseHistory in _case.CaseHistories.DefaultIfEmpty() //will load CaseHistories with left join
                         where _case.Id == caseId
                         select _case);

            return query.Select(CaseToCaseOverviewMapper.Map).FirstOrDefault();
        }

        public MyCase[] GetMyCases(int userId, int? count = null)
        {
            var entities = from cs in this.DataContext.Cases
                           join cus in this.DataContext.Customers on cs.Customer_Id equals cus.Id
                           where (cs.Performer_User_Id == userId && cs.FinishingDate == null && cs.Deleted == 0)
                           orderby (cs.ChangeTime) descending
                           select new
                           {
                               Id = cs.Id,
                               CaseNumber = cs.CaseNumber,
                               RegistrationDate = cs.RegTime,
                               ChangedDate = cs.ChangeTime,
                               Subject = cs.Caption,
                               InitiatorName = cs.PersonsName,
                               Description = cs.Description,
                               CustomerName = cus.Name
                           };

            if (count.HasValue)
            {
                entities = entities.Take(count.Value);
            }

            return entities
                .ToArray()
                .Select(c => new MyCase(
                                c.Id,
                                c.CaseNumber,
                                c.RegistrationDate,
                                c.ChangedDate,
                                c.Subject,
                                c.InitiatorName,
                                c.Description,
                                c.CustomerName))
                                .ToArray();
        }

        public IList<int> GetCasesIdsByType(int caseTypeId)
        {
            return DataContext.Cases
                              .Where(c => c.CaseType_Id == caseTypeId)
                              .Select(o => o.Id).ToList();
        }

        public IList<Case> GetProblemCases(int problemId)
        {
            return DataContext.Cases.Where(x => x.Problem_Id == problemId).ToList();
        }

        public void MarkCaseAsRead(int id)
        {
            SetCaseUnreadFlag(id);
        }

        private void SetCaseUnreadFlag(int id, int unread = 0)
        {
            var cases = this.DataContext.Cases.Single(c => c.Id == id);
            cases.Unread = unread;
            this.Update(cases);
            this.Commit();
        }

        public Case GetCaseIncluding(int id)
        {
            return DataContext.Cases
                .Include(x => x.Department)
                .Include(x => x.Workinggroup)
                .FirstOrDefault(x => x.Id == id);
        }
        
        public CaseModel GetCase(int id)
        {
            var caseEntity =  DataContext.Cases.Find(id);
            return _caseToBusinessModelMapper.Map(caseEntity);
        }

        public int LookupLanguage(int custid, string notid, int regid, int depid, string notifierid)
        {
            string sql = string.Empty;
            int langid = 0;
            bool match = false;

            //Initiation
           // if (notid != string.Empty && notifierid != string.Empty)
            if (notifierid != string.Empty)
            {

                var not = this.DataContext.ComputerUsers.FirstOrDefault(c=> c.UserId == notifierid && c.Customer_Id == custid);
                // var not = this.DataContext.ComputerUsers.FirstOrDefault(c => c.UserCode == notid && c.UserId == notifierid && c.Customer_Id == custid);
                // var not = this.DataContext.ComputerUsers.Single(c=> c.UserId == notifierid);
                if (not != null)
                {
                    if (not.LanguageId > 0)
                    {
                        langid = Convert.ToInt32(not.LanguageId);
                        match = true;
                    }

                }
            }

            //Department
            if (match == false)
            {
                if (depid > 0)
                {
                    var dep = this.DataContext.Departments.Single(c => c.Id == depid);
                    if (dep != null)
                    {
                        if (dep.LanguageId > 0)
                        {
                            langid = Convert.ToInt32(dep.LanguageId);
                            match = true;
                        }

                    }


                }
            }

            //Region
            if (match == false)
            {
                if (regid > 0)
                {
                    var reg = this.DataContext.Regions.Single(c => c.Id == regid);
                    if (reg != null)
                    {
                        if (reg.LanguageId > 0)
                        {
                            langid = Convert.ToInt32(reg.LanguageId);
                            match = true;
                        }

                    }


                }
            }

            //Customer
            if (match == false)
            {
                if (custid > 0)
                {
                    var cust = this.DataContext.Customers.Single(c => c.Id == custid);
                    if (cust != null)
                    {
                        if (cust.Language_Id > 0)
                        {
                            langid = Convert.ToInt32(cust.Language_Id);
                            match = true;
                        }

                    }


                }
            }
            return langid;
        }


        public List<Case> GetTop100CasesToTest()
        {
            return DataContext.Cases.Take(100).ToList();            
        }

        public Case GetCaseQuickOpen(UserOverview currentUser, Expression<Func<Case, bool>> casePermissionFilter, string searchFor)
        {

            IQueryable<Case> query;

            query = from _case in DataContext.Set<Case>()
                    from user in _case.Customer.Users
                    where (_case.CaseNumber.ToString() == searchFor.Replace("#", "")) && user.Id == currentUser.Id
                    orderby _case.RegTime descending
                    select _case;

            if (casePermissionFilter != null)
            {
                query = query.Where(casePermissionFilter);
            }

            if (query.Any())
                return query.FirstOrDefault();

            return null;
        }
    }
}