using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class SystemController : BaseController
    {
        private readonly ISupplierService _supplierService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;

        public SystemController(
            ISupplierService supplierService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IUserService userService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _supplierService = supplierService;
            _systemService = systemService;
            _urgencyService = urgencyService;
            _userService = userService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var systems = _systemService.GetSystems(customer.Id);

            var model = new SystemIndexViewModel { System = systems, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var system = new Domain.System { Customer_Id = customer.Id, Id = 0 };
           
            var model = CreateInputViewModel(system, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Domain.System system)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _systemService.SaveSystem(system, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "system", new { customerid = system.Customer_Id });

            var customer = _customerService.GetCustomer(system.Customer_Id);
            var model = CreateInputViewModel(system, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var system = _systemService.GetSystem(id);

            if (system == null)
                return new HttpNotFoundResult("No system found...");

            var customer = _customerService.GetCustomer(system.Customer_Id);
            var model = CreateInputViewModel(system, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Domain.System system, FormCollection col)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _systemService.SaveSystem(system, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "system", new { customerid = system.Customer_Id });

            var customer = _customerService.GetCustomer(system.Customer_Id);
            var model = CreateInputViewModel(system, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var system = _systemService.GetSystem(id);

            if (_systemService.DeleteSystem(id) == DeleteMessage.Success)
                return RedirectToAction("index", "system", new { customerid = system.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "system", new { area = "admin", customerid = system.Customer_Id });
            }
        }

        private SystemInputViewModel CreateInputViewModel(Domain.System system, Customer customer)
        {
            var model = new SystemInputViewModel
            {
                System = system,
                Customer = customer,
                Administrators = _userService.GetSystemOwners(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList(),
                Suppliers = _supplierService.GetSuppliers(SessionFacade.CurrentCustomer.Id).Where(x => x.IsActive == 1).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                SystemResponsibleUsers = _userService.GetUsers().Select(x => new SelectListItem
                {
                    Text = x.FirstName + x.SurName,
                    Value = x.Id.ToString()
                }).ToList(),
                Urgencies = _urgencyService.GetUrgencies(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}

