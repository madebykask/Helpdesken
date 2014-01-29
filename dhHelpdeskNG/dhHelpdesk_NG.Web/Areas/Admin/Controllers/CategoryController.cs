using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;

        public CategoryController(
            ICategoryService categoryService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _categoryService = categoryService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var categories = _categoryService.GetCategories(customer.Id);

            var model = new CategoryIndexViewModel { Categories = categories, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var category = new Category { Customer_Id = customer.Id, IsActive = 1, Id = 0 };
            var model = new CategoryInputViewModel { Category = category, Customer = customer };

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Category category)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _categoryService.SaveCategory(category, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "category", new { customerId = category.Customer_Id });


            var customer = _customerService.GetCustomer(category.Customer_Id);
            var model = new CategoryInputViewModel { Category = category, Customer = customer };


            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {

            var category = _categoryService.GetCategory(id, customerId);

            if (category == null)                
                return new HttpNotFoundResult("No category found...");

            var customer = _customerService.GetCustomer(category.Customer_Id);
            var model = CreateInputViewModel(category, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(category);
                _categoryService.Commit();

                return RedirectToAction("index", "category", new { customerId = category.Customer_Id});
              
            }

            var customer = _customerService.GetCustomer(category.Customer_Id);
            
            var model = CreateInputViewModel(category, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customer_id)
        {
            var category = _categoryService.GetCategory(id, customer_id);

            if (_categoryService.DeleteCategory(id) == DeleteMessage.Success)
                return RedirectToAction("index", "category", new { customerId = category.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "category", new { area = "admin", id = category.Id, customerId = category.Customer_Id });
            }

        }

        private CategoryInputViewModel CreateInputViewModel(Category category, Customer customer)
        {
            var model = new CategoryInputViewModel
            {
                Category = category,
                Customer = customer
            };

            return model;
        }
    }
}