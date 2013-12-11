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
    public class FinishingCauseCategoryController : BaseController
    {
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICustomerService _customerService;

        public FinishingCauseCategoryController(
            IFinishingCauseService finishingCauseService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _finishingCauseService = finishingCauseService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var finishingCauseCategories = _finishingCauseService.GetFinishingCauseCategories(customer.Id).ToList();

            var model = new FinishingCauseCategoryIndexViewModel { FinishingCauseCategories = finishingCauseCategories, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var finishingCauseCategory = new FinishingCauseCategory { Customer_Id = customer.Id };

            var model = CreateInputViewModel(finishingCauseCategory, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(FinishingCauseCategory finishingCauseCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _finishingCauseService.SaveFinishingCauseCategory(finishingCauseCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "finishingcausecategory", new { customerId = finishingCauseCategory.Customer_Id });

            var customer = _customerService.GetCustomer(finishingCauseCategory.Customer_Id);

            var model = CreateInputViewModel(finishingCauseCategory, customer);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var finishingCauseCategory = _finishingCauseService.GetFinishingCauseCategory(id);

            if (finishingCauseCategory == null)
                return new HttpNotFoundResult("No finishing cause category found...");

            var customer = _customerService.GetCustomer(finishingCauseCategory.Customer_Id);
            var model = CreateInputViewModel(finishingCauseCategory, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FinishingCauseCategory finishingCauseCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _finishingCauseService.SaveFinishingCauseCategory(finishingCauseCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "finishingcausecategory", new { customerId = finishingCauseCategory.Customer_Id });

            var customer = _customerService.GetCustomer(finishingCauseCategory.Customer_Id);
            var model = CreateInputViewModel(finishingCauseCategory, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var finishingCauseCategory = _finishingCauseService.GetFinishingCauseCategory(id);

            if (_finishingCauseService.DeleteFinishingCauseCategory(id) == DeleteMessage.Success)
                return RedirectToAction("index", "finishingcausecategory", new { customerId = finishingCauseCategory.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "finishingcausecategory", new { area = "admin", id = finishingCauseCategory.Id});
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

