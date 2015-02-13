namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.CaseType;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.ProductArea;

    public static class ReportsOptionsMapper
    {
        public static List<ReportType> MapToCustomerAvailableReports(
                                        IQueryable<Customer> customers,
                                        IQueryable<ReportCustomer> reports)
        {
            var entities = (from rc in reports
                           join c in customers on rc.Customer_Id equals c.Id
                           where rc.ShowOnPage != 0
                           select new { rc.Report_Id })
                           .ToList();

            return entities.Select(r => (ReportType)r.Report_Id).ToList();
        } 

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

        public static CaseTypeArticleNoOptions MapToCaseTypeArticleNoOptions(
                                        IQueryable<Department> departments,                                       
                                        IQueryable<WorkingGroupEntity> workingGroups,
                                        IQueryable<CaseType> caseTypes,
                                        IQueryable<ProductArea> productAreas,
                                        bool productAreaLineRelations = false)
        {
            var overviews = departments.Select(d => new { d.Id, Name = d.DepartmentName, Type = "Departments" }).Union(
                            workingGroups.Select(g => new { g.Id, Name = g.WorkingGroupName, Type = "WorkingGroups" }).Union(
                            caseTypes.Select(t => new { t.Id, t.Name, Type = "CaseTypes" })))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToList();

            var productAreaEntities = productAreas.Select(a => new ProductAreaItem
                                                                 {
                                                                     Id = a.Id, 
                                                                     ParentId = a.Parent_ProductArea_Id, 
                                                                     Name = a.Name
                                                                 })
                                                                 .OrderBy(a => a.Name)
                                                                 .ToList();
            var productAreasItems = productAreaLineRelations ? productAreaEntities.BuildLineRelations() : productAreaEntities.BuildRelations();

            return new CaseTypeArticleNoOptions(
                            overviews.Where(o => o.Type == "Departments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),                            
                            overviews.Where(o => o.Type == "WorkingGroups").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),
                            overviews.Where(o => o.Type == "CaseTypes").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),
                            productAreasItems);
        }

        public static ReportGeneratorOptions MapToReportGeneratorOptions(
                                        IQueryable<CaseFieldSetting> fields,
                                        IQueryable<Department> departments,
                                        IQueryable<WorkingGroupEntity> workingGroups,
                                        IQueryable<CaseType> caseTypes,
                                        int languageId)
        {
            var separator = Guid.NewGuid().ToString();

            var overviews = departments.Select(d => new { d.Id, Name = d.DepartmentName, Type = "Department" }).Union(
                            workingGroups.Select(g => new { g.Id, Name = g.WorkingGroupName, Type = "WorkingGroup" }).Union(
                            caseTypes.Select(t => new
                                                      {
                                                          t.Id, 
                                                          Name = t.Name + separator + t.Parent_CaseType_Id, 
                                                          Type = "CaseType"
                                                      })))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToList();

            var fs = fields.Select(f => new
                                   {
                                       f.Id, 
                                       Caption = f.CaseFieldSettingLanguages.FirstOrDefault(l => l.Language_Id == languageId).Label,
                                       FieldName = f.Name
                                   })
                                   .ToList()
                                   .Select(f => new ItemOverview(!string.IsNullOrEmpty(f.Caption) ? f.Caption : f.FieldName, f.Id.ToString(CultureInfo.InvariantCulture)))
                                   .OrderBy(f => f.Name)
                                   .ToList();

            return new ReportGeneratorOptions(
                            fs,
                            overviews.Where(o => o.Type == "Department").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),
                            overviews.Where(o => o.Type == "WorkingGroup").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),
                            overviews.Where(o => o.Type == "CaseType").Select(
                                o =>
                                    {
                                        var values = o.Name.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                                        string name = values[0];
                                        int? parentId = null;
                                        int parentIdVal;
                                        if (values.Length > 1 && int.TryParse(values[1], out parentIdVal))
                                        {
                                            parentId = parentIdVal;
                                        }
                                        return new CaseTypeItem(o.Id, parentId, name);
                                    }).ToList().BuildRelations());
        }

        public static LeadtimeFinishedCasesOptions MapToLeadtimeFinishedCasesOptions(
                                                        IQueryable<Department> departments,
                                                        IQueryable<CaseType> caseTypes,
                                                        IQueryable<WorkingGroupEntity> workingGroups)
        {
            var separator = Guid.NewGuid().ToString();

            var overviews = departments.Select(d => new { d.Id, Name = d.DepartmentName, Type = "Department" }).Union(
                            workingGroups.Select(g => new { g.Id, Name = g.WorkingGroupName, Type = "WorkingGroup" }).Union(
                            caseTypes.Select(t => new
                            {
                                t.Id,
                                Name = t.Name + separator + t.Parent_CaseType_Id,
                                Type = "CaseType"
                            })))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToList();

            return new LeadtimeFinishedCasesOptions(
                            overviews.Where(o => o.Type == "Department").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList(),                                        
                            overviews.Where(o => o.Type == "CaseType").Select(
                                o =>
                                {
                                    var values = o.Name.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                                    string name = values[0];
                                    int? parentId = null;
                                    int parentIdVal;
                                    if (values.Length > 1 && int.TryParse(values[1], out parentIdVal))
                                    {
                                        parentId = parentIdVal;
                                    }
                                    return new CaseTypeItem(o.Id, parentId, name);
                                }).ToList().BuildRelations(),
                                overviews.Where(o => o.Type == "WorkingGroup").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList());
        }
    }
}