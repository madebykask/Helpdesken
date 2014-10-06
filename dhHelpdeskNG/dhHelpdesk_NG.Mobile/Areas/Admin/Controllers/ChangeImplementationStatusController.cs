namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Models;

    public class ChangeImplementationStatusController : BaseAdminController
    {
        private readonly IChangeImplementationStatusService _changeImplementationStatusService;
        private readonly ICustomerService _customerService;

        public ChangeImplementationStatusController(
            IChangeImplementationStatusService changeImplementationStatusService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._changeImplementationStatusService = changeImplementationStatusService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeImplementationStatuses = this._changeImplementationStatusService.GetChangeImplementationStatuses(customer.Id).ToList();

            var model = new ChangeImplementationStatusIndexViewModel { ChangeImplementationStatuses = changeImplementationStatuses, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeImplementationStatus = new ChangeImplementationStatusEntity { Customer_Id = customer.Id };

            var model = new ChangeImplementationStatusInputViewModel { ChangeImplementationStatus = changeImplementationStatus, Customer = customer };
          
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeImplementationStatusEntity changeImplementationStatus)
        {
            if (this.ModelState.IsValid)
            {
                this._changeImplementationStatusService.NewChangeImplementationStatus(changeImplementationStatus);
                this._changeImplementationStatusService.Commit();

                return this.RedirectToAction("index", "changeimplementationstatus", new { customerId = changeImplementationStatus.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(changeImplementationStatus.Customer_Id);
            var model = this.CreateInputViewModel(changeImplementationStatus, customer);
            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeImplementationStatus = this._changeImplementationStatusService.GetChangeImplementationStatus(id, customerId);

            if (changeImplementationStatus == null)                
                return new HttpNotFoundResult("No change implementation status found...");

            var customer = this._customerService.GetCustomer(changeImplementationStatus.Customer_Id);
            var model = this.CreateInputViewModel(changeImplementationStatus, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeImplementationStatusEntity changeImplementationStatus)
        {
            if (this.ModelState.IsValid)
            {
                this._changeImplementationStatusService.UpdateChangeImplementationStatus(changeImplementationStatus);
                this._changeImplementationStatusService.Commit();

                return this.RedirectToAction("index", "changeimplementationstatus", new { customerId = changeImplementationStatus.Customer_Id });
            }
            var customer = this._customerService.GetCustomer(changeImplementationStatus.Customer_Id);
            var model = this.CreateInputViewModel(changeImplementationStatus, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeImplementationStatus = this._changeImplementationStatusService.GetChangeImplementationStatus(id, customerId);

            if (this._changeImplementationStatusService.DeleteChangeImplementationStatus(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "changeimplementationstatus", new { customerId = changeImplementationStatus.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "changeimplementationstatus", new { area = "admin", id = changeImplementationStatus.Id, customerId = changeImplementationStatus.Customer_Id });
            }
        }

        private ChangeImplementationStatusInputViewModel CreateInputViewModel(ChangeImplementationStatusEntity changeImplementationStatus, Customer customer)
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
