using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class EMailGroupController : BaseController
    {
        private readonly IEmailService _emailService;
        private readonly ICustomerService _customerService;

        public EMailGroupController(
            IEmailService emailService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _emailService = emailService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var emailGroups = _emailService.GetEmailGroups(customer.Id);

            var model = new EmailGroupIndexViewModel { EmailGroups = emailGroups, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var emailGroup = new EmailGroupEntity { Customer_Id = customer.Id, IsActive = 1 };

            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(EmailGroupEntity emailGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _emailService.SaveEmailGroup(emailGroup, out errors);

            if (errors.Count == 0)               
                return RedirectToAction("index", "emailgroup", new { customerId = emailGroup.Customer_Id.Value });

            var customer = _customerService.GetCustomer(emailGroup.Customer_Id.Value);
            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var emailGroup = _emailService.GetEmailGroup(id);

            if (emailGroup == null)                
                return new HttpNotFoundResult("No e-mail group found...");

            var customer = _customerService.GetCustomer(emailGroup.Customer_Id.Value);
            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmailGroupEntity emailGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _emailService.SaveEmailGroup(emailGroup, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "emailgroup", new { customerId = emailGroup.Customer_Id.Value });

            var customer = _customerService.GetCustomer(emailGroup.Customer_Id.Value);
            var model = new EmailGroupInputViewModel { EmailGroup = emailGroup, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var emailGroup = _emailService.GetEmailGroup(id);
            var customer = _customerService.GetCustomer(emailGroup.Customer_Id.Value);

            if (_emailService.DeleteEmailGroup(id) == DeleteMessage.Success)                
                return RedirectToAction("index", "emailgroup", new { customerId = customer.Id });
            else
            {
                TempData.Add("Error", "");                
                return RedirectToAction("edit", "emailgroup", new { area = "admin", id = emailGroup.Id });
            }
        }
    }
}
