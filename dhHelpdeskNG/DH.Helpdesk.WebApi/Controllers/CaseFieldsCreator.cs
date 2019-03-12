using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Constants.Case;
using DH.Helpdesk.WebApi.Logic.Case;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;

namespace DH.Helpdesk.WebApi.Controllers
{
    public interface ICaseFieldsCreator
    {
        void CreateInitiatorSection(int cid, CustomerUser customerUserSetting,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CaseDefaultsInfo customerDefaults = null);

        void CreateRegardingSection(int cid, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model);

        void CreateComputerInfoSection(int cid, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model);

        void CreateCaseInfoSection(int cid, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CaseDefaultsInfo customerDefaults = null);

        void CreateCaseManagementSection(int cid, UserOverview currentUserOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CustomerSettings customerSettings,
            CaseDefaultsInfo customerDefaults = null);

        void CreateCommunicationSection(int cid, UserOverview currentUserOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, Case currentCase,
            CaseSolution template,
            int languageId, IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model);
    }

    public class CaseFieldsCreator : ICaseFieldsCreator
    {
        private readonly ICaseFileService _caseFileService;
        private readonly ICaseFieldSettingsHelper _caseFieldSettingsHelper;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ISupplierService _supplierService;
        private readonly ICaseTranslationService _caseTranslationService;
        private readonly IDepartmentService _departmentService;
        private readonly IPriorityService _priorityService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;

        public CaseFieldsCreator(ICaseFileService caseFileService, ICaseFieldSettingsHelper caseFieldSettingsHelper,
            IUserService userService, IWorkingGroupService workingGroupService, ISupplierService supplierService,
            ICaseTranslationService caseTranslationService, IDepartmentService departmentService,
            IPriorityService priorityService,
            IWatchDateCalendarService watchDateCalendarService, ICaseTypeService caseTypeService,
            IProductAreaService productAreaService)
        {
            _caseFileService = caseFileService;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _userService = userService;
            _workingGroupService = workingGroupService;
            _supplierService = supplierService;
            _caseTranslationService = caseTranslationService;
            _departmentService = departmentService;
            _priorityService = priorityService;
            _watchDateCalendarService = watchDateCalendarService;
            _caseTypeService = caseTypeService;
            _productAreaService = productAreaService;
        }

        public void CreateInitiatorSection(int cid, CustomerUser customerUserSetting,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CaseDefaultsInfo customerDefaults = null)
        {
            // Initiator
            //displayUserInfoHtml:TODO: see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayUserInfoHtml
            IBaseCaseField field;
            if (customerUserSetting.UserInfoPermission.ToBool())
            {
                //if (Model.ComputerUserCategories.Any())
                //GlobalEnums.TranslationCaseFields.UserSearchCategory_Id//TODO: add UserSearchCategory_Id
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.ReportedBy))
                {
                    field = GetField(currentCase != null ? currentCase.ReportedBy : template?.ReportedBy, cid,
                        languageId,
                        CaseFieldsNamesApi.ReportedBy, GlobalEnums.TranslationCaseFields.ReportedBy,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 40);
                    model.Fields.Add(field);
                }

                //initiator category Id
                //if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.UserSearchCategory_Id))
                //{
                //    field = new BaseCaseField<string>()
                //    {
                //        Name = CaseFieldsNamesApi.UserSearchCategory_Id.ToString(),
                //        Value = //todo:  set default value from field settings?
                //        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UserSearchCategory_Id, languageId, cid, caseFieldTranslations),
                //        Section = CaseSectionType.Initiator,
                //        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UserSearchCategory_Id, caseFieldSettings)
                //    };
                //    model.Fields.Add(field);
                //}

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Persons_Name))
                {
                    field = GetField(currentCase != null ? currentCase.PersonsName : template?.PersonsName, cid,
                        languageId,
                        CaseFieldsNamesApi.PersonName, GlobalEnums.TranslationCaseFields.Persons_Name,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 50);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Persons_EMail))
                {
                    field = GetField(currentCase != null ? currentCase.PersonsEmail : template?.PersonsEmail, cid,
                        languageId,
                        CaseFieldsNamesApi.PersonEmail, GlobalEnums.TranslationCaseFields.Persons_EMail,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 100);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Persons_Phone))
                {
                    field = GetField(currentCase != null ? currentCase.PersonsPhone : template?.PersonsPhone, cid,
                        languageId,
                        CaseFieldsNamesApi.PersonPhone, GlobalEnums.TranslationCaseFields.Persons_Phone,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 50);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Persons_CellPhone))
                {
                    field = GetField(currentCase != null ? currentCase.PersonsCellphone : template?.PersonsCellPhone,
                        cid, languageId,
                        CaseFieldsNamesApi.PersonCellPhone, GlobalEnums.TranslationCaseFields.Persons_CellPhone,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 50);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Region_Id))
                {
                    var regionId = currentCase != null
                        ? currentCase.Region_Id
                        : template?.Region_Id ?? customerDefaults?.RegionId;
                    field = GetField(regionId, cid, languageId,
                        CaseFieldsNamesApi.RegionId, GlobalEnums.TranslationCaseFields.Region_Id,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Department_Id))
                {
                    field = GetField(currentCase != null ? currentCase.Department_Id : template?.Department_Id, cid,
                        languageId,
                        CaseFieldsNamesApi.DepartmentId, GlobalEnums.TranslationCaseFields.Department_Id,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.OU_Id))
                {
                    field = GetField(currentCase != null ? currentCase.OU_Id : template?.OU_Id, cid, languageId,
                        CaseFieldsNamesApi.OrganizationUnitId, GlobalEnums.TranslationCaseFields.OU_Id,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.CostCentre))
                {
                    field = GetField(currentCase != null ? currentCase.CostCentre : template?.CostCentre, cid,
                        languageId,
                        CaseFieldsNamesApi.CostCentre, GlobalEnums.TranslationCaseFields.CostCentre,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 50);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Place))
                {
                    field = GetField(currentCase != null ? currentCase.Place : template?.Place, cid, languageId,
                        CaseFieldsNamesApi.Place, GlobalEnums.TranslationCaseFields.Place, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 100);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.UserCode))
                {
                    field = GetField(currentCase != null ? currentCase.UserCode : template?.UserCode, cid, languageId,
                        CaseFieldsNamesApi.UserCode, GlobalEnums.TranslationCaseFields.UserCode,
                        CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 50);
                    model.Fields.Add(field);
                }

                //field = new BaseCaseField<bool>()//TODO: for edit
                //{
                //    Name = CaseFieldsNamesApi.UpdateNotifierInformation.ToString(),
                //    Value = true,
                //    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, 
                //        languageId, cid, caseFieldTranslations, ""),
                //    Section = CaseSectionType.Initiator,
                //    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, caseFieldSettings//        )
                //};
                //model.Fields.Add(field);
            }
        }

        public void CreateRegardingSection(int cid, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            IBaseCaseField field;
            // Regarding
            //displayAboutUserInfoHtml:TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayAboutUserInfoHtml

            //regarding category Id
            //if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.UserSearchCategory_Id))
            //{
            //    field = new BaseCaseField<string>()
            //    {
            //        Name = CaseFieldsNamesApi.IsAbout_UserSearchCategory_Id.ToString(),
            //        Value = //todo:  set default value from field settings?
            //        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id, languageId, cid, caseFieldTranslations),
            //        Section = CaseSectionType.Regarding,
            //        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id, caseFieldSettings)
            //    };
            //    model.Fields.Add(field);
            //}

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.ReportedBy : template?.IsAbout_ReportedBy,
                    cid, languageId,
                    CaseFieldsNamesApi.IsAbout_ReportedBy, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 40);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Person_Name : template?.IsAbout_PersonsName,
                    cid, languageId,
                    CaseFieldsNamesApi.IsAbout_PersonName, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail))
            {
                field = GetField(
                    currentCase != null ? currentCase.IsAbout?.Person_Email : template?.IsAbout_PersonsEmail, cid,
                    languageId,
                    CaseFieldsNamesApi.IsAbout_PersonEmail, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone))
            {
                field = GetField(
                    currentCase != null ? currentCase.IsAbout?.Person_Phone : template?.IsAbout_PersonsPhone, cid,
                    languageId,
                    CaseFieldsNamesApi.IsAbout_PersonPhone, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 40);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone))
            {
                field = GetField(
                    currentCase != null ? currentCase.IsAbout?.Person_Cellphone : template?.IsAbout_PersonsCellPhone,
                    cid, languageId,
                    CaseFieldsNamesApi.IsAbout_PersonCellPhone,
                    GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 30);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Region_Id))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Region_Id : template?.IsAbout_Region_Id,
                    cid, languageId,
                    CaseFieldsNamesApi.IsAbout_RegionId, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Department_Id))
            {
                field = GetField(
                    currentCase != null ? currentCase.IsAbout?.Department_Id : template?.IsAbout_Department_Id, cid,
                    languageId,
                    CaseFieldsNamesApi.IsAbout_DepartmentId, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_OU_Id))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.OU_Id : template?.IsAbout_OU_Id, cid,
                    languageId,
                    CaseFieldsNamesApi.IsAbout_OrganizationUnitId, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_CostCentre))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.CostCentre : template?.IsAbout_CostCentre,
                    cid, languageId,
                    CaseFieldsNamesApi.IsAbout_CostCentre, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Place))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Place : template?.IsAbout_Place, cid,
                    languageId,
                    CaseFieldsNamesApi.IsAbout_Place, GlobalEnums.TranslationCaseFields.IsAbout_Place,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 100);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_UserCode))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.UserCode : template?.IsAbout_UserCode, cid,
                    languageId,
                    CaseFieldsNamesApi.IsAbout_UserCode, GlobalEnums.TranslationCaseFields.IsAbout_UserCode,
                    CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }
        }

        public void CreateComputerInfoSection(int cid, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            IBaseCaseField field;
            // ComputerInfo
            //displayComputerInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayComputerInfoHtml
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.InventoryNumber))
            {
                field = GetField(currentCase != null ? currentCase.InventoryNumber : template?.InventoryNumber, cid,
                    languageId,
                    CaseFieldsNamesApi.InventoryNumber, GlobalEnums.TranslationCaseFields.InventoryNumber,
                    CaseSectionType.ComputerInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 60);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.ComputerType_Id))
            {
                field = GetField(currentCase != null ? currentCase.InventoryType : template?.InventoryType, cid,
                    languageId,
                    CaseFieldsNamesApi.ComputerTypeId, GlobalEnums.TranslationCaseFields.ComputerType_Id,
                    CaseSectionType.ComputerInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.InventoryLocation))
            {
                field = GetField(currentCase != null ? currentCase.InventoryLocation : template?.InventoryLocation, cid,
                    languageId,
                    CaseFieldsNamesApi.InventoryLocation, GlobalEnums.TranslationCaseFields.InventoryLocation,
                    CaseSectionType.ComputerInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 100);
                model.Fields.Add(field);
            }
        }

        public void CreateCaseInfoSection(int cid, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CaseDefaultsInfo customerDefaults = null)
        {
            IBaseCaseField field;
            // CaseInfo
            //displayCaseInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.CaseNumber) && currentCase != null)
            {
                field = GetField(
                    currentCase != null ? currentCase.CaseNumber.ToString(CultureInfo.InvariantCulture) : "", cid,
                    languageId,
                    CaseFieldsNamesApi.CaseNumber, GlobalEnums.TranslationCaseFields.CaseNumber,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.RegTime) && currentCase != null)
            {
                field = GetField(
                    currentCase != null ? DateTime.SpecifyKind(currentCase.RegTime, DateTimeKind.Utc) : DateTime.UtcNow,
                    cid, languageId,
                    CaseFieldsNamesApi.RegTime, GlobalEnums.TranslationCaseFields.RegTime, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.ChangeTime) && currentCase != null)
            {
                field = GetField(
                    currentCase != null
                        ? DateTime.SpecifyKind(currentCase.ChangeTime, DateTimeKind.Utc)
                        : new DateTime?(), cid, languageId,
                    CaseFieldsNamesApi.ChangeTime, GlobalEnums.TranslationCaseFields.ChangeTime,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.User_Id) && currentCase != null)
            {
                var userIdValue = "";
                if (currentCase?.User_Id != null)
                {
                    var user = _userService.GetUser(currentCase.User_Id.Value);
                    WorkingGroupEntity caseOwnerDefaultWorkingGroup = null;
                    if (currentCase.DefaultOwnerWG_Id.HasValue && currentCase.DefaultOwnerWG_Id.Value > 0)
                    {
                        caseOwnerDefaultWorkingGroup =
                            _workingGroupService.GetWorkingGroup(currentCase.DefaultOwnerWG_Id.Value);
                    }

                    if (user != null)
                    {
                        userIdValue = $"{user.FirstName} {user.SurName}";
                        if (caseOwnerDefaultWorkingGroup != null)
                            userIdValue += $" {caseOwnerDefaultWorkingGroup.WorkingGroupName}";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(currentCase.RegUserName))
                            userIdValue = currentCase.RegUserName;
                        if (!string.IsNullOrWhiteSpace(currentCase.RegUserId))
                            userIdValue += $" {currentCase.RegUserId}";
                    }
                }

                field = GetField(userIdValue, cid, languageId,
                    CaseFieldsNamesApi.UserId, GlobalEnums.TranslationCaseFields.User_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer))
            {
                field = GetField(currentCase != null ? currentCase.RegistrationSourceCustomer_Id : template?.RegistrationSource, cid,
                    languageId,
                    CaseFieldsNamesApi.RegistrationSourceCustomer,
                    GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.CaseType_Id))
            {
                var caseTypeId = currentCase != null
                    ? currentCase.CaseType_Id
                    : template?.CaseType_Id ?? customerDefaults?.CaseTypeId;
                field = GetField(caseTypeId, cid, languageId,
                    CaseFieldsNamesApi.CaseTypeId, GlobalEnums.TranslationCaseFields.CaseType_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.ProductArea_Id))
            {
                field = GetField(currentCase != null ? currentCase.ProductArea_Id : template?.ProductArea_Id, cid,
                    languageId,
                    CaseFieldsNamesApi.ProductAreaId, GlobalEnums.TranslationCaseFields.ProductArea_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.System_Id))
            {
                field = GetField(currentCase != null ? currentCase.System_Id : template?.System_Id, cid, languageId,
                    CaseFieldsNamesApi.SystemId, GlobalEnums.TranslationCaseFields.System_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Urgency_Id))
            {
                field = GetField(currentCase != null ? currentCase.Urgency_Id : template?.Urgency_Id, cid, languageId,
                    CaseFieldsNamesApi.UrgencyId, GlobalEnums.TranslationCaseFields.Urgency_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Impact_Id))
            {
                field = GetField(currentCase != null ? currentCase.Impact_Id : template?.Impact_Id, cid, languageId,
                    CaseFieldsNamesApi.ImpactId, GlobalEnums.TranslationCaseFields.Impact_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Category_Id))
            {
                field = GetField(currentCase != null ? currentCase.Category_Id : template?.Category_Id, cid, languageId,
                    CaseFieldsNamesApi.CategoryId, GlobalEnums.TranslationCaseFields.Category_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Supplier_Id))
            {
                var supplierId = currentCase != null
                    ? currentCase.Supplier_Id
                    : template?.Supplier_Id ?? customerDefaults?.SupplierId;
                field = GetField(supplierId, cid, languageId,
                    CaseFieldsNamesApi.SupplierId, GlobalEnums.TranslationCaseFields.Supplier_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);

                var supplier = supplierId.HasValue
                    ? _supplierService.GetSupplier(supplierId.Value)
                    : null;
                field = GetField(supplier?.Country_Id, cid, languageId,
                    CaseFieldsNamesApi.SupplierCountryId, GlobalEnums.TranslationCaseFields.Supplier_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.InvoiceNumber))
            {
                field = GetField(currentCase != null ? currentCase.InvoiceNumber : template?.InvoiceNumber, cid,
                    languageId,
                    CaseFieldsNamesApi.InvoiceNumber, GlobalEnums.TranslationCaseFields.InvoiceNumber,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.ReferenceNumber))
            {
                field = GetField(currentCase != null ? currentCase.ReferenceNumber : template?.ReferenceNumber, cid,
                    languageId,
                    CaseFieldsNamesApi.ReferenceNumber, GlobalEnums.TranslationCaseFields.ReferenceNumber,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Caption))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Caption,
                    Value = currentCase != null ? currentCase.Caption : template?.Caption,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Caption,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Caption, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Caption,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.CaptionPermission))
                    AddReadOnlyOption(field.Options);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Description))
            {
                var registrationSource = currentCase != null
                    ? currentCase.RegistrationSource
                    : template?.RegistrationSource;
                var registrationSourceOptions = (currentCase != null &&
                                                 registrationSource == (int) CaseRegistrationSource.Email)
                    ? currentCase.Mail2Tickets.Where(x => x.Log_Id == null)
                        .GroupBy(x => x.Type)
                        .Select(gr =>
                            new KeyValuePair<string, string>(gr.Key, string.Join(";", gr.Select(x => x.EMailAddress))))
                        .ToList()
                    : null;

                // Registration source: values 
                field = new BaseCaseField<int>()
                {
                    Name =
                        CaseFieldsNamesApi
                            .CaseRegistrationSource, //NOTE: should not be mistaken with another field - RegistrationSourceCustomer!
                    Value = registrationSource ?? 0,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Registration source"),
                    Section = CaseSectionType.CaseInfo,
                    Options = registrationSourceOptions
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Description,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.Description : template?.Description, cid, languageId,
                    CaseFieldsNamesApi.Description, GlobalEnums.TranslationCaseFields.Description,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Description,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                AddMaxLengthOption(field.Options, 10000);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Miscellaneous))
            {
                field = GetField(currentCase != null ? currentCase.Miscellaneous : template?.Miscellaneous, cid,
                    languageId,
                    CaseFieldsNamesApi.Miscellaneous, GlobalEnums.TranslationCaseFields.Miscellaneous,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 1000);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.ContactBeforeAction))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.ContactBeforeAction,
                    Value =
                        currentCase?.ContactBeforeAction.ToBool() ?? template?.ContactBeforeAction.ToBool() ?? false,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ContactBeforeAction,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.ContactBeforeAction, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.ContactBeforeAction,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.ContactBeforeActionPermission))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.SMS))
            {
                field = GetField(currentCase?.SMS.ToBool() ?? template?.SMS.ToBool() ?? false, cid, languageId,
                    CaseFieldsNamesApi.Sms, GlobalEnums.TranslationCaseFields.SMS, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.AgreedDate))
            {
                field = GetField(currentCase != null ? currentCase.AgreedDate : template.AgreedDate, cid, languageId,
                    CaseFieldsNamesApi.AgreedDate, GlobalEnums.TranslationCaseFields.AgreedDate,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Available))
            {
                field = GetField(currentCase != null ? currentCase.Available : template?.Available, cid, languageId,
                    CaseFieldsNamesApi.Available, GlobalEnums.TranslationCaseFields.Available, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 100);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Cost))
            {
                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost,
                    Value = currentCase?.Cost ?? template?.Cost ?? 0,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Artikelkostnad"), //Kostnad
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Cost,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                AddMaxLengthOption(field.Options, 7);
                model.Fields.Add(field);

                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost_OtherCost,
                    Value = currentCase?.OtherCost ?? template?.OtherCost ?? 0,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Övrig kostnad"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Cost,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                AddMaxLengthOption(field.Options, 7);
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Cost_Currency,
                    Value = currentCase != null ? currentCase.Currency : template?.Currency ?? "",
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Valuta"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Cost,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);
            }

            if ( /*customerSettings.AttachmentPlacement == 1 &&*/
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Filename))
            {
                field = GetField(currentCase != null ? GetCaseFilesModel(currentCase.Id) : null, cid, languageId,
                    CaseFieldsNamesApi.Filename, GlobalEnums.TranslationCaseFields.Filename, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }
        }

        public void CreateCaseManagementSection(int cid, UserOverview currentUserOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CustomerSettings customerSettings,
            CaseDefaultsInfo customerDefaults = null)
        {
            IBaseCaseField field;
            //CaseManagement
            //displayCaseManagementInfoHtml //TODO: see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
            {
                var workingGroupId = currentCase != null ? currentCase.WorkingGroup_Id : template?.CaseWorkingGroup_Id;
                if (template != null && template.SetCurrentUsersWorkingGroup.ToBool())
                    workingGroupId = currentUserOverview.DefaultWorkingGroupId;

                // Set working group from the case type working group if was not set before for New case only
                if (!workingGroupId.HasValue || workingGroupId.Value == 0)
                {
                    var caseTypeField = model.Fields.FirstOrDefault(f =>
                        f.Name.Equals(CaseFieldsNamesApi.CaseTypeId.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    if (caseTypeField != null)
                    {
                        var caseId = ((BaseCaseField<int?>) caseTypeField).Value;
                        if (caseId.HasValue && caseId > 0)
                        {
                            var caseType = _caseTypeService.GetCaseType(caseId.Value);
                            workingGroupId = caseType.WorkingGroup_Id;
                        }
                    }
                }

                // Set working group from the product area working group if was not set before for New case only
                if (!workingGroupId.HasValue || workingGroupId.Value == 0)
                {
                    var productAreaField = model.Fields.FirstOrDefault(f =>
                        f.Name.Equals(CaseFieldsNamesApi.ProductAreaId.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    if (productAreaField != null)
                    {
                        var productAreaId = ((BaseCaseField<int?>) productAreaField).Value;
                        if (productAreaId.HasValue && productAreaId.Value > 0)
                        {
                            var productArea = _productAreaService.GetProductArea(productAreaId.Value);
                            workingGroupId = productArea.WorkingGroup_Id;
                        }
                    }
                }

                field = GetField(workingGroupId, cid, languageId,
                    CaseFieldsNamesApi.WorkingGroupId, GlobalEnums.TranslationCaseFields.WorkingGroup_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
            {
                var caseResponsibleUserId = currentCase != null
                    ? currentCase.CaseResponsibleUser_Id
                    : currentUserOverview.Id;
                field = GetField(caseResponsibleUserId, cid, languageId,
                    CaseFieldsNamesApi.CaseResponsibleUserId, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Performer_User_Id))
            {
                int? performerId;
                if (currentCase == null && template == null)
                    performerId = customerSettings.SetUserToAdministrator
                        ? currentUserOverview.Id
                        : customerSettings.DefaultAdministratorId;
                else
                {
                    performerId = currentCase != null ? currentCase.Performer_User_Id : template.PerformerUser_Id;
                    if (template != null && template.SetCurrentUserAsPerformer.ToBool())
                        performerId = currentUserOverview.Id; // current user
                }

                // Set perfomer from the case type perfomer if was not set before for New case only
                if (!performerId.HasValue || performerId.Value == 0)
                {
                    var caseTypeField = model.Fields.FirstOrDefault(f =>
                        f.Name == GlobalEnums.TranslationCaseFields.CaseType_Id.ToString());
                    if (caseTypeField != null)
                    {
                        var caseId = ((BaseCaseField<int?>) caseTypeField).Value;
                        if (caseId.HasValue && caseId.Value > 0)
                        {
                            var caseType = _caseTypeService.GetCaseType(caseId.Value);
                            performerId = caseType.User_Id;
                        }
                    }
                }
                field = GetField(performerId, cid, languageId,
                    CaseFieldsNamesApi.PerformerUserId, GlobalEnums.TranslationCaseFields.Performer_User_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            //customerUserSetting.PriorityPermission - if 0 - readonly, else 
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Priority_Id))
            {
                var priorityId = currentCase != null
                    ? currentCase.Priority_Id
                    : template?.Priority_Id ?? customerDefaults?.PriorityId;
                // Set working group from the product area working group if was not set before for New case only
                if (!priorityId.HasValue || priorityId.Value == 0)
                {
                    var productAreaField = model.Fields.FirstOrDefault(f =>
                        f.Name == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString());
                    if (productAreaField != null)
                    {
                        var productAreaId = ((BaseCaseField<int?>) productAreaField).Value;
                        if (productAreaId.HasValue && productAreaId.Value > 0)
                        {
                            var productArea = _productAreaService.GetProductArea(productAreaId.Value);
                            priorityId = productArea.Priority_Id;
                        }
                    }
                }

                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.PriorityId,
                    Value = priorityId,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Priority_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Priority_Id, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Priority_Id,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.PriorityPermission))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Status_Id))
            {
                var statusId = currentCase != null
                    ? currentCase.Status_Id
                    : template?.Status_Id ?? customerDefaults?.StatusId;
                field = GetField(statusId, cid, languageId,
                    CaseFieldsNamesApi.StatusId, GlobalEnums.TranslationCaseFields.Status_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.StateSecondaryId,
                    Value = currentCase != null ? currentCase.StateSecondary_Id : template?.StateSecondary_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.StateSecondary_Id, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.StateSecondaryPermission))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProject &&
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Project))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.Project,
                    Value = currentCase != null ? currentCase.Project_Id : template?.Project_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Project, languageId,
                        cid,
                        caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Project, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Project,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProblem &&
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Problem))
            {
                field = GetField(currentCase != null ? currentCase.Problem_Id : template?.Problem_Id, cid, languageId,
                    CaseFieldsNamesApi.Problem, GlobalEnums.TranslationCaseFields.Problem,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.CausingPart))
            {
                field = GetField(currentCase != null ? currentCase.CausingPartId : template?.CausingPartId, cid,
                    languageId,
                    CaseFieldsNamesApi.CausingPart, GlobalEnums.TranslationCaseFields.CausingPart,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Change)
            ) //TODO: && Model.changes.Any()
            {
                field = GetField(currentCase != null ? currentCase.Change_Id : template?.Change_Id, cid, languageId,
                    CaseFieldsNamesApi.Change, GlobalEnums.TranslationCaseFields.Change, CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.PlanDate))
            {
                field = GetField(
                    currentCase != null
                        ? currentCase.PlanDate.HasValue
                            ? DateTime.SpecifyKind(currentCase.PlanDate.Value, DateTimeKind.Utc)
                            : currentCase.PlanDate
                        : template?.PlanDate != null
                            ? DateTime.SpecifyKind(template.PlanDate.Value, DateTimeKind.Utc)
                            : new DateTime?(),
                    cid, languageId,
                    CaseFieldsNamesApi.PlanDate, GlobalEnums.TranslationCaseFields.PlanDate,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.WatchDate))
            {
                var watchDate = currentCase != null
                    ? currentCase.WatchDate.HasValue
                        ? DateTime.SpecifyKind(currentCase.WatchDate.Value, DateTimeKind.Utc)
                        : currentCase.WatchDate
                    : template?.WatchDate != null
                        ? DateTime.SpecifyKind(template.WatchDate.Value, DateTimeKind.Utc)
                        : new DateTime?();
                if (!watchDate.HasValue && template != null && template.Department_Id.HasValue &&
                    template.Priority_Id.HasValue)
                {
                    var dept = _departmentService.GetDepartment(template.Department_Id.Value);
                    var priority = _priorityService.GetPriority(template.Priority_Id.Value);
                    if (dept?.WatchDateCalendar_Id != null && priority != null && priority.IsActive.ToBool() &&
                        priority.SolutionTime == 0)
                    {
                        watchDate =
                            _watchDateCalendarService.GetClosestDateTo(dept.WatchDateCalendar_Id.Value,
                                DateTime.UtcNow);
                    }
                }

                field = new BaseCaseField<DateTime?>()
                {
                    Name = CaseFieldsNamesApi.WatchDate,
                    Value = watchDate,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.WatchDate,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.WatchDate, caseFieldSettings)
                };
                if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.WatchDate,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.WatchDatePermission))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.Verified))
            {
                field = GetField(currentCase?.Verified.ToBool() ?? template?.Verified.ToBool(), cid, languageId,
                    CaseFieldsNamesApi.Verified, GlobalEnums.TranslationCaseFields.Verified,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.VerifiedDescription))
            {
                field = GetField(currentCase != null ? currentCase.VerifiedDescription : template?.VerifiedDescription,
                    cid, languageId,
                    CaseFieldsNamesApi.VerifiedDescription, GlobalEnums.TranslationCaseFields.VerifiedDescription,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 200);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                GlobalEnums.TranslationCaseFields.SolutionRate))
            {
                field = GetField(currentCase != null ? currentCase.SolutionRate : template?.SolutionRate, cid,
                    languageId,
                    CaseFieldsNamesApi.SolutionRate, GlobalEnums.TranslationCaseFields.SolutionRate,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }
        }

        public void CreateCommunicationSection(int cid, UserOverview userOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, Case currentCase,
            CaseSolution template,
            int languageId, IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            IBaseCaseField field;
            if (userOverview.CloseCasePermission.ToBool())
            {
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.FinishingDescription))
                {
                    field = GetField(
                        currentCase != null ? currentCase.FinishingDescription : template?.FinishingDescription, cid,
                        languageId,
                        CaseFieldsNamesApi.FinishingDescription, GlobalEnums.TranslationCaseFields.FinishingDescription,
                        CaseSectionType.Communication,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                    AddMaxLengthOption(field.Options, 200);
                    model.Fields.Add(field);
                }

                int? finishingCause = null;
                var lastLog = currentCase?.Logs?.FirstOrDefault(); //todo: check if its correct - order
                if (lastLog != null)
                {
                    finishingCause = lastLog.FinishingType;
                }

                // Closing Reason
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.ClosingReason))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = CaseFieldsNamesApi.ClosingReason,
                        Value = finishingCause,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ClosingReason, languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.ClosingReason, caseFieldSettings)
                    };
                    if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.ClosingReason,
                        currentCase?.Id, caseTemplateSettings))
                        AddReadOnlyOption(field.Options);
                    model.Fields.Add(field);
                }

                // FinishingDate
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.FinishingDate))
                {
                    field = new BaseCaseField<DateTime?>()
                    {
                        Name = CaseFieldsNamesApi.FinishingDate,
                        Value = currentCase != null ? currentCase.FinishingDate : template?.FinishingDate,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.FinishingDate, languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.FinishingDate, caseFieldSettings)
                    };
                    if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.FinishingDate,
                        currentCase?.Id, caseTemplateSettings))
                        AddReadOnlyOption(field.Options);
                    model.Fields.Add(field);
                }

                // Log External 
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Text_External))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.Log_ExternalText,
                        Value = "",
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Text_External, languageId, cid, caseFieldTranslations),
                        Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Text_External, caseFieldSettings),
                        Section = CaseSectionType.Communication
                    };
                    if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Text_External,
                        currentCase?.Id, caseTemplateSettings))
                        AddReadOnlyOption(field.Options);
                    model.Fields.Add(field);
                }

                // Log Internal
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Text_Internal) &&
                    userOverview.CaseInternalLogPermission.ToBool())
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.Log_InternalText,
                        Value = template != null ? template.Text_Internal : "",
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, languageId, cid, caseFieldTranslations),
                        Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, caseFieldSettings),
                        Section = CaseSectionType.Communication
                    };
                    if ( _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal,
                        currentCase?.Id, caseTemplateSettings))
                        AddReadOnlyOption(field.Options);
                    model.Fields.Add(field);
                }
            }
        }

        private BaseCaseField<T> GetField<T>(T value, int cid, int languageId,
            string caseFieldNameApi,
            GlobalEnums.TranslationCaseFields translationCaseFieldName,
            CaseSectionType sectionName,
            IList<CaseFieldSetting> caseFieldSettings,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, 
            int? caseId, ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings)
        {
            var field = new BaseCaseField<T>()
            {
                Name = caseFieldNameApi,
                Value = value,
                Label = _caseTranslationService.GetFieldLabel(translationCaseFieldName, languageId,
                    cid, caseFieldTranslations),
                Section = sectionName,
                Options = GetBaseFieldOptions(translationCaseFieldName, caseFieldSettings)
            };

            if ( _caseFieldSettingsHelper.IsReadOnly(translationCaseFieldName,
                caseId, caseTemplateSettings))
                AddReadOnlyOption(field.Options);

            return field;
        }

        private IList<CaseFileModel> GetCaseFilesModel(int caseId)
        {
            return _caseFileService.GetCaseFiles(caseId, true);
        }

        private List<KeyValuePair<string, string>> GetBaseFieldOptions(GlobalEnums.TranslationCaseFields field,
                IList<CaseFieldSetting> caseFieldSettings)
        {
            var options = new List<KeyValuePair<string, string>>();
            var fieldName = field.ToString();

            var setting = _caseFieldSettingsHelper.GetCaseFieldSetting(caseFieldSettings, fieldName);
            if (setting != null && setting.Required.ToBool())
                AddRequiredOption(options);

            //if (settingEx != null && !string.IsNullOrWhiteSpace(settingEx.FieldHelp))
            //{
            //    options.Add(new KeyValuePair<string, string>("description", settingEx.FieldHelp));
            //}
            return options;
        }

        private void AddReadOnlyOption(ICollection<KeyValuePair<string, string>> options)
        {
            const string readonlyStr = "readonly";
            if (options.All(x => x.Key != readonlyStr))
                options.Add(new KeyValuePair<string, string>(readonlyStr, ""));
        }

        private void AddRequiredOption(ICollection<KeyValuePair<string, string>> options)
        {
            const string required = "required";
            if (options.All(x => x.Key != required))
                options.Add(new KeyValuePair<string, string>(required, ""));
        }

        private void AddMaxLengthOption(ICollection<KeyValuePair<string, string>> options, int length)
        {
            const string maxlength = "maxlength";
            if (options.All(x => x.Key != maxlength))
            options.Add(new KeyValuePair<string, string>(maxlength, length.ToString()));
        }
    }
}