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
            this._changeStatusService = changeStatusService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeStatuses = this._changeStatusService.GetChangeStatuses(customer.Id).ToList();

            var model = new ChangeStatusIndexViewModel { ChangeStatuses = changeStatuses, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeStatus = new ChangeStatusEntity { Customer_Id = customer.Id };

            var model = new ChangeStatusInputViewModel { ChangeStatus = changeStatus, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeStatusEntity changeStatus)
        {
            if (this.ModelState.IsValid)
            {
                this._changeStatusService.NewChangeStatus(changeStatus);
                this._changeStatusService.Commit();

                return this.RedirectToAction("index", "changestatus", new { customerId = changeStatus.Customer_Id });
            }
            var customer = this._customerService.GetCustomer(changeStatus.Customer_Id);
            var model = this.CreateInputViewModel(changeStatus, customer);
            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeStatus = this._changeStatusService.GetChangeStatus(id, customerId);

            if (changeStatus == null)                
                return new HttpNotFoundResult("No change status found...");

            var customer = this._customerService.GetCustomer(changeStatus.Customer_Id);
            var model = this.CreateInputViewModel(changeStatus, customer);
            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ChangeStatusEntity changeStatus)
        {
            if (this.ModelState.IsValid)
            {
                this._changeStatusService.UpdateChangeStatus(changeStatus);
                this._changeStatusService.Commit();

                return this.RedirectToAction("index", "changestatus", new { customerId = changeStatus.Customer_Id });
            }
            var customer = this._customerService.GetCustomer(changeStatus.Customer_Id);
            var model = this.CreateInputViewModel(changeStatus, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeStatus = this._changeStatusService.GetChangeStatus(id, customerId);

            if (this._changeStatusService.DeleteChangeStatus(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "changestatus", new { customerId = changeStatus.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "changestatus", new { area = "admin", id = changeStatus.Id, customerId = changeStatus.Customer_Id });
            }

        }

        private ChangeStatusInputViewModel CreateInputViewModel(ChangeStatusEntity changeStatus, Customer customer)
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
