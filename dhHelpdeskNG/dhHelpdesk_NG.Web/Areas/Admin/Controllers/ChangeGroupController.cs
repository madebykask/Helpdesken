namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class ChangeGroupController : BaseAdminController
    {
        private readonly IChangeGroupService _changeGroupService;
        private readonly ICustomerService _customerService;

        public ChangeGroupController(
            IChangeGroupService changeGroupService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._changeGroupService = changeGroupService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changegroups = this._changeGroupService.GetChangeGroups(customer.Id).ToList();

            var model = new ChangeGroupIndexViewModel { ChangeGroups = changegroups, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeGroup = new ChangeGroupEntity { Customer_Id = customer.Id };
            var model = new ChangeGroupInputViewModel { ChangeGroup = changeGroup, Customer = customer };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeGroupEntity changeGroup)
        {
            if (this.ModelState.IsValid)
            {
                this._changeGroupService.NewChangeGroup(changeGroup);
                this._changeGroupService.Commit();

                return this.RedirectToAction("index", "changegroup", new { customerId = changeGroup.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(changeGroup.Customer_Id);
            var model = this.CreateInputViewModel(changeGroup, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeGroup = this._changeGroupService.GetChangeGroup(id, customerId);

            if (changeGroup == null)                
                return new HttpNotFoundResult("No change group found...");

            var customer = this._customerService.GetCustomer(changeGroup.Customer_Id);
            var model = this.CreateInputViewModel(changeGroup, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeGroupEntity changeGroup)
        {
            if (this.ModelState.IsValid)
            {
                this._changeGroupService.UpdateChangeGroup(changeGroup);
                this._changeGroupService.Commit();

                return this.RedirectToAction("index", "changegroup", new { customerId = changeGroup.Customer_Id });
            }
            var customer = this._customerService.GetCustomer(changeGroup.Customer_Id);
            var model = this.CreateInputViewModel(changeGroup, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeGroup = this._changeGroupService.GetChangeGroup(id, customerId);

            if (this._changeGroupService.DeleteChangeGroup(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "changegroup", new { customerId = changeGroup.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "changegroup", new { area = "admin", id = changeGroup.Id, customerId = changeGroup.Customer_Id });
            }
        }

        private ChangeGroupInputViewModel CreateInputViewModel(ChangeGroupEntity changeGroup, Customer customer)
        {
            var model = new ChangeGroupInputViewModel
            {
                ChangeGroup = changeGroup,
                Customer = customer
            };

            return model;
        }
    }
}
