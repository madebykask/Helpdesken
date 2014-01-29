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
    public class ChangePriorityController : BaseController
    {
        private readonly IChangePriorityService _changePriorityService;
        private readonly ICustomerService _customerService;

        public ChangePriorityController(
            IChangePriorityService changePriorityService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _changePriorityService = changePriorityService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changepriorities = _changePriorityService.GetChangePriorities(customer.Id).ToList();

            var model = new ChangePriorityIndexViewModel { ChangePriorities = changepriorities, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changePriority = new ChangePriorityEntity{ Customer_Id = customer.Id };

            var model = new ChangePriorityInputViewModel { ChangePriority = changePriority, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(ChangePriorityEntity changePriority)
        {
            if (ModelState.IsValid)
            {
                _changePriorityService.NewChangePriority(changePriority);
                _changePriorityService.Commit();

                return RedirectToAction("index", "changepriority", new { customerId = changePriority.Customer_Id });
            }
            var customer = _customerService.GetCustomer(changePriority.Customer_Id);
            var model = CreateInputViewModel(changePriority, customer);
            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changePriority = _changePriorityService.GetChangePriority(id, customerId);

            if (changePriority == null)               
                return new HttpNotFoundResult("No change priority found...");

            var customer = _customerService.GetCustomer(changePriority.Customer_Id);
            var model = CreateInputViewModel(changePriority, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangePriorityEntity changePriority)
        {
            if (ModelState.IsValid)
            {
                _changePriorityService.UpdateChangePriority(changePriority);
                _changePriorityService.Commit();

                return RedirectToAction("index", "changepriority", new { customerId = changePriority.Customer_Id });
            }

            var customer = _customerService.GetCustomer(changePriority.Customer_Id);
            var model = CreateInputViewModel(changePriority, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var changePriority = _changePriorityService.GetChangePriority(id, SessionFacade.CurrentCustomer.Id);

            if (_changePriorityService.DeleteChangePriority(id) == DeleteMessage.Success)
                return RedirectToAction("index", "changepriority", new { customerId = changePriority.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "changepriority", new { area = "admin", id = changePriority.Id, customerId = changePriority.Customer_Id });
            }
        }

        private ChangePriorityInputViewModel CreateInputViewModel(ChangePriorityEntity changePriority, Customer customer)
        {
            var model = new ChangePriorityInputViewModel
            {
                ChangePriority = changePriority,
                Customer = customer
            };

            return model;
        }
    }
}
