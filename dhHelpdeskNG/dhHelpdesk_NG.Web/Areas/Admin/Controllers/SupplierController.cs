namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class SupplierController : BaseAdminController
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
            this._countryService = countryService;
            this._supplierService = supplierService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {

            var customer = this._customerService.GetCustomer(customerId);
            var suppliers = this._supplierService.GetSuppliers(customer.Id);

            var model = new SupplierIndexViewModel { Suppliers = suppliers, Customer = customer };

            return this.View(model);

        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var supplier = new Supplier { Customer_Id = customer.Id, IsActive = 1, Id = 0 };
        
            var model = this.CreateInputViewModel(supplier, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Supplier supplier)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._supplierService.SaveSupplier(supplier, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "supplier", new { customerid = supplier.Customer_Id });

            var customer = this._customerService.GetCustomer(supplier.Customer_Id );
            var model = this.CreateInputViewModel(supplier, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var supplier = this._supplierService.GetSupplier(id);

            if (supplier == null)
                return new HttpNotFoundResult("No supplier found...");

            var customer = this._customerService.GetCustomer(supplier.Customer_Id);
            //var model = new SupplierInputViewModel { Supplier = supplier, Customer = customer };

            var model = this.CreateInputViewModel(supplier,  customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            
                this._supplierService.SaveSupplier(supplier, out errors);

                if (errors.Count == 0)
                    //return RedirectToAction("index", "supplier", new { area = "admin" });
                    return this.RedirectToAction("index", "supplier", new { customerid = supplier.Customer_Id });

                var customer = this._customerService.GetCustomer(supplier.Customer_Id);
                var model = this.CreateInputViewModel(supplier, customer);
                
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var supplier = this._supplierService.GetSupplier(id);

            if (this._supplierService.DeleteSupplier(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "supplier", new { customerid = supplier.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "supplier", new { area = "admin", id = supplier.Id});
            }
        }

        private SupplierInputViewModel CreateInputViewModel(Supplier supplier, Customer customer)
        {
            var model = new SupplierInputViewModel
            {
                Supplier = supplier,
                Customer = customer,
                Countries = this._countryService.GetCountries(supplier.Customer_Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
