using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Models.Case.Histories;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Constants.Case;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Infrastructure.Translate;
using DH.Helpdesk.WebApi.Logic.Case;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseHistoryController : BaseApiController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseService _caseService;
        private readonly IMapper _mapper;
        private readonly ICaseFieldSettingsHelper _caseFieldSettingsHelper;
        private readonly ICaseTranslationService _caseTranslationService;

        public CaseHistoryController(ICaseFieldSettingService caseFieldSettingService, ICaseService caseService,
            IMapper mapper, ICaseFieldSettingsHelper caseFieldSettingsHelper, ICaseTranslationService caseTranslationService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseService = caseService;
            _mapper = mapper;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _caseTranslationService = caseTranslationService;
        }

        [HttpGet]
        [Route("{caseId:int}/histories")]
        [CheckUserPermissions(UserPermission.CaseInternalLogPermission)]
        public async Task<IHttpActionResult> Get([FromUri] int caseId, [FromUri] int cid, [FromUri] int langId)
        {
            var historiesDb = await _caseService.GetCaseHistoriesAsync(caseId).ConfigureAwait(false);
            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(cid);

            var histories = new List<ICaseHistoryOutputModel>();
            for (var i = 0; i < historiesDb.Count; i++)
            {
                var current = historiesDb[i];
                var previous = i < historiesDb.Count - 1 ? historiesDb[i + 1] : null;

                // Reported by
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReportedBy) &&
                    current.CaseHistory.ReportedBy != previous?.CaseHistory.ReportedBy)
                {
                    var item = CreateCaseHistoryOutputModel<string>(CaseFieldsNamesApi.ReportedBy, cid, langId, caseFieldTranslations, current,
                        GlobalEnums.TranslationCaseFields.ReportedBy);
                    item.PreviousValue = previous?.CaseHistory.ReportedBy.RemoveHtmlTags();
                    item.CurrentValue = current.CaseHistory.ReportedBy.RemoveHtmlTags();

                    histories.Add(item);
                }

                // Persons name
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_Name) &&
                    current.CaseHistory.PersonsName != previous?.CaseHistory.PersonsName)
                {
                    var item = CreateCaseHistoryOutputModel<string>(CaseFieldsNamesApi.PersonName, cid, langId, caseFieldTranslations, current,
                        GlobalEnums.TranslationCaseFields.Persons_Name);
                    item.PreviousValue = previous?.CaseHistory.PersonsName.RemoveHtmlTags();
                    item.CurrentValue = current.CaseHistory.PersonsName.RemoveHtmlTags();

                    histories.Add(item);
                }

            }

            var model = new CaseHistoryOutputModel();
            model.Changes = histories;
            
            return Ok(model);
        }

        private CaseHistoryItemOutputModel<T> CreateCaseHistoryOutputModel<T>(string fieldName, int cid, int langId, 
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations,
            CaseHistoryMapperData current, GlobalEnums.TranslationCaseFields field)
        {
            var item = new CaseHistoryItemOutputModel<T>();
            item.FieldName = fieldName;
            item.FieldLabel = _caseTranslationService.GetFieldLabel(field, langId, cid,
                caseFieldTranslations);
            item.Id = current.CaseHistory.Id;
            item.CreateAt = current.CaseHistory.CreatedDate;//TODO: TimeZoneInfo.ConvertTimeFromUtc(current.CreatedDate.ToUniversalTime(), userTimeZone);
            item.CreateBy = current.CaseHistory.CreatedByUser;
            return item;
        }
    }
}