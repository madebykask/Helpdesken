using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExtendedCase;
using DH.Helpdesk.BusinessData.Models.Language.Output;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class ExtendedCaseController : BaseAdminController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly IExtendedCaseService _extendedCaseService;
        private readonly ICaseSolutionService _caseSolutionService;

        public ExtendedCaseController(ICaseFieldSettingService caseFieldSettingService,
            ICustomerService customerService,
            ILanguageService languageService,
            IExtendedCaseService extendedCaseService,
            IMasterDataService masterDataService,
            ICaseSolutionService caseSolutionService) : base(masterDataService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _customerService = customerService;
            _languageService = languageService;
            _extendedCaseService = extendedCaseService;
            _caseSolutionService = caseSolutionService;
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpGet]
        public ActionResult Edit(int customerId, int? languageId)
        {
            languageId = languageId ?? SessionFacade.CurrentLanguageId;
            var model = CustomerInputViewModel(customerId, languageId.Value);

            return View("Edit", model);
        }


        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int customerId, int? languageId, IList<int> ShowStatusBarIds, IList<int> ShowExternalStatusBarIds)
        {
            languageId = languageId ?? SessionFacade.CurrentLanguageId;
            var model = CustomerInputViewModel(customerId, languageId.Value);

            model.CaseFieldSettings.ForEach(s =>
            {
                s.ShowStatusBar = (ShowStatusBarIds != null && ShowStatusBarIds.Any(m => m == s.Id));
                s.ShowExternalStatusBar = (ShowExternalStatusBarIds != null && ShowExternalStatusBarIds.Any(m => m == s.Id));
            });
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _customerService.SaveCaseFieldSettingsForCustomer(customerId, languageId.Value, model.CaseFieldSettingWithLangauges, model.CaseFieldSettings.ToList(), out errors);

            if (errors.Count == 0)
                return RedirectToAction("edit", new { customerId });

            return View("Edit", model);
        }


        [CustomAuthorize(Roles = "3,4")]
        [HttpGet]
        public ActionResult GetCustomerForms(int customerId, int? languageId, bool showActive = true)
        {
            languageId = languageId ?? SessionFacade.CurrentLanguageId;
            var customer = _customerService.GetCustomer(customerId);

            ExtendedCaseFormsForCustomer model = new ExtendedCaseFormsForCustomer()
            {
                Customer = customer,
                LanguageId = languageId,
                ExtendedCaseForms = _extendedCaseService.GetExtendedCaseFormsCreatedByEditor(customer, showActive),
                IsShowOnlyActive = showActive
            };

            return View("CustomerForms", model);
        }

        public ActionResult GetCustomerFormsList(int customerId, int? languageId, bool showActive = true)
        {
            languageId = languageId ?? SessionFacade.CurrentLanguageId;
            var customer = _customerService.GetCustomer(customerId);
            ExtendedCaseFormsForCustomer model = new ExtendedCaseFormsForCustomer()
            {
                Customer = customer,
                LanguageId = languageId,
                ExtendedCaseForms = _extendedCaseService.GetExtendedCaseFormsCreatedByEditor(customer, showActive),
                IsShowOnlyActive = showActive
            };
            return PartialView("_CustomerFormsList", model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> New(int customerId, int? languageId)
        {
            languageId = languageId ?? SessionFacade.CurrentLanguageId;
            var customer = _customerService.GetCustomer(customerId);

            var caseSolutions = await _caseSolutionService.GetCustomerCaseSolutionsAsync(customer.Id);
            var caseSolutionsExtendedCaseForms = await _caseSolutionService.GetCaseSolutionsWithExtendeCaseFormAsync(customer.Id, null);

            List<ExtendedCaseFieldTranslation> initialTranslations = GetInitialTranslations();

            ExtendedFormViewModels viewmodel = new ExtendedFormViewModels()
            {
                Customer = customer,
                CustomerCaseSolutions = caseSolutions,
                ExtendedCaseForm = null,
                FieldTranslations = _languageService.GetExtendedCaseTranslations(null, languageId, initialTranslations),
                CustomerCaseSolutionsWithExtendedCaseForm = caseSolutionsExtendedCaseForms,
                ActiveLanguages = _languageService.GetActiveLanguages().Where(x => x.IsActive == 1).OrderBy(x => x.Id).ToList(),
                ExtendedCaseFormInCases = false
            };

            return View("EditForm", viewmodel);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpGet]
        public async Task<ActionResult> EditForm(int extendedCaseFormId, int? languageId)
        {
            languageId = languageId ?? SessionFacade.CurrentLanguageId;
            ExtendedCaseFormEntity extendedCaseForm = _extendedCaseService.GetExtendedCaseFormById(extendedCaseFormId);

            int customerId = extendedCaseForm.Customer_Id  == null ? 0 : (int)extendedCaseForm.Customer_Id;

            var customer = _customerService.GetCustomer(customerId);

            var caseSolutions = await _caseSolutionService.GetCustomerCaseSolutionsAsync(customer.Id);

            var caseSolutionsExtendedCaseForms = await _caseSolutionService.GetCaseSolutionsWithExtendeCaseFormAsync(customer.Id, extendedCaseFormId);

            List<ExtendedCaseFieldTranslation> initialTranslations = GetInitialTranslations();

            ExtendedFormViewModels viewModel = new ExtendedFormViewModels()
            {
                Customer = customer,
                CustomerCaseSolutions = caseSolutions,
                ExtendedCaseForm = extendedCaseForm,
                CustomerCaseSolutionsWithExtendedCaseForm = caseSolutionsExtendedCaseForms,
                ActiveLanguages = _languageService.GetActiveLanguages().Where(x => x.IsActive == 1).OrderBy(x => x.Id).ToList(),
                ExtendedCaseFormInCases = _extendedCaseService.ExtendedCaseFormInCases(extendedCaseFormId)
            };

            string metaData = viewModel.ExtendedCaseForm.MetaData.Replace("function(m) { return ", "").Replace("\";}", "\"");

            string firstTabName = ExtractFirstNameAttribute(metaData);
            metaData = metaData.Replace("]},"+ ExtendedCaseFormsHelper.GetEditorInitiatorData(firstTabName, viewModel.Customer.CustomerGUID.ToString()), "");
            metaData = metaData.Replace(@""" },""dataSource", @" "",""dataSource");
            metaData = metaData.Replace(@"function(m) { if (m.formInfo.applicationType == ""helpdesk"") return true; }",
                                        @"""function(m) { if (m.formInfo.applicationType == helpdesk) return true; }""")
                                .Replace(@"function(m) { if (m.formInfo.applicationType == ""selfservice"") return true; }",
                                        @"""function(m) { if (m.formInfo.applicationType == selfservice) return true; }""");
            viewModel.FormFields = JsonConvert.DeserializeObject<ExtendedCaseFormJsonModel>(metaData);
            viewModel.FieldTranslations = _languageService.GetExtendedCaseTranslations(viewModel.FormFields, languageId, initialTranslations);
            return View("EditForm", viewModel);
        }

        //[CustomAuthorize(Roles = "3,4")]
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult DeleteExtendedCaseForm(int id)
        //{
        //    bool deleted = _extendedCaseService.DeleteExtendedCaseForm(id);
        //    return Json();
        //}

        private string ExtractFirstNameAttribute(string json)
        {
            // Pattern to match the "id" attribute in the first tab
            string pattern = "\"tabs\":\\s*\\[\\s*{\\s*\"id\":\\s*\"([^\"]+)\"";

            Match match = Regex.Match(json, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        private List<ExtendedCaseFieldTranslation> GetInitialTranslations()
        {
            LanguageOverview defaultLanguage = _languageService.GetActiveLanguages().Where(x => x.IsActive == 1).OrderBy(x => x.Id).FirstOrDefault();

            return new List<ExtendedCaseFieldTranslation>()
            {
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Section", Name = Translation.GetCoreTextTranslation("Sektion")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Textfält")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Textarea")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Datumfält")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Infofält")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Filuppladdning")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Message", Name = Translation.GetCoreTextTranslation("Dra filer hit")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Tab", Name = Translation.GetCoreTextTranslation("Fliknamn")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Radioknapp")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Kryssruta")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "Control", Name = Translation.GetCoreTextTranslation("Rullgardinsmeny")},
                new ExtendedCaseFieldTranslation() { Language = defaultLanguage, Prefix = "DataSource.Value", Name = "Val"}
            };
        }

        private CustomerInputViewModel CustomerInputViewModel(int customerId, int languageId)
        {
            #region Model

            var model = new CustomerInputViewModel();
            model.CaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            model.CaseFieldSettingWithLangauges = _caseFieldSettingService.GetAllCaseFieldSettingsWithLanguages(customerId, languageId);
            model.Customer = _customerService.GetCustomer(customerId);
            model.Language = _languageService.GetLanguage(languageId);
            model.ShowStatusBarIds = model.CaseFieldSettings.Where(m => m.ShowStatusBar).Select(m => m.Id).ToList();
            model.ShowExternalStatusBarIds = model.CaseFieldSettings.Where(m => m.ShowExternalStatusBar)
                .Select(m => m.Id).ToList();
            #endregion

            var fieldstoExclude = new[]
            {
                GlobalEnums.TranslationCaseFields.AddUserBtn.ToString(),
                GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString(),
                GlobalEnums.TranslationCaseFields.Filename.ToString(),
                "tblLog.Text_Internal",
                "tblLog.Text_External",
                "tblLog.Charge",
                "tblLog.Text_Internal",
                "tblLog.Filename",
                GlobalEnums.TranslationCaseFields.FinishingDescription.ToString(),
                GlobalEnums.TranslationCaseFields.Miscellaneous.ToString(),
                GlobalEnums.TranslationCaseFields.UpdateNotifierInformation.ToString(),
                GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString(),
                GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(),
                GlobalEnums.TranslationCaseFields.SMS.ToString(),
                GlobalEnums.TranslationCaseFields.Project.ToString(),
                GlobalEnums.TranslationCaseFields.Problem.ToString(),
            };
            var externalFieldsToExclude = new[]
            {
                GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString(),
                GlobalEnums.TranslationCaseFields.RegTime.ToString(),
                GlobalEnums.TranslationCaseFields.ChangeTime.ToString(),
                GlobalEnums.TranslationCaseFields.User_Id.ToString(),
                GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),
                GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString(),
                GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(),
                GlobalEnums.TranslationCaseFields.Priority_Id.ToString(),
                //GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                //GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                GlobalEnums.TranslationCaseFields.CausingPart.ToString(),
                GlobalEnums.TranslationCaseFields.ClosingReason.ToString(),
                GlobalEnums.TranslationCaseFields.FinishingDate.ToString()
            };

            var allFields = model.CaseFieldSettings
                .Where(c => FieldSettingsUiNames.Names.ContainsKey(c.Name) && fieldstoExclude.All(f => f != c.Name));

            ViewBag.AllFields = allFields.Select(c => new CustomKeyValue<int, string>
            {
                Key = c.Id,
                Value = model.CaseFieldSettingWithLangauges.getLabel(c.Name) ?? ""
            })
                .OrderBy(c => c.Value)
                .ToList();

            ViewBag.ShowExternalStatusBarFields = allFields.Where(c => externalFieldsToExclude.All(f => f != c.Name))
                .Select(c => new CustomKeyValue<int, string>
                {
                    Key = c.Id,
                    Value = model.CaseFieldSettingWithLangauges.getLabel(c.Name) ?? ""
                })
                .OrderBy(c => c.Value)
                .ToList();
            return model;
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult SaveForm(ExtendedCaseFormPayloadModel payload)
        {
            List<CaseSolution> caseSolutionsWithForms = _extendedCaseService.GetCaseSolutionsWithExtendedCaseForm(payload);

            if (caseSolutionsWithForms.Count > 0)
            {
                string msg = Translation.GetCoreTextTranslation("Följande ärendemallar har redan ett formulär") + ": " + Environment.NewLine;

                foreach (var c in caseSolutionsWithForms)
                {
                    msg += Environment.NewLine + c.Name;
                }

                return Json(new { result = false, error = msg });
            }

            var id = _extendedCaseService.SaveExtendedCaseForm(payload, SessionFacade.CurrentUser.UserId);

            return Json(new { result = true, formId = id });
        }
    }
}