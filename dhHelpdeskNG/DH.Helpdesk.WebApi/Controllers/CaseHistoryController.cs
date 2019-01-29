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
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case.Histories;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.utils;
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
        private readonly ISettingService _customerSettingsService;

        public CaseHistoryController(ICaseFieldSettingService caseFieldSettingService, ICaseService caseService,
            IMapper mapper, ICaseFieldSettingsHelper caseFieldSettingsHelper,
            ICaseTranslationService caseTranslationService, ISettingService customerSettingsService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseService = caseService;
            _mapper = mapper;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _caseTranslationService = caseTranslationService;
            _customerSettingsService = customerSettingsService;
        }

        [HttpGet]
        [Route("{caseId:int}/histories")]
        [CheckUserPermissions(UserPermission.CaseInternalLogPermission)]
        public async Task<IHttpActionResult> Get([FromUri] int caseId, [FromUri] int cid, [FromUri] int langId)
        {
            var historiesDb = await _caseService.GetCaseHistoriesAsync(caseId).ConfigureAwait(false);
            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(cid);
            var customerSettings = _customerSettingsService.GetCustomerSettings(cid);

            var histories = new List<ICaseHistoryOutputModel>();
            for (var i = 0; i < historiesDb.Count; i++)
            {
                var current = historiesDb[i];
                var previous = i < historiesDb.Count - 1 ? historiesDb[i + 1] : null;
                // Reported by

                ICaseHistoryOutputModel item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.ReportedBy, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.ReportedBy.RemoveHtmlTags(), previous?.CaseHistory.ReportedBy.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons name
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_Name, cid, langId, caseFieldTranslations, caseFieldSettings,
                        current, current.CaseHistory.PersonsName.RemoveHtmlTags(), previous?.CaseHistory.PersonsName.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons e-mail
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_EMail, cid, langId, caseFieldTranslations, caseFieldSettings,
                   current, current.CaseHistory.PersonsEmail.RemoveHtmlTags(), previous?.CaseHistory.PersonsEmail.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons_phone 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_Phone, cid, langId, caseFieldTranslations, caseFieldSettings,
                   current, current.CaseHistory.PersonsPhone.RemoveHtmlTags(), previous?.CaseHistory.PersonsPhone.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons mobile no 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_CellPhone, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.PersonsCellphone.RemoveHtmlTags(), previous?.CaseHistory.PersonsCellphone.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Region
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Region_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.Region?.Name.RemoveHtmlTags(), previous?.Region?.Name.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // OU
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.OU_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.OU?.Name.RemoveHtmlTags(), previous?.OU?.Name.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Department
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Department_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.Department?.departmentDescription(customerSettings.DepartmentFilterFormat).RemoveHtmlTags(),
                    previous?.Department?.departmentDescription(customerSettings.DepartmentFilterFormat).RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // CostCentre
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.CostCentre, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.CostCentre.RemoveHtmlTags(), previous?.CaseHistory.CostCentre.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Placement 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Place, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.Place.RemoveHtmlTags(), previous?.CaseHistory.Place.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // UserCode
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.UserCode, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.UserCode.RemoveHtmlTags(), previous?.CaseHistory.UserCode.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout user

                // IsAbout Persons name

                // IsAbout Persons e-mail

                // IsAbout Persons_phone 

                // IsAbout Persons_cellphone

                // IsAbout Region

                // IsAbout Department

                // IsAbout OU

                // IsAbout Costcentre

                // IsAbout Place

                // IsAbout UserCode

                // Inventory Number

                // Inventory Type

                // Inventory Location

                // Registration Source

                // CaseType

                // ProductArea

                // System

                // Urgency

                // Impact

                // Category

                // Invoice number

                // ReferenceNumber

                // Caption

                // Description

                // Miscellaneous

                // Agreeddate

                // Available

                // Responsible User

                // Performer User

                // Priority

                // WorkingGroup

                // StateSecondary

                // Status

                //Project

                //Problem

                // Causing Part

                // Planned action date

                // Watchdate

                // Verified 

                // Verified description

                // Resolution rate 

                // CaseFile

                // LogFile

                // CaseLog

                // Closing Reason

                // Closing Date

                // Case extra followers

            }

            var model = new CaseHistoryOutputModel();
            model.Changes = histories;

            return Ok(model);
        }

        private bool IsActive(IList<CaseFieldSetting> list, GlobalEnums.TranslationCaseFields fieldName)
        {
            return _caseFieldSettingsHelper.IsActive(list, fieldName);
        }

        private CaseHistoryItemOutputModel<T> CreateCaseHistoryOutputModel<T>(GlobalEnums.TranslationCaseFields field,
            int cid, int langId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations,
            IList<CaseFieldSetting> caseFieldSettings,
            CaseHistoryMapperData current, T currentValue, T previousValue)
        {
            if (!IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReportedBy) ||
                (currentValue == null && previousValue == null) ||
                (currentValue != null && currentValue.Equals(previousValue)) ||
                (previousValue != null && currentValue == null)) return null;

            var item = new CaseHistoryItemOutputModel<T>();
            item.FieldName = CaseFieldsDefaultNames.TranslationCaseFieldsToApiNames[field];
            item.FieldLabel = _caseTranslationService.GetFieldLabel(field, langId, cid,
                caseFieldTranslations);
            item.Id = current.CaseHistory.Id;
            item.CreatedAt = DateTime.SpecifyKind(current.CaseHistory.CreatedDate, DateTimeKind.Utc); // specify that recieved UTC from db
            item.CreatedBy = current.CaseHistory.CreatedByUser;

            item.PreviousValue = previousValue;
            item.CurrentValue = currentValue;

            return item;
        }
    }
}