namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class EMailGroupController : BaseAdminController
    {
        private readonly IEmailGroupService _emailGroupService;
        private readonly ICustomerService _customerService;

        public EMailGroupController(
            IEmailGroupService emailGroupService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._emailGroupService = emailGroupService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var emailGroups = this._emailGroupService.GetEmailGroups(customer.Id);

            var model = new EmailGroupIndexViewModel { EmailGroups = emailGroups, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var emailGroup = new EmailGroupEntity { Customer_Id = customer.Id, IsActive = 1 };

            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(EmailGroupEntity emailGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._emailGroupService.SaveEmailGroup(emailGroup, out errors);

            if (errors.Count == 0)               
                return this.RedirectToAction("index", "emailgroup", new { customerId = emailGroup.Customer_Id.Value });

            var customer = this._customerService.GetCustomer(emailGroup.Customer_Id.Value);
            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var emailGroup = this._emailGroupService.GetEmailGroup(id);

            if (emailGroup == null)                
                return new HttpNotFoundResult("No e-mail group found...");

            var customer = this._customerService.GetCustomer(emailGroup.Customer_Id.Value);
            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmailGroupEntity emailGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._emailGroupService.SaveEmailGroup(emailGroup, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "emailgroup", new { customerId = emailGroup.Customer_Id.Value });

            var customer = this._customerService.GetCustomer(emailGroup.Customer_Id.Value);
            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var emailGroup = this._emailGroupService.GetEmailGroup(id);
            var customer = this._customerService.GetCustomer(emailGroup.Customer_Id.Value);

            if (this._emailGroupService.DeleteEmailGroup(id) == DeleteMessage.Success)                
                return this.RedirectToAction("index", "emailgroup", new { customerId = customer.Id });
            else
            {
                this.TempData.Add("Error", "");                
                return this.RedirectToAction("edit", "emailgroup", new { area = "admin", id = emailGroup.Id });
            }
        }
    }
}
