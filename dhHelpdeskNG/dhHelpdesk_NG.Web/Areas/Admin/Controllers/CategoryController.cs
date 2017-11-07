namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

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

        public JsonResult SetShowOnlyActiveCategoriesInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveCategoriesInAdmin = value;
            return this.Json(new { result = "success" });
        }


        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var categories = this._categoryService.GetAllCategories(customer.Id);

            var model = new CategoryIndexViewModel { Categories = categories, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActiveCategoriesInAdmin };

            return this.View(model);
        }

        public ActionResult New(int customerId, int? parentId = null)
        {
            if (parentId.HasValue)
            {
                if (_categoryService.GetCategory(parentId.Value, customerId) == null)
                    return new HttpNotFoundResult("No category found...");
            }

            var customer = this._customerService.GetCustomer(customerId);
            var category = new Category { Customer_Id = customer.Id, IsActive = 1, Id = 0, Parent_Category_Id = parentId};
            var model = this.CreateInputViewModel(category, customer);

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
            var model = this.CreateInputViewModel(category, customer);

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
            //if (this.ModelState.IsValid)
            //{
            //    this._categoryService.UpdateCategory(category);
            //    this._categoryService.Commit();

            //    return this.RedirectToAction("index", "category", new { customerId = category.Customer_Id});
              
            //}
            


            var categoryToSave = this._categoryService.GetCategory(category.Id, category.Customer_Id);

            categoryToSave.Name = category.Name;
            categoryToSave.Description = category.Description;
            categoryToSave.IsActive = category.IsActive;
            //categoryToSave.ParentCategory = category.ParentCategory;
            //categoryToSave.Parent_Category_Id = category.Parent_Category_Id;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._categoryService.SaveCategory(categoryToSave, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "category", new { customerId = category.Customer_Id });





            var customer = this._customerService.GetCustomer(category.Customer_Id);
            
            var model = this.CreateInputViewModel(category, customer);
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, Translation.Get(error.Value));
            }
            model.Category.IsActive = 0;
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
            const int MAX_LEVEL_DEEP = 6;
            var ca = category;
            var level = 1;
            while (ca.ParentCategory != null)
            {
                level++;
                ca = ca.ParentCategory;
            }
            var model = new CategoryInputViewModel
            {
                Category = category,
                Customer = customer,
                CanAddChild = level < MAX_LEVEL_DEEP
            };

            return model;
        }
    }
}