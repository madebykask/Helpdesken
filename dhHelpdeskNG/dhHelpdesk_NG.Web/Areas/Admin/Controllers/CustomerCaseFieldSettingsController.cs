using System;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.Boolean;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Web.Infrastructure.Cache;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using Microsoft.Ajax.Utilities;

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

    public class CustomerCaseFieldSettingsController : BaseAdminController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly IHelpdeskCache _cache;
        private readonly ISettingService _settingService;
        private readonly ICaseSectionService _caseSectionService;

        public CustomerCaseFieldSettingsController(
            ICaseFieldSettingService caseFieldSettingService,
            ICustomerService customerService,
            ILanguageService languageService,
            IMasterDataService masterDataService,
            IHelpdeskCache cache,
            ICaseSectionService caseSectionService,
            ISettingService settingService)
            : base(masterDataService)
        {
            this._caseFieldSettingService = caseFieldSettingService;
            this._customerService = customerService;
            this._languageService = languageService;
            _cache = cache;
            _settingService = settingService;
            _caseSectionService = caseSectionService;
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpGet]
        public ActionResult Edit(int customerId, int languageId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var language = this._languageService.GetLanguage(languageId);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

            var caseSections = _caseSectionService.GetCaseSections(customerId, languageId);
            //var casefieldsetting = this._caseFieldSettingService.GetCaseFieldSettings(customerId);

            var model = this.CustomerInputViewModel(customer, language);
            model.CaseSections = caseSections;

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int customerId, CustomerInputViewModel vmodel, List<CaseFieldSetting> CaseFieldSettings, int[] UsSelected, int languageId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (vmodel.ShowStatusBarIds != null && vmodel.ShowStatusBarIds.Any())
            {
                var selected = CaseFieldSettings.Where(c => vmodel.ShowStatusBarIds.Any(m => m == c.Id));
                selected.ForEach(c => c.ShowStatusBar = true );
            }
            if (vmodel.ShowExternalStatusBarIds != null && vmodel.ShowExternalStatusBarIds.Any())
            {
                var selected = CaseFieldSettings.Where(c => vmodel.ShowExternalStatusBarIds.Any(m => m == c.Id));
                selected.ForEach(c => c.ShowExternalStatusBar = true);
            }

            _customerService.SaveCaseFieldSettingsForCustomer(customerId, languageId, vmodel.CaseFieldSettingWithLangauges, CaseFieldSettings, out errors);
            _caseSectionService.SaveCaseSections(languageId, vmodel.CaseSections, customerId);
            _cache.ClearCaseTranslations(customerId);

            if (errors.Count == 0)
                return this.RedirectToAction("edit", "customercasefieldsettings", new { customerId = customerId, languageId = languageId });

            var customer = this._customerService.GetCustomer(customerId);
            var language = this._languageService.GetLanguage(languageId);

            var model = this.CustomerInputViewModel(customer, language);

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
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

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult GetCaseSection(int sectionId, int customerId, int languageId)
        {
            var cs = _caseSectionService.GetCaseSection(sectionId, customerId, languageId);
            return Json(new
                    {
                        success = true,
                        isNewCollapsed = cs.IsNewCollapsed.ToInt(),
                        isEditCollapsed = cs.IsEditCollapsed.ToInt(),
                        sectionFields = cs.CaseSectionFields,
                        sectionHeader = !string.IsNullOrEmpty(cs.SectionHeader) ? cs.SectionHeader : string.Empty
            });
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult SaveCaseSectionOptions(int sectionId, int customerId, int isNewCollapsed, int isEditCollapsed, int sectionType, List<int> selectedFields, int languageId)
        {
            var caseSection = _caseSectionService.GetCaseSection(sectionId, customerId, languageId);
            var fields = selectedFields.Where(x => x > 0).ToList();
            if (caseSection != null)
            {
                caseSection.IsEditCollapsed = isEditCollapsed.ToBool();
                caseSection.IsNewCollapsed = isNewCollapsed.ToBool();
                caseSection.CaseSectionFields = fields;
                caseSection.CustomerId = customerId;
                caseSection.SectionType = sectionType;
                caseSection.Id = sectionId;
                sectionId = _caseSectionService.SaveCaseSection(caseSection);
            }
            else
            {
                return Json(new { success = false, sectionId });
            }
            return Json(new {success = true, sectionId});
        }

        private CustomerIndexViewModel IndexViewModel()
        {
            var model = new CustomerIndexViewModel { };

            return model;
        }

        private List<SelectListItem> GetLockedFieldOptions()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "",
                Value = "0",
                Selected = false
            });
            list.Add(new SelectListItem()
            {
                Text = Translation.Get("Om e-form", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });

            return list;
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
                CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetAllCaseFieldSettingsWithLanguages(customer.Id, language.Id),
                Customer = customer,
                Language = language,
                ListCaseForLabel = this._caseFieldSettingService.ListToShowOnCasePage(customer.Id, language.Id),
                Setting = this._settingService.GetCustomerSetting(customer.Id),
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString(),
                }).ToList(),
                LockedFieldOptions = GetLockedFieldOptions() 
            };
            ;
            model.ShowStatusBarIds = model.CaseFieldSettings.Where(m => m.ShowStatusBar).Select(m => m.Id).ToList();
            model.ShowExternalStatusBarIds = model.CaseFieldSettings.Where(m => m.ShowExternalStatusBar)
                .Select(m => m.Id).ToList();
            #endregion
            
            return model;
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")]
        public string ChangeLabel(int id, int customerId)//, int casefieldSettingLanguageId, int caseFieldSettingId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var language = this._languageService.GetLanguage(id);

            var casefieldsetting = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, id);
            var caseSections = _caseSectionService.GetCaseSections(customerId, id);

            var model = this.CustomerInputViewModel(customer, language);
            model.CaseSections = caseSections;

            var view = "~/areas/admin/views/CustomerCaseFieldSettings/_Input.cshtml";
            return this.RenderRazorViewToString(view, model);
        }
    }
}
