using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    using dhHelpdesk_NG.Domain.Changes;

    [CustomAuthorize(Roles = "4")]
    public class ChangeCategoryController : BaseController
    {
        private readonly IChangeCategoryService _changeCategoryService;
        private readonly ICustomerService _customerService;

        public ChangeCategoryController(
            IChangeCategoryService changeCategoryService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _changeCategoryService = changeCategoryService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changecategories = _changeCategoryService.GetChangeCategories(customer.Id).ToList();

            var model = new ChangeCategoryIndexViewModel { ChangeCategories = changecategories, Customer = customer };
            
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeCategory = new ChangeCategoryEntity { Customer_Id = customer.Id };

            var model = new ChangeCategoryInputViewModel { ChangeCategory = changeCategory, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeCategoryEntity changeCategory)
        {
            if (ModelState.IsValid)
            {
                _changeCategoryService.NewChangeCategory(changeCategory);
                _changeCategoryService.Commit();

                return RedirectToAction("index", "changecategory", new { customerId = changeCategory.Customer_Id });
            }

            var customer = _customerService.GetCustomer(changeCategory.Customer_Id);
            var model = CreateInputViewModel(changeCategory, customer);
            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeCategory = _changeCategoryService.GetChangeCategory(id, customerId);

            if (changeCategory == null)               
                return new HttpNotFoundResult("No change category found...");

            var customer = _customerService.GetCustomer(changeCategory.Customer_Id);
            var model = CreateInputViewModel(changeCategory, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeCategoryEntity changeCategory)
        {
            if (ModelState.IsValid)
            {
                _changeCategoryService.UpdateChangeCategory(changeCategory);
                _changeCategoryService.Commit();

                return RedirectToAction("index", "changecategory", new { customerId = changeCategory.Customer_Id });
            }

            var customer = _customerService.GetCustomer(changeCategory.Customer_Id);
            var model = CreateInputViewModel(changeCategory, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeCategory = _changeCategoryService.GetChangeCategory(id, customerId);

            if (_changeCategoryService.DeleteChangeCategory(id) == DeleteMessage.Success)
                return RedirectToAction("index", "changecategory", new { customerId = changeCategory.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "changecategory", new { area = "admin", id = changeCategory.Id, customerId = changeCategory.Customer_Id });
            }
        }

        private ChangeCategoryInputViewModel CreateInputViewModel(ChangeCategoryEntity changeCategory, Customer customer)
        {
            var model = new ChangeCategoryInputViewModel
            {
                ChangeCategory = changeCategory,
                Customer = customer
            };

            return model;
        }
    }
}
