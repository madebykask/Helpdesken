using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class StatusController : BaseController
    {
        private readonly IStatusService _statusService;
        private readonly ICustomerService _customerService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IStateSecondaryService _stateSecondaryService;

        public StatusController(
            IStatusService statusService,
            ICustomerService customerService,
            IWorkingGroupService workingGroupservice,
            IStateSecondaryService stateSecondaryService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _statusService= statusService;
            _customerService = customerService;
            _workingGroupService = workingGroupservice;
            _stateSecondaryService = stateSecondaryService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var statuses = _statusService.GetStatuses(customer.Id).ToList();

            var model = new StatusIndexViewModel { Statuses = statuses, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var status = new Status { Customer_Id = customer.Id, IsActive = 1 };

            var model = CreateInputViewModel(status, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Status status)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _statusService.SaveStatus(status, out errors);

            if(errors.Count == 0)
                return RedirectToAction("index", "status", new { customerId = status.Customer_Id });

            var customer = _customerService.GetCustomer(status.Customer_Id);
            var model = CreateInputViewModel(status, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var status = _statusService.GetStatus(id);

            if(status == null)
                return new HttpNotFoundResult("No status found...");

            var customer = _customerService.GetCustomer(status.Customer_Id);
            var model = CreateInputViewModel(status, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Status status)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _statusService.SaveStatus(status, out errors);

            if(errors.Count == 0)
                return RedirectToAction("index", "status", new { customerId = status.Customer_Id });

            var customer = _customerService.GetCustomer(status.Customer_Id);
            var model = CreateInputViewModel(status, customer);

            return View(model);
           
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var status = _statusService.GetStatus(id);

            if (_statusService.DeleteStatus(id) == DeleteMessage.Success)
                return RedirectToAction("index", "status", new { customerId = status.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "status", new { area = "admin", id = status.Id });
            }
        }

        private StatusInputViewModel CreateInputViewModel(Status status, Customer customer)
        {
            var model = new StatusInputViewModel
            {
                Status = status,
                Customer = customer,
                WorkingGroups = _workingGroupService.GetWorkingGroups(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                StateSecondary = _stateSecondaryService.GetStateSecondaries(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
