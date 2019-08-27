using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Services.Services.Grid;
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
using DH.Helpdesk.Web.Infrastructure.Mvc;
using DH.Helpdesk.Web.Infrastructure.Tools;
using DH.Helpdesk.Web.Models;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.Case.Input;
using DH.Helpdesk.Web.Models.Case.Output;
using DH.Helpdesk.Web.Models.Shared;
using DHDomain = DH.Helpdesk.Domain;
using DH.Helpdesk.Domain;
using System.Web.Script.Serialization;

namespace DH.Helpdesk.Web.Controllers
{
    public class TranslationController : BaseController
    {
        private readonly ITextTranslationService _textTranslationService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IMasterDataService _masterDataService;

        public TranslationController(ITextTranslationService textTranslationService, ICaseFieldSettingService caseFieldSettingService, IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._masterDataService = masterDataService;
            this._textTranslationService = textTranslationService;
            this._caseFieldSettingService = caseFieldSettingService;
        }

        public JsonResult TranslateText(string text)
        {
            var TranslatedString = Translation.Get(text).ToString();
            return this.Json(TranslatedString, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetAllTextTranslations()
        {                       
            var ret = new List<KeyValuePair<string, string>>();
            ret = Translation.GetTranslationsForJS();            
            return this.Json(ret, JsonRequestBehavior.AllowGet);            
        }

        public JsonResult GetCaseFieldsForTranslation()
        {
            var caseFields = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);
            var allCaseFields = this._caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id, LanguageIds.Swedish);
            List<CaseFieldSettingsWithLanguage> newListOfCaseFieldSettingsWithLanguage = new List<CaseFieldSettingsWithLanguage>();
            var allCaseFieldsMissingLabels = allCaseFields.Where(x=> caseFields.All(y => y.Name != x.Name));
            foreach (var caseField in allCaseFieldsMissingLabels)
            {
                var translatedCaseField = new CaseFieldSettingsWithLanguage();
                var coreLabel = caseField.CaseFieldSettingLanguages.Where(x => x.Language_Id == 1).Select(y => y.Label);
                var translatedLabel = Translation.GetCoreTextTranslation(coreLabel.FirstOrDefault());
                translatedCaseField.Name = caseField.Name;
                translatedCaseField.Language_Id = SessionFacade.CurrentLanguageId;
                translatedCaseField.Label = translatedLabel;
                newListOfCaseFieldSettingsWithLanguage.Add(translatedCaseField);
            }
            foreach (var item in caseFields)
            {
                newListOfCaseFieldSettingsWithLanguage.Add(item);
            }

            return this.Json(newListOfCaseFieldSettingsWithLanguage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CurrentLanguageId()
        {
            var currentLanguageId = SessionFacade.CurrentLanguageId;
            return Json(currentLanguageId, JsonRequestBehavior.AllowGet);
        }
               
    }
}
