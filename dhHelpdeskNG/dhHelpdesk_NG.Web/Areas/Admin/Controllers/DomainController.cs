using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Mvc;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class DomainController : BaseController
    {
        private readonly IDomainService _domainService;
        private readonly ISystemService _systemService;
        private readonly ICustomerService _customerService;

        public DomainController(
            IDomainService domainService,
            ISystemService systemService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _domainService = domainService;
            _systemService = systemService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var domains = _domainService.GetDomains(customer.Id);

            var model = new DomainIndexViewModel { Domains = domains, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var domain = new Domain.Domain { Customer_Id = customer.Id };

            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(Domain.Domain domain)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _domainService.SaveDomain(domain, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "domain", new { customerId = domain.Customer_Id });

            var customer = _customerService.GetCustomer(domain.Customer_Id);
            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return View(domain);
        }

        public ActionResult Edit(int id)
        {
            var domain = _domainService.GetDomain(id);

            if (domain.Password != "")
                domain.Password = WebConfigurationManager.AppSettings["dh_maskedpassword"].ToString();

            if (domain == null)
                return new HttpNotFoundResult("No domain found...");

            var customer = _customerService.GetCustomer(domain.Customer_Id);
            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Domain.Domain domain)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var id = domain.Id;
            var dbPassWord = _domainService.GetDomainPassword(id);

            if (domain.Password == WebConfigurationManager.AppSettings["dh_maskedpassword"].ToString())
                domain.Password = dbPassWord.ToString();



            _domainService.SaveDomain(domain, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "domain", new { customerId = domain.Customer_Id });

            var customer = _customerService.GetCustomer(domain.Customer_Id);
            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var domain = _domainService.GetDomain(id);
            if (_domainService.DeleteDomain(id) == DeleteMessage.Success)
                return RedirectToAction("index", "domain", new { customerId = domain.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "domain", new { area = "admin", id = domain.Id });
            }
        }       
    }
}
