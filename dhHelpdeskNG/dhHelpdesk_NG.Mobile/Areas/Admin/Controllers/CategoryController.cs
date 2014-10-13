namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Models;

    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;

        public CategoryController(
            ICategoryService categoryService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._categoryService = categoryService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var categories = this._categoryService.GetCategories(customer.Id);

            var model = new CategoryIndexViewModel { Categories = categories, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var category = new Category { Customer_Id = customer.Id, IsActive = 1, Id = 0 };
            var model = new CategoryInputViewModel { Category = category, Customer = customer };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Category category)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._categoryService.SaveCategory(category, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "category", new { customerId = category.Customer_Id });


            var customer = this._customerService.GetCustomer(category.Customer_Id);
            var model = new CategoryInputViewModel { Category = category, Customer = customer };


            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {

            var category = this._categoryService.GetCategory(id, customerId);

            if (category == null)                
                return new HttpNotFoundResult("No category found...");

            var customer = this._customerService.GetCustomer(category.Customer_Id);
            var model = this.CreateInputViewModel(category, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (this.ModelState.IsValid)
            {
                this._categoryService.UpdateCategory(category);
                this._categoryService.Commit();

                return this.RedirectToAction("index", "category", new { customerId = category.Customer_Id});
              
            }

            var customer = this._customerService.GetCustomer(category.Customer_Id);
            
            var model = this.CreateInputViewModel(category, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customer_id)
        {
            var category = this._categoryService.GetCategory(id, customer_id);

            if (this._categoryService.DeleteCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "category", new { customerId = category.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "category", new { area = "admin", id = category.Id, customerId = category.Customer_Id });
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