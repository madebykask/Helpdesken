using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class CaseTypeController : BaseController
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
            _caseTypeService = caseTypeService;
            _userService = userService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var caseTypes = _caseTypeService.GetCaseTypes(customer.Id);

            var model = new CaseTypeIndexViewModel { CaseTypes = caseTypes, Customer = customer };

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
            var model = new CaseTypeInputViewModel
            {
                CaseType = caseType,
                Customer = customer,
                SystemOwners = _userService.GetAdministrators(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
