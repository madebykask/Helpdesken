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
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Reports.Data;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Shared.Data;

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
            var options = GetOptions(departments, caseTypes, workingGroups, administrators, null, null, false);

            return new RegistratedCasesDayOptions(
                            options.Departments,
                            options.CaseTypesOverviews,
                            options.WorkingGroups,
                            options.Administrators);
        }

        public static CaseTypeArticleNoOptions MapToCaseTypeArticleNoOptions(
                                        IQueryable<Department> departments,                                       
                                        IQueryable<WorkingGroupEntity> workingGroups,
                                        IQueryable<CaseType> caseTypes,
                                        IQueryable<ProductArea> productAreas,
                                        bool productAreaLineRelations = false)
        {
            var options = GetOptions(departments, caseTypes, workingGroups, null, productAreas, null, false, productAreaLineRelations);

            return new CaseTypeArticleNoOptions(
                            options.Departments,                            
                            options.WorkingGroups,
                            options.CaseTypesOverviews,
                            options.ProductAreas);
        }

        public static ReportGeneratorOptions MapToReportGeneratorOptions(
                                        IQueryable<CaseFieldSetting> fields,
                                        IQueryable<Department> departments,
                                        IQueryable<WorkingGroupEntity> workingGroups,
                                        IQueryable<CaseType> caseTypes,
                                        int languageId)
        {
            var options = GetOptions(departments, caseTypes, workingGroups, null, null, fields, true, false, languageId);

            return new ReportGeneratorOptions(
                            options.Fields,
                            options.Departments,
                            options.WorkingGroups,
                            options.CaseTypes);
        }

        public static LeadtimeFinishedCasesOptions MapToLeadtimeFinishedCasesOptions(
                                                        IQueryable<Department> departments,
                                                        IQueryable<CaseType> caseTypes,
                                                        IQueryable<WorkingGroupEntity> workingGroups)
        {
            var options = GetOptions(departments, caseTypes, workingGroups);

            return new LeadtimeFinishedCasesOptions(
                            options.Departments,                                        
                            options.CaseTypes,
                            options.WorkingGroups);
        }

        public static LeadtimeActiveCasesOptions MapToLeadtimeActiveCasesOptions(
                                                        IQueryable<Department> departments,
                                                        IQueryable<CaseType> caseTypes)
        {
            var options = GetOptions(departments, caseTypes);

            return new LeadtimeActiveCasesOptions(
                            options.Departments,                                        
                            options.CaseTypes);
        }

        public static FinishingCauseCustomerOptions MapToFinishingCauseCustomerOptions(
                                                        IQueryable<Department> departments,
                                                        IQueryable<WorkingGroupEntity> workingGroups,
                                                        IQueryable<CaseType> caseTypes,
                                                        IQueryable<User> administrators)
        {
            var options = GetOptions(departments, caseTypes, workingGroups, administrators);

            return new FinishingCauseCustomerOptions(
                            options.Departments,
                            options.CaseTypes,
                            options.WorkingGroups,
                            options.Administrators);
        }

        private static ReportOptions GetOptions(
                                        IQueryable<Department> departments = null,
                                        IQueryable<CaseType> caseTypes = null,
                                        IQueryable<WorkingGroupEntity> workingGroups = null,
                                        IQueryable<User> administrators = null,
                                        IQueryable<ProductArea> productAreas = null,
                                        IQueryable<CaseFieldSetting> fields = null,
                                        bool caseTypeRootsOnly = true,
                                        bool productAreaLineRelations = false,
                                        int? languageId = null)
        {
            var caseTypesResult = new List<CaseTypeItem>();
            var productAreasResult = new List<ProductAreaItem>();
            var fieldsResult = new List<ItemOverview>();

            IQueryable<UnionItemOverview> query = null;
            var separator = Guid.NewGuid().ToString();

            if (departments != null)
            {
                query = departments.Select(d => new UnionItemOverview { Id = d.Id, Name = d.DepartmentName, Type = "Departments" });
            }

            if (caseTypes != null)
            {
                IQueryable<UnionItemOverview> union;
                if (caseTypeRootsOnly)
                {
                    union = caseTypes.Select(t => new UnionItemOverview
                            {
                                Id = t.Id,
                                Name = t.Name + separator + t.Parent_CaseType_Id,
                                Type = "CaseTypes"
                            });
                }
                else
                {
                    union = caseTypes.Select(t => new UnionItemOverview { Id = t.Id, Name = t.Name, Type = "CaseTypes" });
                }

                query = query == null ? union : query.Union(union);
            }

            if (workingGroups != null)
            {
                var union = workingGroups.Select(g => new UnionItemOverview { Id = g.Id, Name = g.WorkingGroupName, Type = "WorkingGroups" });
                query = query == null ? union : query.Union(union);
            }

            if (administrators != null)
            {
                var union = administrators.Select(a => new UnionItemOverview { Id = a.Id, Name = a.FirstName + " " + a.SurName, Type = "Administrators" });
                query = query == null ? union : query.Union(union);
            }

            if (productAreas != null)
            {
                var productAreaEntities = productAreas.Select(a => new ProductAreaItem
                                                    {
                                                        Id = a.Id,
                                                        ParentId = a.Parent_ProductArea_Id,
                                                        Name = a.Name
                                                    })
                                                    .OrderBy(a => a.Name)
                                                    .ToList();

                productAreasResult = productAreaLineRelations ? productAreaEntities.BuildLineRelations() : productAreaEntities.BuildRelations();
            }

            if (fields != null)
            {
                fieldsResult = fields.Select(f => new
                                {
                                    f.Id,
                                    Caption = f.CaseFieldSettingLanguages.FirstOrDefault(l => l.Language_Id == languageId).Label,
                                    FieldName = f.Name
                                })
                                .ToList()
                                .Select(f => new ItemOverview(!string.IsNullOrEmpty(f.Caption) ? f.Caption : f.FieldName, f.Id.ToString(CultureInfo.InvariantCulture)))
                                .OrderBy(f => f.Name)
                                .ToList();
            }

            var overviews = query != null ? query
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToList() : new List<UnionItemOverview>();

            var departmentsResult = overviews.Where(o => o.Type == "Departments").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList();
            var caseTypesOverviewsResult = overviews.Where(o => o.Type == "CaseTypes").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList();
            var workingGroupsResult = overviews.Where(o => o.Type == "WorkingGroups").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList();
            var administratorsResult = overviews.Where(o => o.Type == "Administrators").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList();
            
            if (caseTypeRootsOnly)
            {
                caseTypesResult = overviews.Where(o => o.Type == "CaseTypes").Select(
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
                    }).ToList().BuildRelations();     
            }
            else
            {
                caseTypesOverviewsResult = overviews.Where(o => o.Type == "CaseTypes").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList();
            }                  

            return new ReportOptions(
                    departmentsResult,
                    caseTypesOverviewsResult,
                    caseTypesResult,
                    productAreasResult,
                    workingGroupsResult,
                    administratorsResult,
                    fieldsResult);
        }
    }
}