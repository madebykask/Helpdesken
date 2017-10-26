using System.Data.Entity;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Dal.Mappers;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseHistoryRepository : IRepository<CaseHistory>
    {
        IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId);
        IEnumerable<CaseHistoryOverview> GetCaseHistories(int caseId);

        void SetNullProblemByProblemId(int problemId);

        CaseHistory GetCloneOfLatest(int caseId);
        CaseHistory GetCloneOfPenultimate(int caseId);
    }

    public class CaseHistoryRepository : RepositoryBase<CaseHistory>, ICaseHistoryRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview> _caseHistoryOverviewMapper;

        public CaseHistoryRepository(IDatabaseFactory databaseFactory, 
                                     IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview> caseHistoryOverviewMapper)
            : base(databaseFactory)
        {
            _caseHistoryOverviewMapper = caseHistoryOverviewMapper;
        }

        public IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            var q = (from ch in this.DataContext.CaseHistories
                     where ch.Case_Id == caseId
                     select ch);
            return q.OrderBy(l => l.Id);
        }

        public IEnumerable<CaseHistoryOverview> GetCaseHistories(int caseId)
        {
            var query = 
                from caseHistory in Table
                let emailLogs = caseHistory.Emaillogs.DefaultIfEmpty()
                where caseHistory.Case_Id == caseId
                select new CaseHistoryMapperData
                {
                    CaseHistory = caseHistory,
                    Category = caseHistory.Category,
                    Department = caseHistory.Department_Id != null ?
                        new DepartmentMapperData
                        {
                            Id = caseHistory.Department.Id,
                            DepartmentId = caseHistory.Department.DepartmentId,
                            DepartmentName = caseHistory.Department.DepartmentName,
                            SearchKey = caseHistory.Department.SearchKey,
                            Country = caseHistory.Department.Country
                        } : null,
                    RegistrationSourceCustomer = caseHistory.RegistrationSourceCustomer,
                    CaseType = caseHistory.CaseType,
                    ProductArea = caseHistory.ProductArea,
                    UserPerformer = caseHistory.Performer_User_Id != null ?
                        new UserMapperData
                        {
                            Id = caseHistory.UserPerformer.Id,
                            FirstName = caseHistory.UserPerformer.FirstName,
                            SurName = caseHistory.UserPerformer.SurName
                          
                        } : null,
                    UserResponsible = caseHistory.CaseResponsibleUser_Id != null ?
                        new UserMapperData
                        {
                            Id = caseHistory.UserResponsible.Id,
                            FirstName = caseHistory.UserResponsible.FirstName,
                            SurName = caseHistory.UserResponsible.SurName

                        } : null,
                    Priority = caseHistory.Priority,
                    WorkingGroup = caseHistory.WorkingGroup,
                    StateSecondary = caseHistory.StateSecondary,
                    Status = caseHistory.Status,

                    IsAbout_Department = caseHistory.IsAbout_Department_Id != null ? 
                        new DepartmentMapperData
                        {
                            Id = caseHistory.Department.Id,
                            DepartmentId = caseHistory.Department.DepartmentId,
                            DepartmentName = caseHistory.Department.DepartmentName,
                            SearchKey = caseHistory.Department.SearchKey,
                            Country = caseHistory.Department.Country
                        }
                        : null,

                    EmailLogs = emailLogs.Select(t => new EmailLogMapperData
                    {
                        Id = t.Id,
                        MailId = t.MailId,
                        EmailAddress = t.EmailAddress
                    })

                };

            var items = query.ToList();

            var result = items.Select(_caseHistoryOverviewMapper.Map).ToList();
            return result;
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
                .Skip(1).Take(1)
                .FirstOrDefault();
        }
    }
}
