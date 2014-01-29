using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    using dhHelpdesk_NG.Domain.Changes;

    [CustomAuthorize(Roles = "4")]
    public class ChangeObjectController : BaseController
    {
        private readonly IChangeObjectService _changeObjectService;
        private readonly ICustomerService _customerService;

        public ChangeObjectController(
            IChangeObjectService changeObjectService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _changeObjectService = changeObjectService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeObjects = _changeObjectService.GetChangeObjects(customer.Id).ToList();

            var model = new ChangeObjectIndexViewModel { ChangeObjects = changeObjects, Customer = customer };

            return View(model);
            
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeObject = new ChangeObjectEntity { Customer_Id = customer.Id };

            var model = new ChangeObjectInputViewModel { ChangeObject = changeObject, Customer = customer };

            return View(model);

        }

        [HttpPost]
        public ActionResult New(ChangeObjectEntity changeObject)
        {
            if (ModelState.IsValid)
            {
                _changeObjectService.NewChangeObject(changeObject);
                _changeObjectService.Commit();

                return RedirectToAction("index", "changeobject", new { customerId = changeObject.Customer_Id });
            }
            var customer = _customerService.GetCustomer(changeObject.Customer_Id);
            var model = CreateInputViewModel(changeObject, customer);
            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeObject = _changeObjectService.GetChangeObject(id, customerId);

            if (changeObject == null)                
                return new HttpNotFoundResult("No change object found...");

            var customer = _customerService.GetCustomer(changeObject.Customer_Id);
            var model = CreateInputViewModel(changeObject, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeObjectEntity changeObject)
        {
            if (ModelState.IsValid)
            {
                _changeObjectService.UpdateChangeObject(changeObject);
                _changeObjectService.Commit();

                return RedirectToAction("index", "changeobject", new { customerId = changeObject.Customer_Id });
            }

            var customer = _customerService.GetCustomer(changeObject.Customer_Id);
            var model = CreateInputViewModel(changeObject, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var changeObject = _changeObjectService.GetChangeObject(id, SessionFacade.CurrentCustomer.Id);

            if (_changeObjectService.DeleteChangeObject(id) == DeleteMessage.Success)
                return RedirectToAction("index", "changeobject", new { customerId = changeObject.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "changeobject", new { area = "admin", id = changeObject.Id, customerId = changeObject.Customer_Id });
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
