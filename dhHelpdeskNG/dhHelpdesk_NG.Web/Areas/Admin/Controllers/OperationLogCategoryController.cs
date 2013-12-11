using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class OperationLogCategoryController : BaseController
    {
        private readonly IOperationLogCategoryService _operationLogCategoryService;
        private readonly ICustomerService _customerService;

        public OperationLogCategoryController(
            IOperationLogCategoryService operationLogCategoryService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _operationLogCategoryService = operationLogCategoryService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var operationlogcategories = _operationLogCategoryService.GetOperationLogCategories(customer.Id).ToList();

            var model = new OperationLogCategoryIndexViewModel { OperationLogCategories = operationlogcategories, Customer = customer };
            return View(model);
        }    

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var operationLogCategory = new OperationLogCategory { Customer_Id = customer.Id, IsActive = 1 };

            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };

            return View(model);
        }

        [HttpPost]
        public ActionResult New(OperationLogCategory operationLogCategory)
        {
            if (ModelState.IsValid)
            {
                _operationLogCategoryService.NewOperationLogCategory(operationLogCategory);
                _operationLogCategoryService.Commit();

                return RedirectToAction("index", "operationlogcategory", new { customerId = operationLogCategory.Customer_Id });
            }

            var customer = _customerService.GetCustomer(operationLogCategory.Customer_Id);
            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };
            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var operationLogCategory = _operationLogCategoryService.GetOperationLogCategory(id, customerId);

            if (operationLogCategory == null)                
                return new HttpNotFoundResult("No operation log category found...");

            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OperationLogCategory operationLogCategory)
        {
            if (ModelState.IsValid)
            {
                _operationLogCategoryService.UpdateOperationLogCategory(operationLogCategory);
                _operationLogCategoryService.Commit();

                return RedirectToAction("index", "operationlogcategory", new { area = "admin" });
            }
            var customer = _customerService.GetCustomer(operationLogCategory.Customer_Id);
            var model = new OperationLogCategoryInputViewModel { OperationLogCategory = operationLogCategory, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {

            var operationLogCategory = _operationLogCategoryService.GetOperationLogCategory(id, customerId);
            if (_operationLogCategoryService.DeleteOperationLogCategory(id) == DeleteMessage.Success)
                return RedirectToAction("index", "operationlogcategory", new { customerId = operationLogCategory.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "operationlogcategory", new { area = "admin", id = operationLogCategory.Id, customerId = operationLogCategory.Customer_Id });
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
