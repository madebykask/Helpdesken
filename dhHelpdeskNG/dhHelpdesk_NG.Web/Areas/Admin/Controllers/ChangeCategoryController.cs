namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

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
            this._changeCategoryService = changeCategoryService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changecategories = this._changeCategoryService.GetChangeCategories(customer.Id).ToList();

            var model = new ChangeCategoryIndexViewModel { ChangeCategories = changecategories, Customer = customer };
            
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeCategory = new ChangeCategoryEntity { Customer_Id = customer.Id };

            var model = new ChangeCategoryInputViewModel { ChangeCategory = changeCategory, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeCategoryEntity changeCategory)
        {
            if (this.ModelState.IsValid)
            {
                this._changeCategoryService.NewChangeCategory(changeCategory);
                this._changeCategoryService.Commit();

                return this.RedirectToAction("index", "changecategory", new { customerId = changeCategory.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(changeCategory.Customer_Id);
            var model = this.CreateInputViewModel(changeCategory, customer);
            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeCategory = this._changeCategoryService.GetChangeCategory(id, customerId);

            if (changeCategory == null)               
                return new HttpNotFoundResult("No change category found...");

            var customer = this._customerService.GetCustomer(changeCategory.Customer_Id);
            var model = this.CreateInputViewModel(changeCategory, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeCategoryEntity changeCategory)
        {
            if (this.ModelState.IsValid)
            {
                this._changeCategoryService.UpdateChangeCategory(changeCategory);
                this._changeCategoryService.Commit();

                return this.RedirectToAction("index", "changecategory", new { customerId = changeCategory.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(changeCategory.Customer_Id);
            var model = this.CreateInputViewModel(changeCategory, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeCategory = this._changeCategoryService.GetChangeCategory(id, customerId);

            if (this._changeCategoryService.DeleteChangeCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "changecategory", new { customerId = changeCategory.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "changecategory", new { area = "admin", id = changeCategory.Id, customerId = changeCategory.Customer_Id });
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
