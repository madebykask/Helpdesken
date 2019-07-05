using System.Data.Entity;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Problem.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.Projects.Output;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using System.Data.Linq;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using BusinessData.Models;
    using System;

    public interface ICaseHistoryRepository : IRepository<CaseHistory>
    {
        IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId);
        IEnumerable<CaseHistoryMapperData> GetCaseHistories(int caseId);
        Task<List<CaseHistoryMapperData>> GetCaseHistoriesAsync(int caseId);

        void SetNullProblemByProblemId(int problemId);

        CaseHistory GetCloneOfLatest(int caseId);
        CaseHistory GetCloneOfPenultimate(int caseId);
        CaseHistory GetCaseHistoryByProblemId(int caseId, int problemId);
    }

    public class CaseHistoryRepository : RepositoryBase<CaseHistory>, ICaseHistoryRepository
    {

        public CaseHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            var q = (from ch in this.DataContext.CaseHistories
                     where ch.Case_Id == caseId
                     select ch);
            return q.OrderBy(l => l.Id);
        }

        public IEnumerable<CaseHistoryMapperData> GetCaseHistories(int caseId)
        {
            var query = GetCaseHistoriesQuery(caseId);

            return query.ToList();
        }

        public Task<List<CaseHistoryMapperData>> GetCaseHistoriesAsync(int caseId)
        {
            var query = GetCaseHistoriesQuery(caseId);

            return query.ToListAsync();
        }

        public void SetNullProblemByProblemId(int problemId)
        {
            var cases = this.DataContext.CaseHistories.Where(x => x.Problem_Id == problemId).ToList();

            foreach (var item in cases)
            {
                item.Problem_Id = null;
            }
        }

        public CaseHistory GetCloneOfLatest(int caseId)
        {
            return this.DataContext.Set<CaseHistory>()
                .AsNoTracking()
                .Where(it => it.Case_Id == caseId)
                .OrderByDescending(it => it.Id)
                .FirstOrDefault();
        }

        public CaseHistory GetCloneOfPenultimate(int caseId)
        {
            return DataContext.Set<CaseHistory>()
                .AsNoTracking()
                .Where(it => it.Case_Id == caseId)
                .OrderByDescending(it => it.Id)
                .Skip(1)
                .FirstOrDefault();
        }

        public CaseHistory GetCaseHistoryByProblemId(int caseId, int problemId)
        {
            return this.DataContext.Set<CaseHistory>()
                .AsNoTracking()
                .Where(it => it.Case_Id == caseId && it.Problem_Id == problemId)
                .OrderByDescending(it => it.Id)
                .FirstOrDefault();
        }

        private IQueryable<CaseHistoryMapperData> GetCaseHistoriesQuery(int caseId)
        {
            return 
                from caseHistory in Table
                where caseHistory.Case_Id == caseId
                select new CaseHistoryMapperData
                {
                    CaseHistory = caseHistory,

                    Category = caseHistory.Category_Id != null ? new CategoryOverview()
                    {
                        Id = caseHistory.Category.Id,
                        Name = caseHistory.Category.Name
                    } : null,

                    Department = caseHistory.Department_Id != null ? new DepartmentOverview
                    {
                        Id = caseHistory.Department.Id,
                        DepartmentId = caseHistory.Department.DepartmentId,
                        DepartmentName = caseHistory.Department.DepartmentName,
                        SearchKey = caseHistory.Department.SearchKey,
                        CountryName = caseHistory.Department.Country != null ? caseHistory.Department.Country.Name : null
                    } : null,
                    
                    RegistrationSourceCustomer = caseHistory.RegistrationSourceCustomer != null ? new RegistrationSourceCustomerOverview
                    {
                        Id = caseHistory.RegistrationSourceCustomer.Id,
                        SourceName = caseHistory.RegistrationSourceCustomer.SourceName
                    } : null,

                    CaseType = new CaseTypeOverview()
                    {
                        Id = caseHistory.CaseType.Id,
                        Name = caseHistory.CaseType.Name
                    },

                    ProductArea = caseHistory.ProductArea_Id != null ? new ProductAreaOverview()
                    {
                        Id = caseHistory.ProductArea_Id.Value,
                        Name = caseHistory.ProductArea.Name
                    } : null,

                    CausingPart = caseHistory.CausingPartId != null ? new CausingPartOverview()
                    {
                        Id = caseHistory.CausingPart.Id,
                        Name = caseHistory.CausingPart.Name
                    } : null,

                    Problem =  caseHistory.Problem_Id != null ? new ProblemOverview()
                    {
                        Id = caseHistory.Problem.Id,
                        Name = caseHistory.Problem.Name
                    } : null,

                    Region = caseHistory.Region_Id != null ? new RegionOverview()
                    {
                        Id = caseHistory.Region_Id,
                        Name = caseHistory.Region.Name
                    } : null,

                    OU = caseHistory.OU_Id != null ? new OUOverview()
                    {
                        Id = caseHistory.OU_Id,
                        Name = caseHistory.OU.Name
                    } : null,

                    Project = caseHistory.Project_Id != null ? new ProjectOverview()  
                    {
                        Id = caseHistory.Project.Id,
                        Name = caseHistory.Project.Name
                    } : null,

                    UserPerformer = caseHistory.Performer_User_Id != null ? new UserBasicOvierview()
                    {
                        Id = caseHistory.UserPerformer.Id,
                        FirstName = caseHistory.UserPerformer.FirstName,
                        SurName = caseHistory.UserPerformer.SurName
                    } : null,

                    UserResponsible = caseHistory.CaseResponsibleUser_Id != null ? new UserBasicOvierview()
                    {
                        Id = caseHistory.UserResponsible.Id,
                        FirstName = caseHistory.UserResponsible.FirstName,
                        SurName = caseHistory.UserResponsible.SurName

                    } : null,

                    Priority = caseHistory.Priority_Id != null ? new PriorityOverview()
                    {
                        Id = caseHistory.Priority.Id,
                        Name = caseHistory.Priority.Name
                    } : null,

                    WorkingGroup = caseHistory.WorkingGroup != null ? new WorkingGroupOverview
                    {
                        Id = caseHistory.WorkingGroup.Id,
                        WorkingGroupName = caseHistory.WorkingGroup.WorkingGroupName
                    } : null,

                    StateSecondary = caseHistory.StateSecondary_Id != null ? new StateSecondaryOverview()
                    {
                        Id = caseHistory.StateSecondary.Id,
                        Name = caseHistory.StateSecondary.Name
                    } : null,

                    Status = caseHistory.Status_Id != null ? new StatusOverview()
                    {
                        Id = caseHistory.Status.Id,
                        Name = caseHistory.Status.Name
                    } : null,

                    IsAbout_Department = caseHistory.IsAbout_Department_Id != null ?  new DepartmentOverview
                    {
                        Id = caseHistory.IsAbout_Department.Id,
                        DepartmentId = caseHistory.IsAbout_Department.DepartmentId,
                        DepartmentName = caseHistory.IsAbout_Department.DepartmentName,
                        SearchKey = caseHistory.IsAbout_Department.SearchKey,
                        CountryName = caseHistory.IsAbout_Department.Country != null ? caseHistory.IsAbout_Department.Country.Name : null
                    } : null,

                    IsAbout_Region = caseHistory.IsAbout_Region_Id != null ? new RegionOverview
                    {
                        Id = caseHistory.IsAbout_Region.Id,
                        Name = caseHistory.IsAbout_Region.Name,
                    } : null,

                    IsAbout_OU = caseHistory.IsAbout_OU_Id != null ? new OUOverview
                    {
                        Id = caseHistory.IsAbout_OU.Id,
                        Name = caseHistory.IsAbout_OU.Name,
                    } : null,

                    System = caseHistory.System_Id != null ? new SystemOverview
                    {
                        Id = caseHistory.System.Id,
                        Name = caseHistory.System.SystemName,
                    } : null,

                    Urgency = caseHistory.Urgency_Id != null ? new UrgencyOverview
                    {
                        Id = caseHistory.Urgency_Id,
                        Name = caseHistory.Urgency.Name,
                    } : null,

                    Impact = caseHistory.Impact_Id != null ? new ImpactOverview
                    {
                        Id = caseHistory.Impact_Id,
                        Name = caseHistory.Impact.Name,
                    } : null,

                    EmailLogs = caseHistory.Emaillogs.Select(t => new EmailLogsOverview
                    {
                        Id = t.Id,
                        MailId = t.MailId,
                        EmailAddress = t.EmailAddress
                    }).ToList()
                };
        }

    }
}
