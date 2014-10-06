namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class AccountActivityController : BaseAdminController
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
            this._accountActivityService = accountActivityService;
            this._caseTypeService = caseTypeService;
            this._documentService = documentService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var accountActivities = this._accountActivityService.GetAccountActivities(customer.Id);

            var model = new AccountActivityIndexViewModel { AccountActivities = accountActivities, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var accountAcctivity = new AccountActivity { Customer_Id = customer.Id };
            var model = this.CreateInputViewModel(accountAcctivity, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(AccountActivity accountActivity)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._accountActivityService.SaveAccountActivity(accountActivity, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "accountactivity", new { customerId = accountActivity.Customer_Id.Value });

            var customer = this._customerService.GetCustomer(accountActivity.Customer_Id.Value);
            var model = this.CreateInputViewModel(accountActivity, null);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var accountActivity = this._accountActivityService.GetAccountActivity(id);

            if (accountActivity == null)
                return new HttpNotFoundResult("No account activity found...");

            var customer = this._customerService.GetCustomer(accountActivity.Customer_Id.Value);
            var model = this.CreateInputViewModel(accountActivity, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(AccountActivity accountActivity)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._accountActivityService.SaveAccountActivity(accountActivity, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "accountactivity", new { customerId = accountActivity.Customer_Id.Value });

            var customer = this._customerService.GetCustomer(accountActivity.Customer_Id.Value);
            var model = this.CreateInputViewModel(accountActivity, null);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var accountActivity = this._accountActivityService.GetAccountActivity(id);
            var customer = this._customerService.GetCustomer(accountActivity.Customer_Id.Value);

            if (this._accountActivityService.DeleteAccountActivity(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "accountactivity", new { customerId = customer.Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "accountactivity", new { area = "admin", id = accountActivity.Id });
            }
        }

        private AccountActivityInputViewModel CreateInputViewModel(AccountActivity accountActivity, Customer customer)
        {
            var model = new AccountActivityInputViewModel
            {
                AccountActivity = accountActivity,
                Customer = customer,
                AccountActivityGroups = this._accountActivityService.GetAccountActivityGroups().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Documents = this._documentService.GetDocuments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Cases = this._caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
