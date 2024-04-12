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
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Constants.Case;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;

namespace DH.Helpdesk.WebApi.Logic.Case
{
    public interface ICaseFieldsCreator
    {
        void CreateInitiatorSection(int customerId, CustomerUser customerUserSetting,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CaseDefaultsInfo customerDefaults = null);

        void CreateRegardingSection(int customerId, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model);

        void CreateComputerInfoSection(int customerId, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model);

        void CreateCaseInfoSection(int customerId, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CaseDefaultsInfo customerDefaults = null);

        void CreateCaseManagementSection(int customerId, UserOverview currentUserOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CustomerSettings customerSettings,
            CaseDefaultsInfo customerDefaults = null);

        void CreateCommunicationSection(int customerId, UserOverview currentUserOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, Domain.Case currentCase,
            CaseSolution template,
            int languageId, IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerSettings customerSettings);
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
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICaseService _caseService;

        public CaseFieldsCreator(ICaseFileService caseFileService,
            ICaseFieldSettingsHelper caseFieldSettingsHelper,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            ISupplierService supplierService,
            ICaseTranslationService caseTranslationService,
            IDepartmentService departmentService,
            IPriorityService priorityService,
            IWatchDateCalendarService watchDateCalendarService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            ICaseExtraFollowersService caseExtraFollowersService,
            IStateSecondaryService stateSecondaryService,
            ICaseService caseService)
        {
            _caseExtraFollowersService = caseExtraFollowersService;
            _stateSecondaryService = stateSecondaryService;
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
            _caseService = caseService;
        }

        public void CreateInitiatorSection(int customerId, CustomerUser customerUserSetting,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
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
                //initiator category Id
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.UserSearchCategory_Id))
                {
                    var userSearchCategoryValue = GetUserSearchCategoryValue(template, caseFieldSettings, CaseSectionType.Initiator);
                    field = GetField(userSearchCategoryValue, customerId, languageId,
                        CaseFieldsNamesApi.UserSearchCategoryId,
                        GlobalEnums.TranslationCaseFields.UserSearchCategory_Id,
                        CaseSectionType.Initiator,
                        caseFieldSettings,
                        caseFieldTranslations,
                        currentCase?.Id,
                        caseTemplateSettings);
                    model.Fields.Add(field);
                }

                field = GetField(currentCase != null ? currentCase.ReportedBy : template?.ReportedBy, customerId,
                    languageId,
                    CaseFieldsNamesApi.ReportedBy, GlobalEnums.TranslationCaseFields.ReportedBy,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 40);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.PersonsName : template?.PersonsName, customerId,
                    languageId,
                    CaseFieldsNamesApi.PersonName, GlobalEnums.TranslationCaseFields.Persons_Name,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.PersonsEmail : template?.PersonsEmail, customerId,
                    languageId,
                    CaseFieldsNamesApi.PersonEmail, GlobalEnums.TranslationCaseFields.Persons_EMail,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 100);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.PersonsPhone : template?.PersonsPhone, customerId,
                    languageId,
                    CaseFieldsNamesApi.PersonPhone, GlobalEnums.TranslationCaseFields.Persons_Phone,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.PersonsCellphone : template?.PersonsCellPhone,
                    customerId, languageId,
                    CaseFieldsNamesApi.PersonCellPhone, GlobalEnums.TranslationCaseFields.Persons_CellPhone,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);

                var regionId = currentCase != null
                    ? currentCase.Region_Id
                    : template?.Region_Id ?? customerDefaults?.RegionId;
                field = GetField(regionId, customerId, languageId,
                    CaseFieldsNamesApi.RegionId, GlobalEnums.TranslationCaseFields.Region_Id,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);


                field = GetField(currentCase != null ? currentCase.Department_Id : template?.Department_Id, customerId,
                    languageId,
                    CaseFieldsNamesApi.DepartmentId, GlobalEnums.TranslationCaseFields.Department_Id,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.OU_Id : template?.OU_Id, customerId, languageId,
                    CaseFieldsNamesApi.OrganizationUnitId, GlobalEnums.TranslationCaseFields.OU_Id,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.CostCentre : template?.CostCentre, customerId,
                    languageId,
                    CaseFieldsNamesApi.CostCentre, GlobalEnums.TranslationCaseFields.CostCentre,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.Place : template?.Place, customerId, languageId,
                    CaseFieldsNamesApi.Place, GlobalEnums.TranslationCaseFields.Place, CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 100);
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.UserCode : template?.UserCode, customerId, languageId,
                    CaseFieldsNamesApi.UserCode, GlobalEnums.TranslationCaseFields.UserCode,
                    CaseSectionType.Initiator,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 50);
                model.Fields.Add(field);

                //field = new BaseCaseField<bool>()//TODO: for edit
                //{
                //    Name = CaseFieldsNamesApi.UpdateNotifierInformation.ToString(),
                //    Value = true,
                //    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, 
                //        languageId, customerId, caseFieldTranslations, ""),
                //    Section = CaseSectionType.Initiator,
                //    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, caseFieldSettings//        )
                //};
                //model.Fields.Add(field);
            }
        }

        public void CreateRegardingSection(int customerId, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {

            IBaseCaseField field;

            // Regarding
            //displayAboutUserInfoHtml:TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayAboutUserInfoHtml

            //regarding category Id
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id))
            {
                var userSearchCategoryValue = GetUserSearchCategoryValue(template, caseFieldSettings, CaseSectionType.Regarding);
                field = GetField(userSearchCategoryValue, customerId, languageId,
                    CaseFieldsNamesApi.IsAbout_UserSearchCategoryId,
                    GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id,
                    CaseSectionType.Regarding,
                    caseFieldSettings,
                    caseFieldTranslations,
                    currentCase?.Id,
                    caseTemplateSettings);
                model.Fields.Add(field);
            }

            field = GetField(currentCase != null ? currentCase.IsAbout?.ReportedBy : template?.IsAbout_ReportedBy,
                customerId, languageId,
                CaseFieldsNamesApi.IsAbout_ReportedBy, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 40);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.IsAbout?.Person_Name : template?.IsAbout_PersonsName,
                customerId, languageId,
                CaseFieldsNamesApi.IsAbout_PersonName, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 50);
            model.Fields.Add(field);

            field = GetField(
                currentCase != null ? currentCase.IsAbout?.Person_Email : template?.IsAbout_PersonsEmail, customerId,
                languageId,
                CaseFieldsNamesApi.IsAbout_PersonEmail, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 50);
            model.Fields.Add(field);

            field = GetField(
                currentCase != null ? currentCase.IsAbout?.Person_Phone : template?.IsAbout_PersonsPhone, customerId,
                languageId,
                CaseFieldsNamesApi.IsAbout_PersonPhone, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 40);
            model.Fields.Add(field);

            field = GetField(
                currentCase != null ? currentCase.IsAbout?.Person_Cellphone : template?.IsAbout_PersonsCellPhone,
                customerId, languageId,
                CaseFieldsNamesApi.IsAbout_PersonCellPhone,
                GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 30);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.IsAbout?.Region_Id : template?.IsAbout_Region_Id,
                customerId, languageId,
                CaseFieldsNamesApi.IsAbout_RegionId, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(
                currentCase != null ? currentCase.IsAbout?.Department_Id : template?.IsAbout_Department_Id, customerId,
                languageId,
                CaseFieldsNamesApi.IsAbout_DepartmentId, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.IsAbout?.OU_Id : template?.IsAbout_OU_Id, customerId,
                languageId,
                CaseFieldsNamesApi.IsAbout_OrganizationUnitId, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.IsAbout?.CostCentre : template?.IsAbout_CostCentre,
                customerId, languageId,
                CaseFieldsNamesApi.IsAbout_CostCentre, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 50);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.IsAbout?.Place : template?.IsAbout_Place, customerId,
                languageId,
                CaseFieldsNamesApi.IsAbout_Place, GlobalEnums.TranslationCaseFields.IsAbout_Place,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 100);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.IsAbout?.UserCode : template?.IsAbout_UserCode, customerId,
                languageId,
                CaseFieldsNamesApi.IsAbout_UserCode, GlobalEnums.TranslationCaseFields.IsAbout_UserCode,
                CaseSectionType.Regarding,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 50);
            model.Fields.Add(field);

        }

        public void CreateComputerInfoSection(int customerId, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            IBaseCaseField field;
            // ComputerInfo

            field = GetField(currentCase != null ? currentCase.InventoryNumber : template?.InventoryNumber, customerId,
                languageId,
                CaseFieldsNamesApi.InventoryNumber, GlobalEnums.TranslationCaseFields.InventoryNumber,
                CaseSectionType.ComputerInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 60);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.InventoryType : template?.InventoryType, customerId,
                languageId,
                CaseFieldsNamesApi.ComputerTypeId, GlobalEnums.TranslationCaseFields.ComputerType_Id,
                CaseSectionType.ComputerInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 50);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.InventoryLocation : template?.InventoryLocation, customerId,
                languageId,
                CaseFieldsNamesApi.InventoryLocation, GlobalEnums.TranslationCaseFields.InventoryLocation,
                CaseSectionType.ComputerInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 100);
            model.Fields.Add(field);

        }

        public void CreateCaseInfoSection(int customerId, IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CaseDefaultsInfo customerDefaults = null)
        {
            IBaseCaseField field;
            // CaseInfo
            //displayCaseInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions
            field = GetField(
                currentCase != null ? currentCase.CaseNumber.ToString(CultureInfo.InvariantCulture) : "", customerId,
                languageId,
                CaseFieldsNamesApi.CaseNumber, GlobalEnums.TranslationCaseFields.CaseNumber,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            if (_caseFieldSettingsHelper.IsCaseNew(currentCase?.Id))
                AddHiddenOption(field.Options);
            model.Fields.Add(field);


            field = GetField(
                currentCase != null ? DateTime.SpecifyKind(currentCase.RegTime, DateTimeKind.Utc) : DateTime.UtcNow,
                customerId, languageId,
                CaseFieldsNamesApi.RegTime, GlobalEnums.TranslationCaseFields.RegTime, CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            if (_caseFieldSettingsHelper.IsCaseNew(currentCase?.Id))
                AddHiddenOption(field.Options);
            model.Fields.Add(field);

            field = GetField(
                currentCase != null
                    ? DateTime.SpecifyKind(currentCase.ChangeTime, DateTimeKind.Utc)
                    : new DateTime?(), customerId, languageId,
                CaseFieldsNamesApi.ChangeTime, GlobalEnums.TranslationCaseFields.ChangeTime,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            if (_caseFieldSettingsHelper.IsCaseNew(currentCase?.Id))
                AddHiddenOption(field.Options);
            model.Fields.Add(field);

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

                field = GetField(userIdValue, customerId, languageId,
                    CaseFieldsNamesApi.UserId, GlobalEnums.TranslationCaseFields.User_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                if (_caseFieldSettingsHelper.IsCaseNew(currentCase?.Id))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            field = GetField(currentCase != null ? currentCase.RegistrationSourceCustomer_Id : template?.RegistrationSource, customerId,
                languageId,
                CaseFieldsNamesApi.RegistrationSourceCustomer,
                GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer, CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            var caseTypeId = currentCase != null
                ? currentCase.CaseType_Id
                : template?.CaseType_Id ?? customerDefaults?.CaseTypeId;
            field = GetField(caseTypeId, customerId, languageId,
                CaseFieldsNamesApi.CaseTypeId, GlobalEnums.TranslationCaseFields.CaseType_Id,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.ProductArea_Id : template?.ProductArea_Id, customerId,
                languageId,
                CaseFieldsNamesApi.ProductAreaId, GlobalEnums.TranslationCaseFields.ProductArea_Id,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.System_Id : template?.System_Id, customerId, languageId,
                CaseFieldsNamesApi.SystemId, GlobalEnums.TranslationCaseFields.System_Id, CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.Urgency_Id : template?.Urgency_Id, customerId, languageId,
                CaseFieldsNamesApi.UrgencyId, GlobalEnums.TranslationCaseFields.Urgency_Id,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.Impact_Id : template?.Impact_Id, customerId, languageId,
                CaseFieldsNamesApi.ImpactId, GlobalEnums.TranslationCaseFields.Impact_Id, CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.Category_Id : template?.Category_Id, customerId, languageId,
                CaseFieldsNamesApi.CategoryId, GlobalEnums.TranslationCaseFields.Category_Id,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            {
                var supplierId = currentCase != null
                    ? currentCase.Supplier_Id
                    : template?.Supplier_Id ?? customerDefaults?.SupplierId;
                field = GetField(supplierId, customerId, languageId,
                    CaseFieldsNamesApi.SupplierId, GlobalEnums.TranslationCaseFields.Supplier_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);

                var supplier = supplierId.HasValue
                    ? _supplierService.GetSupplier(supplierId.Value)
                    : null;
                field = GetField(supplier?.Country_Id, customerId, languageId,
                    CaseFieldsNamesApi.SupplierCountryId, GlobalEnums.TranslationCaseFields.Supplier_Id,
                    CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            field = GetField(currentCase != null ? currentCase.InvoiceNumber : template?.InvoiceNumber, customerId,
                languageId,
                CaseFieldsNamesApi.InvoiceNumber, GlobalEnums.TranslationCaseFields.InvoiceNumber,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 50);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.ReferenceNumber : template?.ReferenceNumber, customerId,
                languageId,
                CaseFieldsNamesApi.ReferenceNumber, GlobalEnums.TranslationCaseFields.ReferenceNumber,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 50);
            model.Fields.Add(field);

            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Caption,
                    Value = currentCase != null ? currentCase.Caption : template?.Caption,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Caption,
                        languageId, customerId, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Caption, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Caption,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.CaptionPermission))
                    AddReadOnlyOption(field.Options);
                AddMaxLengthOption(field.Options, 100);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Caption))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            {
                var registrationSource = currentCase != null
                    ? currentCase.RegistrationSource
                    : template?.RegistrationSource;
                var registrationSourceOptions = (currentCase != null &&
                                                 registrationSource == (int)CaseRegistrationSource.Email)
                    ? currentCase.Mail2Tickets.Where(x => x.Log_Id == null)
                        .GroupBy(x => x.Type)
                        .Select(gr =>
                            new KeyValuePair<string, string>(gr.Key, string.Join(";", gr.Select(x => x.EMailAddress))))
                        .ToList()
                    : new List<KeyValuePair<string, string>>();

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
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Description,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                model.Fields.Add(field);
            }

            field = GetField(currentCase != null ? currentCase.Description : template?.Description, customerId, languageId,
                CaseFieldsNamesApi.Description, GlobalEnums.TranslationCaseFields.Description,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Description,
                currentCase?.Id, caseTemplateSettings))
                AddReadOnlyOption(field.Options);
            //AddMaxLengthOption(field.Options, 10000);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.Miscellaneous : template?.Miscellaneous, customerId,
                languageId,
                CaseFieldsNamesApi.Miscellaneous, GlobalEnums.TranslationCaseFields.Miscellaneous,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 1000);
            model.Fields.Add(field);

            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.ContactBeforeAction,
                    Value =
                        currentCase?.ContactBeforeAction.ToBool() ?? template?.ContactBeforeAction.ToBool() ?? false,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ContactBeforeAction,
                        languageId, customerId, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.ContactBeforeAction, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.ContactBeforeAction,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.ContactBeforeActionPermission))
                    AddReadOnlyOption(field.Options);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.ContactBeforeAction))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            field = GetField(currentCase?.SMS.ToBool() ?? template?.SMS.ToBool() ?? false, customerId, languageId,
                CaseFieldsNamesApi.Sms, GlobalEnums.TranslationCaseFields.SMS, CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.AgreedDate : template.AgreedDate, customerId, languageId,
                CaseFieldsNamesApi.AgreedDate, GlobalEnums.TranslationCaseFields.AgreedDate,
                CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

            field = GetField(currentCase != null ? currentCase.Available : template?.Available, customerId, languageId,
                CaseFieldsNamesApi.Available, GlobalEnums.TranslationCaseFields.Available, CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            AddMaxLengthOption(field.Options, 100);
            model.Fields.Add(field);

            {
                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost,
                    Value = currentCase?.Cost ?? template?.Cost ?? 0,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Artikelkostnad"), //Kostnad
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Cost,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                AddMaxLengthOption(field.Options, 7);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Cost))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);

                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost_OtherCost,
                    Value = currentCase?.OtherCost ?? template?.OtherCost ?? 0,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Övrig kostnad"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Cost,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                AddMaxLengthOption(field.Options, 7);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Cost))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Cost_Currency,
                    Value = currentCase != null ? currentCase.Currency : template?.Currency ?? "",
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Valuta"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Cost,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Cost))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            field = GetField(currentCase != null ? GetCaseFilesModel(currentCase.Id) : null, customerId, languageId,
                CaseFieldsNamesApi.Filename, GlobalEnums.TranslationCaseFields.Filename, CaseSectionType.CaseInfo,
                caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
            model.Fields.Add(field);

        }

        public void CreateCaseManagementSection(int customerId, UserOverview currentUserOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings,
            Domain.Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerUser customerUserSetting,
            CustomerSettings customerSettings,
            CaseDefaultsInfo customerDefaults = null)
        {
            IBaseCaseField field;
            //CaseManagement
            {
                var workingGroupId = currentCase != null ? currentCase.WorkingGroup_Id : template?.CaseWorkingGroup_Id;
                if (template != null)
                {
                    var userDefaultWg = _userService.GetUserDefaultWorkingGroupId(currentUserOverview.Id, customerId);
                    if (template.SetCurrentUsersWorkingGroup.ToBool())
                        workingGroupId = userDefaultWg;
                    if (!workingGroupId.HasValue)
                    {
                        if (userDefaultWg.HasValue)
                            workingGroupId = userDefaultWg;
                        else
                        {
                            var defaultWg = _workingGroupService.GetDefaultCreateCaseWorkingGroup(customerId);
                            if (defaultWg != null)
                                workingGroupId = defaultWg.Id;
                        }
                    }

                }

                // Set working group from the case type working group if was not set before
                if (currentCase == null && (!workingGroupId.HasValue || workingGroupId.Value == 0))
                {
                    var caseTypeField = model.Fields.FirstOrDefault(f =>
                        f.Name.Equals(CaseFieldsNamesApi.CaseTypeId.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    if (caseTypeField != null)
                    {
                        var caseId = ((BaseCaseField<int?>)caseTypeField).Value;
                        if (caseId.HasValue && caseId > 0)
                        {
                            var caseType = _caseTypeService.GetCaseType(caseId.Value);
                            workingGroupId = caseType.WorkingGroup_Id;
                        }
                    }
                }

                // Set working group from the product area working group if was not set before
                if (currentCase == null && (!workingGroupId.HasValue || workingGroupId.Value == 0))
                {
                    var productAreaField = model.Fields.FirstOrDefault(f =>
                        f.Name.Equals(CaseFieldsNamesApi.ProductAreaId.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    if (productAreaField != null)
                    {
                        var productAreaId = ((BaseCaseField<int?>)productAreaField).Value;
                        if (productAreaId.HasValue && productAreaId.Value > 0)
                        {
                            var productArea = _productAreaService.GetProductArea(productAreaId.Value);
                            workingGroupId = productArea.WorkingGroup_Id;
                        }
                    }
                }

                field = GetField(workingGroupId, customerId, languageId,
                    CaseFieldsNamesApi.WorkingGroupId, GlobalEnums.TranslationCaseFields.WorkingGroup_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            {
                var caseResponsibleUserId = currentCase != null
                    ? currentCase.CaseResponsibleUser_Id
                    : currentUserOverview.Id;
                field = GetField(caseResponsibleUserId, customerId, languageId,
                    CaseFieldsNamesApi.CaseResponsibleUserId, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            {
                int? performerId;
                if (currentCase == null && template == null)
                    performerId = customerSettings.SetUserToAdministrator
                        ? currentUserOverview.Id
                        : customerSettings.DefaultAdministratorId;
                else
                {
                    performerId = currentCase != null ? currentCase.Performer_User_Id : template?.PerformerUser_Id;
                    if (template != null && template.SetCurrentUserAsPerformer.ToBool())
                        performerId = currentUserOverview.Id; // current user
                }

                // Set perfomer from the case type perfomer if was not set before
                if (currentCase == null && (!performerId.HasValue || performerId.Value == 0))
                {
                    var caseTypeField = model.Fields.FirstOrDefault(f =>
                        f.Name.Equals(CaseFieldsNamesApi.CaseTypeId.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    if (caseTypeField != null)
                    {
                        var caseId = ((BaseCaseField<int?>)caseTypeField).Value;
                        if (caseId.HasValue && caseId.Value > 0)
                        {
                            var caseType = _caseTypeService.GetCaseType(caseId.Value);
                            performerId = caseType.User_Id;
                        }
                    }
                }
                field = GetField(performerId, customerId, languageId,
                    CaseFieldsNamesApi.PerformerUserId, GlobalEnums.TranslationCaseFields.Performer_User_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            {
                var priorityId = currentCase != null
                    ? currentCase.Priority_Id
                    : template?.Priority_Id ?? customerDefaults?.PriorityId;
                // Set working group from the product area working group if was not set before
                if (currentCase == null && (!priorityId.HasValue || priorityId.Value == 0))
                {
                    var productAreaField = model.Fields.FirstOrDefault(f =>
                        f.Name.Equals(CaseFieldsNamesApi.ProductAreaId.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    if (productAreaField != null)
                    {
                        var productAreaId = ((BaseCaseField<int?>)productAreaField).Value;
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
                        languageId, customerId, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Priority_Id, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Priority_Id,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.PriorityPermission))
                    AddReadOnlyOption(field.Options);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Priority_Id))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            {
                var statusId = currentCase != null
                    ? currentCase.Status_Id
                    : template?.Status_Id ?? customerDefaults?.StatusId;
                field = GetField(statusId, customerId, languageId,
                    CaseFieldsNamesApi.StatusId, GlobalEnums.TranslationCaseFields.Status_Id,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.StateSecondaryId,
                    Value = currentCase != null ? currentCase.StateSecondary_Id : template?.StateSecondary_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                        languageId, customerId, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.StateSecondary_Id, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.StateSecondaryPermission))
                    AddReadOnlyOption(field.Options);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.StateSecondary_Id))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProject)
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.Project,
                    Value = currentCase != null ? currentCase.Project_Id : template?.Project_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Project, languageId,
                        customerId,
                        caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.Project, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Project,
                    currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Project))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProblem)
            {
                field = GetField(currentCase != null ? currentCase.Problem_Id : template?.Problem_Id, customerId, languageId,
                    CaseFieldsNamesApi.Problem, GlobalEnums.TranslationCaseFields.Problem,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.Problem))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            {
                field = GetField(currentCase != null ? currentCase.CausingPartId : template?.CausingPartId, customerId,
                    languageId,
                    CaseFieldsNamesApi.CausingPart, GlobalEnums.TranslationCaseFields.CausingPart,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            //TODO: && Model.changes.Any()
            {
                field = GetField(currentCase != null ? currentCase.Change_Id : template?.Change_Id, customerId, languageId,
                    CaseFieldsNamesApi.Change, GlobalEnums.TranslationCaseFields.Change, CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            {
                field = GetField(
                    currentCase != null
                        ? currentCase.PlanDate.HasValue
                            ? DateTime.SpecifyKind(currentCase.PlanDate.Value, DateTimeKind.Utc)
                            : currentCase.PlanDate
                        : template?.PlanDate != null
                            ? DateTime.SpecifyKind(template.PlanDate.Value, DateTimeKind.Utc)
                            : new DateTime?(),
                    customerId, languageId,
                    CaseFieldsNamesApi.PlanDate, GlobalEnums.TranslationCaseFields.PlanDate,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            {
                var watchDate = currentCase != null
                    ? currentCase.WatchDate.HasValue
                        ? DateTime.SpecifyKind(currentCase.WatchDate.Value, DateTimeKind.Utc)
                        : currentCase.WatchDate
                    : template?.WatchDate != null
                        ? DateTime.SpecifyKind(template.WatchDate.Value, DateTimeKind.Utc)
                        : new DateTime?();
                if (!watchDate.HasValue && template?.Department_Id != null && template.Priority_Id.HasValue)
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
                        languageId, customerId, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.WatchDate, caseFieldSettings)
                };
                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.WatchDate,
                    currentCase?.Id, caseTemplateSettings,
                    customerUserSetting.WatchDatePermission))
                    AddReadOnlyOption(field.Options);
                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                    GlobalEnums.TranslationCaseFields.WatchDate))
                    AddHiddenOption(field.Options);
                model.Fields.Add(field);
            }

            {
                field = GetField(currentCase?.Verified.ToBool() ?? template?.Verified.ToBool(), customerId, languageId,
                    CaseFieldsNamesApi.Verified, GlobalEnums.TranslationCaseFields.Verified,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }

            {
                field = GetField(currentCase != null ? currentCase.VerifiedDescription : template?.VerifiedDescription,
                    customerId, languageId,
                    CaseFieldsNamesApi.VerifiedDescription, GlobalEnums.TranslationCaseFields.VerifiedDescription,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                AddMaxLengthOption(field.Options, 200);
                model.Fields.Add(field);
            }

            {
                field = GetField(currentCase != null ? currentCase.SolutionRate : template?.SolutionRate, customerId,
                    languageId,
                    CaseFieldsNamesApi.SolutionRate, GlobalEnums.TranslationCaseFields.SolutionRate,
                    CaseSectionType.CaseManagement,
                    caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);
                model.Fields.Add(field);
            }
        }

        public void CreateCommunicationSection(int customerId, UserOverview userOverview,
            IList<CaseFieldSetting> caseFieldSettings,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, Domain.Case currentCase,
            CaseSolution template,
            int languageId, IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model,
            CustomerSettings customerSettings)
        {
            IBaseCaseField field;
            var noMailToNotifier = false;

            //New from BusinessRules  - Disable closure if BR sais so
            bool dontShowClosingFields = false;
            if (currentCase != null)
            {
                List<string> disableCaseFields = new List<string>();
                (disableCaseFields, dontShowClosingFields) = _caseService.ExecuteBusinessActionsDisable(currentCase);
            }

            if (userOverview.CloseCasePermission.ToBool() && !dontShowClosingFields)
            {
                {
                    field = GetField(
                        currentCase != null ? currentCase.FinishingDescription : template?.FinishingDescription,
                        customerId,
                        languageId,
                        CaseFieldsNamesApi.FinishingDescription, GlobalEnums.TranslationCaseFields.FinishingDescription,
                        CaseSectionType.Communication,
                        caseFieldSettings, caseFieldTranslations, currentCase?.Id, caseTemplateSettings);

                    AddMaxLengthOption(field.Options, 200);

                    model.Fields.Add(field);
                }

                // Closing Reason 
                {
                    int? finishingCause = null;
                    var lastLog = currentCase?.Logs?.OrderByDescending(l => l.ChangeTime).FirstOrDefault();
                    if (lastLog != null)
                        finishingCause = lastLog.FinishingType;

                    field = new BaseCaseField<int?>()
                    {
                        Name = CaseFieldsNamesApi.ClosingReason,
                        Value = finishingCause,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ClosingReason,
                            languageId, customerId, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.ClosingReason,
                            caseFieldSettings)
                    };

                    if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.ClosingReason,
                        currentCase?.Id, caseTemplateSettings))
                        AddReadOnlyOption(field.Options);

                    if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                        GlobalEnums.TranslationCaseFields.ClosingReason))
                        AddHiddenOption(field.Options);

                    model.Fields.Add(field);
                }

                // FinishingDate
                {
                    field = new BaseCaseField<DateTime?>()
                    {
                        Name = CaseFieldsNamesApi.FinishingDate,
                        Value = currentCase != null
                            ? currentCase.FinishingDate
                            : template?.FinishingDate ??
                              (template?.FinishingCause_Id != null ? DateTime.UtcNow : new DateTime?()),
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.FinishingDate,
                            languageId, customerId, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.FinishingDate,
                            caseFieldSettings)
                    };

                    if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.FinishingDate,
                            currentCase?.Id, caseTemplateSettings) ||
                        customerSettings.DisableCaseEndDate)
                        AddReadOnlyOption(field.Options);

                    if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings,
                        GlobalEnums.TranslationCaseFields.FinishingDate))
                        AddHiddenOption(field.Options);

                    model.Fields.Add(field);
                }

                var stateSecondaryField = model.Fields.FirstOrDefault(f =>
                    f.Name.Equals(CaseFieldsNamesApi.StateSecondaryId.ToString(),
                        StringComparison.InvariantCultureIgnoreCase));
                if (stateSecondaryField != null)
                {
                    var stateSecondaryId = ((BaseCaseField<int?>)stateSecondaryField).Value;
                    if (stateSecondaryId.HasValue)
                    {
                        var stateSecondary = _stateSecondaryService.GetStateSecondary(stateSecondaryId.Value);
                        noMailToNotifier = stateSecondary.NoMailToNotifier.ToBool();
                    }
                }
            }

            // Log External Emails to (Extra Followers)
            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.Log_SendMailToNotifier,
                    Value = true,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Till"),
                    Section = CaseSectionType.Communication
                };

                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Text_External, currentCase?.Id, caseTemplateSettings)
                || noMailToNotifier)
                    AddReadOnlyOption(field.Options);

                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Text_External))
                    AddHiddenOption(field.Options);

                model.Fields.Add(field);
            }

            // Log External Emails cc (Extra Followers)
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Log_ExternalEmailsCC,
                    Value = currentCase != null ? GetExtraFollowersEmails(currentCase.Id) : "",
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Kopia"),
                    Section = CaseSectionType.Communication
                };

                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Text_External, currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);

                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Text_External))
                    AddHiddenOption(field.Options);

                model.Fields.Add(field);
            }

            // Log External
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Log_ExternalText,
                    Value = template?.Text_External ?? "",
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Text_External, languageId, customerId, caseFieldTranslations),
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Text_External, caseFieldSettings),
                    Section = CaseSectionType.Communication
                };

                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Text_External, currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);

                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Text_External))
                    AddHiddenOption(field.Options);

                //AddMaxLengthOption(field.Options, 3000);
                model.Fields.Add(field);
            }

            // Log Internal Emails to (Performer)
            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.Log_SendMailToPerformer,
                    Value = true,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Till"),
                    Section = CaseSectionType.Communication
                };

                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);

                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Text_Internal))
                    AddHiddenOption(field.Options);

                model.Fields.Add(field);
            }

            // Log Internal
            if (userOverview.CaseInternalLogPermission.ToBool())
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Log_InternalText,
                    Value = template?.Text_Internal ?? "",
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, languageId, customerId, caseFieldTranslations),
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, caseFieldSettings),
                    Section = CaseSectionType.Communication
                };

                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);

                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Text_Internal))
                    AddHiddenOption(field.Options);

                //AddMaxLengthOption(field.Options, 3000);
                model.Fields.Add(field);
            }

            // Log File
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Log_Filename,
                    Value = "",
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Filename, languageId, customerId, caseFieldTranslations),
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Filename, caseFieldSettings),
                    Section = CaseSectionType.Communication
                };

                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Filename, currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);

                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Filename))
                    AddHiddenOption(field.Options);

                model.Fields.Add(field);
            }

            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Log_Filename_Internal,
                    Value = "",
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal, languageId, customerId, caseFieldTranslations),
                    Options = GetBaseFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal, caseFieldSettings),
                    Section = CaseSectionType.Communication
                };

                if (_caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal, currentCase?.Id, caseTemplateSettings))
                    AddReadOnlyOption(field.Options);

                if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal))
                    AddHiddenOption(field.Options);

                model.Fields.Add(field);
            }
        }

        private int? GetUserSearchCategoryValue(CaseSolution template, IList<CaseFieldSetting> fieldSettings, CaseSectionType sectionType)
        {
            var fieldName = sectionType == CaseSectionType.Regarding
                ? GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id.ToString()
                : GlobalEnums.TranslationCaseFields.UserSearchCategory_Id.ToString();

            //check template first
            if (template != null)
            {
                if (sectionType == CaseSectionType.Initiator && template.UserSearchCategory_Id.HasValue)
                {
                    return template.UserSearchCategory_Id;
                }

                if (sectionType == CaseSectionType.Regarding && template.IsAbout_UserSearchCategory_Id.HasValue)
                {
                    return template.IsAbout_UserSearchCategory_Id;
                }
            }

            //find default categories if any - default is empy category (0 - users without categories)
            var categoryFieldSettings = _caseFieldSettingsHelper.GetCaseFieldSetting(fieldSettings, fieldName);

            var defaultCategoryId = ComputerUserCategory.EmptyCategoryId;
            if (Int32.TryParse(categoryFieldSettings.DefaultValue, out defaultCategoryId))
            {
                return defaultCategoryId;
            }

            return null;
        }

        private string GetExtraFollowersEmails(int caseId)
        {
            var emails = _caseExtraFollowersService.GetCaseExtraFollowers(caseId).Select(x => x.Follower).ToList();
            return emails.JoinToString(";");
        }

        private BaseCaseField<T> GetField<T>(T value, int customerId, int languageId,
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
                Label = _caseTranslationService.GetFieldLabel(translationCaseFieldName, languageId, customerId, caseFieldTranslations),
                Section = sectionName,
                Options = GetBaseFieldOptions(translationCaseFieldName, caseFieldSettings)
            };

            if (_caseFieldSettingsHelper.IsReadOnly(translationCaseFieldName, caseId, caseTemplateSettings))
                AddReadOnlyOption(field.Options);

            if (!_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, translationCaseFieldName))
                AddHiddenOption(field.Options);

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

        private void AddHiddenOption(List<KeyValuePair<string, string>> options)
        {
            const string hidden = "hidden";
            if (options.All(x => x.Key != hidden))
                options.Add(new KeyValuePair<string, string>(hidden, ""));
        }
    }
}