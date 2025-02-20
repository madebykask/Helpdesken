using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using DH.Helpdesk.BusinessData.Models.Reports;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Interceptors;
using DH.Helpdesk.Dal.NewInfrastructure;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
    using DH.Helpdesk.Domain;
    using System;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using System.Linq.Expressions;
    using DH.Helpdesk.Domain.Helpers;



    #region REPORT


    public interface IReportRepository : IRepository<Report>
    {
        List<ReportGeneratorFields> GetCaseList(
          int customerId,
          List<Tuple<int, int>> departmentAndOuis,
          List<int> workingGroupIds,
          List<int> productAreaIds,
          List<int> administratorIds,
          List<int> caseStatusIds,
          List<int> caseTypeId,
          List<string> extendedCaseFormFieldIds,
          int? extendedCaseFormId,
          DateTime? periodFrom,
          DateTime? periodUntil,
           DateTime? closeFrom,
           DateTime? closeTo);

        List<ReportGeneratorFields> GetCaseList_FeatureTooglePreviousSearch(
          int customerId,
          List<Tuple<int, int>> departmentAndOuis,
          List<int> workingGroupIds,
          List<int> productAreaIds,
          List<int> administratorIds,
          List<int> caseStatusIds,
          List<int> caseTypeId,
          DateTime? periodFrom,
          DateTime? periodUntil,
           DateTime? closeFrom,
           DateTime? closeTo);

        Dictionary<DateTime, int> GetCaseAggregation(
            int customerId,
            List<Tuple<int, int>> departmentAndOuis,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeId,
            List<string> extendedCaseFormFieldIds,
            int? extendedCaseFormId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            DateTime? closeFrom,
            DateTime? closeTo);

    }
    public class ReportRepository : RepositoryBase<Report>, IReportRepository
    {
        public ReportRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ReportGeneratorFields> GetCaseList(
           int customerId,
           List<Tuple<int, int>> departmentAndOuis,
           List<int> workingGroupIds,
           List<int> productAreaIds,
           List<int> administratorIds,
           List<int> caseStatusIds,
           List<int> caseTypeId,
           List<string> extendedCaseFormFieldIds,
           int? extendedCaseFormId,
           DateTime? periodFrom,
           DateTime? periodUntil,
           DateTime? closeFrom,
           DateTime? closeTo)
        {
            return GetQuery(customerId,
                departmentAndOuis,
                workingGroupIds,
                productAreaIds,
                administratorIds,
                caseStatusIds,
                caseTypeId,
                extendedCaseFormFieldIds,
                extendedCaseFormId,
                periodFrom,
                periodUntil,
                closeFrom,
                closeTo);
        }

        public List<ReportGeneratorFields> GetCaseList_FeatureTooglePreviousSearch(
          int customerId,
          List<Tuple<int, int>> departmentAndOuis,
          List<int> workingGroupIds,
          List<int> productAreaIds,
          List<int> administratorIds,
          List<int> caseStatusIds,
          List<int> caseTypeId,
          DateTime? periodFrom,
          DateTime? periodUntil,
          DateTime? closeFrom,
          DateTime? closeTo)
        {
            return GetQuery_FeatureTooglePreviousSearch(customerId,
                departmentAndOuis,
                workingGroupIds,
                productAreaIds,
                administratorIds,
                caseStatusIds,
                caseTypeId,
                periodFrom,
                periodUntil,
                closeFrom,
                closeTo);
        }


        public Dictionary<DateTime, int> GetCaseAggregation(
            int customerId,
            List<Tuple<int, int>> departmentAndOuis,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeId,
            List<string> extendedCaseFormFieldIds,
            int? extendedCaseFormId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            DateTime? closeFrom,
            DateTime? closeTo)
        {
            // Default date range if needed
            if (!periodFrom.HasValue)
                periodFrom = DateTime.Now;
            if (!periodUntil.HasValue)
                periodUntil = DateTime.Now;

            // Convert closeTo to end-of-day, if you have a GetEndOfDay() extension
            var closeToEndOfDay = closeTo?.GetEndOfDay();

            using (new OptionHintScope(DataContext))
            {
                DataContext.Database.Log = s => Trace.WriteLine(s);

                // ------------------------------------------------------
                // 1) Build base IQueryable<Case> with date filtering
                // ------------------------------------------------------
                var baseCases = this.DataContext.Cases
                    .Where(c =>
                        c.Customer_Id == customerId &&
                        c.Deleted != 1 &&
                        DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) &&
                        DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil) &&
                        (!closeFrom.HasValue
                            || DbFunctions.TruncateTime(c.FinishingDate) >= DbFunctions.TruncateTime(closeFrom.Value)) &&
                        (!closeToEndOfDay.HasValue
                            || DbFunctions.TruncateTime(c.FinishingDate) <= DbFunctions.TruncateTime(closeToEndOfDay.Value))
                    );

                // ------------------------------------------------------
                // 2) Apply all other filters
                // ------------------------------------------------------

                // CaseType filter
                if (caseTypeId.Any())
                {
                    baseCases = baseCases.Where(c => caseTypeId.Contains(c.CaseType_Id));
                }

                // WorkingGroup filter
                if (workingGroupIds.Any())
                {
                    if (workingGroupIds.Contains(0))
                    {
                        // 0 means also include (WorkingGroup_Id == null)
                        baseCases = baseCases.Where(c =>
                            !c.WorkingGroup_Id.HasValue ||
                            (c.WorkingGroup_Id.HasValue && workingGroupIds.Contains(c.WorkingGroup_Id.Value))
                        );
                    }
                    else
                    {
                        baseCases = baseCases.Where(c =>
                            c.WorkingGroup_Id.HasValue &&
                            workingGroupIds.Contains(c.WorkingGroup_Id.Value)
                        );
                    }
                }

                // Department & OU filter (dynamic expression)
                if (departmentAndOuis != null && departmentAndOuis.Any())
                {
                    var lambdaDeptOu = BuildDeptOuExpression(departmentAndOuis);
                    baseCases = baseCases.Where(lambdaDeptOu);
                }

                // ProductArea filter
                if (productAreaIds.Any())
                {
                    baseCases = baseCases.Where(c =>
                        c.ProductArea_Id.HasValue && productAreaIds.Contains(c.ProductArea_Id.Value));
                }

                // Administrator filter
                if (administratorIds.Any())
                {
                    baseCases = baseCases.Where(c =>
                        c.Performer_User_Id.HasValue && administratorIds.Contains(c.Performer_User_Id.Value));
                }

                // CaseStatus filter
                if (caseStatusIds.Any())
                {
                    // e.g. if 1 => "Finished"
                    if (caseStatusIds.Contains(1))
                        baseCases = baseCases.Where(c => c.FinishingDate.HasValue);
                    else
                        baseCases = baseCases.Where(c => !c.FinishingDate.HasValue);
                }

                // ExtendedCaseForm filter
                if (extendedCaseFormId.HasValue)
                {
                    baseCases = baseCases.Where(c =>
                        c.CaseExtendedCaseDatas.Any(ecd =>
                            ecd.ExtendedCaseData.ExtendedCaseFormId == extendedCaseFormId.Value));
                }

                // ------------------------------------------------------
                // 3) Group by Year & Month + materialize results
                // ------------------------------------------------------
                // Now that everything is filtered, we do the grouping.
                // EF can handle "GroupBy(...)" on baseCases
                var groupedQuery = baseCases
                    .GroupBy(
                        c => new { c.RegTime.Year, c.RegTime.Month },
                        (key, g) => new
                        {
                            Year = key.Year,
                            Month = key.Month,
                            Count = g.Count()
                        }
                    );

                // Build a dictionary of (Year/Month => Count)
                var results = groupedQuery
                    .AsNoTracking()
                    .ToDictionary(
                        x => new DateTime(x.Year, x.Month, 1),
                        x => x.Count
                    );

                return results;
            }
        }


        #region Private Methods

        private List<ReportGeneratorFields> GetQuery(int customerId,
           List<Tuple<int, int>> departmentAndOuis,
           List<int> workingGroupIds,
           List<int> productAreaIds,
           List<int> administratorIds,
           List<int> caseStatusIds,
           List<int> caseTypeId,
           List<string> extendedCaseFormFieldIds,
           int? extendedCaseFormId,
           DateTime? periodFrom,
           DateTime? periodUntil,
           DateTime? closeFrom,
           DateTime? closeTo)
        {
            if (!periodFrom.HasValue)
            {
                periodFrom = DateTime.Now;
            }

            if (!periodUntil.HasValue)
            {
                periodUntil = DateTime.Now;
            }
            var closeToEndOfDay = closeTo?.GetEndOfDay();

            var result = Search(customerId, departmentAndOuis, workingGroupIds, productAreaIds, administratorIds, caseStatusIds, caseTypeId, extendedCaseFormFieldIds, extendedCaseFormId, periodFrom, periodUntil, closeFrom, closeToEndOfDay);

            return result;
        }


        private List<ReportGeneratorFields> GetQuery_FeatureTooglePreviousSearch(int customerId,
          List<Tuple<int, int>> departmentAndOuis,
          List<int> workingGroupIds,
          List<int> productAreaIds,
          List<int> administratorIds,
          List<int> caseStatusIds,
          List<int> caseTypeId,
          DateTime? periodFrom,
          DateTime? periodUntil,
          DateTime? closeFrom,
          DateTime? closeTo)
        {
            if (!periodFrom.HasValue)
            {
                periodFrom = DateTime.Now;
            }

            if (!periodUntil.HasValue)
            {
                periodUntil = DateTime.Now;
            }
            var closeToEndOfDay = closeTo?.GetEndOfDay();


            List<ReportGeneratorFields> result;

            result = Search_FeatureTooglePreviousSearch(customerId, departmentAndOuis, workingGroupIds, productAreaIds, administratorIds, caseStatusIds, caseTypeId, periodFrom, periodUntil, closeFrom, closeToEndOfDay);

            return result;
        }

        private List<ReportGeneratorFields> Search(
            int customerId,
            List<Tuple<int, int>> departmentAndOuis,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeId,
            List<string> extendedCaseFormFieldIds,
            int? extendedCaseFormId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            DateTime? closeFrom,
            DateTime? closeToEndOfDay)
        {
            // If the date params aren't provided, default to Now
            if (!periodFrom.HasValue)
                periodFrom = DateTime.Now;
            if (!periodUntil.HasValue)
                periodUntil = DateTime.Now;

            List<ReportGeneratorFields> result;

            using (new OptionHintScope(DataContext))
            {
                DataContext.Database.Log = s => Trace.WriteLine(s);

                // -------------------------------------------------
                // 1) Build a base IQueryable<Case> with your date filters
                // -------------------------------------------------
                var baseCases = this.DataContext.Cases
                    .Where(c =>
                        c.Customer_Id == customerId &&
                        c.Deleted != 1 &&
                        // Registration date in [periodFrom..periodUntil]
                        DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) &&
                        DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil) &&
                        // FinishingDate >= closeFrom?
                        (!closeFrom.HasValue || DbFunctions.TruncateTime(c.FinishingDate) >= DbFunctions.TruncateTime(closeFrom.Value)) &&
                        // FinishingDate <= closeToEndOfDay?
                        (!closeToEndOfDay.HasValue || DbFunctions.TruncateTime(c.FinishingDate) <= DbFunctions.TruncateTime(closeToEndOfDay.Value))
                    );

                // 2) Apply filters that reference c => ...
                // -------------------------------------------

                // CaseType filter
                if (caseTypeId.Any())
                {
                    baseCases = baseCases.Where(c => caseTypeId.Contains(c.CaseType_Id));
                }

                // WorkingGroup filter
                if (workingGroupIds.Any())
                {
                    if (workingGroupIds.Contains(0))
                    {
                        // "0" means "include null or any"
                        baseCases = baseCases.Where(c =>
                            !c.WorkingGroup_Id.HasValue ||
                            (c.WorkingGroup_Id.HasValue && workingGroupIds.Contains(c.WorkingGroup_Id.Value))
                        );
                    }
                    else
                    {
                        baseCases = baseCases.Where(c =>
                            c.WorkingGroup_Id.HasValue &&
                            workingGroupIds.Contains(c.WorkingGroup_Id.Value)
                        );
                    }
                }

                // Department & OU filter (using the dynamic expression)
                if (departmentAndOuis != null && departmentAndOuis.Any())
                {
                    // Build Expression<Func<Case,bool>> 
                    var lambdaDeptOu = BuildDeptOuExpression(departmentAndOuis);
                    // Apply it
                    baseCases = baseCases.Where(lambdaDeptOu);
                }

                // ProductArea filter
                if (productAreaIds.Any())
                {
                    baseCases = baseCases.Where(c =>
                        c.ProductArea_Id.HasValue &&
                        productAreaIds.Contains(c.ProductArea_Id.Value));
                }

                // Administrator filter
                if (administratorIds.Any())
                {
                    baseCases = baseCases.Where(c =>
                        c.Performer_User_Id.HasValue &&
                        administratorIds.Contains(c.Performer_User_Id.Value));
                }

                // CaseStatus filter
                if (caseStatusIds.Any())
                {
                    // For example, if "1" means "finished"
                    if (caseStatusIds.Contains(1))
                        baseCases = baseCases.Where(c => c.FinishingDate.HasValue);
                    else
                        baseCases = baseCases.Where(c => !c.FinishingDate.HasValue);
                }

                // ExtendedCaseForm filter
                if (extendedCaseFormId.HasValue)
                {
                    baseCases = baseCases.Where(c =>
                        c.CaseExtendedCaseDatas.Any(ecd => ecd.ExtendedCaseData.ExtendedCaseFormId == extendedCaseFormId.Value));
                }

                // -------------------------------------------------
                // 3) Join to CaseStatistics (LEFT JOIN) + final projection
                // -------------------------------------------------
                var query = from c in baseCases
                            join caseStatis in this.DataContext.CaseStatistics
                                on c.Id equals caseStatis.CaseId into casestatiss
                            from _caseStatis in casestatiss.DefaultIfEmpty()
                            select new
                            {
                                Case = c,
                                CaseStatistic = _caseStatis
                            };

                // Decide which ExtendedCaseFields you need
                var hasExtendedCaseFields = (extendedCaseFormFieldIds != null && extendedCaseFormFieldIds.Any());
                if (!hasExtendedCaseFields)
                    extendedCaseFormFieldIds = new List<string>();

                var getAllExtendedCaseValues = extendedCaseFormId.HasValue && !hasExtendedCaseFields;

                // Now project to ReportGeneratorFields
                var resultQuery = query.Select(c => new ReportGeneratorFields
                {
                    Id = c.Case.Id,
                    User = c.Case.ReportedBy,
                    Notifier = c.Case.PersonsName,
                    Email = c.Case.PersonsEmail,
                    Phone = c.Case.PersonsPhone,
                    CellPhone = c.Case.PersonsCellphone,
                    Customer = c.Case.Customer.Name,
                    Region = c.Case.Region_Id.HasValue ? c.Case.Region.Name : "",
                    Department = c.Case.Department_Id.HasValue ? c.Case.Department.DepartmentName : "",
                    Unit = c.Case.OU_Id.HasValue ? c.Case.OU_Id.ToString() : "",
                    Place = c.Case.Place,
                    OrdererCode = c.Case.UserCode,
                    CostCentre = c.Case.CostCentre,

                    // isAbout fields, etc...
                    IsAbout_User = c.Case.IsAbout.ReportedBy,
                    IsAbout_Persons_Name = c.Case.IsAbout.Person_Name,
                    IsAbout_Persons_Phone = c.Case.IsAbout.Person_Phone,
                    IsAbout_Persons_CellPhone = c.Case.IsAbout.Person_Cellphone,
                    IsAbout_Department = "",
                    IsAbout_OU = "",
                    IsAbout_CostCentre = c.Case.IsAbout.CostCentre,
                    IsAbout_Place = c.Case.IsAbout.Place,
                    IsAbout_UserCode = c.Case.IsAbout.UserCode,
                    IsAbout_Persons_Email = c.Case.IsAbout.Person_Email,

                    PcNumber = c.Case.InventoryNumber,
                    ComputerType = c.Case.InventoryType,
                    ComputerPlace = c.Case.InventoryLocation,

                    Case = c.Case.CaseNumber,
                    RegistrationDate = c.Case.RegTime,
                    ChangeDate = c.Case.ChangeTime,
                    RegistratedBy = c.Case.User_Id.HasValue
                                        ? c.Case.User.FirstName + " " + c.Case.User.SurName
                                        : !string.IsNullOrEmpty(c.Case.RegUserName)
                                            ? c.Case.RegUserName
                                            : !string.IsNullOrEmpty(c.Case.RegUserId) ? c.Case.RegUserId : "",

                    CaseType = c.Case.CaseType_Id,
                    ProductArea = c.Case.ProductArea_Id.HasValue ? c.Case.ProductArea_Id.ToString() : "",
                    System = c.Case.System_Id.HasValue ? c.Case.System.SystemName : "",
                    UrgentDegree = c.Case.Urgency_Id.HasValue ? c.Case.Urgency.Name : "",
                    Impact = c.Case.Impact_Id.HasValue ? c.Case.Impact.Name : "",
                    Category = c.Case.Category_Id.HasValue ? c.Case.Category_Id.ToString() : "",
                    Supplier = c.Case.Supplier_Id.HasValue ? c.Case.Supplier.Name : "",
                    InvoiceNumber = c.Case.InvoiceNumber,
                    ReferenceNumber = c.Case.ReferenceNumber,
                    Caption = c.Case.Caption,
                    Description = c.Case.Description,
                    Other = c.Case.Miscellaneous,
                    PhoneContact = c.Case.ContactBeforeAction,
                    Sms = c.Case.SMS,
                    AgreedDate = c.Case.AgreedDate,
                    Available = c.Case.Available,
                    Cost = c.Case.Cost,
                    AttachedFile = "",
                    FinishingDate = c.Case.FinishingDate,
                    FinishingDescription = c.Case.FinishingDescription,

                    RegistrationSource = c.Case.RegistrationSourceCustomer_Id.HasValue
                                            ? c.Case.RegistrationSourceCustomer.SourceName
                                            : "",
                    SolvedInTime = c.CaseStatistic.WasSolvedInTime,

                    WorkingGroup = c.Case.WorkingGroup_Id.HasValue
                                        ? c.Case.Workinggroup.WorkingGroupName
                                        : "",
                    Responsible = c.Case.CaseResponsibleUser_Id.HasValue
                                        ? c.Case.CaseResponsibleUser.FirstName + " " + c.Case.CaseResponsibleUser.SurName
                                        : "",
                    Administrator = c.Case.Performer_User_Id.HasValue
                                        ? c.Case.Administrator.FirstName + " " + c.Case.Administrator.SurName
                                        : "",
                    Priority = c.Case.Priority_Id.HasValue ? c.Case.Priority.Name : "",
                    State = c.Case.Status_Id.HasValue ? c.Case.Status.Name : "",
                    SubState = c.Case.StateSecondary_Id.HasValue ? c.Case.StateSecondary.Name : "",
                    PlannedActionDate = c.Case.PlanDate,
                    WatchDate = c.Case.WatchDate,
                    Verified = c.Case.Verified,
                    VerifiedDescription = c.Case.VerifiedDescription,
                    SolutionRate = c.Case.SolutionRate,
                    CausingPart = c.Case.CausingPartId.HasValue ? c.Case.CausingPart.Name : "",
                    Problem = c.Case.Problem_Id.HasValue ? c.Case.Problem.Name : "",

                    Logs = c.Case.Logs,

                    ExtendedCaseValues = c.Case.CaseExtendedCaseDatas
                        .SelectMany(ecd => ecd.ExtendedCaseData.ExtendedCaseValues
                            .Where(ecv =>
                                getAllExtendedCaseValues
                                || (hasExtendedCaseFields && extendedCaseFormFieldIds.Contains(ecv.FieldId.ToLower()))
                            )
                            .Select(ecv => new ExtendedCaseValue
                            {
                                FieldId = ecv.FieldId,
                                Value = !string.IsNullOrEmpty(ecv.SecondaryValue)
                                            ? ecv.SecondaryValue
                                            : ecv.Value
                            })
                        )
                });

                // Materialize
                var list = resultQuery
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .ToList();

                // ----------------------------------------
                // 4) Post-processing each ReportGeneratorFields
                // ----------------------------------------
                foreach (var r in list)
                {
                    r.Logs = r.Logs.OrderByDescending(o => o.LogDate).ToList();

                    r.AllInternalText = r.Logs.Select(l => l.Text_Internal).ToList();
                    r.AllExtenalText = r.Logs.Select(l => l.Text_External).ToList();
                    r.TotalWork = r.Logs.Select(l => l.WorkingTime).DefaultIfEmpty(0).Sum();
                    r.TotalPrice = r.Logs.Select(l => l.EquipmentPrice).DefaultIfEmpty(0).Sum();
                    r.TotalOverTime = r.Logs.Select(l => l.OverTime).DefaultIfEmpty(0).Sum();
                    r.TotalMaterial = r.Logs.Select(l => l.Price).DefaultIfEmpty(0).Sum();
                    r.LogData = r.Logs.OrderByDescending(l => l.LogDate).FirstOrDefault();
                }

                result = list;
            }

            return result;
        }


        private List<ReportGeneratorFields> Search_FeatureTooglePreviousSearch(
            int customerId,
            List<Tuple<int, int>> departmentAndOuis,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            DateTime? closeFrom,
            DateTime? closeToEndOfDay)
        {
            // Default date range if null
            if (!periodFrom.HasValue)
                periodFrom = DateTime.Now;
            if (!periodUntil.HasValue)
                periodUntil = DateTime.Now;

            using (new OptionHintScope(DataContext))
            {
                DataContext.Database.Log = s => Trace.WriteLine(s);

                // ----------------------------------------------------------------
                // 1) Build base IQueryable<Case> with date filters
                // ----------------------------------------------------------------
                var baseCases = this.DataContext.Cases
                    .Where(c =>
                        c.Customer_Id == customerId &&
                        c.Deleted != 1 &&

                        // Registration date within [periodFrom..periodUntil]
                        DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) &&
                        DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil) &&

                        // FinishingDate >= closeFrom?
                        (!closeFrom.HasValue || DbFunctions.TruncateTime(c.FinishingDate) >= DbFunctions.TruncateTime(closeFrom.Value)) &&
                        // FinishingDate <= closeToEndOfDay?
                        (!closeToEndOfDay.HasValue || DbFunctions.TruncateTime(c.FinishingDate) <= DbFunctions.TruncateTime(closeToEndOfDay.Value))
                    );

                // ----------------------------------------------------------------
                // 2) Apply additional filters on baseCases
                // ----------------------------------------------------------------

                // CaseType filter
                if (caseTypeId.Any())
                {
                    baseCases = baseCases.Where(c => caseTypeId.Contains(c.CaseType_Id));
                }

                // WorkingGroup filter
                if (workingGroupIds.Any())
                {
                    if (workingGroupIds.Contains(0))
                    {
                        // "0" means also include cases where WorkingGroup_Id is null
                        baseCases = baseCases.Where(c =>
                            !c.WorkingGroup_Id.HasValue ||
                            (c.WorkingGroup_Id.HasValue && workingGroupIds.Contains(c.WorkingGroup_Id.Value))
                        );
                    }
                    else
                    {
                        baseCases = baseCases.Where(c =>
                            c.WorkingGroup_Id.HasValue &&
                            workingGroupIds.Contains(c.WorkingGroup_Id.Value)
                        );
                    }
                }

                // Department & OU dynamic expression
                if (departmentAndOuis != null && departmentAndOuis.Any())
                {
                    var lambdaCase = BuildDeptOuExpression(departmentAndOuis);
                    baseCases = baseCases.Where(lambdaCase);
                }

                // ProductArea filter
                if (productAreaIds.Any())
                {
                    baseCases = baseCases.Where(c =>
                        c.ProductArea_Id.HasValue &&
                        productAreaIds.Contains(c.ProductArea_Id.Value));
                }

                // Administrator filter
                if (administratorIds.Any())
                {
                    baseCases = baseCases.Where(c =>
                        c.Performer_User_Id.HasValue &&
                        administratorIds.Contains(c.Performer_User_Id.Value));
                }

                // CaseStatus filter
                if (caseStatusIds.Any())
                {
                    // e.g. if 1 means "finished"
                    if (caseStatusIds.Contains(1))
                        baseCases = baseCases.Where(c => c.FinishingDate.HasValue);
                    else
                        baseCases = baseCases.Where(c => !c.FinishingDate.HasValue);
                }

                // ----------------------------------------------------------------
                // 3) Join baseCases to CaseStatistics with left join
                // ----------------------------------------------------------------
                var query = from c in baseCases
                            join caseStatis in this.DataContext.CaseStatistics
                                on c.Id equals caseStatis.CaseId into casestatiss
                            from _caseStatis in casestatiss.DefaultIfEmpty()
                            select new
                            {
                                Case = c,
                                CaseStatistic = _caseStatis
                            };

                // ----------------------------------------------------------------
                // 4) Final projection -> ReportGeneratorFields
                // ----------------------------------------------------------------
                var resultQuery = query.Select(c => new ReportGeneratorFields
                {
                    Id = c.Case.Id,
                    User = c.Case.ReportedBy,
                    Notifier = c.Case.PersonsName,
                    Email = c.Case.PersonsEmail,
                    Phone = c.Case.PersonsPhone,
                    CellPhone = c.Case.PersonsCellphone,
                    Customer = c.Case.Customer.Name,
                    Region = c.Case.Region_Id.HasValue ? c.Case.Region.Name : "",
                    Department = c.Case.Department_Id.HasValue ? c.Case.Department.DepartmentName : "",
                    Unit = c.Case.OU_Id.HasValue ? c.Case.OU_Id.ToString() : "",
                    Place = c.Case.Place,
                    OrdererCode = c.Case.UserCode,
                    CostCentre = c.Case.CostCentre,

                    IsAbout_User = c.Case.IsAbout.ReportedBy,
                    IsAbout_Persons_Name = c.Case.IsAbout.Person_Name,
                    IsAbout_Persons_Phone = c.Case.IsAbout.Person_Phone,
                    IsAbout_Persons_CellPhone = c.Case.IsAbout.Person_Cellphone,
                    IsAbout_Region = "",
                    IsAbout_Department = "",
                    IsAbout_OU = "",
                    IsAbout_CostCentre = c.Case.IsAbout.CostCentre,
                    IsAbout_Place = c.Case.IsAbout.Place,
                    IsAbout_UserCode = c.Case.IsAbout.UserCode,
                    IsAbout_Persons_Email = c.Case.IsAbout.Person_Email,

                    PcNumber = c.Case.InventoryNumber,
                    ComputerType = c.Case.InventoryType,
                    ComputerPlace = c.Case.InventoryLocation,

                    Case = c.Case.CaseNumber,
                    RegistrationDate = c.Case.RegTime,
                    ChangeDate = c.Case.ChangeTime,
                    RegistratedBy = c.Case.User_Id.HasValue
                                        ? c.Case.User.FirstName + " " + c.Case.User.SurName
                                        : "",
                    CaseType = c.Case.CaseType_Id,
                    ProductArea = c.Case.ProductArea_Id.HasValue ? c.Case.ProductArea_Id.ToString() : "",
                    System = c.Case.System_Id.HasValue ? c.Case.System.SystemName : "",
                    UrgentDegree = c.Case.Urgency_Id.HasValue ? c.Case.Urgency.Name : "",
                    Impact = c.Case.Impact_Id.HasValue ? c.Case.Impact.Name : "",
                    Category = c.Case.Category_Id.HasValue ? c.Case.Category_Id.ToString() : "",
                    Supplier = c.Case.Supplier_Id.HasValue ? c.Case.Supplier.Name : "",
                    InvoiceNumber = c.Case.InvoiceNumber,
                    ReferenceNumber = c.Case.ReferenceNumber,
                    Caption = c.Case.Caption,
                    Description = c.Case.Description,
                    Other = c.Case.Miscellaneous,
                    PhoneContact = c.Case.ContactBeforeAction,
                    Sms = c.Case.SMS,
                    AgreedDate = c.Case.AgreedDate,
                    Available = c.Case.Available,
                    Cost = c.Case.Cost,
                    AttachedFile = "",
                    FinishingDate = c.Case.FinishingDate,
                    FinishingDescription = c.Case.FinishingDescription,

                    RegistrationSource = c.Case.RegistrationSourceCustomer_Id.HasValue
                                            ? c.Case.RegistrationSourceCustomer.SourceName
                                            : "",
                    SolvedInTime = c.CaseStatistic.WasSolvedInTime,

                    WorkingGroup = c.Case.WorkingGroup_Id.HasValue
                                        ? c.Case.Workinggroup.WorkingGroupName
                                        : "",
                    Responsible = c.Case.CaseResponsibleUser_Id.HasValue
                                        ? c.Case.CaseResponsibleUser.FirstName + " " + c.Case.CaseResponsibleUser.SurName
                                        : "",
                    Administrator = c.Case.Performer_User_Id.HasValue
                                        ? c.Case.Administrator.FirstName + " " + c.Case.Administrator.SurName
                                        : "",
                    Priority = c.Case.Priority_Id.HasValue ? c.Case.Priority.Name : "",
                    State = c.Case.Status_Id.HasValue ? c.Case.Status.Name : "",
                    SubState = c.Case.StateSecondary_Id.HasValue ? c.Case.StateSecondary.Name : "",
                    PlannedActionDate = c.Case.PlanDate,
                    WatchDate = c.Case.WatchDate,
                    Verified = c.Case.Verified,
                    VerifiedDescription = c.Case.VerifiedDescription,
                    SolutionRate = c.Case.SolutionRate,
                    CausingPart = c.Case.CausingPartId.HasValue ? c.Case.CausingPart.Name : "",
                    Problem = c.Case.Problem_Id.HasValue ? c.Case.Problem.Name : "",

                    // Logs from DataContext.Logs
                    // We'll do sub-queries for each property
                    LogData = (from r in this.DataContext.Logs
                               where (r.Case_Id == c.Case.Id)
                               orderby r.LogDate descending
                               select r).FirstOrDefault(),

                    AllInternalText = (from r2 in this.DataContext.Logs
                                       where (r2.Case_Id == c.Case.Id
                                              && r2.Text_Internal != null
                                              && r2.Text_Internal != "")
                                       orderby r2.LogDate
                                       select r2.Text_Internal).ToList(),

                    AllExtenalText = (from r2 in this.DataContext.Logs
                                      where (r2.Case_Id == c.Case.Id
                                             && r2.Text_External != null
                                             && r2.Text_External != "")
                                      orderby r2.LogDate
                                      select r2.Text_External).ToList(),

                    TotalWork = (from r2 in DataContext.Logs
                                 where (r2.Case_Id == c.Case.Id)
                                 select r2.WorkingTime).DefaultIfEmpty(0).Sum(),

                    TotalPrice = (from r2 in DataContext.Logs
                                  where (r2.Case_Id == c.Case.Id)
                                  select r2.EquipmentPrice).DefaultIfEmpty(0).Sum(),

                    TotalOverTime = (from r2 in DataContext.Logs
                                     where (r2.Case_Id == c.Case.Id)
                                     select r2.OverTime).DefaultIfEmpty(0).Sum(),

                    TotalMaterial = (from r2 in DataContext.Logs
                                     where (r2.Case_Id == c.Case.Id)
                                     select r2.Price).DefaultIfEmpty(0).Sum(),
                });

                // ----------------------------------------------------------------
                // 5) Materialize + return
                // ----------------------------------------------------------------
                var list = resultQuery.AsNoTracking()
                    .OrderBy(c => c.Id)
                    .ToList();

                return list;
            }
        }


        private Expression<Func<Case, bool>> BuildDeptOuExpression(List<Tuple<int, int>> deptAndOus)
        {
            // param "c"
            var cParam = Expression.Parameter(typeof(Case), "c");

            // start finalExpr = false
            Expression finalExpr = Expression.Constant(false, typeof(bool));

            foreach (var (deptId, ouId) in deptAndOus)
            {
                // c.Department_Id != null
                var deptProp = Expression.Property(cParam, nameof(Case.Department_Id));
                var deptNotNull = Expression.NotEqual(
                    deptProp,
                    Expression.Constant(null, typeof(int?))
                );

                // c.Department_Id.Value == deptId
                var deptVal = Expression.Property(deptProp, "Value");
                var deptEqual = Expression.Equal(
                    deptVal,
                    Expression.Constant(deptId, typeof(int))
                );

                // Combine department checks
                var deptChecks = Expression.AndAlso(deptNotNull, deptEqual);

                // If ouId = 0 => ignore OU => "true"
                Expression ouChecks = Expression.Constant(true, typeof(bool));
                if (ouId != 0)
                {
                    var ouProp = Expression.Property(cParam, nameof(Case.OU_Id));
                    var ouNotNull = Expression.NotEqual(
                        ouProp,
                        Expression.Constant(null, typeof(int?))
                    );

                    var ouVal = Expression.Property(ouProp, "Value");
                    var ouEqual = Expression.Equal(
                        ouVal,
                        Expression.Constant(ouId, typeof(int))
                    );

                    ouChecks = Expression.AndAlso(ouNotNull, ouEqual);
                }

                // (dept AND ou)
                var combinedAnd = Expression.AndAlso(deptChecks, ouChecks);

                // finalExpr = finalExpr OR combinedAnd
                finalExpr = Expression.OrElse(finalExpr, combinedAnd);
            }

            // Build lambda: c => finalExpr
            var lambda = Expression.Lambda<Func<Case, bool>>(finalExpr, cParam);
            return lambda;
        }


        #endregion Private Methods
    }

    #endregion

    #region REPORTCUSTOMER

    public interface IReportCustomerRepository : IRepository<ReportCustomer>
    {
        IEnumerable<CustomerReportList> GetCustomerReportListForCustomer(int id);
    }

    public class ReportCustomerRepository : RepositoryBase<ReportCustomer>, IReportCustomerRepository
    {
        private readonly ITranslator translator;

        public ReportCustomerRepository(IDatabaseFactory databaseFactory, ITranslator translator)
            : base(databaseFactory)
        {
            this.translator = translator;
        }

        public IEnumerable<CustomerReportList> GetCustomerReportListForCustomer(int id)
        {
            var query = from rc in this.DataContext.ReportCustomers
                        join c in this.DataContext.Customers on rc.Customer_Id equals c.Id
                        where c.Id == id
                        group rc by new { c.Id, rc.Report_Id, rc.ShowOnPage } into g
                        select new CustomerReportList
                        {
                            CustomerId = g.Key.Id,
                            ReportId = g.Key.Report_Id,
                            ActiveOnPage = g.Key.ShowOnPage
                        };

            return query;
        }


    }

    #endregion

    #region REPORTFAVORITE

    public interface IReportFavoriteRepository : IRepository<ReportFavorite>
    {
        IEnumerable<ReportFavoriteList> GetCustomerReportFavoriteList(int customerId, int? userId);
        ReportFavorite GetCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId);
        bool IsCustomerReportFavoriteNameUnique(int reportFavoriteId, int customerId, int? userId, string name);
        void DeleteCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId);
    }

    public class ReportFavoriteRepository : RepositoryBase<ReportFavorite>, IReportFavoriteRepository
    {
        private readonly ITranslator translator;

        public ReportFavoriteRepository(IDatabaseFactory databaseFactory, ITranslator translator)
            : base(databaseFactory)
        {
            this.translator = translator;
        }

        public IEnumerable<ReportFavoriteList> GetCustomerReportFavoriteList(int customerId, int? userId)
        {
            var query = DataContext.ReportFavorites.Where(x => x.Customer_Id == customerId);
            if (userId.HasValue)
            {
                query = query.Where(x => x.User_Id == userId.Value);
            }

            return query.Select(x => new ReportFavoriteList
            {
                Id = x.Id,
                Type = x.Type,
                Name = x.Name,
            }).ToList();
        }

        public ReportFavorite GetCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId)
        {
            var query = DataContext.ReportFavorites.Where(x => x.Id == reportFavoriteId && x.Customer_Id == customerId);
            if (userId.HasValue)
            {
                query = query.Where(x => x.User_Id == userId.Value);
            }

            return query.FirstOrDefault();
        }

        public bool IsCustomerReportFavoriteNameUnique(int reportFavoriteId, int customerId, int? userId, string name)
        {
            var query = DataContext.ReportFavorites.Where(x => x.Id != reportFavoriteId && x.Customer_Id == customerId && x.Name.ToLower() == name.ToLower());
            if (userId.HasValue)
            {
                query = query.Where(x => x.User_Id == userId.Value);
            }
            return !query.Any();
        }

        public void DeleteCustomerReportFavoriteById(int reportFavoriteId, int customerId, int? userId)
        {
            var query = DataContext.ReportFavorites.Where(x => x.Id == reportFavoriteId && x.Customer_Id == customerId);
            if (userId.HasValue)
            {
                query = query.Where(x => x.User_Id == userId.Value);
            }

            var report = query.FirstOrDefault();
            if (report != null)
            {
                Delete(report);
            }
        }
    }

    #endregion
}
