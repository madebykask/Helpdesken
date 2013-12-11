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
    public class ImpactController : BaseController
    {
        private readonly IImpactService _impactService;
        private readonly ICustomerService _customerService;

        public ImpactController(
            IImpactService impactService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _impactService = impactService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var impacts = _impactService.GetImpacts(customer.Id).ToList();

            var model = new ImpactIndexViewModel { Impacts = impacts, Customer = customer };
            
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var impact = new Impact { Customer_Id = customer.Id};

            var model = CreateInputViewModel(impact, customer);
            return View(model);
        }

        [HttpPost]
        public ActionResult New(Impact impact)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _impactService.SaveImpact(impact, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "impact", new { customerId = impact.Customer_Id });

            var customer = _customerService.GetCustomer(impact.Customer_Id);
            var model = CreateInputViewModel(impact, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var impact = _impactService.GetImpact(id);

            if (impact == null)                
                return new HttpNotFoundResult("No impact found...");

            var customer = _customerService.GetCustomer(impact.Customer_Id);
            var model = CreateInputViewModel(impact, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Impact impact)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _impactService.SaveImpact(impact, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "impact", new { customerId = impact.Customer_Id });

            var customer = _customerService.GetCustomer(impact.Customer_Id);
            var model = CreateInputViewModel(impact, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var impact = _impactService.GetImpact(id);

            if (_impactService.DeleteImpact(id) == DeleteMessage.Success)
                return RedirectToAction("index", "impact", new { customerid = impact.Customer_Id });
            else
            {
                TempData.Add("Error", "");                
                return RedirectToAction("edit", "impact", new { area = "admin", id = impact.Id });
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
