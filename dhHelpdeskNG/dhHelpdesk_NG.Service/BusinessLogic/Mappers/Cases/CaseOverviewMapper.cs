using DH.Helpdesk.Dal.Repositories;

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
        private static string GetCaseTypeFullName(this int caseTypeId, IList<CaseType> caseTypeQuery)
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

        private static string GetProductAreaFullName(this int? productId, IList<ProductArea> productAreaQuery)
        {                        
            var res = string.Empty;

            if (!productId.HasValue) return res;
            
            var pa = productAreaQuery.Where(p => p.Id == productId).FirstOrDefault();
            if (pa != null)
            {
                if (pa.Parent_ProductArea_Id.HasValue)
                    res = pa.Parent_ProductArea_Id.GetProductAreaFullName(productAreaQuery) + " - " + pa.Name;
                else
                    res = pa.Name;
            }
          

            return res;
        }

        private static string GetCategoryFullName(this int? categoryId, IList<Category> categories)
        {
            var res = string.Empty;

            if (!categoryId.HasValue) return res;

            var category = categories.Where(p => p.Id == categoryId).FirstOrDefault();
            if (category != null)
            {
                if (category.Parent_Category_Id.HasValue)
                    res = category.Parent_Category_Id.GetCategoryFullName(categories) + " - " + category.Name;
                else
                    res = category.Name;
            }

            return res;
        }

        private static string GetOUFullName(this int? ouId, IList<OU> organizationUnitQuery)
        {            
            var res = string.Empty;

            if (!ouId.HasValue) return res;

            var ou = organizationUnitQuery.Where(o => o.Id == ouId).FirstOrDefault();
            if (ou != null)
            {
                if (ou.Parent_OU_Id.HasValue)
                    res = ou.Parent_OU_Id.GetOUFullName(organizationUnitQuery) + " - " + ou.Name;
                else
                    res = ou.Name;
            }
            
            return res;
        }

        private static string GetClosingReasonFullName(this int? closingReasonId, IList<FinishingCause> finishingCauseQuery)
        {           
            var res = string.Empty;

            if (!closingReasonId.HasValue) return res;

            var cr = finishingCauseQuery.Where(c => c.Id == closingReasonId).FirstOrDefault();
            if (cr != null)
            {
                if (cr.Parent_FinishingCause_Id.HasValue)
                    res = cr.Parent_FinishingCause_Id.GetClosingReasonFullName(finishingCauseQuery) + " - " + cr.Name;
                else
                    res = cr.Name;
            }

            return res;
        }             

        public static List<FullCaseOverview> MapToCaseOverviews(this List<ReportGeneratorFields> query, 
                                                                ICaseTypeRepository caseTypesRep,
                                                                IProductAreaRepository productAreasRep, 
                                                                IQueryable<OU> ous,
                                                                IQueryable<FinishingCause> finishingCauses,
                                                                IQueryable<Category> categories)
        {
            var availableCaseTypes = query.Select(c=> c.CaseType).Distinct().ToList();
            var caseTypesList = caseTypesRep.GetWithParents(availableCaseTypes.ToArray()).ToList();
            var caseTypeFullNames = availableCaseTypes.ToDictionary(ct => ct, ct => ct.GetCaseTypeFullName(caseTypesList));

            var availableProductAreas = query
                .Select(c => string.IsNullOrEmpty(c.ProductArea) ? new int?() : int.Parse(c.ProductArea))
                .Distinct().ToList();
            var productAreasList = productAreasRep.GetWithParents(availableProductAreas.Where(p => p.HasValue).Select(p => p.Value).ToArray());
            var productAreaFullNames = availableProductAreas.ToDictionary(pa => pa.HasValue ? pa.ToString() : string.Empty, pa => pa.GetProductAreaFullName(productAreasList));

            var availableOUs = query
                .Select(c => string.IsNullOrEmpty(c.Unit) ? new int?() : int.Parse(c.Unit))
                .Distinct().ToList();
            var ousList = ous.ToList(); //Can be optimize by using CTE. See CaseType, ProductArea GetWithParents method (now not so many data queried)
            var ouFullNames = availableOUs.ToDictionary(ou => ou.HasValue ? ou.ToString() : string.Empty, ou => ou.GetOUFullName(ousList));

            var availableClosingReasons = query.Where(c=> c.LogData != null)
                                               .Select(c => c.LogData.FinishingType)
                                               .Distinct()
                                               .ToList();
            var finishingCausesList = finishingCauses.ToList(); //Can be optimize by using CTE. See CaseType, ProductArea GetWithParents method (now not so many data queried)
            var closingReasonFullNames = availableClosingReasons.ToDictionary(cr => cr.HasValue ? cr.ToString() : string.Empty, cr => cr.GetClosingReasonFullName(finishingCausesList));

            var availablecategories = query
                .Select(c => string.IsNullOrEmpty(c.Category) ? new int?() : int.Parse(c.Category))
                .Distinct().ToList();
            var availableCategoriesList = categories.ToList(); //Can be optimize by using CTE. See CaseType, ProductArea GetWithParents method (now not so many data queried)
            var categoriesFullNames = availablecategories.ToDictionary(c => c.HasValue ? c.ToString() : string.Empty, c => c.GetCategoryFullName(availableCategoriesList));

            var ret = query.Select(q => new FullCaseOverview(
                q.Id,
                new UserOverview(q.User, q.Notifier, q.Email, q.Phone, q.CellPhone,q.Customer, q.Region, q.Department,
                    ouFullNames[q.Unit], q.Place, q.OrdererCode, q.CostCentre, q.IsAbout_User, q.IsAbout_Persons_Name,
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
                        entity.CostCentre,
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

    }
}