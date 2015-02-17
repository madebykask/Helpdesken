namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;
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
                if (isShowDetails)
                {
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
                }

                periodFrom = periodFrom.Value.AddMonths(1);
            }

            return new LeadtimeFinishedCasesData(
                                leadTime,
                                numberOfCases,
                                numberOfCasesShorterEqual,
                                numberOfCasesLonger,
                                casesByLeadTime,
                                casesByLeadTimes);
        }
    }
}