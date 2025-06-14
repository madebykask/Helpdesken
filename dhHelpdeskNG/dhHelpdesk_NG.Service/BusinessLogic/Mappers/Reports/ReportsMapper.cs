﻿namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Department;
    using DH.Helpdesk.BusinessData.Models.FinishingCause;
    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CasesInProgressDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ClosedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.FinishingCause;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.ProductArea;

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
            var caseEntities = (from c in cases
                            join d in departments on c.Department_Id equals d.Id into dgj
                            join wg in workingGroups on c.WorkingGroup_Id equals wg.Id into wggj
                            join ct in caseTypes on c.CaseType_Id equals ct.Id into ctgj
                            join pa in productAreas on c.ProductArea_Id equals pa.Id into pagj
                            from department in dgj.DefaultIfEmpty()
                            from caseType in ctgj.DefaultIfEmpty()
                            from workingGroup in wggj.DefaultIfEmpty()
                            from productArea in pagj.DefaultIfEmpty()
                            select new
                                       {
                                           CaseTypeId = c.CaseType_Id,
                                           ProductAreaId = c.ProductArea_Id
                                       }).ToList();

            var caseTypesData = caseTypes.Select(t => new CaseTypeData
                                                             {
                                                                 Id = t.Id, 
                                                                 Name = t.Name,
                                                                 Details = t.RelatedField
                                                             })
                                                             .OrderBy(t => t.Name)
                                                             .ToList();

            var productAreaItems = productAreas.Select(a => new ProductAreaItem
                                                                {
                                                                    Id = a.Id,
                                                                    ParentId = a.Parent_ProductArea_Id,
                                                                    Name = a.Name
                                                                })
                                                                .OrderBy(a => a.Name)
                                                                .ToList()
                                                                .BuildLineRelations();

            var productAreasData = new List<ProductAreaData>();
            foreach (var productAreaItem in productAreaItems)
            {
                var casesDatas = new List<CasesData>();

                foreach (var caseTypeData in caseTypesData)
                {
                    casesDatas.Add(new CasesData(
                            caseEntities.Where(c => c.ProductAreaId == productAreaItem.Id && c.CaseTypeId == caseTypeData.Id).Count(),
                            productAreaItem.Id,
                            caseTypeData.Id));
                }

                productAreasData.Add(new ProductAreaData(productAreaItem, casesDatas));
            }

            foreach (var productArea in productAreasData)
            {
                if (productArea.ProductArea.ParentId.HasValue)
                {
                    productArea.Parent = productAreasData.FirstOrDefault(a => a.ProductArea.Id == productArea.ProductArea.ParentId);
                }

                productArea.Children.AddRange(productAreasData.Where(a => a.ProductArea.ParentId == productArea.ProductArea.Id));
            }

            return new CaseTypeArticleNoData(caseTypesData, productAreasData.Where(a => a.Parent == null).ToList());
        }

        public static LeadtimeFinishedCasesData MapToLeadtimeFinishedCasesData(
                                                IQueryable<Case> cases,
                                                IQueryable<Department> departments,      
                                                IQueryable<CaseType> caseTypes,                              
                                                IQueryable<WorkingGroupEntity> workingGroups,
                                                DateTime? periodFrom,
                                                DateTime? periodUntil,
                                                int leadTime,
                                                bool isShowDetails)
        {
            var entities = (from c in cases
                                join d in departments on c.Department_Id equals d.Id into dgj
                                join wg in workingGroups on c.WorkingGroup_Id equals wg.Id into wggj
                                join ct in caseTypes on c.CaseType_Id equals ct.Id into ctgj
                                from department in dgj.DefaultIfEmpty()
                                from caseType in ctgj.DefaultIfEmpty()
                                from workingGroup in wggj.DefaultIfEmpty()
                                select new
                                {
                                    c.FinishingDate,
                                    c.LeadTime
                                }).ToList();

            var numberOfCases = entities.Count;
            var numberOfCasesShorterEqual = entities.Count(e => e.LeadTime.IsHoursLessEqualDays(leadTime));
            var numberOfCasesLonger = entities.Count(e => e.LeadTime.IsHoursGreaterDays(leadTime));
            var casesByLeadTime = new List<FinishedCasesLeadTime>();
            var casesByLeadTimes = new List<FinishedCasesLeadTimes>();

            if (isShowDetails)
            {
                if (!periodFrom.HasValue)
                {
                    periodFrom = DateTime.Today.AddYears(-1);
                }

                if (!periodUntil.HasValue)
                {
                    periodUntil = DateTime.Today;
                }

                periodFrom = periodFrom.RoundToMonthOrGetCurrent();
                periodUntil = periodUntil.RoundToMonthOrGetCurrent();

                while (periodFrom <= periodUntil)
                {
                    var casesLeadTime = entities.Where(e => e.FinishingDate.Value.RoundToMonth() == periodFrom).ToList();
                    casesByLeadTime.Add(new FinishedCasesLeadTime(
                                            periodFrom.Value,
                                            casesLeadTime.Count,
                                            casesLeadTime.Count(e => e.LeadTime.IsHoursLessEqualDays(leadTime)),
                                            casesLeadTime.Count(e => e.LeadTime.IsHoursGreaterDays(leadTime))));

                    var numberOfCasesLeadTime = new List<int>
                                                    {
                                                        casesLeadTime.Count(e => e.LeadTime.IsHoursLessDay())                    
                                                    };
                    int i;
                    for (i = 1; i <= 10; i++)
                    {
                        numberOfCasesLeadTime.Add(casesLeadTime.Count(e => e.LeadTime.IsHoursEqualDays(i)));
                    }

                    numberOfCasesLeadTime.Add(casesLeadTime.Count(e => e.LeadTime.IsHoursGreaterDays(i)));

                    casesByLeadTimes.Add(new FinishedCasesLeadTimes(
                                        periodFrom.Value,
                                        casesLeadTime.Count,
                                        numberOfCasesLeadTime));

                    periodFrom = periodFrom.Value.AddMonths(1);
                }
            }

            return new LeadtimeFinishedCasesData(
                                leadTime,
                                numberOfCases,
                                numberOfCasesShorterEqual,
                                numberOfCasesLonger,
                                casesByLeadTime,
                                casesByLeadTimes);
        }

        public static LeadtimeActiveCasesData MapToLeadtimeActiveCasesData(
                                                IQueryable<Case> cases,
                                                IQueryable<Department> departments,      
                                                IQueryable<CaseType> caseTypes,
                                                int highHours,
                                                int mediumDays,
                                                int lowDays)
        {
            var entities = (from c in cases
                                join d in departments on c.Department_Id equals d.Id into dgj
                                join ct in caseTypes on c.CaseType_Id equals ct.Id into ctgj
                                from department in dgj.DefaultIfEmpty()
                                from caseType in ctgj.DefaultIfEmpty()
                                select new
                                {
                                    c.LeadTime
                                }).ToList();

            var highHoursResult = new CasesTimeLeftOnSolution(highHours, entities.Count(c => c.LeadTime <= highHours));
            var mediumDaysResult = new List<CasesTimeLeftOnSolution>();
            var lowDaysResult = new List<CasesTimeLeftOnSolution>();

            for (int i = 0; i <= mediumDays; i++)
            {
                mediumDaysResult.Add(new CasesTimeLeftOnSolution(i, entities.Count(c => c.LeadTime.IsHoursEqualDays(i))));
            }

            for (int i = 0; i <= lowDays; i++)
            {
                lowDaysResult.Add(new CasesTimeLeftOnSolution(i, entities.Count(c => c.LeadTime.IsHoursEqualDays(i))));
            }

            return new LeadtimeActiveCasesData(highHoursResult, mediumDaysResult, lowDaysResult);
        }

        public static FinishingCauseCustomerData MapToFinishingCauseCustomerData(
                                                IQueryable<Case> cases,
                                                IQueryable<Department> departments,      
                                                IQueryable<WorkingGroupEntity> workingGroups,
                                                IQueryable<CaseType> caseTypes,  
                                                IQueryable<User> administrators,
                                                IQueryable<FinishingCause> finishingCauses, 
                                                DateTime? periodFrom,
                                                DateTime? periodUntil)
        {
            var entities = (from c in cases
                            join d in departments on c.Department_Id equals d.Id into dgj
                            join wg in workingGroups on c.WorkingGroup_Id equals wg.Id into wggj
                            join ct in caseTypes on c.CaseType_Id equals ct.Id into ctgj
                            join u in administrators on c.Performer_User_Id equals u.Id into ugj
                            from department in dgj.DefaultIfEmpty()
                            from caseType in ctgj.DefaultIfEmpty()
                            from workingGroup in wggj.DefaultIfEmpty()
                            from user in ugj.DefaultIfEmpty()
                            where c.Department_Id.HasValue && c.Logs.Any(l => l.FinishingType.HasValue)
                            select new FinishingCauseCase
                            {
                                CaseId = c.Id,
                                DepartmentId = c.Department_Id,
                                FinishingCause = c.Logs.Where(l => l.FinishingType.HasValue).Select(l => l.FinishingType).FirstOrDefault()
                            }).ToList();

            var fcs = finishingCauses.Select(f => new { f.Id, f.Parent_FinishingCause_Id, f.Name })
                                    .ToList()
                                    .Select(f => new FinishingCauseItem(f.Id, f.Parent_FinishingCause_Id, f.Name))
                                    .ToList()
                                    .BuildRelations();
            var ds = departments.Select(d => new DepartmentOverview
                                                 {
                                                     DepartmentId = d.Id,
                                                     DepartmentName = d.DepartmentName
                                                 })
                                                 .OrderBy(d => d.DepartmentName)
                                                 .ToList();

            var rows = new List<FinishingCauseRow>();
            foreach (var fc in fcs)
            {
                BuildFinishingCauseRows(rows, fc, ds, entities);
            }

            return new FinishingCauseCustomerData(rows, ds);
        }

        public static FinishingCauseCategoryCustomerData MapToFinishingCauseCategoryCustomerData(
                                            IQueryable<Case> cases,
                                            IQueryable<Department> departments,
                                            IQueryable<WorkingGroupEntity> workingGroups,
                                            IQueryable<CaseType> caseTypes,
                                            IQueryable<User> users,
                                            DateTime? periodFrom,
                                            DateTime? periodUntil)
        {
            var entities = (from c in cases
                            join d in departments on c.Department_Id equals d.Id into dgj
                            join wg in workingGroups on c.WorkingGroup_Id equals wg.Id into wggj
                            join ct in caseTypes on c.CaseType_Id equals ct.Id into ctgj
                            join u in users on c.Performer_User_Id equals u.Id into ugj
                            from department in dgj.DefaultIfEmpty()
                            from caseType in ctgj.DefaultIfEmpty()
                            from workingGroup in wggj.DefaultIfEmpty()
                            from user in ugj.DefaultIfEmpty()
                            where c.Department_Id.HasValue
                            select new FinishingCauseCategoryCustomerCase
                            {
                                DepartmentId = c.Department_Id.Value,
                                HasFinishingCause = c.Logs.Any(l => l.FinishingType.HasValue)
                            }).ToList();

            var ds = departments.Select(d => new FinishingCauseCategoryCustomerDepartment
                                                {
                                                    DepartmentId = d.Id,
                                                    DepartmentName = d.DepartmentName,
                                                    NumberOfUsers = d.Users.Count
                                                })
                                                 .OrderBy(d => d.DepartmentName)
                                                 .ToList();

            var rows = new List<FinishingCauseCategoryCustomerRow>();
            foreach (var department in ds)
            {
                var departmentCases = entities.Where(c => c.DepartmentId == department.DepartmentId);
                var departmentCasesCount = departmentCases.Count();
                var finishedCasesCount = departmentCases.Count(c => c.HasFinishingCause);
                var row = new FinishingCauseCategoryCustomerRow(
                            department.DepartmentName,
                            department.NumberOfUsers,
                            finishedCasesCount,
                            department.NumberOfUsers > 0 ? Math.Round(((double)finishedCasesCount / department.NumberOfUsers) * 100, 1) : (double?)null,
                            departmentCasesCount > 0 ? Math.Round(((double)finishedCasesCount / departmentCasesCount) * 100, 1) : (double?)null);
                rows.Add(row);
            }

            return new FinishingCauseCategoryCustomerData(rows);
        }

        public static ClosedCasesDayData MapToClosedCasesDayData(
                                    IQueryable<Case> cases,
                                    IQueryable<Department> departments,
                                    IQueryable<WorkingGroupEntity> workingGroups,
                                    IQueryable<CaseType> caseTypes,
                                    IQueryable<User> administrators,
                                    DateTime period)
        {
            var entities = (from c in cases
                            join d in departments on c.Department_Id equals d.Id into dgj
                            join wg in workingGroups on c.WorkingGroup_Id equals wg.Id into wggj
                            join ct in caseTypes on c.CaseType_Id equals ct.Id into ctgj
                            join u in administrators on c.Performer_User_Id equals u.Id into ugj
                            from department in dgj.DefaultIfEmpty()
                            from caseType in ctgj.DefaultIfEmpty()
                            from workingGroup in wggj.DefaultIfEmpty()
                            from user in ugj.DefaultIfEmpty()
                            where c.FinishingDate.HasValue
                            select new 
                            {
                                c.FinishingDate
                            }).ToList();

            var list = entities.Select(e => new ClosedCasesDayCase(e.FinishingDate.Value)).ToList();
            return new ClosedCasesDayData(list);
        }

        public static CasesInProgressDayData MapToCasesInProgressDayData(
                                    IQueryable<Case> cases,
                                    IQueryable<Department> departments,
                                    IQueryable<WorkingGroupEntity> workingGroups,
                                    IQueryable<User> administrators,
                                    DateTime period)
        {
            var entities = (from c in cases
                            join d in departments on c.Department_Id equals d.Id into dgj
                            join wg in workingGroups on c.WorkingGroup_Id equals wg.Id into wggj
                            join u in administrators on c.Performer_User_Id equals u.Id into ugj
                            from department in dgj.DefaultIfEmpty()
                            from workingGroup in wggj.DefaultIfEmpty()
                            from user in ugj.DefaultIfEmpty()
                            select new 
                            {
                                c.RegTime
                            }).ToList();

            var list = entities.Select(e => new CasesInProgressDayCase(e.RegTime)).ToList();
            return new CasesInProgressDayData(list);
        }

        private static void BuildFinishingCauseRows(
                        List<FinishingCauseRow> rows,
                        FinishingCauseItem finishingCause,
                        List<DepartmentOverview> departments,
                        List<FinishingCauseCase> cases)
        {
            var columns = new List<FinishingCauseColumn>();
            foreach (var department in departments)
            {
                var cs = cases.Where(c => c.FinishingCause == finishingCause.Id && c.DepartmentId == department.DepartmentId);
                var casesNumber = cs.Count();
                var total = cases.Count(c => c.DepartmentId == department.DepartmentId); 
                var column = new FinishingCauseColumn(
                            casesNumber,
                            total != 0 ? Math.Round(((double)casesNumber / total) * 100, 1) : 0,
                            cs.Select(c => c.CaseId).ToList());
                columns.Add(column);
            }

            var row = new FinishingCauseRow(finishingCause, columns);
            rows.Add(row);

            foreach (var child in finishingCause.Children)
            {
                BuildFinishingCauseRows(rows, child, departments, cases);
            }
        }
    }
}