using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class AccountActivityController : BaseController
    {
        private readonly IAccountActivityService _accountActivityService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IDocumentService _documentService;
        private readonly ICustomerService _customerService;

        public AccountActivityController(
            IAccountActivityService accountActivityService,
            ICaseTypeService caseTypeService,
            IDocumentService documentService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _accountActivityService = accountActivityService;
            _caseTypeService = caseTypeService;
            _documentService = documentService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var accountActivities = _accountActivityService.GetAccountActivities(customer.Id);

            var model = new AccountActivityIndexViewModel { AccountActivities = accountActivities, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var accountAcctivity = new AccountActivity { Customer_Id = customer.Id };
            var model = CreateInputViewModel(accountAcctivity, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(AccountActivity accountActivity)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _accountActivityService.SaveAccountActivity(accountActivity, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "accountactivity", new { customerId = accountActivity.Customer_Id.Value });

            var customer = _customerService.GetCustomer(accountActivity.Customer_Id.Value);
            var model = CreateInputViewModel(accountActivity, null);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var accountActivity = _accountActivityService.GetAccountActivity(id);

            if (accountActivity == null)
                return new HttpNotFoundResult("No account activity found...");

            var customer = _customerService.GetCustomer(accountActivity.Customer_Id.Value);
            var model = CreateInputViewModel(accountActivity, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AccountActivity accountActivity)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _accountActivityService.SaveAccountActivity(accountActivity, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "accountactivity", new { customerId = accountActivity.Customer_Id.Value });

            var customer = _customerService.GetCustomer(accountActivity.Customer_Id.Value);
            var model = CreateInputViewModel(accountActivity, null);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var accountActivity = _accountActivityService.GetAccountActivity(id);
            var customer = _customerService.GetCustomer(accountActivity.Customer_Id.Value);

            if (_accountActivityService.DeleteAccountActivity(id) == DeleteMessage.Success)
                return RedirectToAction("index", "accountactivity", new { customerId = customer.Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "accountactivity", new { area = "admin", id = accountActivity.Id });
            }
        }

        private AccountActivityInputViewModel CreateInputViewModel(AccountActivity accountActivity, Customer customer)
        {
            var model = new AccountActivityInputViewModel
            {
                AccountActivity = accountActivity,
                Customer = customer,
                AccountActivityGroups = _accountActivityService.GetAccountActivityGroups().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Documents = _documentService.GetDocuments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Cases = _caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
