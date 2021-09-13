using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class SystemController : BaseAdminController
    {
        private readonly ISupplierService _supplierService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IDomainService _domainService;

        public SystemController(
            ISupplierService supplierService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IUserService userService,
            ICustomerService customerService,
            IDomainService domainService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._supplierService = supplierService;
            this._systemService = systemService;
            this._urgencyService = urgencyService;
            this._userService = userService;
            this._customerService = customerService;
            this._domainService = domainService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var systems = this._systemService.GetSystems(customer.Id);

            var model = new SystemIndexViewModel { System = systems, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActiveSystemsInAdmin };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var system = new System { Customer_Id = customer.Id, Id = 0, Status = 1};
           
            var model = this.CreateInputViewModel(system, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(System system)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._systemService.SaveSystem(system, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "system", new { customerid = system.Customer_Id });

            var customer = this._customerService.GetCustomer(system.Customer_Id);
            var model = this.CreateInputViewModel(system, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var system = this._systemService.GetSystem(id);

            //var systemowneruser = _userService.GetSystemUserOwnerId(system.SystemOwnerUser_Id);

            if (system == null)
                return new HttpNotFoundResult("No system found...");

            var customer = this._customerService.GetCustomer(system.Customer_Id);
            var model = this.CreateInputViewModel(system, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(System system, FormCollection col)
        {

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._systemService.SaveSystem(system, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "system", new { customerid = system.Customer_Id });

            var customer = this._customerService.GetCustomer(system.Customer_Id);
            var model = this.CreateInputViewModel(system, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var system = this._systemService.GetSystem(id);

            if (this._systemService.DeleteSystem(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "system", new { customerid = system.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "system", new { area = "admin", id = id });
            }
        }

        public JsonResult SetShowOnlyActiveSystemsInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveSystemsInAdmin = value;
            return this.Json(new { result = "success" });
        }

        private SystemInputViewModel CreateInputViewModel(System system, Customer customer)
        {
            var model = new SystemInputViewModel
            {
                System = system,
                Customer = customer,
                Administrators = this._userService.GetSystemOwners(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList(),
                Suppliers = this._supplierService.GetSuppliers(customer.Id).Where(x => x.IsActive == 1).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                SystemResponsibleUsers = this._userService.GetUsers().Select(x => new SelectListItem
                {
                    Text = x.FirstName + x.SurName,
                    Value = x.Id.ToString()
                }).ToList(),
                Urgencies = this._urgencyService.GetUrgencies(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Domains = this._domainService.GetDomains(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                OperatingSystem = this._systemService.GetOperatingSystem(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Value
                }).ToList()
            };

            return model;
        }
    }
}

