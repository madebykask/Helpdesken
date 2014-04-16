namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class InfoTextController : BaseController
    {
        private readonly IInfoService _infoService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;

        public InfoTextController(
            IInfoService infoService,
            ICustomerService customerService,
            ILanguageService languageService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._infoService = infoService;
            this._customerService = customerService;
            this._languageService = languageService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var infoTexts = this._infoService.GetInfoTexts(customer.Id, customer.Language_Id);

            var model = new InfoTextIndexViewModel { InfoTexts = infoTexts, Customer = customer };
            return this.View(model);
        }

        public ActionResult Edit(int infoTypeId, int customerId, int languageId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var infoText = this._infoService.GetInfoText(infoTypeId, customer.Id, languageId);

            if (infoText == null)
                return new HttpNotFoundResult("No information text found");

            var model = this.InfoTextInputViewModel(customer);
            model.InfoTextShowViewModel = this.InfoTextShowViewModel(infoText, customer, languageId);

            return this.View(model);
            
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(InfoText infoText, int customerId, int languageId, int infoTypeId)
        {

            var customer = this._customerService.GetCustomer(customerId);
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (infoText.Id == 0)
            {

                infoText = new InfoText { Customer_Id = customerId, Type = infoTypeId, Id = 0, Language_Id = languageId, Name = infoText.Name };
             
            }

           
            this._infoService.SaveInfoText(infoText, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("edit", "infotext", new { infoTypeId = infoTypeId, customerId = customerId, languageId = languageId });
                //return this.RedirectToAction("index", "infotext", new { customerId = customer.Id });

            var model = this.InfoTextInputViewModel(customer);
            return this.View(model);
          
            
        }

        private InfoTextInputViewModel CreateInputViewModel(InfoText infoText, Customer customer)
        {
            var model = new InfoTextInputViewModel
            {
                InfoText = infoText,
                Customer = customer,
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList()
            };

            return model;
        }

        private InfoTextInputViewModel InfoTextInputViewModel(Customer customer)
        {
            if (customer.Id == 0)
            {
                customer.Language_Id = SessionFacade.CurrentLanguageId;
            }

            #region Model

            var model = new InfoTextInputViewModel
            {
                InfoTextShowViewModel = new InfoTextShowViewModel(),
                Customer = customer,
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList()
            };
            #endregion

            return model;
        }

        private InfoTextShowViewModel InfoTextShowViewModel(InfoText infoText, Customer customer, int languageId)
        {
            var model = new InfoTextShowViewModel
            {
                InfoText = infoText,
                Customer = customer
            };

            return model;
        }

        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string UpdateLanguageList(int id, int customerId, int infoTypeId)
        {

            var customer = this._customerService.GetCustomer(customerId);
            var infoTextToUpdate = this._infoService.GetInfoText(infoTypeId, customer.Id, id);

            var infoText = new InfoText() { };

            var model = this.InfoTextShowViewModel(infoText, customer, id);

            model.InfoText = infoTextToUpdate;
            model.Customer = customer;

            this.UpdateModel(model, "infoText");

            //return View(model);
            var view = "~/areas/admin/views/Infotext/_InfoTextPartialView.cshtml";
            return this.RenderRazorViewToString(view, model);
        }

        private InfoTextShowViewModel CreateShowViewModel(InfoText infoText, Customer customer)
        {
            var model = new InfoTextShowViewModel
            {
                InfoText = infoText,
                Customer = customer
            };

            return model;
        }
    }
}
