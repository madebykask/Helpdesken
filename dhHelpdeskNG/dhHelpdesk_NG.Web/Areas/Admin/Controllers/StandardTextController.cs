using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class StandardTextController : BaseController
    {
        private readonly IStandardTextService _textService;
        private readonly ICustomerService _customerService;

        public StandardTextController(
            IStandardTextService textService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _textService = textService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var standardTexts = _textService.GetStandardTexts(customer.Id);

            var model = new StandardTextIndexViewModel { StandardTexts = standardTexts, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var standardText = new StandardText { Customer_Id = customer.Id, IsActive = 1 };

            var model = new StandardTextInputViewModel { StandardText = standardText, Customer = customer };
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(StandardText standardText)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _textService.SaveStandardText(standardText, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "standardtext", new { customerId = standardText.Customer_Id });

            var customer = _customerService.GetCustomer(standardText.Customer_Id);
            var model = new StandardTextInputViewModel { StandardText = standardText, Customer = customer };
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var standardText = _textService.GetStandardText(id);

            if (standardText == null)               
                return new HttpNotFoundResult("No standardText found...");

            var customer = _customerService.GetCustomer(standardText.Customer_Id);
            var model = new StandardTextInputViewModel { StandardText = standardText, Customer = customer };
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(StandardText standardText)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _textService.SaveStandardText(standardText, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "standardtext", new { customerId = standardText.Customer_Id });

            return View(standardText);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var standardText = _textService.GetStandardText(id);

            if (_textService.DeleteStandardText(id) == DeleteMessage.Success)               
                return RedirectToAction("index", "standardtext", new { customerId = standardText.Customer_Id });           
            else
            {
                TempData.Add("Error", "");                
                return RedirectToAction("edit", "standardtext", new { area = "admin", id = standardText.Id });
            }
        }
    }
}
