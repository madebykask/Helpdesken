namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Reports
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;
    using DH.Helpdesk.Domain;

    public static class ReportsMapper
    {
        public static RegistratedCasesDayData MapToRegistratedCasesDayData(
                                                IQueryable<Case> cases,
                                                IQueryable<Department> departments,
                                                IQueryable<CaseType> caseTypes,
                                                IQueryable<WorkingGroupEntity> workingGroups,
                                                IQueryable<User> administrators)
        {
            var entities = (from c in cases
                            join d in departments on c.Department_Id equals d.Id into dgj
                            join ct in caseTypes on c.CaseType_Id equals ct.Id into ctgj
                            join wg in workingGroups on c.WorkingGroup_Id equals wg.Id into wggj
                            join u in administrators on c.Performer_User_Id equals u.Id into ugj
                            from department in dgj.DefaultIfEmpty()
                            from caseType in ctgj.DefaultIfEmpty()
                            from workingGroup in wggj.DefaultIfEmpty()
                            from user in ugj.DefaultIfEmpty()
                            select new { c.RegTime }).ToList();

            var registeredCases = entities.Select(e => new RegisteredCaseDay(e.RegTime)).ToList();
            return new RegistratedCasesDayData(registeredCases);
        }

        public static CaseTypeArticleNoData MapToCaseTypeArticleNoData(
                                                IQueryable<Case> cases,
                                                IQueryable<Department> departments,                                                
                                                IQueryable<WorkingGroupEntity> workingGroups,
                                                IQueryable<CaseType> caseTypes,
                                                IQueryable<ProductArea> productAreas)
        {
            return new CaseTypeArticleNoData();
        }
    }
}