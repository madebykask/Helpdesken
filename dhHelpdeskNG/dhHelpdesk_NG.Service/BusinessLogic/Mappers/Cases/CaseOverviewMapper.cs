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
       
        //public static List<FullCaseOverview> MapToCaseOverviews(this IQueryable<Case> query, CaseDataSet caseDataSet)
        //{            
        //    var ret = new List<FullCaseOverview>();
        //    foreach (var caseEntity in query.ToList())
        //    {
        //        caseEntity.Customer = caseDataSet.CustomerQuery.Where(c => c.Id == caseEntity.Customer_Id).FirstOrDefault() ?? new Customer() { Name = string.Empty};

        //        caseEntity.CaseType = new CaseType { Name = string.Empty };
        //        caseEntity.CaseType.Name = caseEntity.CaseType_Id.GetCaseTypeFullName(caseDataSet.CaseTypeQuery.AsQueryable());
                
        //        caseEntity.RegLanguage = caseDataSet.LanguageQuery.Where(l => l.Id == caseEntity.RegLanguage_Id).FirstOrDefault() ?? new Language() { Name = string.Empty };                

        //        caseEntity.Region = new Region { Name = string.Empty };
        //        if (caseEntity.Region_Id.HasValue)
        //            caseEntity.Region = caseDataSet.RegionQuery.Where(r => r.Id == caseEntity.Region_Id.Value).FirstOrDefault() ?? caseEntity.Region;

        //        caseEntity.Department = new Department { DepartmentName = string.Empty };
        //        if (caseEntity.Department_Id.HasValue)
        //            caseEntity.Department = caseDataSet.DepartmentQuery.Where(r => r.Id == caseEntity.Department_Id.Value).FirstOrDefault() ?? caseEntity.Department;

        //        caseEntity.Ou = new OU { Name = string.Empty };
        //        if (caseEntity.OU_Id.HasValue)                                 
        //            caseEntity.Ou.Name = caseEntity.OU_Id.GetOUFullName(caseDataSet.OrganizationUnitQuery.AsQueryable());                

        //        caseEntity.User = new User { FirstName = string.Empty, SurName = string.Empty };
        //        if (caseEntity.User_Id.HasValue)
        //            caseEntity.User = caseDataSet.UserQuery.Where(u => u.Id == caseEntity.User_Id.Value).FirstOrDefault() ?? caseEntity.User;

        //        caseEntity.CaseResponsibleUser = new User { FirstName = string.Empty, SurName = string.Empty };
        //        if (caseEntity.CaseResponsibleUser_Id.HasValue)
        //            caseEntity.CaseResponsibleUser = caseDataSet.UserQuery.Where(u => u.Id == caseEntity.CaseResponsibleUser_Id.Value).FirstOrDefault() ?? caseEntity.CaseResponsibleUser;

        //        caseEntity.Administrator = new User { FirstName = string.Empty, SurName = string.Empty };
        //        if (caseEntity.Performer_User_Id.HasValue)
        //            caseEntity.Administrator = caseDataSet.UserQuery.Where(u => u.Id == caseEntity.Performer_User_Id.Value).FirstOrDefault() ?? caseEntity.Administrator;

        //        caseEntity.LastChangedByUser= new User { FirstName = string.Empty, SurName = string.Empty };
        //        if (caseEntity.ChangeByUser_Id.HasValue)
        //            caseEntity.LastChangedByUser = caseDataSet.UserQuery.Where(u => u.Id == caseEntity.ChangeByUser_Id.Value).FirstOrDefault() ?? caseEntity.LastChangedByUser;

        //        caseEntity.Category = new Category { Name = string.Empty };
        //        if (caseEntity.Category_Id.HasValue)
        //            caseEntity.Category = caseDataSet.CategoryQuery.Where(c => c.Id == caseEntity.Category_Id.Value).FirstOrDefault() ?? caseEntity.Category;

        //        caseEntity.ProductArea = new ProductArea { Name = string.Empty };
        //        if (caseEntity.ProductArea_Id.HasValue)                
        //            caseEntity.ProductArea.Name = GetProductAreaFullName(caseEntity.ProductArea_Id.Value, caseDataSet.ProductAreaQuery.ToList());                                    

        //        caseEntity.System = new System { SystemName = string.Empty };
        //        if (caseEntity.System_Id.HasValue)
        //            caseEntity.System = caseDataSet.SystemQuery.Where(s => s.Id == caseEntity.System_Id.Value).FirstOrDefault() ?? caseEntity.System;

        //        caseEntity.Urgency = new Urgency { Name = string.Empty };
        //        if (caseEntity.Urgency_Id.HasValue)
        //            caseEntity.Urgency = caseDataSet.UrgencyQuery.Where(u => u.Id == caseEntity.Urgency_Id.Value).FirstOrDefault() ?? caseEntity.Urgency;

        //        caseEntity.Impact = new Impact { Name = string.Empty };
        //        if (caseEntity.Impact_Id.HasValue)
        //            caseEntity.Impact = caseDataSet.ImpactQuery.Where(i => i.Id == caseEntity.Impact_Id.Value).FirstOrDefault() ?? caseEntity.Impact;

        //        caseEntity.Supplier = new Supplier { Name = string.Empty };
        //        if (caseEntity.Supplier_Id.HasValue)
        //            caseEntity.Supplier = caseDataSet.SupplierQuery.Where(s => s.Id == caseEntity.Supplier_Id.Value).FirstOrDefault() ?? caseEntity.Supplier;

        //        caseEntity.Workinggroup = new WorkingGroupEntity { WorkingGroupName = string.Empty };
        //        if (caseEntity.WorkingGroup_Id.HasValue)
        //            caseEntity.Workinggroup = caseDataSet.WorkingGroupQuery.Where(w => w.Id == caseEntity.WorkingGroup_Id.Value).FirstOrDefault() ?? caseEntity.Workinggroup;

        //        caseEntity.Priority = new Priority { Name = string.Empty };
        //        if (caseEntity.Priority_Id.HasValue)
        //            caseEntity.Priority = caseDataSet.PriorityQuery.Where(p => p.Id == caseEntity.Priority_Id.Value).FirstOrDefault() ?? caseEntity.Priority;

        //        caseEntity.Status = new Status { Name = string.Empty };
        //        if (caseEntity.Status_Id.HasValue)
        //            caseEntity.Status = caseDataSet.StatusQuery.Where(s => s.Id == caseEntity.Status_Id.Value).FirstOrDefault() ?? caseEntity.Status;

        //        caseEntity.StateSecondary = new StateSecondary { Name = string.Empty };
        //        if (caseEntity.StateSecondary_Id.HasValue)
        //            caseEntity.StateSecondary = caseDataSet.StateSecondaryQuery.Where(s => s.Id == caseEntity.StateSecondary_Id.Value).FirstOrDefault() ?? caseEntity.StateSecondary;

        //        caseEntity.CausingPart = new CausingPart { Name = string.Empty };
        //        if (caseEntity.CausingPartId.HasValue)
        //            caseEntity.CausingPart = caseDataSet.CausingPartQuery.Where(s => s.Id == caseEntity.CausingPartId.Value).FirstOrDefault() ?? caseEntity.CausingPart;

        //        caseEntity.RegistrationSourceCustomer = new RegistrationSourceCustomer { SourceName = string.Empty };
        //        if (caseEntity.RegistrationSourceCustomer_Id.HasValue)
        //            caseEntity.RegistrationSourceCustomer = caseDataSet.RegistrationSourceCustomerQuery.Where(s => s.Id == caseEntity.RegistrationSourceCustomer_Id.Value).FirstOrDefault() ?? caseEntity.RegistrationSourceCustomer;                

        //        caseEntity.Logs = new List<Log>();                
        //        var lastLog = caseDataSet.LogQuery.Where(l => l.Case_Id == caseEntity.Id).OrderByDescending(l=> l.LogDate).FirstOrDefault();
        //        var logFiles = new List<LogFile>();
        //        var finishingType = new FinishingCause { Name = string.Empty };

        //        if (lastLog == null)
        //        {                    
        //            caseEntity.Logs.Add(new Log
        //            {                    
        //                Text_External = string.Empty,
        //                Text_Internal = string.Empty,
        //                FinishingDate = caseEntity.FinishingDate,
        //                LogFiles = logFiles,
        //                FinishingTypeEntity = finishingType                        
        //            });
        //        }
        //        else
        //        {
        //            if (lastLog.FinishingType.HasValue)                    
        //                finishingType = new FinishingCause { Name = lastLog.FinishingType.Value.GetClosingReasonFullName(caseDataSet.ClosingReasonQuery.AsQueryable()) };
                    
        //            caseEntity.Logs.Add(new Log
        //            {
        //                Text_Internal = lastLog.Text_Internal,
        //                Text_External = lastLog.Text_External,
        //                Charge = lastLog.Charge,
        //                LogFiles = logFiles,
        //                FinishingDate = caseEntity.FinishingDate,
        //                FinishingTypeEntity = finishingType,
        //                LogDate = lastLog.LogDate
        //            });
        //        }

        //        /*foreach (var log in allLogs)
        //        {
                    
        //            if (log.FinishingType.HasValue)
        //            {
        //                finishingType = new FinishingCause { Name = log.FinishingType.Value.GetClosingReasonFullName(caseDataSet.ClosingReasonQuery.AsQueryable()) };
        //            }
                    
        //            //logFiles = caseDataSet.LogFileQuery.Where(lf => lf.Log_Id == log.Id).ToList();                    

        //            caseEntity.Logs.Add(new Log
        //                                    {
        //                                        Text_Internal = log.Text_Internal,
        //                                        Text_External = log.Text_External,
        //                                        Charge = log.Charge,
        //                                        LogFiles = logFiles,
        //                                        FinishingDate = caseEntity.FinishingDate,
        //                                        FinishingTypeEntity = finishingType,
        //                                        LogDate = log.LogDate
        //                                    });                                                                                
        //        }*/                                                
                
        //        caseEntity.CaseFiles = new List<CaseFile>();
        //        caseEntity.CaseFiles = caseDataSet.CaseFileQuery.Where(cf => cf.Case_Id == caseEntity.Id).ToList();

        //        var caseStatistic = caseDataSet.CaseStatisticsQuery.Where(cs => cs.CaseId == caseEntity.Id).FirstOrDefault();
        //        ret.Add(CreateFullOverview(caseEntity, caseStatistic));                
        //    }

        //    return ret;
        //}

        public static List<FullCaseOverview> MapToCaseOverviews(this List<ReportGeneratorFields> query, 
                                                                IQueryable<CaseType> caseTypes, 
                                                                IQueryable<ProductArea> productAreas, 
                                                                IQueryable<OU> ous,
                                                                IQueryable<FinishingCause> finishingCauses)
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
                                               .Select(c => (c.LogData.FinishingType.HasValue? c.LogData.FinishingType.Value.ToString(): string.Empty))
                                               .Distinct()
                                               .ToList();

            foreach (var cr in availableClosingReasons)
            {
                closingReasonFullNames.Add(cr, cr.GetClosingReasonFullName(finishingCauses));
            }

            ret = query.Select(q => new FullCaseOverview(
                                q.Id,
                                new UserOverview(q.User, q.Notifier, q.Email, q.Phone, q.CellPhone,q.Customer, q.Region, q.Department,
                                                 ouFullNames[q.Unit], q.Place, q.OrdererCode),

                                new ComputerOverview(q.PcNumber, q.ComputerType, q.ComputerPlace),

                                new CaseInfoOverview(q.Case, q.RegistrationDate, q.ChangeDate, q.RegistratedBy,
                                                     caseTypeFullNames[q.CaseType],
                                                     productAreaFullNames[q.ProductArea], 
                                                     q.System, q.UrgentDegree, q.Impact, q.Category,
                                                     q.Supplier, q.InvoiceNumber, q.ReferenceNumber, q.Caption, q.Description,
                                                     q.Other, Convert.ToBoolean(q.PhoneContact), Convert.ToBoolean(q.Sms), q.AgreedDate, q.Available, q.Cost, 
                                                     string.Empty, q.RegistrationSource, q.SolvedInTime),

                                new OtherOverview (q.WorkingGroup, q.Responsible, q.Administrator, q.Priority, q.State, q.SubState,
                                                   q.PlannedActionDate, q.WatchDate, Convert.ToBoolean(q.Verified), q.VerifiedDescription,
                                                   q.SolutionRate, q.CausingPart),

                                q.LogData != null ? new LogsOverview((q.LogData.FinishingType.HasValue && q.FinishingDate.HasValue? closingReasonFullNames[q.LogData.FinishingType.ToString()] : string.Empty), 
                                                                      q.FinishingDate) : new LogsOverview(string.Empty, null)

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
                        entity.UserCode);
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
                                
            return new LogsOverview("", null);
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
            return new LogsOverview("", null);
        }
    }
}