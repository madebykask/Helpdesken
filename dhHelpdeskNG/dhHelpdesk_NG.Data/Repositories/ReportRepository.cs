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

    #region REPORT

  
    public interface IReportRepository : IRepository<Report>
    {
        List<ReportGeneratorFields> GetCaseList(
          int customerId,
          List<int> departmentIds,
          List<int> ouIds,
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
		  List<int> departmentIds,
		  List<int> ouIds,
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
            List<int> departmentIds,
            List<int> ouIds,
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
           List<int> departmentIds,
           List<int> ouIds,
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
                departmentIds,
                ouIds,
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
		  List<int> departmentIds,
		  List<int> ouIds,
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
				departmentIds,
				ouIds,
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
           List<int> departmentIds,
           List<int> ouIds,
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

            Dictionary<DateTime, int> result;
            using (new OptionHintScope(DataContext))
            {
                DataContext.Database.Log = s => Trace.WriteLine(s);

                var query = from c in this.DataContext.Cases
                            join cu in this.DataContext.Customers on c.Customer_Id equals cu.Id

                            where
                                c.Customer_Id == customerId && c.Deleted != 1 &&
                                (DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) &&
                                 DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil))

                                && (!closeFrom.HasValue || DbFunctions.TruncateTime(c.FinishingDate) >=
                                    DbFunctions.TruncateTime(closeFrom.Value))
                                && (!closeToEndOfDay.HasValue || DbFunctions.TruncateTime(c.FinishingDate) <=
                                    DbFunctions.TruncateTime(closeToEndOfDay.Value))

                            select c;

                if (caseTypeId.Any())
                {
                    query = query.Where(c => caseTypeId.Contains(c.CaseType_Id));
                }
                if (workingGroupIds.Any())
                {
                    query = workingGroupIds.Contains(0)
                        ? query.Where(c =>
                            !c.WorkingGroup_Id.HasValue || c.WorkingGroup_Id.HasValue &&
                            workingGroupIds.Contains(c.WorkingGroup_Id.Value))
                        : query.Where(c =>
                            c.WorkingGroup_Id.HasValue &&
                            workingGroupIds.Contains(c.WorkingGroup_Id.Value));
                }
                if (departmentIds.Any())
                {
                    query = ouIds.Any()
                        ? query.Where(c => c.Department_Id.HasValue && departmentIds.Contains(c.Department_Id.Value) ||
                                           c.OU_Id.HasValue && ouIds.Contains(c.OU_Id.Value))
                        : query.Where(c => c.Department_Id.HasValue && departmentIds.Contains(c.Department_Id.Value));
                }
                if (productAreaIds.Any())
                {
                    query = query.Where(c => c.ProductArea_Id.HasValue && productAreaIds.Contains(c.ProductArea_Id.Value));
                }
                if (administratorIds.Any())
                {
                    query = query.Where(c => c.Performer_User_Id.HasValue && administratorIds.Contains(c.Performer_User_Id.Value));
                }
                if (caseStatusIds.Any())
                {
                    query = caseStatusIds.Contains(1)
                        ? query.Where(c => c.FinishingDate.HasValue)
                        : query.Where(c => !c.FinishingDate.HasValue);
                }
				if (extendedCaseFormId.HasValue)
				{
					query = query.Where(c => c.CaseExtendedCaseDatas.Any(ecd => ecd.ExtendedCaseData.ExtendedCaseFormId == extendedCaseFormId.Value));
				}

                var query2 = query.GroupBy(c => new { c.RegTime.Month, c.RegTime.Year },
                    c => c,
                    (key, g) => new {Date = key, g}).Select(x => new { x.Date, Count = x.g.Count() });

                result = query2.AsNoTracking().ToDictionary(x => new DateTime(x.Date.Year, x.Date.Month, 1), y => y.Count);
            }

            return result;
        }
        
        #region Private Methods

        private List<ReportGeneratorFields> GetQuery(int customerId,
           List<int> departmentIds,
           List<int> ouIds,
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
		
			var result = Search(customerId, departmentIds, ouIds, workingGroupIds, productAreaIds, administratorIds, caseStatusIds, caseTypeId, extendedCaseFormFieldIds, extendedCaseFormId, periodFrom, periodUntil, closeFrom, closeToEndOfDay);
			
			return result;
		}


		private List<ReportGeneratorFields> GetQuery_FeatureTooglePreviousSearch(int customerId,
		  List<int> departmentIds,
		  List<int> ouIds,
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

			result = Search_FeatureTooglePreviousSearch(customerId, departmentIds, ouIds, workingGroupIds, productAreaIds, administratorIds, caseStatusIds, caseTypeId, periodFrom, periodUntil, closeFrom, closeToEndOfDay);
	
			return result;
		}

		private List<ReportGeneratorFields> Search(int customerId, List<int> departmentIds, List<int> ouIds, List<int> workingGroupIds, List<int> productAreaIds, List<int> administratorIds, List<int> caseStatusIds, List<int> caseTypeId, List<string> extendedCaseFormFieldIds, int? extendedCaseFormId, DateTime? periodFrom, DateTime? periodUntil, DateTime? closeFrom, DateTime? closeToEndOfDay)
		{
			List<ReportGeneratorFields> result;
			using (new OptionHintScope(DataContext))
			{
				//DataContext.Database.CommandTimeout = 60;
				DataContext.Database.Log = s => Trace.WriteLine(s);
				var query = from c in this.DataContext.Cases
							join cu in this.DataContext.Customers on c.Customer_Id equals cu.Id

							join caseStatis in this.DataContext.CaseStatistics on c.Id equals caseStatis.CaseId into casestatiss
							from _caseStatis in casestatiss.DefaultIfEmpty()

							where
								c.Customer_Id == customerId && c.Deleted != 1 &&
								(DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) &&
								 DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil))

								&& (!closeFrom.HasValue || DbFunctions.TruncateTime(c.FinishingDate) >=
								 DbFunctions.TruncateTime(closeFrom.Value))
								&& (!closeToEndOfDay.HasValue || DbFunctions.TruncateTime(c.FinishingDate) <=
								 DbFunctions.TruncateTime(closeToEndOfDay.Value))
							select new
							{
								Case = c,
								CaseStatistic = _caseStatis
							};

				if (caseTypeId.Any())
				{
					query = query.Where(c => caseTypeId.Contains(c.Case.CaseType_Id));
				}
				if (workingGroupIds.Any())
				{
					query = workingGroupIds.Contains(0)
						? query.Where(c =>
							!c.Case.WorkingGroup_Id.HasValue || c.Case.WorkingGroup_Id.HasValue &&
							workingGroupIds.Contains(c.Case.WorkingGroup_Id.Value))
						: query.Where(c =>
							c.Case.WorkingGroup_Id.HasValue &&
							workingGroupIds.Contains(c.Case.WorkingGroup_Id.Value));
				}
				if (departmentIds.Any())
				{
					query = ouIds.Any()
						? query.Where(c => c.Case.Department_Id.HasValue && departmentIds.Contains(c.Case.Department_Id.Value) ||
										   c.Case.OU_Id.HasValue && ouIds.Contains(c.Case.OU_Id.Value))
						: query.Where(c => c.Case.Department_Id.HasValue && departmentIds.Contains(c.Case.Department_Id.Value));
				}
				if (productAreaIds.Any())
				{
					query = query.Where(c => c.Case.ProductArea_Id.HasValue && productAreaIds.Contains(c.Case.ProductArea_Id.Value));
				}
				if (administratorIds.Any())
				{
					query = query.Where(c => c.Case.Performer_User_Id.HasValue && administratorIds.Contains(c.Case.Performer_User_Id.Value));
				}
				if (caseStatusIds.Any())
				{
					query = caseStatusIds.Contains(1)
						? query.Where(c => c.Case.FinishingDate.HasValue)
						: query.Where(c => !c.Case.FinishingDate.HasValue);
				}

				if (extendedCaseFormId.HasValue)
				{
					query = query.Where(o => o.Case.CaseExtendedCaseDatas.Any(ecd => ecd.ExtendedCaseData.ExtendedCaseFormId == extendedCaseFormId.Value));
				}

				var hasExtendedCaseFields = extendedCaseFormFieldIds != null && extendedCaseFormFieldIds.Any();

				if (!hasExtendedCaseFields)
					extendedCaseFormFieldIds = new List<string>();

				var getAllExtendedCaseValues = extendedCaseFormId.HasValue && !hasExtendedCaseFields;

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
					IsAbout_Region =
							"", //_isAbout.Region_Id.HasValue ? _r1.Name : "",   //Disabled for version 5.3.21
					IsAbout_Department = "", //_isAbout.Department_Id.HasValue ? _d1.DepartmentName : "",
					IsAbout_OU = "", //_isAbout.OU_Id.HasValue ? _isAbout.OU_Id.ToString() : "",
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
					RegistratedBy = c.Case.User_Id.HasValue ? 
                        c.Case.User.FirstName + " " + c.Case.User.SurName : 
                        !string.IsNullOrEmpty(c.Case.RegUserName) ? 
                            c.Case.RegUserName :
                            !string.IsNullOrEmpty(c.Case.RegUserId) ? c.Case.RegUserId : "",
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

					RegistrationSource = c.Case.RegistrationSourceCustomer_Id.HasValue ? c.Case.RegistrationSourceCustomer.SourceName : "",
					SolvedInTime = c.CaseStatistic.WasSolvedInTime,

					WorkingGroup = c.Case.WorkingGroup_Id.HasValue ? c.Case.Workinggroup.WorkingGroupName : "",
					Responsible = c.Case.CaseResponsibleUser_Id.HasValue ? c.Case.CaseResponsibleUser.FirstName + " " + c.Case.CaseResponsibleUser.SurName : "",
					Administrator = c.Case.Performer_User_Id.HasValue ? c.Case.Administrator.FirstName + " " + c.Case.Administrator.SurName : "",
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

					ExtendedCaseValues = 
						c.Case.CaseExtendedCaseDatas.SelectMany(ecd => ecd.ExtendedCaseData.ExtendedCaseValues
							.Where(ecv => getAllExtendedCaseValues || (hasExtendedCaseFields && extendedCaseFormFieldIds.Contains(ecv.FieldId.ToLower())))
							.Select(ecv => new ExtendedCaseValue
							{
								FieldId = ecv.FieldId,
								Value = !string.IsNullOrEmpty(ecv.SecondaryValue) ? ecv.SecondaryValue : ecv.Value 
							})
						)				

				});

				result = resultQuery.AsNoTracking()
					.OrderBy(c => c.Id)
					.ToList();

				foreach (var r in result)
				{
					r.Logs = r.Logs.OrderByDescending(o => o.LogDate).ToList();

					r.AllInternalText = (from r2 in r.Logs
										 select r2.Text_Internal).ToList();

					r.AllExtenalText = (from r2 in r.Logs
										select r2.Text_External).ToList();
					r.TotalWork = (from r2 in r.Logs
								   select r2.WorkingTime).DefaultIfEmpty(0).Sum();
					r.TotalPrice = (from r2 in r.Logs
									select r2.EquipmentPrice).DefaultIfEmpty(0).Sum();
					r.TotalOverTime = (from r2 in r.Logs
									   select r2.OverTime).DefaultIfEmpty(0).Sum();
					r.TotalMaterial = (from r2 in r.Logs
									   select r2.Price).DefaultIfEmpty(0).Sum();
					r.LogData = (from r2 in r.Logs
								 orderby r2.LogDate descending
								 select r2).FirstOrDefault();

				}
			}

			return result;
		}

		private List<ReportGeneratorFields> Search_FeatureTooglePreviousSearch(int customerId, List<int> departmentIds, List<int> ouIds, List<int> workingGroupIds, List<int> productAreaIds, List<int> administratorIds, List<int> caseStatusIds, List<int> caseTypeId, DateTime? periodFrom, DateTime? periodUntil, DateTime? closeFrom, DateTime? closeToEndOfDay)
		{
			List<ReportGeneratorFields> result;
			using (new OptionHintScope(DataContext))
			{
				DataContext.Database.Log = s => Trace.WriteLine(s);
				var query = from c in this.DataContext.Cases
							join cu in this.DataContext.Customers on c.Customer_Id equals cu.Id

							join caseStatis in this.DataContext.CaseStatistics on c.Id equals caseStatis.CaseId into casestatiss
							from _caseStatis in casestatiss.DefaultIfEmpty()

							where
								c.Customer_Id == customerId && c.Deleted != 1 &&
								(DbFunctions.TruncateTime(c.RegTime) >= DbFunctions.TruncateTime(periodFrom) &&
								 DbFunctions.TruncateTime(c.RegTime) <= DbFunctions.TruncateTime(periodUntil))

								&& (!closeFrom.HasValue || DbFunctions.TruncateTime(c.FinishingDate) >=
								 DbFunctions.TruncateTime(closeFrom.Value))
								&& (!closeToEndOfDay.HasValue || DbFunctions.TruncateTime(c.FinishingDate) <=
								 DbFunctions.TruncateTime(closeToEndOfDay.Value))
							select new
							{
								Case = c,
								CaseStatistic = _caseStatis,
							};

				if (caseTypeId.Any())
				{
					query = query.Where(c => caseTypeId.Contains(c.Case.CaseType_Id));
				}
				if (workingGroupIds.Any())
				{
					query = workingGroupIds.Contains(0)
						? query.Where(c =>
							!c.Case.WorkingGroup_Id.HasValue || c.Case.WorkingGroup_Id.HasValue &&
							workingGroupIds.Contains(c.Case.WorkingGroup_Id.Value))
						: query.Where(c =>
							c.Case.WorkingGroup_Id.HasValue &&
							workingGroupIds.Contains(c.Case.WorkingGroup_Id.Value));
				}
				if (departmentIds.Any())
				{
					query = ouIds.Any()
						? query.Where(c => c.Case.Department_Id.HasValue && departmentIds.Contains(c.Case.Department_Id.Value) ||
										   c.Case.OU_Id.HasValue && ouIds.Contains(c.Case.OU_Id.Value))
						: query.Where(c => c.Case.Department_Id.HasValue && departmentIds.Contains(c.Case.Department_Id.Value));
				}
				if (productAreaIds.Any())
				{
					query = query.Where(c => c.Case.ProductArea_Id.HasValue && productAreaIds.Contains(c.Case.ProductArea_Id.Value));
				}
				if (administratorIds.Any())
				{
					query = query.Where(c => c.Case.Performer_User_Id.HasValue && administratorIds.Contains(c.Case.Performer_User_Id.Value));
				}
				if (caseStatusIds.Any())
				{
					query = caseStatusIds.Contains(1)
						? query.Where(c => c.Case.FinishingDate.HasValue)
						: query.Where(c => !c.Case.FinishingDate.HasValue);
				}

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
					IsAbout_Region =
							"", //_isAbout.Region_Id.HasValue ? _r1.Name : "",   //Disabled for version 5.3.21
					IsAbout_Department = "", //_isAbout.Department_Id.HasValue ? _d1.DepartmentName : "",
					IsAbout_OU = "", //_isAbout.OU_Id.HasValue ? _isAbout.OU_Id.ToString() : "",
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
					RegistratedBy = c.Case.User_Id.HasValue ? c.Case.User.FirstName + " " + c.Case.User.SurName : "",
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

					RegistrationSource = c.Case.RegistrationSourceCustomer_Id.HasValue ? c.Case.RegistrationSourceCustomer.SourceName : "",
					SolvedInTime = c.CaseStatistic.WasSolvedInTime,

					WorkingGroup = c.Case.WorkingGroup_Id.HasValue ? c.Case.Workinggroup.WorkingGroupName : "",
					Responsible = c.Case.CaseResponsibleUser_Id.HasValue ? c.Case.CaseResponsibleUser.FirstName + " " + c.Case.CaseResponsibleUser.SurName : "",
					Administrator = c.Case.Performer_User_Id.HasValue ? c.Case.Administrator.FirstName + " " + c.Case.Administrator.SurName : "",
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

					LogData = (from r in this.DataContext.Logs
							   where (r.Case_Id == c.Case.Id)
							   orderby r.LogDate descending
							   select r).FirstOrDefault(),

					AllInternalText = (from r2 in this.DataContext.Logs
									   where (r2.Case_Id == c.Case.Id && r2.Text_Internal != null && r2.Text_Internal != "")
									   orderby r2.LogDate
									   select r2.Text_Internal).ToList(),

					AllExtenalText = (from r2 in this.DataContext.Logs
									  where (r2.Case_Id == c.Case.Id && r2.Text_External != null && r2.Text_External != "")
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

				result = resultQuery.AsNoTracking()
					.OrderBy(c => c.Id)
					.ToList();
			}

			return result;
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
            var query =  DataContext.ReportFavorites.Where(x => x.Id == reportFavoriteId && x.Customer_Id == customerId);
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
