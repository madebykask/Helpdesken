using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using Microsoft.Ajax.Utilities;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class ExtendedCaseController : BaseAdminController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;

        public ExtendedCaseController(ICaseFieldSettingService caseFieldSettingService,
            ICustomerService customerService,
            ILanguageService languageService,
            IMasterDataService masterDataService) : base(masterDataService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _customerService = customerService;
            _languageService = languageService;
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
        public ActionResult Edit(int customerId, int? languageId,  IList<int> ShowStatusBarIds, IList<int> ShowExternalStatusBarIds)
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
    }
}