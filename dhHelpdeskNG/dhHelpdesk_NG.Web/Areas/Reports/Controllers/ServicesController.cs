using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Areas.Reports.Models.ReportService;

namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
	using System.Web.Mvc;

	using DH.Helpdesk.Dal.Infrastructure.Context;
	using DH.Helpdesk.Services.Services;
	using DH.Helpdesk.Web.Infrastructure;
	using Services.Services.Reports;
	using BusinessData.Models.ReportService;
	using System;

	public class ServicesController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseTypeService _caseTypeService;
		private readonly IReportServiceService _reportServiceService;

		public ServicesController(
            IMasterDataService masterDataService, 
            IUserService userService, 
            IWorkContext workContext,
            IWorkingGroupService workingGroupService,
            ICaseTypeService caseTypeService,
			IReportServiceService reportServiceService)
            : base(masterDataService)
        {
            _userService = userService;
            _workContext = workContext;
            _workingGroupService = workingGroupService;
            _caseTypeService = caseTypeService;
			_reportServiceService = reportServiceService;

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

			if (filter.WorkingGroups != null && filter.WorkingGroups.Any())
			{
				workingGroups = workingGroups.Where(o => filter.WorkingGroups.Contains(o.Id)).ToList();
			}

			var result = _reportServiceService.GetHistoricalData(new HistoricalDataFilter
			{
				CustomerID = customerId,
				CaseStatus = filter.CaseStatus == 2 ? 1 : filter.CaseStatus == 1 ? 0 : (int?)null, // 1 active, 0 closed else null
				RegisterFrom = filter.RegisterFrom,
				RegisterTo = filter.RegisterTo,
				CloseFrom = filter.CloseFrom,
				CloseTo = filter.CloseTo,
				Administrators = filter.Administrators,
				Departments = filter.Departments,
				CaseTypes = filter.CaseTypes,
				ProductAreas = filter.ProductAreas,
				ChangeWorkingGroups = filter.HistoricalWorkingGroups,
				ChangeFrom = filter.HistoricalChangeDateFrom ?? DateTime.Now.AddYears(-20),
				ChangeTo = filter.HistoricalChangeDateTo ?? DateTime.Now.AddYears(20),
				WorkingGroups = workingGroups.Select(o => o.Id).ToList()
			});

			var wgs = result.Select(o => new { o.WorkingGroup, o.WorkingGroupID }).Distinct().OrderBy(o => o.WorkingGroup).ToArray();
			var data = new
			{
				labels = wgs.Select(o => o.WorkingGroup).ToArray(),
				datasets = result.GroupBy(o => new { o.CaseTypeID, o.CaseType }).Select(ct => new
				{
					label = ct.Key.CaseType,
					data = wgs.Select(wg => ct.Where(o => o.WorkingGroupID == wg.WorkingGroupID).Count()).ToArray()
				}).ToArray()
			};

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
