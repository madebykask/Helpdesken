using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class ChangeStatusController : BaseController
    {
        private readonly IChangeStatusService _changeStatusService;
        private readonly ICustomerService _customerService;

        public ChangeStatusController(
            IChangeStatusService changeStatusService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _changeStatusService = changeStatusService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeStatuses = _changeStatusService.GetChangeStatuses(customer.Id).ToList();

            var model = new ChangeStatusIndexViewModel { ChangeStatuses = changeStatuses, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeStatus = new ChangeStatus { Customer_Id = customer.Id };

            var model = new ChangeStatusInputViewModel { ChangeStatus = changeStatus, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeStatus changeStatus)
        {
            if (ModelState.IsValid)
            {
                _changeStatusService.NewChangeStatus(changeStatus);
                _changeStatusService.Commit();

                return RedirectToAction("index", "changestatus", new { customerId = changeStatus.Customer_Id });
            }
            var customer = _customerService.GetCustomer(changeStatus.Customer_Id);
            var model = CreateInputViewModel(changeStatus, customer);
            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeStatus = _changeStatusService.GetChangeStatus(id, customerId);

            if (changeStatus == null)                
                return new HttpNotFoundResult("No change status found...");

            var customer = _customerService.GetCustomer(changeStatus.Customer_Id);
            var model = CreateInputViewModel(changeStatus, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeStatus changeStatus)
        {
            if (ModelState.IsValid)
            {
                _changeStatusService.UpdateChangeStatus(changeStatus);
                _changeStatusService.Commit();

                return RedirectToAction("index", "changestatus", new { customerId = changeStatus.Customer_Id });
            }
            var customer = _customerService.GetCustomer(changeStatus.Customer_Id);
            var model = CreateInputViewModel(changeStatus, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeStatus = _changeStatusService.GetChangeStatus(id, customerId);

            if (_changeStatusService.DeleteChangeStatus(id) == DeleteMessage.Success)
                return RedirectToAction("index", "changestatus", new { customerId = changeStatus.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "changestatus", new { area = "admin", id = changeStatus.Id, customerId = changeStatus.Customer_Id });
            }

        }

        private ChangeStatusInputViewModel CreateInputViewModel(ChangeStatus changeStatus, Customer customer)
        {
            var model = new ChangeStatusInputViewModel
            {
                ChangeStatus = changeStatus,
                Customer = customer
            };

            return model;
        }
    }
}
