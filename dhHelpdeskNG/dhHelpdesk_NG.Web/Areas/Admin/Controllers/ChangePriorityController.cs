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
            this._changePriorityService = changePriorityService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changepriorities = this._changePriorityService.GetChangePriorities(customer.Id).ToList();

            var model = new ChangePriorityIndexViewModel { ChangePriorities = changepriorities, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changePriority = new ChangePriorityEntity{ Customer_Id = customer.Id };

            var model = new ChangePriorityInputViewModel { ChangePriority = changePriority, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ChangePriorityEntity changePriority)
        {
            if (this.ModelState.IsValid)
            {
                this._changePriorityService.NewChangePriority(changePriority);
                this._changePriorityService.Commit();

                return this.RedirectToAction("index", "changepriority", new { customerId = changePriority.Customer_Id });
            }
            var customer = this._customerService.GetCustomer(changePriority.Customer_Id);
            var model = this.CreateInputViewModel(changePriority, customer);
            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changePriority = this._changePriorityService.GetChangePriority(id, customerId);

            if (changePriority == null)               
                return new HttpNotFoundResult("No change priority found...");

            var customer = this._customerService.GetCustomer(changePriority.Customer_Id);
            var model = this.CreateInputViewModel(changePriority, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangePriorityEntity changePriority)
        {
            if (this.ModelState.IsValid)
            {
                this._changePriorityService.UpdateChangePriority(changePriority);
                this._changePriorityService.Commit();

                return this.RedirectToAction("index", "changepriority", new { customerId = changePriority.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(changePriority.Customer_Id);
            var model = this.CreateInputViewModel(changePriority, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var changePriority = this._changePriorityService.GetChangePriority(id, SessionFacade.CurrentCustomer.Id);

            if (this._changePriorityService.DeleteChangePriority(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "changepriority", new { customerId = changePriority.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "changepriority", new { area = "admin", id = changePriority.Id, customerId = changePriority.Customer_Id });
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
