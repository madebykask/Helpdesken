namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class OUController : BaseAdminController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IOUService _ouService;
        private readonly IRegionService _regionService;
        private readonly ICustomerService _customerService;

        public OUController(
            IDepartmentService departmentService,
            IOUService ouService,
            IRegionService regionService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._departmentService = departmentService;
            this._ouService = ouService;
            this._regionService = regionService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var ous = this._ouService.GetOUs(customer.Id);

            var model = new OUIndexViewModel { OUs = ous, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var ou = new OU { IsActive = 1 };
            
            var model = this.CreateInputViewModel(ou, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(OU ou, int customerId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._ouService.SaveOU(ou, out errors);

            //var department = _departmentService.GetDepartment(ou.Department_Id);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "ou", new { customerId = customerId });

            var customer = this._customerService.GetCustomer(customerId);
            var model = this.CreateInputViewModel(ou, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var ou = this._ouService.GetOU(id);

            if (ou == null)
                return new HttpNotFoundResult("No ou found...");

            var model = this.CreateInputViewModel(ou, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Department")]OU ou)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var department = this._departmentService.GetDepartment(ou.Department_Id.GetValueOrDefault());

            this._ouService.SaveOU(ou, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "ou", new { customerId = department.Customer_Id });

            var customer = this._customerService.GetCustomer(department.Customer_Id);
            var model = this.CreateInputViewModel(ou, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var ou = this._ouService.GetOU(id);
            var customer = this._customerService.GetCustomer(customerId);

            if (this._ouService.DeleteOU(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "ou", new { customerId = customerId });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "ou", new { area = "admin", id = id, customerId = customerId });
            }
        }

        private OUInputViewModel CreateInputViewModel(OU ou, Customer customer)
        {
            var ous = this._ouService.GetOUs(SessionFacade.CurrentCustomer.Id);
            var departments = this._departmentService.GetDepartments(customer.Id);
            
            var model = new OUInputViewModel(ous, ou.Parent_OU_Id ?? 0)
            {
                OU = ou, 
                Customer = customer,
                Regions = this._regionService.GetRegions(SessionFacade.CurrentCustomer.Id).Where(x => x.IsActive == 1).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Departments = departments.Where(x => x.IsActive == 1).Select(x => new SelectListItem
                {
                    Text = x.DepartmentName,
                    Value = x.Id.ToString()
                }).ToList(),
                SDepartments = departments.Select(x => new SDepartment
                {
                    Id = x.Id,
                    Name = x.DepartmentName,
                    Region_Id = x.Region_Id
                }).ToList()
            };

            return model;
        }
    }
}
