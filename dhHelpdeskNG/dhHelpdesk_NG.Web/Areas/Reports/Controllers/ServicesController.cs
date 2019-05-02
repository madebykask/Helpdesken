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

    public class ServicesController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseTypeService _caseTypeService;

        public ServicesController(
            IMasterDataService masterDataService, 
            IUserService userService, 
            IWorkContext workContext,
            IWorkingGroupService workingGroupService,
            ICaseTypeService caseTypeService)
            : base(masterDataService)
        {
            _userService = userService;
            _workContext = workContext;
            _workingGroupService = workingGroupService;
            _caseTypeService = caseTypeService;
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
            /*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */
            var workingGroups = user.UserGroup_Id > UserGroups.Administrator ?
                _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).ToList() :
                _workingGroupService.GetWorkingGroups(customerId, user.Id, false, true).ToList();
            
            var caseTypes = this._caseTypeService.GetCaseTypes(customerId).ToList();
            var caseTypesInRow = this._caseTypeService.GetChildrenInRow(caseTypes).ToList();

            var data = new {
                WorkingGroups = workingGroups.Take(10).Select(wg => wg.WorkingGroupName).ToList(),
                CaseTypes = caseTypesInRow.Take(10).Select(ct => ct.Name).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
