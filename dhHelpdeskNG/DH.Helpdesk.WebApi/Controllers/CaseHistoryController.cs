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
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case.Histories;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.utils;
using DH.Helpdesk.Web.Common.Constants.Case;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Logic.Case;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseHistoryController : BaseApiController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseService _caseService;
        private readonly ICaseFieldSettingsHelper _caseFieldSettingsHelper;
        private readonly ICaseTranslationService _caseTranslationService;
        private readonly ISettingService _customerSettingsService;
        private readonly ITranslateCacheService _translateCacheService;

        public CaseHistoryController(ICaseFieldSettingService caseFieldSettingService, ICaseService caseService,
            ICaseFieldSettingsHelper caseFieldSettingsHelper, ICaseTranslationService caseTranslationService,
            ISettingService customerSettingsService, ITranslateCacheService translateCacheService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseService = caseService;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _caseTranslationService = caseTranslationService;
            _customerSettingsService = customerSettingsService;
            _translateCacheService = translateCacheService;
        }

        [HttpGet]
        [Route("{caseId:int}/histories")]
        [CheckUserPermissions(UserPermission.CaseInternalLogPermission)]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        public async Task<IHttpActionResult> Get([FromUri] int caseId, [FromUri] int cid, [FromUri] int langId)
        {
            var historiesDb = await _caseService.GetCaseHistoriesAsync(caseId).ConfigureAwait(false);
            historiesDb = historiesDb.OrderByDescending(x => x.CaseHistory.Id).ToList();
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
                    current, current.CaseHistory.ReportedBy?.RemoveHtmlTags(), previous?.CaseHistory.ReportedBy?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons name
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_Name, cid, langId, caseFieldTranslations, caseFieldSettings,
                        current, current.CaseHistory.PersonsName?.RemoveHtmlTags(), previous?.CaseHistory.PersonsName?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons e-mail
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_EMail, cid, langId, caseFieldTranslations, caseFieldSettings,
                   current, current.CaseHistory.PersonsEmail?.RemoveHtmlTags(), previous?.CaseHistory.PersonsEmail?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons_phone 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_Phone, cid, langId, caseFieldTranslations, caseFieldSettings,
                   current, current.CaseHistory.PersonsPhone?.RemoveHtmlTags(), previous?.CaseHistory.PersonsPhone?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Persons mobile no 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Persons_CellPhone, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.PersonsCellphone?.RemoveHtmlTags(), previous?.CaseHistory.PersonsCellphone?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Region
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Region_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.Region?.Name?.RemoveHtmlTags(), previous?.Region?.Name?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // OU
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.OU_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.OU?.Name?.RemoveHtmlTags(), previous?.OU?.Name?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Department
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Department_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.Department?.departmentDescription(customerSettings.DepartmentFilterFormat)?.RemoveHtmlTags(),
                    previous?.Department?.departmentDescription(customerSettings.DepartmentFilterFormat)?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // CostCentre
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.CostCentre, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.CostCentre?.RemoveHtmlTags(), previous?.CaseHistory.CostCentre?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Placement 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Place, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.Place?.RemoveHtmlTags(), previous?.CaseHistory.Place?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // UserCode
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.UserCode, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.UserCode?.RemoveHtmlTags(), previous?.CaseHistory.UserCode?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout user
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_ReportedBy?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_ReportedBy?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Persons name
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_Persons_Name?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_Persons_Name?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Persons e-mail
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_Persons_EMail?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_Persons_EMail?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Persons_phone 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_Persons_Phone?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_Persons_Phone?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Persons_cellphone
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_Persons_CellPhone?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_Persons_CellPhone?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Region
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.IsAbout_Region?.Name?.RemoveHtmlTags(), previous?.IsAbout_Region?.Name?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Department
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.IsAbout_Department?.departmentDescription(customerSettings.DepartmentFilterFormat)?.RemoveHtmlTags(),
                    previous?.IsAbout_Department?.departmentDescription(customerSettings.DepartmentFilterFormat)?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout OU
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.IsAbout_OU?.Name?.RemoveHtmlTags(), previous?.IsAbout_OU?.Name?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Costcentre
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_CostCentre?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_CostCentre?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout Place
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_Place, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_Place?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_Place?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // IsAbout UserCode
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.IsAbout_UserCode, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.IsAbout_UserCode?.RemoveHtmlTags(), previous?.CaseHistory.IsAbout_UserCode?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Inventory Number
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.InventoryNumber, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.InventoryNumber?.RemoveHtmlTags(), previous?.CaseHistory.InventoryNumber?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Inventory Type
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.ComputerType_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.InventoryType?.RemoveHtmlTags(), previous?.CaseHistory.InventoryType?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Inventory Location
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.InventoryLocation, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.InventoryLocation?.RemoveHtmlTags(), previous?.CaseHistory.InventoryLocation?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Registration Source
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.RegistrationSourceCustomer?.SourceName?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.RegistrationSourceCustomer?.SourceName?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // CaseType
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.CaseType_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.CaseType?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.CaseType?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // ProductArea
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.ProductArea_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.ProductArea?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.ProductArea?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // System
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.System_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.System?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.System?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // Urgency
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Urgency_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.Urgency?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.Urgency?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // Impact
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Impact_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.Impact?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.Impact?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // Category
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Category_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.Category?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.Category?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // Invoice number
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.InvoiceNumber, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.InvoiceNumber?.RemoveHtmlTags(), previous?.CaseHistory.InvoiceNumber?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // ReferenceNumber
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.ReferenceNumber, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.ReferenceNumber?.RemoveHtmlTags(), previous?.CaseHistory.ReferenceNumber?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Caption
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Caption, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.Caption?.RemoveHtmlTags(), previous?.CaseHistory.Caption?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Description
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Description, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.Description?.RemoveHtmlTags(), previous?.CaseHistory.Description?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Miscellaneous
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Miscellaneous, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.Miscellaneous?.RemoveHtmlTags(), previous?.CaseHistory.Miscellaneous?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Agreeddate
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.AgreedDate, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.AgreedDate, previous?.CaseHistory.AgreedDate);
                if (item != null) histories.Add(item);

                // Available
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Available, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.Available?.RemoveHtmlTags(), previous?.CaseHistory.Available?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Responsible User
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    current.UserResponsible?.FormatUserName(customerSettings.IsUserFirstLastNameRepresentation.ToBool()),
                    previous?.UserResponsible?.FormatUserName(customerSettings.IsUserFirstLastNameRepresentation.ToBool()));
                if (item != null) histories.Add(item);

                // Performer User
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Performer_User_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    current.UserPerformer?.FormatUserName(customerSettings.IsUserFirstLastNameRepresentation.ToBool()),
                    previous?.UserPerformer?.FormatUserName(customerSettings.IsUserFirstLastNameRepresentation.ToBool()));
                if (item != null) histories.Add(item);

                // Priority
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Priority_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.Priority?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.Priority?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // WorkingGroup
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.WorkingGroup_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.WorkingGroup?.WorkingGroupName?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.WorkingGroup?.WorkingGroupName?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // StateSecondary
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.StateSecondary_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.StateSecondary?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.StateSecondary?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                // Status
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Status_Id, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    _translateCacheService.GetTextTranslation(current.Status?.Name?.RemoveHtmlTags(), cid),
                    _translateCacheService.GetTextTranslation(previous?.Status?.Name?.RemoveHtmlTags(), cid));
                if (item != null) histories.Add(item);

                //Project
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Project, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.Project?.Name, previous?.Project?.Name);
                if (item != null) histories.Add(item);

                //Problem
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Problem, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.Problem?.Name, previous?.Problem?.Name);
                if (item != null) histories.Add(item);

                // Causing Part
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.CausingPart, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CausingPart?.Name, previous?.CausingPart?.Name);
                if (item != null) histories.Add(item);

                // Planned action date
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.PlanDate, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.PlanDate, previous?.CaseHistory.PlanDate);
                if (item != null) histories.Add(item);

                // Watchdate
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.WatchDate, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.WatchDate, previous?.CaseHistory.WatchDate);
                if (item != null) histories.Add(item);

                // Verified 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Verified, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.Verified, previous?.CaseHistory.Verified);
                if (item != null) histories.Add(item);

                // Verified description
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.VerifiedDescription, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.VerifiedDescription?.RemoveHtmlTags(), previous?.CaseHistory.VerifiedDescription?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Resolution rate 
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.SolutionRate, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.SolutionRate?.RemoveHtmlTags(), previous?.CaseHistory.SolutionRate?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // CaseFile
                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Filename) &&
                    current.CaseHistory.CaseFile != previous?.CaseHistory.CaseFile && !string.IsNullOrEmpty(current.CaseHistory.CaseFile))
                {
                    if (!string.IsNullOrEmpty(current.CaseHistory.CaseFile))
                    {
                        var curValue = "";
                        var prevValue = "";
                        if (current.CaseHistory.CaseFile.StartsWith(StringTags.Add))
                        {
                            curValue = string.Format("{0} {1}", _translateCacheService.GetTextTranslation("Lägg till", cid),
                                current.CaseHistory.CaseFile.Substring(StringTags.Add.Length));
                        }
                        else if (current.CaseHistory.CaseFile.StartsWith(StringTags.Delete))
                        {
                            curValue = string.Format("{0} {1}", _translateCacheService.GetTextTranslation("Ta bort", cid),
                                current.CaseHistory.CaseFile.Substring(StringTags.Delete.Length));
                        }
                        else
                            curValue = current.CaseHistory.CaseFile;

                        CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.Filename, cid, langId,
                            caseFieldTranslations, caseFieldSettings,
                            current, curValue, prevValue);
                        if (item != null) histories.Add(item);
                    }
                }

                // Closing Reason
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.ClosingReason, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.ClosingReason?.RemoveHtmlTags(), previous?.CaseHistory.ClosingReason?.RemoveHtmlTags());
                if (item != null) histories.Add(item);

                // Closing Date
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.FinishingDate, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, current.CaseHistory.FinishingDate, previous?.CaseHistory.FinishingDate);
                if (item != null) histories.Add(item);

                // Case extra followers
                item = CreateCaseHistoryOutputModel(GlobalEnums.TranslationCaseFields.AddFollowersBtn, cid, langId, caseFieldTranslations, caseFieldSettings,
                    current, 
                    current.CaseHistory.CaseExtraFollowers.Replace(BRConstItem.Email_Separator, BRConstItem.Email_Separator + " "),
                    previous?.CaseHistory.CaseExtraFollowers.Replace(BRConstItem.Email_Separator, BRConstItem.Email_Separator + " "));
                if (item != null) histories.Add(item);
            }

            var model = new CaseHistoryOutputModel
            {
                Changes = histories
            };

            return Ok(model);
        }

        private bool IsActive(IList<CaseFieldSetting> caseFieldSettings, GlobalEnums.TranslationCaseFields fieldName)
        {
            return _caseFieldSettingsHelper.IsActive(caseFieldSettings, null, fieldName);
        }

        private CaseHistoryItemOutputModel<T> CreateCaseHistoryOutputModel<T>(GlobalEnums.TranslationCaseFields field,
            int cid, int langId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations,
            IList<CaseFieldSetting> caseFieldSettings,
            CaseHistoryMapperData current, T currentValue, T previousValue)
        {

            if (!IsActive(caseFieldSettings, field) ||
                (currentValue == null && previousValue == null) ||
                (currentValue != null && currentValue.Equals(previousValue)) ||
                (previousValue != null && currentValue == null)) return null;

            var item = new CaseHistoryItemOutputModel<T>
            {
                Id = current.CaseHistory.Id,
                FieldName = CaseFieldsDefaultNames.TranslationCaseFieldsToApiNames.ContainsKey(field) 
                    ? CaseFieldsDefaultNames.TranslationCaseFieldsToApiNames[field]
                    : "",
                FieldLabel = _caseTranslationService.GetFieldLabel(field, langId, cid, caseFieldTranslations),
                CreatedAt = DateTime.SpecifyKind(current.CaseHistory.CreatedDate, DateTimeKind.Utc),
                CreatedBy = current.CaseHistory.CreatedByUser,
                PreviousValue = previousValue,
                CurrentValue = currentValue
            };
            // specify that recieved UTC from db


            return item;
        }
    }
}