namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;

    public static class CaseOverviewMapper
    {
        public static string GetCaseTypeFullName(this int caseTypeId, IQueryable<CaseType> caseTypeQuery)
        {
            var res = string.Empty;
            var ct = caseTypeQuery.Where(c => c.Id == caseTypeId).FirstOrDefault();
            if (ct != null)
            {
                if (ct.Parent_CaseType_Id.HasValue)
                    res = ct.Parent_CaseType_Id.Value.GetCaseTypeFullName(caseTypeQuery) + " - " + ct.Name;
                else
                    res = ct.Name;
            }
            return res;
        }

        public static string GetProductAreaFullName(string strProductId, List<ProductArea> productAreaQuery)
        {                        
            var res = string.Empty;
            if (!string.IsNullOrEmpty(strProductId))
            {
                int productId = 0;
                if (int.TryParse(strProductId, out productId))
                {
                    var pa = productAreaQuery.Where(p => p.Id == productId).FirstOrDefault();
                    if (pa != null)
                    {
                        if (pa.Parent_ProductArea_Id.HasValue)
                            res = GetProductAreaFullName(pa.Parent_ProductArea_Id.Value.ToString(), productAreaQuery) + " - " + pa.Name;
                        else
                            res = pa.Name;
                    }
                }
            }
            return res;
        }

        private static string GetCategoryFullName(string strCategoryId, IList<Category> categories)
        {
            var res = string.Empty;
            if (!string.IsNullOrEmpty(strCategoryId))
            {
                var categoryId = 0;
                if (int.TryParse(strCategoryId, out categoryId))
                {
                    var category = categories.Where(p => p.Id == categoryId).FirstOrDefault();
                    if (category != null)
                    {
                        if (category.Parent_Category_Id.HasValue)
                            res = GetCategoryFullName(category.Parent_Category_Id.Value.ToString(), categories) + " - " + category.Name;
                        else
                            res = category.Name;
                    }
                }
            }
            return res;
        }

        public static string GetOUFullName(this string strOUId, IQueryable<OU> organizationUnitQuery)
        {            
            var res = string.Empty;
            if (!string.IsNullOrEmpty(strOUId))
            {
                int ouId = 0;
                if (int.TryParse(strOUId, out ouId))
                {
                    var ou = organizationUnitQuery.Where(o => o.Id == ouId).FirstOrDefault();
                    if (ou != null)
                    {
                        if (ou.Parent_OU_Id.HasValue)
                            res = ou.Parent_OU_Id.ToString().GetOUFullName(organizationUnitQuery) + " - " + ou.Name;
                        else
                            res = ou.Name;
                    }
                }
            }
            return res;
        }

        public static string GetClosingReasonFullName(this string strClosingReasonId, IQueryable<FinishingCause> finishingCauseQuery)
        {           
             var res = string.Empty;
             if (!string.IsNullOrEmpty(strClosingReasonId))
             {
                int closingReasonId = 0;
                if (int.TryParse(strClosingReasonId, out closingReasonId))
                {
                    var cr = finishingCauseQuery.Where(c => c.Id == closingReasonId).FirstOrDefault();
                    if (cr != null)
                    {
                        if (cr.Parent_FinishingCause_Id.HasValue)
                            res = cr.Parent_FinishingCause_Id.ToString().GetClosingReasonFullName(finishingCauseQuery) + " - " + cr.Name;
                        else
                            res = cr.Name;
                    }
                }
             }
            return res;
        }             

        public static List<FullCaseOverview> MapToCaseOverviews(this List<ReportGeneratorFields> query, 
                                                                IQueryable<CaseType> caseTypes, 
                                                                IQueryable<ProductArea> productAreas, 
                                                                IQueryable<OU> ous,
                                                                IQueryable<FinishingCause> finishingCauses,
                                                                IQueryable<Category> categoryies)
        {
            var ret = new List<FullCaseOverview>();
            var caseTypeFullNames = new Dictionary<int, string>();
            var availableCaseTypes = query.Select(c=> c.CaseType).Distinct().ToList();
            foreach (var ct in availableCaseTypes)
            {
                caseTypeFullNames.Add(ct, ct.GetCaseTypeFullName(caseTypes));
            }

            var productAreaFullNames = new Dictionary<string, string>();
            var availableProductAreas = query.Select(c => c.ProductArea).Distinct().ToList();
            foreach (var pa in availableProductAreas)
            {                
                productAreaFullNames.Add(pa, GetProductAreaFullName(pa, productAreas.ToList()));
            }

            var ouFullNames = new Dictionary<string, string>();
            var availableOUs = query.Select(c => c.Unit).Distinct().ToList();
            foreach (var ou in availableOUs)
            {
                ouFullNames.Add(ou, ou.GetOUFullName(ous));
            }

            var closingReasonFullNames = new Dictionary<string, string>();
            var availableClosingReasons = query.Where(c=> c.LogData != null )
                                               .Select(c => c.LogData.FinishingType?.ToString() ?? string.Empty)
                                               .Distinct()
                                               .ToList();

            foreach (var cr in availableClosingReasons)
            {
                closingReasonFullNames.Add(cr, cr.GetClosingReasonFullName(finishingCauses));
            }

            var categoriesFullNames = new Dictionary<string, string>();
            var availablecategories = query.Select(c => c.Category).Distinct().ToList();
            foreach (var c in availablecategories)
            {
                categoriesFullNames.Add(c, GetCategoryFullName(c, categoryies.ToList()));
            }

            ret = query.Select(q => new FullCaseOverview(
                                q.Id,
                                new UserOverview(q.User, q.Notifier, q.Email, q.Phone, q.CellPhone,q.Customer, q.Region, q.Department,
                                                 ouFullNames[q.Unit], q.Place, q.OrdererCode, q.IsAbout_User, q.IsAbout_Persons_Name,
                                                 q.IsAbout_Persons_Phone, q.IsAbout_UserCode, q.IsAbout_Persons_Email, q.IsAbout_Persons_CellPhone,
                                                 q.IsAbout_CostCentre, q.IsAbout_Place, 
                                                 string.Empty,string.Empty, string.Empty),
                                                 // Disabled to Release version 5.3.21
                                                 //q.IsAbout_Department,
                                                 //ouFullNames[q.IsAbout_OU]"", q.IsAbout_Region),

                                new ComputerOverview(q.PcNumber, q.ComputerType, q.ComputerPlace),

                                new CaseInfoOverview(q.Case, q.RegistrationDate, q.ChangeDate, q.RegistratedBy,
                                                     caseTypeFullNames[q.CaseType],
                                                     productAreaFullNames[q.ProductArea], 
                                                     q.System, q.UrgentDegree, q.Impact, categoriesFullNames[q.Category],
                                                     q.Supplier, q.InvoiceNumber, q.ReferenceNumber, q.Caption, q.Description,
                                                     q.Other, Convert.ToBoolean(q.PhoneContact), Convert.ToBoolean(q.Sms), q.AgreedDate, q.Available, q.Cost, 
                                                     string.Empty, q.RegistrationSource, q.SolvedInTime),

                                new OtherOverview (q.WorkingGroup, q.Responsible, q.Administrator, q.Priority, q.State, q.SubState,
                                                   q.PlannedActionDate, q.WatchDate, Convert.ToBoolean(q.Verified), q.VerifiedDescription,
                                                   q.SolutionRate, q.CausingPart),

                                q.LogData != null ? new LogsOverview((q.LogData.FinishingType.HasValue && q.FinishingDate.HasValue? closingReasonFullNames[q.LogData.FinishingType.ToString()] : string.Empty), 
                                                                      q.FinishingDate, q.FinishingDescription, q.AllInternalText, q.AllExtenalText, q.TotalMaterial, q.TotalOverTime, q.TotalPrice, q.TotalWork) : new LogsOverview(string.Empty, null, string.Empty, null, null)

                                )).ToList();
           
            return ret;
        }


        private static FullCaseOverview CreateFullOverview(Case entity, CaseStatistic caseStatistic)
        {                        
            var id = entity.Id;
            var user = CreateUserOverview(entity);
            var computer = CreateComputerOverview(entity);
            var caseInfo = CreateCaseInfoOverview(entity, caseStatistic);
            var other = CreateOtherOverview(entity);
            var log = CreateLogOverview(entity);

            return new FullCaseOverview(
                        id,
                        user,
                        computer,
                        caseInfo,
                        other,
                        log);
        }

        private static UserOverview CreateUserOverview(Case entity)
        {
            return new UserOverview(
                        entity.ReportedBy,
                        entity.PersonsName,
                        entity.PersonsEmail,
                        entity.PersonsPhone,
                        entity.PersonsCellphone,
                        entity.Customer.Name,
                        entity.Region.Name,
                        entity.Department.DepartmentName,
                        entity.Ou.Name,
                        entity.Place,
                        entity.UserCode,
                        entity.IsAbout.ReportedBy,
                        entity.IsAbout.UserCode,
                        entity.IsAbout.Person_Name,
                        entity.IsAbout.Person_Phone,
                        entity.IsAbout.Person_Cellphone,
                        entity.IsAbout.Person_Email,
                        entity.IsAbout.CostCentre,
                        entity.IsAbout.Place,
                        entity.IsAbout.Department_Id.ToString(),
                        entity.IsAbout.Region_Id.ToString(),
                        entity.IsAbout.OU_Id.ToString());
        }

        private static ComputerOverview CreateComputerOverview(Case entity)
        {
            return new ComputerOverview(
                        entity.InventoryNumber,
                        entity.InventoryType,
                        entity.InventoryLocation);
        }

        private static CaseInfoOverview CreateCaseInfoOverview(Case entity, CaseStatistic caseStatistics)
        {
            var registratedBy = new UserName(entity.User.FirstName, entity.User.SurName);
            var attachedFile = entity.CaseFiles.Any() ? entity.CaseFiles.First().FileName : string.Empty;
            var solvedInTime = (caseStatistics != null ? caseStatistics.WasSolvedInTime : null);
            var sourceName = entity.RegistrationSourceCustomer == null
                                 ? string.Empty
                                 : entity.RegistrationSourceCustomer.SourceName; 

            return new CaseInfoOverview(
                        entity.CaseNumber,
                        entity.RegTime,
                        entity.ChangeTime,
                        registratedBy.GetFullName(),
                        entity.CaseType.Name,
                        entity.ProductArea.Name,
                        entity.System.SystemName,
                        entity.Urgency.Name,
                        entity.Impact.Name,
                        entity.Category.Name,
                        entity.Supplier.Name,
                        entity.InvoiceNumber,
                        entity.ReferenceNumber,
                        entity.Caption,
                        entity.Description,
                        entity.Miscellaneous,
                        entity.ContactBeforeAction.ToBool(),
                        entity.SMS.ToBool(),
                        entity.AgreedDate,
                        entity.Available,
                        entity.Cost,
                        attachedFile,
                        sourceName,
                        solvedInTime);
        }

        private static OtherOverview CreateOtherOverview(Case entity)
        {
            var caseResponsibleUser = new UserName(entity.CaseResponsibleUser.FirstName, entity.CaseResponsibleUser.SurName);
            var administrator = new UserName(entity.Administrator.FirstName, entity.Administrator.SurName);

            return new OtherOverview(
                        entity.Workinggroup.WorkingGroupName,
                        caseResponsibleUser.GetFullName(),
                        administrator.GetFullName(),
                        entity.Priority.Name,
                        entity.Status.Name,
                        entity.StateSecondary.Name,
                        entity.PlanDate,
                        entity.WatchDate,
                        entity.Verified.ToBool(),
                        entity.VerifiedDescription,
                        entity.SolutionRate,
                        entity.CausingPart.Name);
        }

        private static LogsOverview CreateLogOverview(Case entity)
        {
            var logs = new List<LogOverview>();
            var lastLog = entity.Logs.OrderByDescending(l=> l.LogDate).FirstOrDefault();
            if (lastLog != null)
            {
                var lastLogOverview = new LogOverview(
                                                lastLog.Text_Internal,
                                                lastLog.Text_External,
                                                lastLog.Charge.ToBool(),
                                                lastLog.LogFiles != null && lastLog.LogFiles.Any() ? lastLog.LogFiles.First().FileName : string.Empty,
                                                entity.FinishingDescription,
                                                lastLog.FinishingDate,
                                                lastLog.FinishingTypeEntity != null? lastLog.FinishingTypeEntity.Name : string.Empty);
                logs.Add(lastLogOverview);
            }

            return new LogsOverview(string.Empty, null, string.Empty, null, null);
        }

        private static LogsOverview GetLogOverview(string finishingCause, DateTime? closingDate)
        {
            var logs = new List<LogOverview>();
            var logOverview = new LogOverview(
                                            string.Empty,
                                            string.Empty,
                                            false,
                                            string.Empty,
                                            string.Empty,
                                            closingDate,                                            
                                            finishingCause);
            logs.Add(logOverview);           
            return new LogsOverview("", null, "", null, null);
        }
    }
}