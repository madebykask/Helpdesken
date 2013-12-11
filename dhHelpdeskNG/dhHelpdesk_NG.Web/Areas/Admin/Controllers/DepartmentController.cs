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
    public class DepartmentController : BaseController
    {
        private readonly ICountryService _countryService;
        private readonly IDepartmentService _departmentService;
        private readonly IRegionService _regionService;
        private readonly IHolidayService _holidayService;
        private readonly ICustomerService _customerService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;

        public DepartmentController(
            ICountryService countryService,
            IDepartmentService departmentService,
            IRegionService regionService,
            IHolidayService holidayService,
            IWatchDateCalendarService watchDateCalendarService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _countryService = countryService;
            _departmentService = departmentService;
            _regionService = regionService;
            _holidayService = holidayService;
            _watchDateCalendarService = watchDateCalendarService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var departments = _departmentService.GetDepartments(customer.Id).ToList();

            var model = new DepartmentIndexViewModel { Departments = departments, Customer = customer};
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var department = new Department { Customer_Id = customer.Id, IsActive = 1};
                
            var model = CreateInputViewModel(department, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Department department)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _departmentService.SaveDepartment(department, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "department", new { customerId = department.Customer_Id });

            var customer = _customerService.GetCustomer(department.Customer_Id);
            var model = CreateInputViewModel(department, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var department = _departmentService.GetDepartment(id);

            if (department == null)
                return new HttpNotFoundResult("No department found...");

            var customer = _customerService.GetCustomer(department.Customer_Id);
            var model = CreateInputViewModel(department, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _departmentService.SaveDepartment(department, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "department", new { customerId = department.Customer_Id });

            var customer = _customerService.GetCustomer(department.Customer_Id);
            var model = CreateInputViewModel(department, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var department = _departmentService.GetDepartment(id);

            if (_departmentService.DeleteDepartment(id) == DeleteMessage.Success)
                return RedirectToAction("index", "department", new { customerId = department.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "department", new { area = "admin", id = id });
            }
        }

        private DepartmentInputViewModel CreateInputViewModel(Department department, Customer customer)
        {
            var model = new DepartmentInputViewModel
            {
                Department = department,
                Customer = customer,
                Regions = _regionService.GetRegions(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Countries = _countryService.GetCountries(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Holidays = _holidayService.GetHolidayHeaders().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(), 
                WatchDateCalendar = _watchDateCalendarService.GetAllWatchDateCalendars().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
