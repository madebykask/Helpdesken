namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

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
            this._domainService = domainService;
            this._systemService = systemService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var domains = this._domainService.GetDomains(customer.Id);

            var model = new DomainIndexViewModel { Domains = domains, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var domain = new Domain.Domain { Customer_Id = customer.Id };

            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Domain.Domain domain)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._domainService.SaveDomain(domain, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "domain", new { customerId = domain.Customer_Id });

            var customer = this._customerService.GetCustomer(domain.Customer_Id);
            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return this.View(domain);
        }

        public ActionResult Edit(int id)
        {
            var domain = this._domainService.GetDomain(id);

            //if (domain.Password != "")
            domain.Password = WebConfigurationManager.AppSettings["dh_maskedpassword"].ToString();

            if (domain == null)
                return new HttpNotFoundResult("No domain found...");

            var customer = this._customerService.GetCustomer(domain.Customer_Id);
            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Domain.Domain domain)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var id = domain.Id;
            var dbPassWord = this._domainService.GetDomainPassword(id);

            //if (domain.Password == WebConfigurationManager.AppSettings["dh_maskedpassword"].ToString())
                domain.Password = dbPassWord.ToString();



            this._domainService.SaveDomain(domain, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "domain", new { customerId = domain.Customer_Id });

            var customer = this._customerService.GetCustomer(domain.Customer_Id);
            var model = new DomainInputViewModel { Domain = domain, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var domain = this._domainService.GetDomain(id);
            if (this._domainService.DeleteDomain(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "domain", new { customerId = domain.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "domain", new { area = "admin", id = domain.Id });
            }
        }

        [HttpPost]
        public void EditPassword(int id, string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword)
            {
                this._domainService.SavePassword(id, newPassword);
            }
            else
                throw new ArgumentNullException("The password fields do not  match, please re-type them...");
        }
    }
}
