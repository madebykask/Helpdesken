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
    public class RegionController : BaseController
    {
        private readonly IRegionService _regionService;
        private readonly ICustomerService _customerService;

        public RegionController(
            IRegionService regionService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _regionService = regionService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var regions = _regionService.GetRegions(customer.Id).ToList();

            var model = new RegionIndexViewModel { Regions = regions, Customer = customer};
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            //return View(new Region() { Customer_Id = SessionFacade.CurrentCustomer.Id, IsActive = 1 });
            var customer = _customerService.GetCustomer(customerId);
            var region = new Region { Customer_Id = customer.Id, IsActive = 1 };

            var model = CreateInputViewModel(region, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Region region)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _regionService.SaveRegion(region, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "region", new { customerid = region.Customer_Id });

            var customer = _customerService.GetCustomer(region.Customer_Id);
            var model = CreateInputViewModel(region, customer);
            return View(model);

        }

        public ActionResult Edit(int id)
        {
            var region = _regionService.GetRegion(id);

            if (region == null)               
                return new HttpNotFoundResult("No region found...");

            var customer = _customerService.GetCustomer(region.Customer_Id);
            var model = CreateInputViewModel(region, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Region region)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _regionService.SaveRegion(region, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "region", new { customerId = region.Customer_Id });

            var customer = _customerService.GetCustomer(region.Customer_Id);
            var model = CreateInputViewModel(region, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var region = _regionService.GetRegion(id);
            _regionService.DeleteRegion(id);

            return RedirectToAction("index", "region", new { customerId = region.Customer_Id });
        }

        private RegionInputViewModel CreateInputViewModel(Region region, Customer customer)
        {
            var model = new RegionInputViewModel
            {
                Region = region,
                Customer = customer
            };

            return model;
        }
    }
}
