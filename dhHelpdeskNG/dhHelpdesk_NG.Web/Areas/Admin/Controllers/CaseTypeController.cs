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
            this._caseTypeService = caseTypeService;
            this._userService = userService;
            this._customerService = customerService;
            this._workingGroupService = workingGroupService;
        }

        public JsonResult SetShowOnlyActiveCaseTypesInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveCaseTypesInAdmin = value;
            return this.Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var caseTypes = this._caseTypeService.GetCaseTypes(customer.Id);

            var model = new CaseTypeIndexViewModel { CaseTypes = caseTypes, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActiveCaseTypesInAdmin };

            return this.View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (this._caseTypeService.GetCaseType(parentId.Value) == null)
                    return new HttpNotFoundResult("No parent case type found...");
            }

            var caseType = new CaseType { Customer_Id = customer.Id, Parent_CaseType_Id = parentId, IsActive = 1};
            caseType.Selectable = 1;
            var model = this.CreateInputViewModel(caseType, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(CaseType caseType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._caseTypeService.SaveCaseType(caseType, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "casetype", new { customerid = caseType.Customer_Id });

            var customer = this._customerService.GetCustomer(caseType.Customer_Id);
            var model = this.CreateInputViewModel(caseType, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var caseType = this._caseTypeService.GetCaseType(id);

            if (caseType == null)
                return new HttpNotFoundResult("No case type found...");
            
            var customer = this._customerService.GetCustomer(caseType.Customer_Id);
            var model = this.CreateInputViewModel(caseType, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(CaseType caseType)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._caseTypeService.SaveCaseType(caseType, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "casetype", new { customerid = caseType.Customer_Id });

            var customer = this._customerService.GetCustomer(caseType.Customer_Id);
            var model = this.CreateInputViewModel(caseType, customer);

            return this.View(model);
        }

        public ActionResult Delete(int id)
        {

            var caseType = this._caseTypeService.GetCaseType(id);

            if (this._caseTypeService.DeleteCaseType(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "casetype", new { customerId = caseType.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "casetype", new { area = "admin", id = caseType.Id, customerId = caseType.Customer_Id });
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
                SystemOwners = this._userService.GetAdministrators(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList(),
                WorkingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customer.Id, true)
            };

            return model;
        }

        private int GetCaseTypeParentsCount(CaseType caseType)
        {
            if (caseType.ParentCaseType == null)
                return 1;
            else
                return GetCaseTypeParentsCount(caseType.ParentCaseType) + 1;
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId)
        {
            IList<BusinessData.Models.User.CustomerUserInfo> performersList;
            var customerSettings = GetCustomerSettings(customerId);
            if (customerSettings.DontConnectUserToWorkingGroup == 0 && id > 0)
            {
                performersList = this._userService.GetAvailablePerformersForWorkingGroup(customerId, id);
            }
            else
            {
                performersList = this._userService.GetAvailablePerformersOrUserId(customerId);
            }

            if (customerSettings.IsUserFirstLastNameRepresentation == 1)
            {
                return
                    this.Json(
                        new
                        {
                            list =
                                    performersList.OrderBy(it => it.FirstName)
                                        .ThenBy(it => it.SurName)
                                        .Select(
                                            it =>
                                            new IdName
                                            {
                                                id = it.Id,
                                                name = string.Format("{0} {1}", it.FirstName, it.SurName)
                                            })
                        });
            }

            return
                this.Json(
                    new
                    {
                        list =
                                performersList.OrderBy(it => it.SurName)
                                    .ThenBy(it => it.FirstName)
                                    .Select(
                                        it =>
                                        new IdName
                                        {
                                            id = it.Id,
                                            name = string.Format("{0} {1}", it.SurName, it.FirstName)
                                        })
                    });
        }
    }
}
