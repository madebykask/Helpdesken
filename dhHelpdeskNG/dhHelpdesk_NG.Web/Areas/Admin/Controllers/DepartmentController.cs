namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Web.Infrastructure;

    public class DepartmentController : BaseAdminController
    {
        private readonly ICountryService _countryService;
        private readonly IDepartmentService _departmentService;
        private readonly IRegionService _regionService;
        private readonly IHolidayService _holidayService;
        private readonly ICustomerService _customerService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;
        private readonly ISettingService _settingService;
        private readonly ILanguageService _languageService;
        private readonly IOUService _ouService;


        public DepartmentController(
            ICountryService countryService,
            IDepartmentService departmentService,
            IRegionService regionService,
            IHolidayService holidayService,
            IWatchDateCalendarService watchDateCalendarService,
            ICustomerService customerService,
            IMasterDataService masterDataService,
            ISettingService settingService,
            ILanguageService languageService,
            IOUService ouService)
            : base(masterDataService)
        {
            _countryService = countryService;
            _departmentService = departmentService;
            _regionService = regionService;
            _holidayService = holidayService;
            _watchDateCalendarService = watchDateCalendarService;
            _customerService = customerService;
            _settingService = settingService;
            _languageService = languageService;
            _ouService = ouService;

        }

        public JsonResult SetShowOnlyActiveDepartmentInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveDepartmentInAdmin = value;
            return this.Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var departments = this._departmentService.GetDepartments(customer.Id, ActivationStatus.All).ToList();

            var model = new DepartmentIndexViewModel { Departments = departments,
                                                       Customer = customer,
                                                       IsShowOnlyActive = SessionFacade.ShowOnlyActiveDepartmentInAdmin
                                                     };
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
        public ActionResult New(Department department, int[] invoiceSelectedOUs)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _departmentService.SaveDepartment(department, invoiceSelectedOUs, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "department", new { customerId = department.Customer_Id });

            var customer = _customerService.GetCustomer(department.Customer_Id);
            var model = CreateInputViewModel(department, customer);

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
        public ActionResult Edit(Department department, int[] invoiceSelectedOUs)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _departmentService.SaveDepartment(department, invoiceSelectedOUs, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "department", new { customerId = department.Customer_Id });

            var customer = _customerService.GetCustomer(department.Customer_Id);
            var model = CreateInputViewModel(department, customer);

            return View(model);
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
            var allOUsForDep = _ouService.GetOuForDepartment(department.Id).Where(d=> d.Parent_OU_Id == null || (d.Parent?.Parent == null));
            var invoiceAvailableOUs = allOUsForDep.Where(o => !o.ShowInvoice).Select(x => new SelectListItem
            {                
                Text = (x.Parent == null)? x.Name : $"{x.Parent.Name} - {x.Name}",
                Value = x.Id.ToString(),
                Disabled = x.IsActive == 0
            }).ToList();

            var invoiceSelectedOUs = allOUsForDep.Where(o => o.ShowInvoice).Select(x => new SelectListItem
            {
                Text = (x.Parent == null) ? x.Name : $"{x.Parent.Name} - {x.Name}",
                Value = x.Id.ToString(),
                Disabled = x.IsActive == 0
            }).ToList();

            var model = new DepartmentInputViewModel
            {
                Department = department,
                Customer = customer,
                CustomerSettings = _settingService.GetCustomerSettings(customer.Id),
                Regions = _regionService.GetRegions(customer.Id).Where(x => x.IsActive == 1).Select(x => new SelectListItem
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
                }).ToList(),
                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                InvoiceAvailableOUs = invoiceAvailableOUs,
                InvoiceSelectedOUs = invoiceSelectedOUs 
            };

            return model;
        }
    }
}
