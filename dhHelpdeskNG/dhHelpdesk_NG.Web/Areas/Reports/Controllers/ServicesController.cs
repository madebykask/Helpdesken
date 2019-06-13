using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Reports;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Areas.Reports.Models.ReportService;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using WebGrease.Css.Extensions;

namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using Services.Services.Reports;
    using BusinessData.Models.ReportService;
    using System;
    using DH.Helpdesk.BusinessData.OldComponents;

    public class ServicesController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;
		private readonly IReportServiceService _reportServiceService;
        private readonly IDepartmentService _departmentService;

		public ServicesController(
            IMasterDataService masterDataService, 
            IUserService userService, 
            IWorkContext workContext,
            IWorkingGroupService workingGroupService,
            ICaseTypeService caseTypeService,
			IReportServiceService reportServiceService,
            IProductAreaService productAreaService,
            IDepartmentService departmentService)
            : base(masterDataService)
        {
            _userService = userService;
            _workContext = workContext;
            _workingGroupService = workingGroupService;
            _caseTypeService = caseTypeService;
			_reportServiceService = reportServiceService;
            _productAreaService = productAreaService;
            _departmentService = departmentService;
        }

        [HttpGet]
        public JsonResult GetWorkingGroupUsers(int? workingGroupId)
        {
            var users = _userService.GetWorkingGroupUsers(_workContext.Customer.CustomerId, workingGroupId);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHistoricalData(HistoricalReportFilterModel filter)
        {
			var customerId = SessionFacade.CurrentCustomer.Id;
			var user = _userService.GetUser(SessionFacade.CurrentUser.Id);

			//*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */
			var workingGroups = user.UserGroup_Id > UserGroups.Administrator ?
				_workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).ToList() :
				_workingGroupService.GetWorkingGroups(customerId, user.Id, false, true).ToList();
            var includeCasesWithNoWorkingGroup = filter.WorkingGroups == null || !filter.WorkingGroups.Any();

			if (filter.HistoricalWorkingGroups != null && filter.HistoricalWorkingGroups.Any())
				workingGroups = workingGroups.Where(o => filter.HistoricalWorkingGroups.Contains(o.Id)).ToList();

            filter.WorkingGroups = GetWorkingGroups(filter.WorkingGroups, customerId);

            var includeCasesWithNoDepartments = filter.Departments == null || !filter.Departments.Any();
            filter.Departments = GetDepartments(filter.Departments, customerId);

            var dataFilter = GetCommonDataFilter<HistoricalDataFilter>(filter);
            dataFilter.CaseStatus = filter.CaseStatus == 2 ? 1 : filter.CaseStatus == 1 ? 0 : (int?) null; // 1 active, 0 closed else null
            dataFilter.ChangeFrom = filter.HistoricalChangeDateFrom ?? DateTime.Now.AddYears(-100);
            dataFilter.ChangeTo = filter.HistoricalChangeDateTo.HasValue
                ? filter.HistoricalChangeDateTo.GetEndOfDay().Value
                : DateTime.Now.AddYears(20);
            dataFilter.ChangeWorkingGroups = workingGroups.Select(o => o.Id).ToList();
            dataFilter.IncludeHistoricalCasesWithNoWorkingGroup = filter.HistoricalWorkingGroups == null ||
                                                        !filter.HistoricalWorkingGroups.Any();
            dataFilter.IncludeCasesWithNoWorkingGroup = includeCasesWithNoWorkingGroup;
            dataFilter.IncludeCasesWithNoDepartments = includeCasesWithNoDepartments;

			var result = _reportServiceService.GetHistoricalData(dataFilter);

			var wgs = result.Select(o => new { o.WorkingGroup, o.WorkingGroupID }).Distinct().OrderBy(o => o.WorkingGroup).ToArray();
            var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true);
            TranslateCaseTypes(caseTypes, 0);
            var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList()).ToList();
            var total = result.Count;
			var responce = new 
            {
                totalLabel = string.Format("{0}: {1}", Translation.GetCoreTextTranslation("Summa"), total),
                data = new
			    {
				    labels = wgs.Select(o => o.WorkingGroup ?? "").ToArray(),
				    datasets = result.GroupBy(o => new { o.CaseTypeID, o.CaseType }).Select(ct => new
				    {
					    label = caseTypesFullNames.Single(ctf => ctf.Id == ct.Key.CaseTypeID).Name,
					    data = wgs.Select(wg => ct.Count(o => o.WorkingGroupID == wg.WorkingGroupID)).ToArray()
				    }).ToArray()
			    }
            };

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportedTimeData(ReportedTimeReportFilterModel filter)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var includeCasesWithNoWorkingGroup = filter.WorkingGroups == null || !filter.WorkingGroups.Any();
            var includeCasesWithNoDepartments = filter.Departments == null || !filter.Departments.Any();
            filter.WorkingGroups = GetWorkingGroups(filter.WorkingGroups, customerId);
            filter.Departments = GetDepartments(filter.Departments, customerId);

            var dataFilter = GetCommonDataFilter<ReportedTimeDataFilter>(filter);
            dataFilter.GroupBy = (ReportedTimeGroup) filter.GroupBy;
            dataFilter.LogNoteFrom = filter.LogNoteFrom;
            dataFilter.LogNoteTo = filter.LogNoteTo.HasValue ? filter.LogNoteTo.GetEndOfDay() : new DateTime?();
            dataFilter.IncludeCasesWithNoWorkingGroup = includeCasesWithNoWorkingGroup;
            dataFilter.IncludeCasesWithNoDepartments = includeCasesWithNoDepartments;

            var result = _reportServiceService.GetReportedTimeData(dataFilter);
            var minutesInHour = 60.0;
            var totalHours = result.Sum(o => o.TotalTime) / minutesInHour;
            switch (dataFilter.GroupBy)
            {
                case ReportedTimeGroup.CaseType_Id:
                {
                    var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true);
                    TranslateCaseTypes(caseTypes, 0);
                    var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList()).ToList();
                    result.ForEach( p => p.Label = caseTypesFullNames
                                                       .SingleOrDefault(c => c.Id == p.Id)
                                                       ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                }
                case ReportedTimeGroup.ProductArea_Id:
                {
                    var productAreas = _productAreaService.GetTopProductAreasWithChilds(customerId, false);
                    TranslateProductAreas(productAreas, 0);
                    var productAreasFullNames = _productAreaService.GetChildrenInRow(productAreas.OrderBy(p => p.Name).ToList()).ToList();
                    result.ForEach( p => p.Label = productAreasFullNames
                        .SingleOrDefault(c => c.Id == p.Id)
                        ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                }
                case ReportedTimeGroup.LogNoteDate:
                {
                    var formatter = new OutputFormatter(true, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    result.ForEach(d => d.Label = formatter.FormatNullableDate(d.DateTime));
                    result = result.OrderBy(d => d.DateTime).ToList();
                    break;
                }
                default:
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
            }
            var responce = new
            {
                totalLabel = string.Format("{0}: {1}", Translation.GetCoreTextTranslation("Summa"), totalHours),
                data = new 
                {
                    labels = result.Select(o => o.Label ?? "").ToList(),
                    datasets = new []
                    {
                        new 
                        {
                            label = Translation.GetCoreTextTranslation("Antal"),
                            data = result.Select(o => o.TotalTime / minutesInHour)
                        }
                    }
                }
            };

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNumberOfCasesData(NumberOfCasesReportFilterModel filter)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var includeCasesWithNoWorkingGroup = filter.WorkingGroups == null || !filter.WorkingGroups.Any();
            var includeCasesWithNoDepartments = filter.Departments == null || !filter.Departments.Any();
            filter.WorkingGroups = GetWorkingGroups(filter.WorkingGroups, customerId);
            filter.Departments = GetDepartments(filter.Departments, customerId);

            var dataFilter = GetCommonDataFilter<NumberOfCasesDataFilter>(filter);
            dataFilter.GroupBy = (NumberOfCasesGroup) filter.GroupBy;
            dataFilter.IncludeCasesWithNoWorkingGroup = includeCasesWithNoWorkingGroup;
            dataFilter.IncludeCasesWithNoDepartments = includeCasesWithNoDepartments;

            var result = _reportServiceService.GetNumberOfCasesData(dataFilter);
            var totalCases = result.Sum(c => c.CasesAmount);
            switch (dataFilter.GroupBy)
            {
                case NumberOfCasesGroup.CaseType_Id:
                    var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true);
                    TranslateCaseTypes(caseTypes, 0);
                    var caseTypesFullNames = _caseTypeService.GetChildrenInRow(caseTypes.OrderBy(p => p.Name).ToList()).ToList();
                    result.ForEach( p => p.Label = caseTypesFullNames
                                                       .SingleOrDefault(c => c.Id == p.Id)
                                                       ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationWeekday:
                    result.ForEach(p => { p.Label = GetWeekDayName(p.Id); });
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationMonth:
                    result.ForEach(p => { p.Label = GetMonthName(p.Id); });
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationYear:
                case NumberOfCasesGroup.RegistrationHour:
                    result = result.OrderBy(d => d.Id).ToList();
                    break;
                case NumberOfCasesGroup.RegistrationDate:
                case NumberOfCasesGroup.FinishingDate:
                    var formatter = new OutputFormatter(true, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    result.ForEach(d => d.Label = formatter.FormatNullableDate(d.DateTime));
                    result = result.OrderBy(d => d.DateTime).ToList();
                    break;
                case NumberOfCasesGroup.ProductArea_Id:
                    var productAreas = _productAreaService.GetTopProductAreasWithChilds(customerId, false);
                    TranslateProductAreas(productAreas, 0);
                    var productAreasFullNames = _productAreaService.GetChildrenInRow(productAreas.OrderBy(p => p.Name).ToList()).ToList();
                    result.ForEach( p => p.Label = productAreasFullNames
                                                       .SingleOrDefault(c => c.Id == p.Id)
                                                       ?.Name ?? "");
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
                default:
                    result = result.OrderBy(d => d.Label).ToList();
                    break;
            }
            
            var responce = new
            {
                totalLabel = string.Format("{0}: {1}", Translation.GetCoreTextTranslation("Summa"), totalCases),
                data = new 
                {
                    labels = result.Select(o => o.Label ?? "").ToList(),
                    datasets = new []
                    {
                        new 
                        {
                            label = Translation.GetCoreTextTranslation("Antal"),
                            data = result.Select(c => c.CasesAmount)
                        }
                    }
                }
            };
            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        private List<int> GetWorkingGroups(List<int> filterWorkingGroups, int customerId)
        {
            //*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);
            var workingGroups = user.UserGroup_Id > UserGroups.Administrator
                ? _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).Select(w => w.Id).ToList()
                : _workingGroupService.GetWorkingGroups(customerId, user.Id, false, true).Select(w => w.Id).ToList();
            if (filterWorkingGroups == null || !filterWorkingGroups.Any())
                return workingGroups;

            return filterWorkingGroups.Where(w => workingGroups.Contains(w)).ToList();
        }

        public List<int> GetDepartments(List<int> filterDepartments, int customerId)
        {
            var departments = _departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, customerId, false).Select(w => w.Id).ToList();
            if (!departments.Any())
                departments = _departmentService.GetDepartments(customerId, ActivationStatus.All).Select(w => w.Id).ToList();

            if (filterDepartments == null || !filterDepartments.Any())
                return departments;

            return filterDepartments.Where(w => departments.Contains(w)).ToList();
        }

        private string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return Translation.GetCoreTextTranslation("Januari");
                case 2:
                    return Translation.GetCoreTextTranslation("Februari");
                case 3:
                    return Translation.GetCoreTextTranslation("Mars");
                case 4:
                    return Translation.GetCoreTextTranslation("April");
                case 5:
                    return Translation.GetCoreTextTranslation("Maj");
                case 6:
                    return Translation.GetCoreTextTranslation("Juni");
                case 7:
                    return Translation.GetCoreTextTranslation("Juli");
                case 8:
                    return Translation.GetCoreTextTranslation("Augusti");
                case 9:
                    return Translation.GetCoreTextTranslation("September");
                case 10:
                    return Translation.GetCoreTextTranslation("Oktober");
                case 11:
                    return Translation.GetCoreTextTranslation("November");
                case 12:
                    return Translation.GetCoreTextTranslation("December");
            }

            return "";
        }

        private string GetWeekDayName(int weekDay)
        {
            switch (weekDay)
            {
                case 1:
                    return Translation.GetCoreTextTranslation("Söndag");
                case 2:
                    return Translation.GetCoreTextTranslation("Måndag");
                case 3:
                    return Translation.GetCoreTextTranslation("Tisdag");
                case 4:
                    return Translation.GetCoreTextTranslation("Onsdag");
                case 5:
                    return Translation.GetCoreTextTranslation("Torsdag");
                case 6:
                    return Translation.GetCoreTextTranslation("Fredag");
                case 7:
                    return Translation.GetCoreTextTranslation("Lördag");
            }

            return Translation.GetCoreTextTranslation("Saknas");
        }

        private T GetCommonDataFilter<T>(CommonReportFilterModel filter) where T: CommonReportDataFilter, new()
        {
            return new T
            {
                CustomerID = SessionFacade.CurrentCustomer.Id,
                Administrators = filter.Administrators,
                CaseStatus = filter.CaseStatus,
                CaseTypes = filter.CaseTypes,
                RegisterFrom = filter.RegisterFrom,
                RegisterTo = filter.RegisterTo.HasValue ? filter.RegisterTo.GetEndOfDay() : new DateTime?(),
                CloseFrom = filter.CloseFrom,
                CloseTo = filter.CloseTo.HasValue ? filter.CloseTo.GetEndOfDay() : new DateTime?(),
                Departments = filter.Departments,
                ProductAreas = filter.ProductAreas,
                WorkingGroups = filter.WorkingGroups,
            };
        }

        private void TranslateCaseTypes(IEnumerable<CaseType> caseTypes, int depth)
        {
            if (depth >= 20)
                throw new Exception("Iteration depth exceeded. Suspicion of infinte loop.");

            depth++;

            caseTypes.ForEach(p => 
            {
                p.Name = Translation.GetCoreTextTranslation(p.Name);

                if (p.SubCaseTypes != null && p.SubCaseTypes.Any())
                {
                    TranslateCaseTypes(p.SubCaseTypes, depth);
                }
            });
        }

        private void TranslateProductAreas(IEnumerable<ProductArea> products, int depth)
        {
            if (depth >= 20)
                throw new Exception("Iteration depth exceeded. Suspicion of infinte loop.");

            depth++;

            products.ForEach(p =>
            {
                p.Name = Translation.GetCoreTextTranslation(p.Name);
                if (p.SubProductAreas != null && p.SubProductAreas.Any())
                {
                    TranslateProductAreas(p.SubProductAreas, depth);
                }
            });
        }
    }
}
