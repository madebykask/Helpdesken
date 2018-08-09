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
    using DH.Helpdesk.BusinessData.Enums.Case.Fields;
    using DH.Helpdesk.Common.Enums;

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

        public static FinishingCauseCategoryCustomerOptions MapToFinishingCauseCategoryCustomerOptions(
                                                        IQueryable<Department> departments,
                                                        IQueryable<WorkingGroupEntity> workingGroups,
                                                        IQueryable<CaseType> caseTypes)
        {
            var options = GetOptions(departments, caseTypes, workingGroups);

            return new FinishingCauseCategoryCustomerOptions(
                            options.Departments,
                            options.CaseTypes,
                            options.WorkingGroups);
        }

        public static ClosedCasesDayOptions MapToClosedCasesDayOptions(
                                                        IQueryable<Department> departments,
                                                        IQueryable<WorkingGroupEntity> workingGroups,
                                                        IQueryable<CaseType> caseTypes,
                                                        IQueryable<User> administrators)
        {
            var options = GetOptions(departments, caseTypes, workingGroups, administrators);

            return new ClosedCasesDayOptions(
                            options.Departments,
                            options.CaseTypes,
                            options.WorkingGroups,
                            options.Administrators);
        }

        public static CasesInProgressDayOptions MapToCasesInProgressDayOptions(
                                                        IQueryable<Department> departments,
                                                        IQueryable<WorkingGroupEntity> workingGroups,
                                                        IQueryable<User> administrators)
        {
            var options = GetOptions(departments, null, workingGroups, administrators);

            return new CasesInProgressDayOptions(
                            options.Departments,
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

            var departmentsResult = new List<ItemOverview>();
            var caseTypesOverviewsResult = new List<ItemOverview>();
            var workingGroupsResult = new List<ItemOverview>();
            var administratorsResult = new List<ItemOverview>();

            if (departments != null)
            {
                departmentsResult = departments.OrderBy(d=> d.DepartmentName).ToList()
                                               .Select(d => new ItemOverview(d.DepartmentName, d.Id.ToString(CultureInfo.InvariantCulture)))
                                               .ToList();
            }

            if (caseTypes != null)
            {
                IQueryable<UnionItemOverview> union;
                caseTypes = caseTypes.Where(t => t.Id != Common.Constants.CaseType.EmptyId);
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
                caseTypesOverviewsResult = query.OrderBy(c => c.Name).ToList()
                                                .Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture)))
                                                .ToList();
            }

            if (workingGroups != null)
            {
                var wgs = workingGroups.Select(g => new UnionItemOverview { Id = g.Id, Name = g.WorkingGroupName, Type = "WorkingGroups" });                
                workingGroupsResult = wgs.OrderBy(c => c.Name).ToList()
                                         .Select(w => new ItemOverview(w.Name, w.Id.ToString(CultureInfo.InvariantCulture)))
                                         .ToList();
            }

            if (administrators != null)
            {
                var union = administrators.Select(a => new UnionItemOverview { Id = a.Id, Name = a.FirstName + " " + a.SurName, Type = "Administrators" });
                query = query == null ? union : query.Union(union);
                administratorsResult = query.OrderBy(c => c.Name).ToList()
                                            .Select(a => new ItemOverview(a.Name, a.Id.ToString(CultureInfo.InvariantCulture)))
                                            .ToList();
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

            string[] exceptionList = {"IsAbout_Region_Id", "IsAbout_Department_Id", "IsAbout_OU_Id", "AddFollowersBtn" };
            if (fields != null)
            {
                fieldsResult = fields
                                .Where(f => !exceptionList.Contains(f.Name))
                                .Select(f => new { f.Id, f.Name })
                                .ToList()
                                .Select(f => new ItemOverview(f.Name, f.Id.ToString(CultureInfo.InvariantCulture)))                                
                                .ToList();

                // Add calculation fields manually to the available fields
                var leadTimeId = Convert.ToInt32(CalculationFields.LeadTime).ToString();
                fieldsResult.Add(new ItemOverview(CaseInfoFields.LeadTime, leadTimeId));

                var twId = Convert.ToInt32(CalculationFields.TotalWork).ToString();
                fieldsResult.Add(new ItemOverview(LogFields.TotalWork, twId));

                var totId = Convert.ToInt32(CalculationFields.TotalOverTime).ToString();
                fieldsResult.Add(new ItemOverview(LogFields.TotalOverTime, totId));

                var tmId = Convert.ToInt32(CalculationFields.TotalMaterial).ToString();
                fieldsResult.Add(new ItemOverview(LogFields.TotalMaterial, tmId));

                var tpId = Convert.ToInt32(CalculationFields.TotalPrice).ToString();
                fieldsResult.Add(new ItemOverview(LogFields.TotalPrice, tpId));
            }
  
            if (caseTypeRootsOnly)
            {
                caseTypesResult = caseTypesOverviewsResult.Select(
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

                        return new CaseTypeItem(int.Parse(o.Value), parentId, name);
                    }).ToList().BuildRelations();     
            }
            else
            {
                caseTypesOverviewsResult = caseTypesOverviewsResult.Select(o => new ItemOverview(o.Name, o.Value)).ToList();
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