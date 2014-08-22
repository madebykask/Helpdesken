namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class FinishingCauseCategoryController : BaseAdminController
    {
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICustomerService _customerService;

        public FinishingCauseCategoryController(
            IFinishingCauseService finishingCauseService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._finishingCauseService = finishingCauseService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var finishingCauseCategories = this._finishingCauseService.GetFinishingCauseCategories(customer.Id).ToList();

            var model = new FinishingCauseCategoryIndexViewModel { FinishingCauseCategories = finishingCauseCategories, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var finishingCauseCategory = new FinishingCauseCategory { Customer_Id = customer.Id };

            var model = this.CreateInputViewModel(finishingCauseCategory, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(FinishingCauseCategory finishingCauseCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._finishingCauseService.SaveFinishingCauseCategory(finishingCauseCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "finishingcausecategory", new { customerId = finishingCauseCategory.Customer_Id });

            var customer = this._customerService.GetCustomer(finishingCauseCategory.Customer_Id);

            var model = this.CreateInputViewModel(finishingCauseCategory, customer);
            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var finishingCauseCategory = this._finishingCauseService.GetFinishingCauseCategory(id);

            if (finishingCauseCategory == null)
                return new HttpNotFoundResult("No finishing cause category found...");

            var customer = this._customerService.GetCustomer(finishingCauseCategory.Customer_Id);
            var model = this.CreateInputViewModel(finishingCauseCategory, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(FinishingCauseCategory finishingCauseCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._finishingCauseService.SaveFinishingCauseCategory(finishingCauseCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "finishingcausecategory", new { customerId = finishingCauseCategory.Customer_Id });

            var customer = this._customerService.GetCustomer(finishingCauseCategory.Customer_Id);
            var model = this.CreateInputViewModel(finishingCauseCategory, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var finishingCauseCategory = this._finishingCauseService.GetFinishingCauseCategory(id);

            if (this._finishingCauseService.DeleteFinishingCauseCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "finishingcausecategory", new { customerId = finishingCauseCategory.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "finishingcausecategory", new { area = "admin", id = finishingCauseCategory.Id});
            }
        }

        private FinishingCauseCategoryInputViewModel CreateInputViewModel(FinishingCauseCategory finishingCauseCategory, Customer customer)
        {
            var model = new FinishingCauseCategoryInputViewModel
            {
                FinishingCauseCategory = finishingCauseCategory,
                Customer = customer
            };

            return model;
        }
    }
}

