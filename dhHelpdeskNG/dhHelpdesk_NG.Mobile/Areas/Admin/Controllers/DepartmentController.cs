namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Models;

    public class DepartmentController : BaseAdminController
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
            this._countryService = countryService;
            this._departmentService = departmentService;
            this._regionService = regionService;
            this._holidayService = holidayService;
            this._watchDateCalendarService = watchDateCalendarService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var departments = this._departmentService.GetDepartments(customer.Id).ToList();

            var model = new DepartmentIndexViewModel { Departments = departments, Customer = customer};
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var department = new Department { Customer_Id = customer.Id, IsActive = 1};
                
            var model = this.CreateInputViewModel(department, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Department department)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._departmentService.SaveDepartment(department, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "department", new { customerId = department.Customer_Id });

            var customer = this._customerService.GetCustomer(department.Customer_Id);
            var model = this.CreateInputViewModel(department, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var department = this._departmentService.GetDepartment(id);

            if (department == null)
                return new HttpNotFoundResult("No department found...");

            var customer = this._customerService.GetCustomer(department.Customer_Id);
            var model = this.CreateInputViewModel(department, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._departmentService.SaveDepartment(department, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "department", new { customerId = department.Customer_Id });

            var customer = this._customerService.GetCustomer(department.Customer_Id);
            var model = this.CreateInputViewModel(department, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var department = this._departmentService.GetDepartment(id);

            if (this._departmentService.DeleteDepartment(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "department", new { customerId = department.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "department", new { area = "admin", id = id });
            }
        }

        private DepartmentInputViewModel CreateInputViewModel(Department department, Customer customer)
        {
            var model = new DepartmentInputViewModel
            {
                Department = department,
                Customer = customer,
                Regions = this._regionService.GetRegions(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Countries = this._countryService.GetCountries(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Holidays = this._holidayService.GetHolidayHeaders().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(), 
                WatchDateCalendar = this._watchDateCalendarService.GetAllWatchDateCalendars().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
