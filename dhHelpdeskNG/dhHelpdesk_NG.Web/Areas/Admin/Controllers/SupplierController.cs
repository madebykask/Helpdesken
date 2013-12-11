using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly ICountryService _countryService;
        private readonly ISupplierService _supplierService;
        private readonly ICustomerService _customerService;

        public SupplierController(
            ICountryService countryService,
            ISupplierService supplierService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _countryService = countryService;
            _supplierService = supplierService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {

            var customer = _customerService.GetCustomer(customerId);
            var suppliers = _supplierService.GetSuppliers(customer.Id);

            var model = new SupplierIndexViewModel { Suppliers = suppliers, Customer = customer };

            return View(model);

        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var supplier = new Supplier { Customer_Id = customer.Id, IsActive = 1, Id = 0 };
        
            var model = CreateInputViewModel(supplier, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Supplier supplier)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _supplierService.SaveSupplier(supplier, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "supplier", new { customerid = supplier.Customer_Id });

            var customer = _customerService.GetCustomer(supplier.Customer_Id );
            var model = CreateInputViewModel(supplier, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var supplier = _supplierService.GetSupplier(id);

            if (supplier == null)
                return new HttpNotFoundResult("No supplier found...");

            var customer = _customerService.GetCustomer(supplier.Customer_Id);
            //var model = new SupplierInputViewModel { Supplier = supplier, Customer = customer };

            var model = CreateInputViewModel(supplier,  customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            
                _supplierService.SaveSupplier(supplier, out errors);

                if (errors.Count == 0)
                    //return RedirectToAction("index", "supplier", new { area = "admin" });
                    return RedirectToAction("index", "supplier", new { customerid = supplier.Customer_Id });

                var customer = _customerService.GetCustomer(supplier.Customer_Id);
                var model = CreateInputViewModel(supplier, customer);
                
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var supplier = _supplierService.GetSupplier(id);

            if (_supplierService.DeleteSupplier(id) == DeleteMessage.Success)
                return RedirectToAction("index", "supplier", new { customerid = supplier.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "supplier", new { area = "admin", id = supplier.Id});
            }
        }

        private SupplierInputViewModel CreateInputViewModel(Supplier supplier, Customer customer)
        {
            var model = new SupplierInputViewModel
            {
                Supplier = supplier,
                Customer = customer,
                Countries = _countryService.GetCountries(supplier.Customer_Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
