namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class CaseTypeController : BaseAdminController
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;

        public CaseTypeController(
            ICaseTypeService caseTypeService,
            IUserService userService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base (masterDataService)
        {
            this._caseTypeService = caseTypeService;
            this._userService = userService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var caseTypes = this._caseTypeService.GetCaseTypes(customer.Id);

            var model = new CaseTypeIndexViewModel { CaseTypes = caseTypes, Customer = customer };

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
            var model = new CaseTypeInputViewModel
            {
                CaseType = caseType,
                Customer = customer,
                SystemOwners = this._userService.GetAdministrators(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
