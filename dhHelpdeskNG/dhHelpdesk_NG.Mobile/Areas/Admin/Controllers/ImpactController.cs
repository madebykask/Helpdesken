namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Models;

    public class ImpactController : BaseAdminController
    {
        private readonly IImpactService _impactService;
        private readonly ICustomerService _customerService;

        public ImpactController(
            IImpactService impactService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._impactService = impactService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var impacts = this._impactService.GetImpacts(customer.Id).ToList();

            var model = new ImpactIndexViewModel { Impacts = impacts, Customer = customer };
            
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var impact = new Impact { Customer_Id = customer.Id};

            var model = this.CreateInputViewModel(impact, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Impact impact)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._impactService.SaveImpact(impact, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "impact", new { customerId = impact.Customer_Id });

            var customer = this._customerService.GetCustomer(impact.Customer_Id);
            var model = this.CreateInputViewModel(impact, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var impact = this._impactService.GetImpact(id);

            if (impact == null)                
                return new HttpNotFoundResult("No impact found...");

            var customer = this._customerService.GetCustomer(impact.Customer_Id);
            var model = this.CreateInputViewModel(impact, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Impact impact)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._impactService.SaveImpact(impact, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "impact", new { customerId = impact.Customer_Id });

            var customer = this._customerService.GetCustomer(impact.Customer_Id);
            var model = this.CreateInputViewModel(impact, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var impact = this._impactService.GetImpact(id);

            if (this._impactService.DeleteImpact(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "impact", new { customerid = impact.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");                
                return this.RedirectToAction("edit", "impact", new { area = "admin", id = impact.Id });
            }
        }

        private ImpactInputViewModel CreateInputViewModel(Impact impact, Customer customer)
        {
            var model = new ImpactInputViewModel
            {
                Impact = impact,
                Customer = customer
            };

            return model;
        }
    }
}
