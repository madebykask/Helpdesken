namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class OperationLogCategoryController : BaseAdminController
    {
        private readonly IOperationLogCategoryService _operationLogCategoryService;
        private readonly ICustomerService _customerService;

        public OperationLogCategoryController(
            IOperationLogCategoryService operationLogCategoryService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._operationLogCategoryService = operationLogCategoryService;
            this._customerService = customerService;
        }

        public JsonResult SetShowOnlyActiveOperationLogCategoriesInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveOperationLogCategoriesInAdmin = value;
            return this.Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var operationlogcategories = this._operationLogCategoryService.GetOperationLogCategories(customer.Id).ToList();

            var model = new OperationLogCategoryIndexViewModel { OperationLogCategories = operationlogcategories, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActiveOperationLogCategoriesInAdmin };
            return this.View(model);
        }    

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var operationLogCategory = new OperationLogCategory { Customer_Id = customer.Id, IsActive = 1 };

            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(OperationLogCategory operationLogCategory)
        {
            if (this.ModelState.IsValid)
            {
                this._operationLogCategoryService.NewOperationLogCategory(operationLogCategory);
                this._operationLogCategoryService.Commit();

                return this.RedirectToAction("index", "operationlogcategory", new { customerId = operationLogCategory.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(operationLogCategory.Customer_Id);
            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };
            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var operationLogCategory = this._operationLogCategoryService.GetOperationLogCategory(id, customerId);

            if (operationLogCategory == null)                
                return new HttpNotFoundResult("No operation log category found...");

            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(OperationLogCategory operationLogCategory)
        {
            if (this.ModelState.IsValid)
            {
                this._operationLogCategoryService.UpdateOperationLogCategory(operationLogCategory);
                this._operationLogCategoryService.Commit();

                return this.RedirectToAction("index", "operationlogcategory", new { customerId = operationLogCategory.Customer_Id });
            }
            var customer = this._customerService.GetCustomer(operationLogCategory.Customer_Id);
            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {

            var operationLogCategory = this._operationLogCategoryService.GetOperationLogCategory(id, customerId);
            if (this._operationLogCategoryService.DeleteOperationLogCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "operationlogcategory", new { customerId = operationLogCategory.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "operationlogcategory", new { area = "admin", id = operationLogCategory.Id, customerId = operationLogCategory.Customer_Id });
            }

            //var operationLogCategory = _operationLogCategoryService.GetOperationLogCategory(id, customerId);

            //if (operationLogCategory != null)
            //{
            //    _operationLogCategoryService.DeleteOperationLogCategory(operationLogCategory);
            //    _operationLogCategoryService.Commit();
            //}

            //return RedirectToAction("index", "operationlogcategory", new { customerId = customerId });
        }
    }
}
