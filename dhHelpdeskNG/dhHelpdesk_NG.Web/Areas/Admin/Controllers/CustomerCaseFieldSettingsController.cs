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

    public class CustomerCaseFieldSettingsController : BaseController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;

        public CustomerCaseFieldSettingsController(
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSettingsService caseSettingsService,
            ICustomerService customerService,
            ILanguageService languageService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._caseFieldSettingService = caseFieldSettingService;
            this._caseSettingsService = caseSettingsService;
            this._customerService = customerService;
            this._languageService = languageService;
        }

        [HttpGet]
        public ActionResult Edit(int customerId, int languageId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var language = this._languageService.GetLanguage(languageId);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

            var casefieldsetting = this._caseFieldSettingService.GetCaseFieldSettings(customerId);

            var model = this.CustomerInputViewModel(customer, language);
            
            return this.View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int customerId, CustomerInputViewModel vmodel, List<CaseFieldSetting> CaseFieldSettings, int[] UsSelected, int languageId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var customer = this._customerService.GetCustomer(customerId);
            var language = this._languageService.GetLanguage(languageId);

            var caseFieldSettingWithLanguages = vmodel.CaseFieldSettingWithLangauges;
            //var caseFieldSettingLanguages = vmodel.CaseFieldSettingLanguage;

            this._customerService.SaveCaseFieldSettingsForCustomer(customer, caseFieldSettingWithLanguages, UsSelected, CaseFieldSettings, languageId, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("edit", "customer");

            var model = this.CustomerInputViewModel(customer, language);
           
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._customerService.DeleteCustomer(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "customer");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "customer", new { id = id });
            }
        }

        private CustomerIndexViewModel IndexViewModel()
        {
            var model = new CustomerIndexViewModel { };

            return model;
        }

        private CustomerInputViewModel CustomerInputViewModel(Customer customer, Language language)
        {
            if (customer.Id == 0)
            {
                customer.Language_Id = SessionFacade.CurrentLanguageId;
            }
            
            #region Model

            var model = new CustomerInputViewModel
            {
                CustomerCaseSummaryViewModel = new CustomerCaseSummaryViewModel(),
                CaseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customer.Id),
                CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customer.Id, language.Id),
                Customer = customer,
                Language = language,
                ListCaseForLabel = this._caseFieldSettingService.ListToShowOnCasePage(customer.Id, language.Id),
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList()
            };

            #endregion

            return model;
        }

        private CustomerCaseSummaryViewModel CustomerCaseSummaryViewModel(CaseSettings caseSetting)
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Info", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Utökad info", Enums.TranslationSource.TextTranslation),
                Value = "2",
                Selected = false
            });

            var model = new CustomerCaseSummaryViewModel
            {
                CaseSettings = this._caseSettingsService.GetCaseSettings(SessionFacade.CurrentCustomer.Id),
                CSetting = caseSetting,
                CaseFieldSettingLanguages = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId),
                LineList = li,
            };

            return model;
        }

        private CustomerInputViewModel CreateInputViewModel(Customer customer, int languageId)
        {
            var model = new CustomerInputViewModel
            {
                CustomerCaseSummaryViewModel = new CustomerCaseSummaryViewModel(),
                CaseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customer.Id),
                Customer = customer,
                ListCaseForLabel = this._caseFieldSettingService.ListToShowOnCasePage(customer.Id, languageId),
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList()

            };

            return model;
        }

        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string ChangeLabel(int id, int customerId)//, int casefieldSettingLanguageId, int caseFieldSettingId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var language = this._languageService.GetLanguage(id);

            var casefieldsetting = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, id);

            var model = this.CustomerInputViewModel(customer, language);


            var view = "~/areas/admin/views/CustomerCaseFieldSettings/_Input.cshtml";
            return this.RenderRazorViewToString(view, model);
        }
         

    }
}
