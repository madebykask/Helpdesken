using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class ChangeImplementationStatusController : BaseController
    {
        private readonly IChangeImplementationStatusService _changeImplementationStatusService;
        private readonly ICustomerService _customerService;

        public ChangeImplementationStatusController(
            IChangeImplementationStatusService changeImplementationStatusService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _changeImplementationStatusService = changeImplementationStatusService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeImplementationStatuses = _changeImplementationStatusService.GetChangeImplementationStatuses(customer.Id).ToList();

            var model = new ChangeImplementationStatusIndexViewModel { ChangeImplementationStatuses = changeImplementationStatuses, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeImplementationStatus = new ChangeImplementationStatus { Customer_Id = customer.Id };

            var model = new ChangeImplementationStatusInputViewModel { ChangeImplementationStatus = changeImplementationStatus, Customer = customer };
          
            return View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeImplementationStatus changeImplementationStatus)
        {
            if (ModelState.IsValid)
            {
                _changeImplementationStatusService.NewChangeImplementationStatus(changeImplementationStatus);
                _changeImplementationStatusService.Commit();

                return RedirectToAction("index", "changeimplementationstatus", new { customerId = changeImplementationStatus.Customer_Id });
            }

            var customer = _customerService.GetCustomer(changeImplementationStatus.Customer_Id);
            var model = CreateInputViewModel(changeImplementationStatus, customer);
            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeImplementationStatus = _changeImplementationStatusService.GetChangeImplementationStatus(id, customerId);

            if (changeImplementationStatus == null)                
                return new HttpNotFoundResult("No change implementation status found...");

            var customer = _customerService.GetCustomer(changeImplementationStatus.Customer_Id);
            var model = CreateInputViewModel(changeImplementationStatus, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeImplementationStatus changeImplementationStatus)
        {
            if (ModelState.IsValid)
            {
                _changeImplementationStatusService.UpdateChangeImplementationStatus(changeImplementationStatus);
                _changeImplementationStatusService.Commit();

                return RedirectToAction("index", "changeimplementationstatus", new { customerId = changeImplementationStatus.Customer_Id });
            }
            var customer = _customerService.GetCustomer(changeImplementationStatus.Customer_Id);
            var model = CreateInputViewModel(changeImplementationStatus, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeImplementationStatus = _changeImplementationStatusService.GetChangeImplementationStatus(id, customerId);

            if (_changeImplementationStatusService.DeleteChangeImplementationStatus(id) == DeleteMessage.Success)
                return RedirectToAction("index", "changeimplementationstatus", new { customerId = changeImplementationStatus.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "changeimplementationstatus", new { area = "admin", id = changeImplementationStatus.Id, customerId = changeImplementationStatus.Customer_Id });
            }
        }

        private ChangeImplementationStatusInputViewModel CreateInputViewModel(ChangeImplementationStatus changeImplementationStatus, Customer customer)
        {
            var model = new ChangeImplementationStatusInputViewModel
            {
                ChangeImplementationStatus = changeImplementationStatus,
                Customer = customer
            };

            return model;
        }
    }
}
