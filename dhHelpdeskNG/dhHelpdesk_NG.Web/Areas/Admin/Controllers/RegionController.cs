namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class RegionController : BaseAdminController
    {
        private readonly IRegionService _regionService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;

        public RegionController(
            IRegionService regionService,
            ICustomerService customerService,
            ILanguageService languageService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._regionService = regionService;
            this._customerService = customerService;
            this._languageService = languageService;
        }

        public JsonResult SetShowOnlyActiveRegionInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveRegionInAdmin = value;
            return this.Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var regions = this._regionService.GetRegions(customer.Id).ToList();
            

            var model = new RegionIndexViewModel { Regions = regions, 
                                                   Customer = customer,
                                                   IsShowOnlyActive = SessionFacade.ShowOnlyActiveRegionInAdmin
                                                 };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            //return View(new Region() { Customer_Id = SessionFacade.CurrentCustomer.Id, IsActive = 1 });
            var customer = this._customerService.GetCustomer(customerId);
            var region = new Region { Customer_Id = customer.Id, IsActive = 1 };

            var model = this.CreateInputViewModel(region, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Region region)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._regionService.SaveRegion(region, region.Customer_Id, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "region", new { customerid = region.Customer_Id });

            var customer = this._customerService.GetCustomer(region.Customer_Id);
            var model = this.CreateInputViewModel(region, customer);
            return this.View(model);

        }

        public ActionResult Edit(int id)
        {
            var region = this._regionService.GetRegion(id);

            if (region == null)               
                return new HttpNotFoundResult("No region found...");

            var customer = this._customerService.GetCustomer(region.Customer_Id);
            var model = this.CreateInputViewModel(region, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Region region)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var regionToUpdate = this._regionService.GetRegion(region.Id);
            regionToUpdate.Name = region.Name;
            regionToUpdate.IsActive = region.IsActive;
            regionToUpdate.IsDefault = region.IsDefault;
            regionToUpdate.Code = region.Code;
            regionToUpdate.LanguageId = region.LanguageId;
            this._regionService.SaveRegion(regionToUpdate, region.Customer_Id, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "region", new { customerId = region.Customer_Id });

            var customer = this._customerService.GetCustomer(region.Customer_Id);
            var model = this.CreateInputViewModel(region, customer);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var region = this._regionService.GetRegion(id);
            this._regionService.DeleteRegion(id);

            return this.RedirectToAction("index", "region", new { customerId = region.Customer_Id });
        }

        private RegionInputViewModel CreateInputViewModel(Region region, Customer customer)
        {
            var model = new RegionInputViewModel
            {
                Region = region,
                Customer = customer,
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
