namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;
    using BusinessData.Models;

    public class CaseTypeController : BaseAdminController
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private const int MaxCaseTpyeLevels = 6;
        private readonly IWorkingGroupService _workingGroupService;

        public CaseTypeController(
            ICaseTypeService caseTypeService,
            IUserService userService,
            ICustomerService customerService,
            IMasterDataService masterDataService,
            IWorkingGroupService workingGroupService)
            : base (masterDataService)
        {
            _caseTypeService = caseTypeService;
            _userService = userService;
            _customerService = customerService;
            _workingGroupService = workingGroupService;
        }

        public JsonResult SetShowOnlyActiveCaseTypesInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveCaseTypesInAdmin = value;
            return Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var caseTypes = _caseTypeService.GetCaseTypes(customer.Id);

            var model = new CaseTypeIndexViewModel { CaseTypes = caseTypes, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActiveCaseTypesInAdmin };

            return View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (_caseTypeService.GetCaseType(parentId.Value) == null)
                    return new HttpNotFoundResult("No parent case type found...");
            }

            var caseType = new CaseType { Customer_Id = customer.Id, Parent_CaseType_Id = parentId, IsActive = 1};
            caseType.Selectable = 1;
            var model = CreateInputViewModel(caseType, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(CaseType caseType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _caseTypeService.SaveCaseType(caseType, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "casetype", new { customerid = caseType.Customer_Id });

            var customer = _customerService.GetCustomer(caseType.Customer_Id);
            var model = CreateInputViewModel(caseType, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var caseType = _caseTypeService.GetCaseType(id);

            if (caseType == null)
                return new HttpNotFoundResult("No case type found...");
            
            var customer = _customerService.GetCustomer(caseType.Customer_Id);
            var model = CreateInputViewModel(caseType, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CaseType caseType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _caseTypeService.SaveCaseType(caseType, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "casetype", new { customerid = caseType.Customer_Id });

            var customer = _customerService.GetCustomer(caseType.Customer_Id);
            var model = CreateInputViewModel(caseType, customer);

            return View(model);
        }

        public ActionResult Delete(int id)
        {

            var caseType = _caseTypeService.GetCaseType(id);

            if (_caseTypeService.DeleteCaseType(id) == DeleteMessage.Success)
                return RedirectToAction("index", "casetype", new { customerId = caseType.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "casetype", new { area = "admin", id = caseType.Id, customerId = caseType.Customer_Id });
            }

        }

        private CaseTypeInputViewModel CreateInputViewModel(CaseType caseType, Customer customer)
        {
            var parentCount = GetCaseTypeParentsCount(caseType);        

            var model = new CaseTypeInputViewModel
            {
                CanAddSubCaseType = (parentCount < MaxCaseTpyeLevels),
                CaseType = caseType,
                Customer = customer,
                SystemOwners = _userService.GetCustomerUsers(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList(),

                WorkingGroups = _workingGroupService.GetAllWorkingGroupsForCustomer(customer.Id, true)
            };

            return model;
        }

        private int GetCaseTypeParentsCount(CaseType caseType)
        {
            return caseType.ParentCaseType == null ? 1 : GetCaseTypeParentsCount(caseType.ParentCaseType) + 1;
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId)
        {
            IList<BusinessData.Models.User.CustomerUserInfo> performersList;
            var customerSettings = GetCustomerSettings(customerId);
            if (customerSettings.DontConnectUserToWorkingGroup == 0 && id > 0)
            {
                performersList = _userService.GetAvailablePerformersForWorkingGroup(customerId, id);
            }
            else
            {
                performersList = _userService.GetAvailablePerformersOrUserId(customerId);
            }

            if (customerSettings.IsUserFirstLastNameRepresentation == 1)
            {
                return
                    Json(new
                    {
                        list = performersList.OrderBy(it => it.FirstName).ThenBy(it => it.SurName).Select(it => new IdName
                        {
                            id = it.Id,
                            name = $"{it.FirstName} {it.SurName}".Trim()
                        })
                    });
            }

            return
                Json(new
                {
                    list = performersList.OrderBy(it => it.SurName).ThenBy(it => it.FirstName).Select(it => new IdName
                    {
                        id = it.Id,
                        name = $"{it.SurName} {it.FirstName}".Trim()
                    })
                });
        }
    }
}
