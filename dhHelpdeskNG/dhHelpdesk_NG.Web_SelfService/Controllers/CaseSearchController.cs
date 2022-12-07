namespace DH.Helpdesk.SelfService.Controllers
{
    using DH.Helpdesk.SelfService.Infrastructure;
    using DH.Helpdesk.Services.Services;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class CaseSearchController : Controller
    {

        private readonly ISettingService _settingService;
        private readonly IDepartmentService _departmentService;
        private readonly IComputerService _computerService;

        public CaseSearchController(ISettingService settingService, IDepartmentService departmentService, IComputerService computerService)
        {
            _settingService = settingService;
            _departmentService = departmentService;
            _computerService = computerService;
        }

        //
        // GET: /CaseSearch/

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Search_User(string query, int customerId, string searchKey, int? categoryID = null)
        {
            IList<int> departmentIds = new List<int>();
            var applyUserSearchRestriction = _settingService.GetCustomerSettings(customerId).ComputerUserSearchRestriction == 1;
            if (applyUserSearchRestriction)
            {
                departmentIds = _departmentService.GetDepartmentsIdsByUserPermissions(SessionFacade.CurrentUser.Id, customerId);
                //user has no departments checked == access to all departments. TODO: change getdepartmentsbyuserpermissions to actually reflect the "none selected"
                if (departmentIds.Count == 0)
                {
                    departmentIds = _departmentService.GetDepartmentsIds(customerId);
                }
            }

            var result = _computerService.SearchComputerUsers(customerId, query, categoryID, departmentIds);
            return Json(new { searchKey = searchKey, result = result });
        }

    }
}
