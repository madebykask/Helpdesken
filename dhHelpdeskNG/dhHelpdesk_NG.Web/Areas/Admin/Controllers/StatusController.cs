namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

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
            this._statusService= statusService;
            this._customerService = customerService;
            this._workingGroupService = workingGroupservice;
            this._stateSecondaryService = stateSecondaryService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var statuses = this._statusService.GetStatuses(customer.Id).ToList();

            var model = new StatusIndexViewModel { Statuses = statuses, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var status = new Status { Customer_Id = customer.Id, IsActive = 1 };

            var model = this.CreateInputViewModel(status, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Status status)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._statusService.SaveStatus(status, out errors);

            if(errors.Count == 0)
                return this.RedirectToAction("index", "status", new { customerId = status.Customer_Id });

            var customer = this._customerService.GetCustomer(status.Customer_Id);
            var model = this.CreateInputViewModel(status, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var status = this._statusService.GetStatus(id);

            if(status == null)
                return new HttpNotFoundResult("No status found...");

            var customer = this._customerService.GetCustomer(status.Customer_Id);
            var model = this.CreateInputViewModel(status, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Status status)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._statusService.SaveStatus(status, out errors);

            if(errors.Count == 0)
                return this.RedirectToAction("index", "status", new { customerId = status.Customer_Id });

            var customer = this._customerService.GetCustomer(status.Customer_Id);
            var model = this.CreateInputViewModel(status, customer);

            return this.View(model);
           
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var status = this._statusService.GetStatus(id);

            if (this._statusService.DeleteStatus(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "status", new { customerId = status.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "status", new { area = "admin", id = status.Id });
            }
        }

        private StatusInputViewModel CreateInputViewModel(Status status, Customer customer)
        {
            var model = new StatusInputViewModel
            {
                Status = status,
                Customer = customer,
                WorkingGroups = this._workingGroupService.GetWorkingGroups(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                StateSecondary = this._stateSecondaryService.GetStateSecondaries(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
