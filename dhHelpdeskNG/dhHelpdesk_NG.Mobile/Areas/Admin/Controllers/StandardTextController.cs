namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class StandardTextController : BaseAdminController
    {
        private readonly IStandardTextService _textService;
        private readonly ICustomerService _customerService;

        public StandardTextController(
            IStandardTextService textService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._textService = textService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var standardTexts = this._textService.GetStandardTexts(customer.Id);

            var model = new StandardTextIndexViewModel { StandardTexts = standardTexts, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var standardText = new StandardText { Customer_Id = customer.Id, IsActive = 1 };

            var model = new StandardTextInputViewModel { StandardText = standardText, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(StandardText standardText)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._textService.SaveStandardText(standardText, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "standardtext", new { customerId = standardText.Customer_Id });

            var customer = this._customerService.GetCustomer(standardText.Customer_Id);
            var model = new StandardTextInputViewModel { StandardText = standardText, Customer = customer };
            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var standardText = this._textService.GetStandardText(id);

            if (standardText == null)               
                return new HttpNotFoundResult("No standardText found...");

            var customer = this._customerService.GetCustomer(standardText.Customer_Id);
            var model = new StandardTextInputViewModel { StandardText = standardText, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(StandardText standardText)
        {
            if (this.ModelState.IsValid)
            {
                IDictionary<string, string> errors = new Dictionary<string, string>();
                this._textService.SaveStandardText(standardText, out errors);

                if (errors.Count == 0)
                    return this.RedirectToAction("index", "standardtext", new { customerId = standardText.Customer_Id });

                
            }
            return this.View(standardText);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var standardText = this._textService.GetStandardText(id);

            if (this._textService.DeleteStandardText(id) == DeleteMessage.Success)               
                return this.RedirectToAction("index", "standardtext", new { customerId = standardText.Customer_Id });           
            else
            {
                this.TempData.Add("Error", "");                
                return this.RedirectToAction("edit", "standardtext", new { area = "admin", id = standardText.Id });
            }
        }
    }
}
