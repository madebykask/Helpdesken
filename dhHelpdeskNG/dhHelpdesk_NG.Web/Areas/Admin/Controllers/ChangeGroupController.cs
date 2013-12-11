using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class ChangeGroupController : BaseController
    {
        private readonly IChangeGroupService _changeGroupService;
        private readonly ICustomerService _customerService;

        public ChangeGroupController(
            IChangeGroupService changeGroupService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _changeGroupService = changeGroupService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changegroups = _changeGroupService.GetChangeGroups(customer.Id).ToList();

            var model = new ChangeGroupIndexViewModel { ChangeGroups = changegroups, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var changeGroup = new ChangeGroup { Customer_Id = customer.Id };
            var model = new ChangeGroupInputViewModel { ChangeGroup = changeGroup, Customer = customer };

            return View(model);
        }

        [HttpPost]
        public ActionResult New(ChangeGroup changeGroup)
        {
            if (ModelState.IsValid)
            {
                _changeGroupService.NewChangeGroup(changeGroup);
                _changeGroupService.Commit();

                return RedirectToAction("index", "changegroup", new { customerId = changeGroup.Customer_Id });
            }

            var customer = _customerService.GetCustomer(changeGroup.Customer_Id);
            var model = CreateInputViewModel(changeGroup, customer);

            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var changeGroup = _changeGroupService.GetChangeGroup(id, customerId);

            if (changeGroup == null)                
                return new HttpNotFoundResult("No change group found...");

            var customer = _customerService.GetCustomer(changeGroup.Customer_Id);
            var model = CreateInputViewModel(changeGroup, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChangeGroup changeGroup)
        {
            if (ModelState.IsValid)
            {
                _changeGroupService.UpdateChangeGroup(changeGroup);
                _changeGroupService.Commit();

                return RedirectToAction("index", "changegroup", new { customerId = changeGroup.Customer_Id });
            }
            var customer = _customerService.GetCustomer(changeGroup.Customer_Id);
            var model = CreateInputViewModel(changeGroup, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var changeGroup = _changeGroupService.GetChangeGroup(id, customerId);

            if (_changeGroupService.DeleteChangeGroup(id) == DeleteMessage.Success)
                return RedirectToAction("index", "changegroup", new { customerId = changeGroup.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "changegroup", new { area = "admin", id = changeGroup.Id, customerId = changeGroup.Customer_Id });
            }
        }

        private ChangeGroupInputViewModel CreateInputViewModel(ChangeGroup changeGroup, Customer customer)
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
