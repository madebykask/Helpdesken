using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class OUController : BaseController
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
            _departmentService = departmentService;
            _ouService = ouService;
            _regionService = regionService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var ous = _ouService.GetOUs(customer.Id);

            var model = new OUIndexViewModel { OUs = ous, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var ou = new OU { IsActive = 1 };
            
            var model = CreateInputViewModel(ou, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult New(OU ou, int customerId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _ouService.SaveOU(ou, out errors);

            //var department = _departmentService.GetDepartment(ou.Department_Id);

            if (errors.Count == 0)
                return RedirectToAction("index", "ou", new { customerId = customerId });

            var customer = _customerService.GetCustomer(customerId);
            var model = CreateInputViewModel(ou, customer);

            return View(model);
        }

        public ActionResult Edit(int id, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var ou = _ouService.GetOU(id);

            if (ou == null)
                return new HttpNotFoundResult("No ou found...");

            var model = CreateInputViewModel(ou, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Department")]OU ou)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var department = _departmentService.GetDepartment(ou.Department_Id.GetValueOrDefault());

            _ouService.SaveOU(ou, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "ou", new { customerId = department.Customer_Id });

            var customer = _customerService.GetCustomer(department.Customer_Id);
            var model = CreateInputViewModel(ou, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, int customerId)
        {
            var ou = _ouService.GetOU(id);
            var customer = _customerService.GetCustomer(customerId);

            if (_ouService.DeleteOU(id) == DeleteMessage.Success)
                return RedirectToAction("index", "ou", new { customerId = customerId });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "ou", new { area = "admin", id = id, customerId = customerId });
            }
        }

        private OUInputViewModel CreateInputViewModel(OU ou, Customer customer)
        {
            var ous = _ouService.GetOUs(SessionFacade.CurrentCustomer.Id);
            var departments = _departmentService.GetDepartments(customer.Id);
            
            var model = new OUInputViewModel(ous, ou.Parent_OU_Id ?? 0)
            {
                OU = ou, 
                Customer = customer,
                Regions = _regionService.GetRegions(SessionFacade.CurrentCustomer.Id).Where(x => x.IsActive == 1).Select(x => new SelectListItem
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
