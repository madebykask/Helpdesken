namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Reports
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;

    public static class ReportsMapper
    {
        public static RegistratedCasesDayOptions MapToRegistratedCasesDayOptions(
                                                IQueryable<Department> departments,
                                                IQueryable<CaseType> caseTypes,
                                                IQueryable<WorkingGroupEntity> workingGroups,
                                                IQueryable<User> administrators)
        {
            var overviews = departments.Select(d => new { d.Id, Name = d.DepartmentName, Type = "Departments" }).Union(
                            caseTypes.Select(t => new { t.Id, t.Name, Type = "CaseTypes" }).Union(
                            workingGroups.Select(g => new { g.Id, Name = g.WorkingGroupName, Type = "WorkingGroups" }).Union(
                            administrators.Select(a => new { a.Id, Name = a.FirstName + " " + a.SurName, Type = "Administrators" }))))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToList();

            return new RegistratedCasesDayOptions(
                            overviews.Where(o => o.Type == "Departments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),
                            overviews.Where(o => o.Type == "CaseTypes").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),
                            overviews.Where(o => o.Type == "WorkingGroups").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),
                            overviews.Where(o => o.Type == "Administrators").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList());
        }

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
    }
}