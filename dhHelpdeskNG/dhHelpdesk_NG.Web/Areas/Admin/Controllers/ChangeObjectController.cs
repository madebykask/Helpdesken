namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class ChangeObjectController : BaseAdminController
    {
        private readonly IChangeObjectService _changeObjectService;
        private readonly ICustomerService _customerService;

        public ChangeObjectController(
            IChangeObjectService changeObjectService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._changeObjectService = changeObjectService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeObjects = this._changeObjectService.GetChangeObjects(customer.Id).ToList();

            var model = new ChangeObjectIndexViewModel { ChangeObjects = changeObjects, Customer = customer };

            return this.View(model);
            
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var changeObject = new ChangeObjectEntity { Customer_Id = customer.Id };

            var model = new ChangeObjectInputViewModel { ChangeObject = changeObject, Customer = customer };

            return this.View(model);

        }

        [HttpPost]
        public ActionResult New(ChangeObjectEntity changeObject)
        {
            if (this.ModelState.IsValid)
            {
                this._changeObjectService.NewChangeObject(changeObject);
                this._changeObjectService.Commit();

                return this.RedirectToAction("index", "changeobject", new { customerId = changeObject.Customer_Id });
            }
            var customer = this._customerService.GetCustomer(changeObject.Customer_Id);
            var model = this.CreateInputViewModel(changeObject, customer);
            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeObject = this._changeObjectService.GetChangeObject(id, customerId);

            if (changeObject == null)                
                return new HttpNotFoundResult("No change object found...");

            var customer = this._customerService.GetCustomer(changeObject.Customer_Id);
            var model = this.CreateInputViewModel(changeObject, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeObjectEntity changeObject)
        {
            if (this.ModelState.IsValid)
            {
                this._changeObjectService.UpdateChangeObject(changeObject);
                this._changeObjectService.Commit();

                return this.RedirectToAction("index", "changeobject", new { customerId = changeObject.Customer_Id });
            }

            var customer = this._customerService.GetCustomer(changeObject.Customer_Id);
            var model = this.CreateInputViewModel(changeObject, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var changeObject = this._changeObjectService.GetChangeObject(id, SessionFacade.CurrentCustomer.Id);

            if (this._changeObjectService.DeleteChangeObject(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "changeobject", new { customerId = changeObject.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "changeobject", new { area = "admin", id = changeObject.Id, customerId = changeObject.Customer_Id });
            }

        }

        private ChangeObjectInputViewModel CreateInputViewModel(ChangeObjectEntity changeObject, Customer customer)
        {
            var model = new ChangeObjectInputViewModel
            {
                ChangeObject = changeObject,
                Customer = customer
            };

            return model;
        }
    }
}
