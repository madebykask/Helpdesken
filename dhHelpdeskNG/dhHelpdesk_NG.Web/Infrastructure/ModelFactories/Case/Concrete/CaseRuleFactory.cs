using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Models.CaseRules;
using System.Collections.Generic;
using System.Linq;
using static DH.Helpdesk.BusinessData.OldComponents.GlobalEnums;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Common.Extensions.Integer;
using System.Threading;
using DH.Helpdesk.Web.Models;
using DH.Helpdesk.Common.Enums.Settings;
using DH.Helpdesk.Services.Services;
using System;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete
{
    public interface ICaseRuleFactory
    {
        void Initialize(IRegionService regionService,
                    IDepartmentService departmentService,
                    IOUService ouService,
                    IRegistrationSourceCustomerService registrationSourceCustomerService,
                    ICaseTypeService caseTypeService,
                    IProductAreaService productAreaService,
                    ISystemService systemService,
                    IUrgencyService urgencyService,
                    IImpactService impactService,
                    ICategoryService categoryService,
                    ISupplierService supplierService,
                    IWorkingGroupService workingGroupService,
                    IUserService userService,
                    IPriorityService priorityService,
                    IStatusService statusService,
                    IStateSecondaryService stateSecondaryService,
                    IProjectService projectService,
                    IProblemService problemService,
                    ICausingPartService causingPartService,
                    IChangeService changeService,
                    IFinishingCauseService finishingCauseService,
                    IWatchDateCalendarService watchDateCalendarService,
                    IComputerService computerService);

        CaseRuleModel GetCaseRuleModel(int customerId,
                                        CaseRuleMode mode,                                        
                                        CaseCurrentDataModel currentData,
                                        Setting customerSettings,
                                        List<CaseFieldSetting> caseFieldSettings,
                                        List<CaseSolutionSettingModel> templateSettingModel);
    }


    public class CaseRuleFactory: ICaseRuleFactory
    {
        #region Services

        private IRegionService _regionService;
        private IDepartmentService _departmentService;
        private IOUService _ouService;
        private IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private ICaseTypeService _caseTypeService;
        private IProductAreaService _productAreaService;
        private ISystemService _systemService;
        private IUrgencyService _urgencyService;
        private IImpactService _impactService;
        private ICategoryService _categoryService;
        private ISupplierService _supplierService;
        private IWorkingGroupService _workingGroupService;
        private IUserService _userService;
        private IPriorityService _priorityService;
        private IStatusService _statusService;
        private IStateSecondaryService _stateSecondaryService;
        private IProjectService _projectService;
        private IProblemService _problemService;
        private ICausingPartService _causingPartService;
        private IChangeService _changeService;
        private IFinishingCauseService _finishingCauseService;
        private IWatchDateCalendarService _watchDateCalendarService;
        private IComputerService _computerService;
        #endregion

        private bool initiated;
        private const string CURRENT_USER_ITEM_CAPTION = "Inloggad användare";
        private const string CURRENT_USER_WORKINGGROUP_CAPTION = "Inloggad användares driftgrupp";

        public CaseRuleFactory()
        {
            initiated = false;
        }

        public void Initialize(IRegionService regionService,
                    IDepartmentService departmentService,
                    IOUService ouService,
                    IRegistrationSourceCustomerService registrationSourceCustomerService,
                    ICaseTypeService caseTypeService,
                    IProductAreaService productAreaService,
                    ISystemService systemService,
                    IUrgencyService urgencyService,
                    IImpactService impactService,
                    ICategoryService categoryService,
                    ISupplierService supplierService,
                    IWorkingGroupService workingGroupService,
                    IUserService userService,
                    IPriorityService priorityService,
                    IStatusService statusService,
                    IStateSecondaryService stateSecondaryService,
                    IProjectService projectService,
                    IProblemService problemService,
                    ICausingPartService causingPartService,
                    IChangeService changeService,
                    IFinishingCauseService finishingCauseService,
                    IWatchDateCalendarService watchDateCalendarService,
                    IComputerService computerService)
        {
            _regionService = regionService;
            _departmentService = departmentService;
            _ouService = ouService;
            _registrationSourceCustomerService = registrationSourceCustomerService;
            _caseTypeService = caseTypeService;
            _productAreaService = productAreaService;
            _systemService = systemService;
            _urgencyService = urgencyService;
            _impactService = impactService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _workingGroupService = workingGroupService;
            _userService = userService;
            _priorityService = priorityService;
            _statusService = statusService;
            _stateSecondaryService = stateSecondaryService;
            _projectService = projectService;
            _problemService = problemService;
            _causingPartService = causingPartService;
            _changeService = changeService;
            _finishingCauseService = finishingCauseService;
            _watchDateCalendarService = watchDateCalendarService;
            _computerService = computerService;

            initiated = true;
        }        

        public CaseRuleModel GetCaseRuleModel(int customerId, 
                                              CaseRuleMode mode,                                              
                                              CaseCurrentDataModel currentData,
                                              Setting customerSettings,
                                              List<CaseFieldSetting> caseFieldSettings,
                                              List<CaseSolutionSettingModel> templateSettingModel)
        {                        
            var ret = new CaseRuleModel();
            if (!initiated)
                return ret;

            ret.RuleMode = mode;
            ret.DateFormat = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;

            var basicInformation = PrepareRuleInformation(customerId, mode, currentData, customerSettings, caseFieldSettings, templateSettingModel);
            ret.FieldAttributes = GetGlobalRules(customerId, caseFieldSettings.ToList(),
                                                   basicInformation, customerSettings, mode);           
            return ret;
        }
                
        private BasicCaseInformation PrepareRuleInformation(int customerId,
                                                            CaseRuleMode ruleMode,                                                            
                                                            CaseCurrentDataModel currentData,
                                                            Setting customerSettings,
                                                            List<CaseFieldSetting> caseFieldSettings, 
                                                            List<CaseSolutionSettingModel> templateSettingModel)
        {
            var caseBasicInfo = new BasicCaseInformation();

            #region Initiator

            var userSearchCategories = _computerService.GetComputerUserCategoriesByCustomerID(customerId);

            caseBasicInfo.UserSearchCategory = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.UserSearchCategory_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.UserSearchCategory_Id, caseFieldSettings, CaseSolutionFields.UserSearchCategory_Id, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = userSearchCategories.Where(x => !x.IsEmpty || (currentData.UserSearchCategory_Id.HasValue && currentData.UserSearchCategory_Id.Value == x.Id)).Select(x => new FieldItem(x.Id.ToString(), Translation.GetMasterDataTranslation(x.Name))).OrderBy(r => r.ItemText).ToList()
            };

            caseBasicInfo.ReportedBy = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.ReportedBy, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.ReportedBy, caseFieldSettings, CaseSolutionFields.UserNumber, templateSettingModel.ToList())
            };

            caseBasicInfo.PersonsName = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.PersonsName, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Persons_Phone, caseFieldSettings, CaseSolutionFields.PersonsName, templateSettingModel.ToList())
            };

            caseBasicInfo.PersonsEmail = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.PersonsEmail, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Persons_EMail, caseFieldSettings, CaseSolutionFields.PersonsEmail, templateSettingModel.ToList())
            };

            caseBasicInfo.MailToNotifier = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.NoMailToNotifier.ToBool().ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.MailToNotifier, caseFieldSettings, CaseSolutionFields.Email, templateSettingModel.ToList())
            };

            caseBasicInfo.PersonsPhone = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.PersonsPhone, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Persons_Phone, caseFieldSettings, CaseSolutionFields.PersonsPhone, templateSettingModel.ToList())
            };

            caseBasicInfo.PersonsCellPhone = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.PersonsCellPhone, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Persons_CellPhone, caseFieldSettings, CaseSolutionFields.PersonsCellPhone, templateSettingModel.ToList())
            };

            caseBasicInfo.CostCentre = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.CostCentre, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.CostCentre, caseFieldSettings, CaseSolutionFields.CostCentre, templateSettingModel.ToList())
            };

            caseBasicInfo.Place = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Place, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Place, caseFieldSettings, CaseSolutionFields.Place, templateSettingModel.ToList())
            };

            caseBasicInfo.UserCode = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.UserCode, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.UserCode, caseFieldSettings, CaseSolutionFields.Usercode, templateSettingModel.ToList())
            };

            caseBasicInfo.UpdateUserInfo = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.UpdateNotifierInformation.ToBool().ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.UpdateNotifierInformation, caseFieldSettings, CaseSolutionFields.UpdateNotifierInformation, templateSettingModel.ToList())
            };

            caseBasicInfo.AddFollowersBtn = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.AddFollowersBtn.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.AddFollowersBtn, caseFieldSettings, CaseSolutionFields.AddFollowersBtn, templateSettingModel.ToList())
            };

            var regions = _regionService.GetRegions(customerId);
            var defaultRegion = regions.FirstOrDefault(r => r.IsDefault != 0 && r.IsActive != 0);

            caseBasicInfo.Regions = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Region_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Region_Id, caseFieldSettings, CaseSolutionFields.Region, templateSettingModel.ToList()),
                DefaultItem = defaultRegion != null ? new FieldItem(defaultRegion.Id.ToString(), defaultRegion.Name) : FieldItem.CreateEmpty(),
                Items = regions.Where(r => r.IsActive != 0 || (currentData.Region_Id.HasValue && currentData.Region_Id.Value == r.Id))
                               .Select(r => new FieldItem(r.Id.ToString(), r.Name, r.IsActive != 0)).OrderBy(r => r.ItemText).ToList()
            };

            var departments = _departmentService.GetDepartments(customerId);
            caseBasicInfo.Departments = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Department_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Department_Id, caseFieldSettings, CaseSolutionFields.Department, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = departments.Where(d => d.IsActive != 0 || (currentData.Department_Id.HasValue && currentData.Department_Id.Value == d.Id))
                                   .Select(d => new FieldItem(d.Id.ToString(), d.DepartmentName, d.IsActive != 0)
                                   { ForeignKeyValue1 = d.Region_Id?.ToString() })
                                   .OrderBy(d => d.ItemText).ToList()
            };

            var ous = _ouService.GetThemAllOUs(customerId);
            caseBasicInfo.OUs = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.OU_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.OU_Id, caseFieldSettings, CaseSolutionFields.OU, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = ous.Where(o => o.Id != o.Parent_OU_Id && (o.IsActive != 0 || (currentData.OU_Id.HasValue && currentData.OU_Id.Value == o.Id)) &&
                                       (o.Parent == null || (o.Parent != null && o.Parent.Parent == null))) // Max deep 2 levels
                           .Select(o => new FieldItem(o.Id.ToString(), (o.Parent == null ? o.Name : string.Format("{0} - {1}", o.Parent.Name, o.Name)), o.IsActive != 0)
                           { ForeignKeyValue1 = o.Department_Id?.ToString() })
                           .OrderBy(o => o.ItemText).ToList()
            };

            #endregion

            #region IsAbout

            caseBasicInfo.IsAbout_UserSearchCategory = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_UserSearchCategory_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_UserSearchCategory_Id, caseFieldSettings, CaseSolutionFields.IsAbout_UserSearchCategory_Id, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = userSearchCategories.Where(x => !x.IsEmpty || (currentData.IsAbout_UserSearchCategory_Id.HasValue && currentData.IsAbout_UserSearchCategory_Id.Value == x.Id)).Select(x => new FieldItem(x.Id.ToString(), Translation.GetMasterDataTranslation(x.Name))).OrderBy(r => r.ItemText).ToList()
            };


            caseBasicInfo.IsAbout_ReportedBy = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_ReportedBy, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_ReportedBy, caseFieldSettings, CaseSolutionFields.IsAbout_ReportedBy, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_PersonsName = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_PersonsName, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_Persons_Name, caseFieldSettings, CaseSolutionFields.IsAbout_PersonsName, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_PersonsEmail = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_PersonsEmail, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_Persons_EMail, caseFieldSettings, CaseSolutionFields.IsAbout_PersonsEmail, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_PersonsPhone = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_PersonsPhone, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_Persons_Phone, caseFieldSettings, CaseSolutionFields.IsAbout_PersonsPhone, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_PersonsCellPhone = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_PersonsCellPhone, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_Persons_CellPhone, caseFieldSettings, CaseSolutionFields.IsAbout_PersonsCellPhone, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_CostCentre = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_CostCentre, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_CostCentre, caseFieldSettings, CaseSolutionFields.IsAbout_CostCentre, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_Place = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_Place, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_Place, caseFieldSettings, CaseSolutionFields.IsAbout_Place, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_UserCode = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_UserCode, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_UserCode, caseFieldSettings, CaseSolutionFields.IsAbout_UserCode, templateSettingModel.ToList())
            };

            caseBasicInfo.IsAbout_Regions = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_Region_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_Region_Id, caseFieldSettings, CaseSolutionFields.IsAbout_Region_Id, templateSettingModel.ToList()),
                DefaultItem = defaultRegion != null ? new FieldItem(defaultRegion.Id.ToString(), defaultRegion.Name) : FieldItem.CreateEmpty(),
                Items = regions.Where(r => r.IsActive != 0 || (currentData.IsAbout_Region_Id.HasValue && currentData.IsAbout_Region_Id.Value == r.Id))
                               .Select(r => new FieldItem(r.Id.ToString(), r.Name, r.IsActive != 0)).OrderBy(r => r.ItemText).ToList()
            };

            caseBasicInfo.IsAbout_Departments = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_Department_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_Department_Id, caseFieldSettings, CaseSolutionFields.IsAbout_Department_Id, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = departments.Where(d => d.IsActive != 0 || (currentData.IsAbout_Department_Id.HasValue && currentData.IsAbout_Department_Id.Value == d.Id))
                                   .Select(d => new FieldItem(d.Id.ToString(), d.DepartmentName, d.IsActive != 0)
                                   { ForeignKeyValue1 = d.Region_Id?.ToString() })
                                   .OrderBy(d => d.ItemText).ToList()
            };

            caseBasicInfo.IsAbout_OUs = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.IsAbout_OU_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.IsAbout_OU_Id, caseFieldSettings, CaseSolutionFields.IsAbout_OU_Id, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = ous.Where(o => o.Id != o.Parent_OU_Id && (o.IsActive != 0 || (currentData.IsAbout_OU_Id.HasValue && currentData.IsAbout_OU_Id.Value == o.Id)) &&
                                       (o.Parent == null || (o.Parent != null && o.Parent.Parent == null))) // Max deep 2 levels
                           .Select(o => new FieldItem(o.Id.ToString(), (o.Parent == null ? o.Name : string.Format("{0} - {1}", o.Parent.Name, o.Name)), o.IsActive != 0)
                           { ForeignKeyValue1 = o.Department_Id?.ToString() })
                           .OrderBy(o => o.ItemText).ToList()
            };

            #endregion

            #region Computer Info

            caseBasicInfo.InventoryNumber = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.InventoryNumber, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.InventoryNumber, caseFieldSettings, CaseSolutionFields.InventoryNumber, templateSettingModel.ToList())
            };

            caseBasicInfo.InventoryType = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.InventoryType, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.ComputerType_Id, caseFieldSettings, CaseSolutionFields.InventoryType, templateSettingModel.ToList())
            };

            caseBasicInfo.InventoryLocation = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.InventoryLocation, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.InventoryLocation, caseFieldSettings, CaseSolutionFields.InventoryLocation, templateSettingModel.ToList())
            };

            #endregion

            #region Case Info           

            var registationSources = _registrationSourceCustomerService.GetRegistrationSources(customerId);
            caseBasicInfo.RegistrationSources = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.RegistrationSource?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.RegistrationSourceCustomer, caseFieldSettings, CaseSolutionFields.RegistrationSourceCustomer, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = registationSources.Where(r => r.IsActive != 0 || (currentData.RegistrationSource.HasValue && currentData.RegistrationSource.Value == r.Id))
                                          .Select(r => new FieldItem(r.Id.ToString(), r.SourceName, r.IsActive != 0))
                                          .OrderBy(r => r.ItemText).ToList()
            };

            var caseTypes = _caseTypeService.GetAllCaseTypes(customerId);
            var defaultCaseType = caseTypes.FirstOrDefault(c => c.IsDefault != 0 && c.IsActive != 0);
            caseBasicInfo.CaseTypes = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.CaseType_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.CaseType_Id, caseFieldSettings, CaseSolutionFields.CaseType, templateSettingModel.ToList()),
                DefaultItem = defaultCaseType != null ? new FieldItem(defaultCaseType.Id.ToString(), defaultCaseType.Name) : FieldItem.CreateEmpty(),
                Items = caseTypes.Where(c => c.IsActive != 0 || (currentData.CaseType_Id.HasValue && currentData.CaseType_Id.Value == c.Id))
                                 .Select(c => new FieldItem(c.Id.ToString(), Translation.GetCoreTextTranslation(c.Name), c.IsActive != 0, c.Parent_CaseType_Id?.ToString())
                                 {
                                     ForeignKeyValue1 = c.User_Id?.ToString(),
                                     ForeignKeyValue2 = c.WorkingGroup_Id?.ToString()
                                 })
                                 .OrderBy(i => i.ItemText).ToList()
            };

            var prodAreas = _productAreaService.GetAll(customerId);
            caseBasicInfo.ProductAreas = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.ProductArea_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.ProductArea_Id, caseFieldSettings, CaseSolutionFields.ProductArea, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = prodAreas.Where(p => p.IsActive != 0 || (currentData.ProductArea_Id.HasValue && currentData.ProductArea_Id.Value == p.Id))
                                 .Select(p => new FieldItem(p.Id.ToString(), Translation.GetCoreTextTranslation(p.Name), p.IsActive != 0, p.Parent_ProductArea_Id?.ToString())
                                 {
                                     ForeignKeyValue1 = p.WorkingGroup_Id?.ToString(),
                                     ForeignKeyValue2 = p.Priority_Id?.ToString()
                                 })
                                 .OrderBy(i => i.ItemText).ToList()
            };

            var systems = _systemService.GetSystems(customerId);
            caseBasicInfo.Systems = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.System_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.System_Id, caseFieldSettings, CaseSolutionFields.System, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = systems.Select(s => new FieldItem(s.Id.ToString(), s.SystemName, true) { ForeignKeyValue1 = s.Urgency_Id?.ToString() })
                               .OrderBy(i => i.ItemText).ToList()
            };

            var urgencies = _urgencyService.GetUrgencies(customerId);
            caseBasicInfo.Urgencies = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Urgency_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Urgency_Id, caseFieldSettings, CaseSolutionFields.Urgency, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = urgencies.Select(u => new FieldItem(u.Id.ToString(), u.Name, true)).OrderBy(i => i.ItemText).ToList()
            };

            var impacts = _impactService.GetImpacts(customerId);
            caseBasicInfo.Impacts = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Impact_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Impact_Id, caseFieldSettings, CaseSolutionFields.Impact, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = impacts.Select(i => new FieldItem(i.Id.ToString(), i.Name, true)).OrderBy(i => i.ItemText).ToList()
            };

            var categories = _categoryService.GetCategories(customerId);
            caseBasicInfo.Categories = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Category_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Category_Id, caseFieldSettings, CaseSolutionFields.Category, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = categories.Where(c => c.IsActive != 0 || (currentData.Category_Id.HasValue && currentData.Category_Id.Value == c.Id))
                                  .Select(c => new FieldItem(c.Id.ToString(), c.Name, c.IsActive != 0))
                                  .OrderBy(i => i.ItemText).ToList()
            };

            var supliers = _supplierService.GetSuppliers(customerId);
            caseBasicInfo.Supliers = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Supplier_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Supplier_Id, caseFieldSettings, CaseSolutionFields.Supplier, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = supliers.Where(s => s.IsActive != 0 || (currentData.Supplier_Id.HasValue && currentData.Supplier_Id.Value == s.Id))
                                .Select(s => new FieldItem(s.Id.ToString(), s.Name, s.IsActive != 0))
                                .OrderBy(i => i.ItemText).ToList()
            };

            caseBasicInfo.InvoiceNumber = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.InvoiceNumber, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.InvoiceNumber, caseFieldSettings, CaseSolutionFields.InvoiceNumber, templateSettingModel.ToList()),
            };

            caseBasicInfo.ReferenceNumber = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.ReferenceNumber, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.ReferenceNumber, caseFieldSettings, CaseSolutionFields.ReferenceNumber, templateSettingModel.ToList()),
            };

            caseBasicInfo.Caption = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Caption, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Caption, caseFieldSettings, CaseSolutionFields.Caption, templateSettingModel.ToList()),
            };

            caseBasicInfo.Description = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Description, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Description, caseFieldSettings, CaseSolutionFields.Description, templateSettingModel.ToList()),
            };

            caseBasicInfo.Miscellaneous = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Miscellaneous, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Miscellaneous, caseFieldSettings, CaseSolutionFields.Miscellaneous, templateSettingModel.ToList()),
            };

            caseBasicInfo.ContactBeforeAction = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.ContactBeforeAction.ToBool().ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.ContactBeforeAction, caseFieldSettings, CaseSolutionFields.ContactBeforeAction, templateSettingModel.ToList()),
            };

            caseBasicInfo.SMS = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.SMS.ToBool().ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.SMS, caseFieldSettings, CaseSolutionFields.SMS, templateSettingModel.ToList()),
            };

            caseBasicInfo.AgreedDate = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.AgreedDate.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.AgreedDate, caseFieldSettings, CaseSolutionFields.AgreedDate, templateSettingModel.ToList()),
            };

            caseBasicInfo.Available = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Available, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Available, caseFieldSettings, CaseSolutionFields.Available, templateSettingModel.ToList()),
            };

            caseBasicInfo.Cost = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Cost.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Cost, caseFieldSettings, CaseSolutionFields.Cost, templateSettingModel.ToList()),
            };

            caseBasicInfo.OtherCost = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.OtherCost.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Cost, caseFieldSettings, CaseSolutionFields.Cost, templateSettingModel.ToList()),
            };

            caseBasicInfo.Currency = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Currency, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Cost, caseFieldSettings, CaseSolutionFields.Cost, templateSettingModel.ToList()),
            };

            caseBasicInfo.CaseFile = new BasicSingleItemField()
            {
                Selected = FieldItem.CreateEmpty(),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Filename, caseFieldSettings, CaseSolutionFields.FileName, templateSettingModel.ToList()),
            };

            #endregion

            #region Other Info           

            var workinGroups = _workingGroupService.GetWorkingGroups(customerId, false);
            var defaultWG = workinGroups.FirstOrDefault(w => w.IsDefault != 0 && w.IsActive != 0);
            caseBasicInfo.WorkingGroups = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.WorkingGroup_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.WorkingGroup_Id, caseFieldSettings, CaseSolutionFields.WorkingGroup, templateSettingModel.ToList()),
                DefaultItem = defaultWG != null ? new FieldItem(defaultWG.Id.ToString(), defaultWG.WorkingGroupName) : FieldItem.CreateEmpty(),
                Items = workinGroups.Where(w => w.IsActive != 0 || (currentData.WorkingGroup_Id.HasValue && currentData.WorkingGroup_Id.Value == w.Id))
                                    .Select(w => new FieldItem(w.Id.ToString(), w.WorkingGroupName, w.IsActive != 0)
                                    { ForeignKeyValue2 = w.StateSecondary_Id?.ToString() })
                                    .OrderBy(r => r.ItemText).ToList()
            };

            caseBasicInfo.WorkingGroups.Items.Insert(0, new FieldItem("-1", string.Format("-- {0} --", Translation.GetCoreTextTranslation(CURRENT_USER_WORKINGGROUP_CAPTION))));

            var admins = _userService.GetAvailablePerformersOrUserId(customerId, null, true);
            caseBasicInfo.Administrators = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.PerformerUser_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Performer_User_Id, caseFieldSettings, CaseSolutionFields.Administrator, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = admins.Where(a => a.IsActive != 0 || (currentData.PerformerUser_Id.HasValue && currentData.PerformerUser_Id.Value == a.Id))
                              .Select(a => new FieldItem(a.Id.ToString(),
                                                         (customerSettings.IsUserFirstLastNameRepresentation == 1 ?
                                                                string.Format("{0} {1}", a.FirstName, a.SurName) : string.Format("{0} {1}", a.SurName, a.FirstName)),
                                                         a.IsActive != 0)
                              {
                                  ForeignKeyValue1 = string.Join(",", a.WorkingGroups.Where(w => w.UserRole == WorkingGroupUserPermission.ADMINSTRATOR && w.IsMemberOfGroup)
                                                                                        .Select(w => w.WorkingGroupId)
                                                                                        .ToArray())
                              })
                              .OrderBy(i => i.ItemText).ToList()
            };
            caseBasicInfo.Administrators.Items.Insert(0, new FieldItem("-1", string.Format("-- {0} --", Translation.GetCoreTextTranslation(CURRENT_USER_ITEM_CAPTION))));


            var priorities = _priorityService.GetPriorities(customerId);
            var defaultPrio = priorities.FirstOrDefault(p => p.IsDefault != 0 && p.IsActive != 0);
            caseBasicInfo.Priorities = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Priority_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Priority_Id, caseFieldSettings, CaseSolutionFields.Priority, templateSettingModel.ToList()),
                DefaultItem = defaultPrio != null ? new FieldItem(defaultPrio.Id.ToString(), defaultPrio.Name) : FieldItem.CreateEmpty(),
                Items = priorities.Where(p => p.IsActive != 0 || (currentData.Priority_Id.HasValue && currentData.Priority_Id.Value == p.Id))
                                  .Select(p => new FieldItem(p.Id.ToString(), p.Name, p.IsActive != 0)
                                  {
                                      ForeignKeyValue1 = p.LogText,
                                      ForeignKeyValue2 = p.SolutionTime.ToString()
                                  })
                                  .OrderBy(i => i.ItemText).ToList()
            };

            var statuses = _statusService.GetStatuses(customerId);
            var defaultStatus = statuses.FirstOrDefault(s => s.IsDefault != 0 && s.IsActive != 0);
            caseBasicInfo.Status = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Status_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Status_Id, caseFieldSettings, CaseSolutionFields.Status, templateSettingModel.ToList()),
                DefaultItem = defaultStatus != null ? new FieldItem(defaultStatus.Id.ToString(), defaultStatus.Name) : FieldItem.CreateEmpty(),
                Items = statuses.Where(s => s.IsActive != 0 || (currentData.Status_Id.HasValue && currentData.Status_Id.Value == s.Id))
                                .Select(s => new FieldItem(s.Id.ToString(), s.Name, s.IsActive != 0)
                                {
                                    ForeignKeyValue1 = s.WorkingGroup_Id?.ToString(),
                                    ForeignKeyValue2 = s.StateSecondary_Id?.ToString(),
                                })
                                .OrderBy(i => i.ItemText).ToList()
            };

            var subStatuses = _stateSecondaryService.GetStateSecondaries(customerId);
            var defaultSubStatus = statuses.FirstOrDefault(s => s.IsDefault != 0 && s.IsActive != 0);
            caseBasicInfo.SubStatus = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.StateSecondary_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.StateSecondary_Id, caseFieldSettings, CaseSolutionFields.StateSecondary, templateSettingModel.ToList()),
                DefaultItem = defaultSubStatus != null ? new FieldItem(defaultSubStatus.Id.ToString(), defaultSubStatus.Name) : FieldItem.CreateEmpty(),
                Items = subStatuses.Where(s => s.IsActive != 0 || (currentData.StateSecondary_Id.HasValue && currentData.StateSecondary_Id.Value == s.Id))
                                   .Select(s => new FieldItem(s.Id.ToString(), s.Name, s.IsActive != 0)
                                   {
                                       ForeignKeyValue1 = s.WorkingGroup_Id?.ToString(),
                                       ForeignKeyValue2 = (!s.NoMailToNotifier.ToBool()).ToString(),
                                       ForeignKeyValue3 = s.RecalculateWatchDate.ToString()
                                   })
                                   .OrderBy(i => i.ItemText).ToList()
            };

            var projects = _projectService.GetCustomerProjects(customerId);
            caseBasicInfo.Projects = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Project_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Project, caseFieldSettings, CaseSolutionFields.Project, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = projects.Where(p => p.IsActive != 0 || (currentData.Project_Id.HasValue && currentData.Project_Id.Value == p.Id))
                                .Select(p => new FieldItem(p.Id.ToString(), p.Name, p.IsActive != 0))
                                .OrderBy(i => i.ItemText).ToList()
            };

            var problems = _problemService.GetCustomerProblems(customerId);
            caseBasicInfo.Problems = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Problem_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Problem, caseFieldSettings, CaseSolutionFields.Problem, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = problems.Select(p => new FieldItem(p.Id.ToString(), p.Name, true))
                                .OrderBy(i => i.ItemText).ToList()
            };

            var causingParts = _causingPartService.GetCausingParts(customerId);
            caseBasicInfo.CausingParts = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.CausingPart_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.CausingPart, caseFieldSettings, CaseSolutionFields.CausingPart, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = causingParts.Where(c => c.IsActive || (currentData.CausingPart_Id.HasValue && currentData.CausingPart_Id.Value == c.Id))
                                    .Select(c => new FieldItem(c.Id.ToString(), c.Name, c.IsActive))
                                    .OrderBy(i => i.ItemText).ToList()
            };

            var changes = _changeService.GetChanges(customerId);
            caseBasicInfo.Changes = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.Change_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Change, caseFieldSettings, CaseSolutionFields.Change, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = changes.Select(c => new FieldItem(c.Id.ToString(), c.ChangeTitle, true)).OrderBy(i => i.ItemText).ToList()
            };

            caseBasicInfo.PlanDate = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.PlanDate?.ToShortDateString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.PlanDate, caseFieldSettings, CaseSolutionFields.PlanDate, templateSettingModel.ToList())
            };

            caseBasicInfo.WatchDate = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.WatchDate?.ToShortDateString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.WatchDate, caseFieldSettings, CaseSolutionFields.WatchDate, templateSettingModel.ToList())
            };

            caseBasicInfo.Verified = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Verified.ToBool().ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.Verified, caseFieldSettings, CaseSolutionFields.Verified, templateSettingModel.ToList())
            };

            caseBasicInfo.VerifiedDescription = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.VerifiedDescription, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.VerifiedDescription, caseFieldSettings, CaseSolutionFields.VerifiedDescription, templateSettingModel.ToList())
            };

            var solutionRateItems = new List<FieldItem>();
            for (var i = 10; i < 110; i = i + 10)
                solutionRateItems.Add(new FieldItem(i.ToString(), i.ToString()));

            caseBasicInfo.SolutionRate = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.SolutionRate, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.SolutionRate, caseFieldSettings, CaseSolutionFields.SolutionRate, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = solutionRateItems
            };

            #endregion

            #region Log Info

            caseBasicInfo.ExternalLog = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Text_External, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.tblLog_Text_External, caseFieldSettings, CaseSolutionFields.ExternalLogNote, templateSettingModel.ToList())
            };

            caseBasicInfo.InternalLog = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.Text_Internal, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.tblLog_Text_Internal, caseFieldSettings, CaseSolutionFields.InternalLogNote, templateSettingModel.ToList())
            };

            caseBasicInfo.FinishingDescription = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.FinishingDescription, string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.FinishingDescription, caseFieldSettings, CaseSolutionFields.FinishingDescription, templateSettingModel.ToList())
            };

            caseBasicInfo.LogFile = new BasicSingleItemField()
            {
                Selected = FieldItem.CreateEmpty(),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.tblLog_Filename, caseFieldSettings, CaseSolutionFields.LogFileName, templateSettingModel.ToList())
            };

            caseBasicInfo.LogFileInternal = new BasicSingleItemField()
            {
                Selected = FieldItem.CreateEmpty(),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.tblLog_Filename_Internal, caseFieldSettings, CaseSolutionFields.LogFileName_Internal, templateSettingModel.ToList())
            };

            caseBasicInfo.FinishingDate = new BasicSingleItemField()
            {
                Selected = new FieldItem(currentData.FinishingDate?.ToShortDateString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.FinishingDate, caseFieldSettings, CaseSolutionFields.FinishingDate, templateSettingModel.ToList())
            };

            var closingReasons = _finishingCauseService.GetFinishingCauses(customerId);
            caseBasicInfo.ClosingReason = new BasicMultiItemField()
            {
                Selected = new FieldItem(currentData.FinishingCause_Id?.ToString(), string.Empty),
                StatusType = GetFieldStatusType(ruleMode, TranslationCaseFields.ClosingReason, caseFieldSettings, CaseSolutionFields.FinishingCause, templateSettingModel.ToList()),
                DefaultItem = FieldItem.CreateEmpty(),
                Items = closingReasons.Where(c => c.IsActive != 0 || (currentData.FinishingCause_Id.HasValue && currentData.FinishingCause_Id.Value == c.Id))
                                      .Select(c => new FieldItem(c.Id.ToString(), c.Name, c.IsActive != 0))
                                      .OrderBy(r => r.ItemText).ToList()
            };

            #endregion

            #region Virtual Data Store

            var prio_Impact_Urgent = _urgencyService.GetPriorityImpactUrgencies(customerId);
            caseBasicInfo.Priority_Impact_Urgent = new BasicVirtualDataField()
            {
                Items = prio_Impact_Urgent.Where(p => p.Impact_Id.HasValue && p.Urgency_Id.HasValue)
                                          .Select(p => new FieldItem(string.Empty, string.Empty)
                                          {
                                              ForeignKeyValue1 = p.Impact_Id.ToString(),
                                              ForeignKeyValue2 = p.Urgency_Id.ToString(),
                                              ResultKeyValue = p.Priority_Id.ToString()
                                          })
                                          .OrderBy(r => r.ItemText).ToList()
            };

            var allWatchDates = _watchDateCalendarService.GetAllClosestDateTo(DateTime.UtcNow);
            var department_WatchDate = departments.Where(d => (d.IsActive != 0 || (currentData.Department_Id.HasValue && currentData.Department_Id.Value == d.Id)) &&
                                                               d.WatchDateCalendar_Id.HasValue)
                                                  .Select(d => new Tuple<int, string>(d.Id, allWatchDates.FirstOrDefault(w => w.WatchDateCalendar_Id == d.WatchDateCalendar_Id.Value)?.WatchDate.ToShortDateString()))
                                                  .ToList();

            caseBasicInfo.Department_WatchDate = new BasicVirtualDataField()
            {
                Items = department_WatchDate.Select(dw => new FieldItem(string.Empty, string.Empty)
                {
                    ForeignKeyValue1 = dw.Item1.ToString(), // Depatment Id 
                    ResultKeyValue = dw.Item2  // WatchDate                
                }).ToList()
            };

            #endregion

            return caseBasicInfo;
        }
        
        private CaseFieldStatusType GetFieldStatusType(CaseRuleMode ruleMode, 
                                                       TranslationCaseFields caseField, List<CaseFieldSetting> caseFieldSettings,
                                                       CaseSolutionFields templateField, List<CaseSolutionSettingModel> templateFieldSettings)
        {

            var mode = CaseSolutionModes.DisplayField;

            if (ruleMode == CaseRuleMode.TemplateUserChangeMode)
            {                
                var setting = templateFieldSettings.SingleOrDefault(s => s.CaseSolutionField == templateField);
                if (setting != null)
                    mode = setting.CaseSolutionMode;

                switch (mode)
                {
                    case CaseSolutionModes.DisplayField:
                        return CaseFieldStatusType.Editable;
                    case CaseSolutionModes.Hide:
                        return CaseFieldStatusType.Hidden;
                    case CaseSolutionModes.ReadOnly:
                        return CaseFieldStatusType.Readonly;
                }
            }
            else
            {
                var checkedTemplateSetting = false;
                var templateSettingMode = CaseSolutionModes.Hide;
                var caseSettingMode = CaseSolutionModes.Hide;

                // Check template behavior if exists
                if (templateFieldSettings != null && templateFieldSettings.Any())
                {
                    var templateSetting = templateFieldSettings.SingleOrDefault(s => s.CaseSolutionField == templateField);
                    if (templateSetting != null)
                    {
                        checkedTemplateSetting = true;
                        templateSettingMode = templateSetting.CaseSolutionMode;
                    }
                }

                // Check Customer Case-Field Settings behavior 
                var caseFieldSetting = caseFieldSettings.getCaseFieldSettingId(caseField.ToString());
                if (caseFieldSettings.IsFieldLocked(caseField))
                {
                    caseSettingMode = CaseSolutionModes.ReadOnly;
                }
                else if (caseFieldSettings.IsFieldRequiredOrVisible(caseField))
                {
                    caseSettingMode = CaseSolutionModes.DisplayField;
                }

                if (checkedTemplateSetting)
                {
                    if (caseSettingMode == CaseSolutionModes.Hide)
                        mode = CaseSolutionModes.Hide;
                    else
                    {
                        if (templateSettingMode == CaseSolutionModes.DisplayField && caseSettingMode == CaseSolutionModes.ReadOnly)
                            mode = CaseSolutionModes.ReadOnly;
                        else
                            mode = caseSettingMode;
                    }
                }
                else
                {
                    mode = caseSettingMode;
                }

                switch (mode)
                {
                    case CaseSolutionModes.DisplayField:
                        return CaseFieldStatusType.Editable;

                    case CaseSolutionModes.Hide:
                        return CaseFieldStatusType.Hidden;

                    case CaseSolutionModes.ReadOnly:
                        return CaseFieldStatusType.Readonly;
                }
            }
            
            return CaseFieldStatusType.Editable;
        }

        private List<FieldAttributeModel> GetGlobalRules(int customerId,
                                                           List<CaseFieldSetting> caseFieldSettings,
                                                           BasicCaseInformation basicInformation,
                                                           Setting customerSettings,
                                                           CaseRuleMode ruleMode)
        {
            var ret = new List<FieldAttributeModel>();

            // *** Initiator ***
            #region Initiator

            #region Initiator Category

            var curField = TranslationCaseFields.UserSearchCategory_Id.ToString();
            var attrReportedBy = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.GetForCase(curField, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = new FieldItem(string.Empty, string.Empty),
                Selected = basicInformation.UserSearchCategory.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.UserSearchCategory.StatusType,
                Items = basicInformation.UserSearchCategory.Items
            };
            ret.Add(attrReportedBy);

            #endregion

            #region ReportedBy

            curField = TranslationCaseFields.ReportedBy.ToString();
            attrReportedBy = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ReportedBy.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ReportedBy.StatusType
            };
            ret.Add(attrReportedBy);

            #endregion

            #region PersonsName

            curField = TranslationCaseFields.Persons_Name.ToString();
            var attrPersonName = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.GetForCase(curField, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsName.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsName.StatusType
            };
            ret.Add(attrPersonName);

            #endregion 

            #region PersonsEmail

            curField = TranslationCaseFields.Persons_EMail.ToString();
            var attrPersonEmail = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsEmail.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsEmail.StatusType
            };
            ret.Add(attrPersonEmail);

            #endregion

            #region MailToNotifier

            curField = TranslationCaseFields.MailToNotifier.ToString();
            var attrMailToNotifier = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.MailToNotifier.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.MailToNotifier.StatusType
            };
            ret.Add(attrMailToNotifier);

            #endregion

            #region PersonsPhone

            curField = TranslationCaseFields.Persons_Phone.ToString();
            var attrPersonPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsPhone.StatusType
            };
            ret.Add(attrPersonPhone);

            #endregion

            #region PersonsCellPhone

            curField = TranslationCaseFields.Persons_CellPhone.ToString();
            var attrPersonCellPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsCellPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsCellPhone.StatusType
            };
            ret.Add(attrPersonCellPhone);

            #endregion

            #region CostCentre

            curField = TranslationCaseFields.CostCentre.ToString();
            var attrCostCentre = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.CostCentre.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CostCentre.StatusType
            };
            ret.Add(attrCostCentre);

            #endregion

            #region Place

            curField = TranslationCaseFields.Place.ToString();
            var attrPlace = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Place.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Place.StatusType
            };
            ret.Add(attrPlace);

            #endregion

            #region UserCode

            curField = TranslationCaseFields.UserCode.ToString();
            var attrUserCode = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.UserCode.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.UserCode.StatusType
            };
            ret.Add(attrUserCode);

            #endregion

            #region UpdateUserInfo

            curField = TranslationCaseFields.UpdateNotifierInformation.ToString();
            var attrUpdateUserInfo = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.UpdateUserInfo.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.UpdateUserInfo.StatusType
            };
            ret.Add(attrUpdateUserInfo);
            #endregion

            #region AddFollowersBtn

            curField = TranslationCaseFields.AddFollowersBtn.ToString();
            var attrAddFollowersBtn = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.AddFollowersBtn.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.AddFollowersBtn.StatusType
            };
            ret.Add(attrAddFollowersBtn);
            #endregion

            #region Region

            curField = TranslationCaseFields.Region_Id.ToString();
            var attrRegion = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Regions.DefaultItem,
                Selected = basicInformation.Regions.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Regions.StatusType,
                Items = basicInformation.Regions.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode, CaseRuleMode.TemplateUserChangeMode, 
                                      CaseRuleMode.CaseNewTemplateMode, CaseRuleMode.CaseInheritTemplateMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.Department_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),                        
                        ShowAllIfKeyIsNull = true
                    }
                }
            };
            ret.Add(attrRegion);

            #endregion

            #region Department

            curField = TranslationCaseFields.Department_Id.ToString();
            var attrDepartment = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Departments.DefaultItem,
                Selected = basicInformation.Departments.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Departments.StatusType,
                Items = basicInformation.Departments.Items,                                
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode, CaseRuleMode.TemplateUserChangeMode,
                                      CaseRuleMode.CaseNewTemplateMode, CaseRuleMode.CaseInheritTemplateMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.OU_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },

                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 1,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Department_WatchDate.ToString(),
                        DataStore1 = TranslationCaseFields.Department_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.WatchDate.ToString(),
                        StaticMessage = Translation.GetCoreTextTranslation("När denna avdelning används beräknas") + " <b>" + 
                                        Translation.Get(TranslationCaseFields.WatchDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + "</b> " +  
                                        Translation.GetCoreTextTranslation("enligt kalendern"),
                        Conditions =  new List<FieldRelationCondition> {                            
                            new FieldRelationCondition(TranslationCaseFields.Department_Id.ToString(), ForeignKeyNum.FKeyNum0, ConditionOperator.HasValue),
                            new FieldRelationCondition(TranslationCaseFields.Priority_Id.ToString(), ForeignKeyNum.FKeyNum2, ConditionOperator.Equal, "0")
                        }
                    }
                }
            };
            ret.Add(attrDepartment);

            #endregion

            #region OU

            curField = TranslationCaseFields.OU_Id.ToString();
            var attrOU = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.OUs.DefaultItem,
                Selected = basicInformation.OUs.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.OUs.StatusType,
                Items = basicInformation.OUs.Items
            };
            ret.Add(attrOU);

            #endregion

            #endregion

            // *** IsAbout ***
            #region IsAbout

            #region IsAbout_UserSearchCategory

            curField = TranslationCaseFields.IsAbout_UserSearchCategory_Id.ToString();
            var attrIsAboutUserCategory = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.GetForCase(curField, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = new FieldItem(string.Empty, string.Empty),
                Selected = basicInformation.IsAbout_UserSearchCategory.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_UserSearchCategory.StatusType,
                Items = basicInformation.IsAbout_UserSearchCategory.Items
            };
            ret.Add(attrIsAboutUserCategory);

            #endregion

            #region IsAbout_ReportedBy

            curField = TranslationCaseFields.IsAbout_ReportedBy.ToString();
            var attrIsAbout_ReportedBy = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_ReportedBy.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_ReportedBy.StatusType
            };
            ret.Add(attrIsAbout_ReportedBy);

            #endregion

            #region IsAbout_PersonsName

            curField = TranslationCaseFields.IsAbout_Persons_Name.ToString();
            var attrIsAbout_PersonName = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_PersonsName.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsName.StatusType
            };
            ret.Add(attrIsAbout_PersonName);

            #endregion 

            #region IsAbout_PersonsEmail

            curField = TranslationCaseFields.IsAbout_Persons_EMail.ToString();
            var attrIsAbout_PersonEmail = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsEmail.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsEmail.StatusType
            };
            ret.Add(attrIsAbout_PersonEmail);

            #endregion           

            #region IsAbout_PersonsPhone

            curField = TranslationCaseFields.IsAbout_Persons_Phone.ToString();
            var attrIsAbout_PersonPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_PersonsPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsPhone.StatusType
            };
            ret.Add(attrIsAbout_PersonPhone);

            #endregion

            #region IsAbout_PersonsCellPhone

            curField = TranslationCaseFields.IsAbout_Persons_CellPhone.ToString();
            var attrIsAbout_PersonCellPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_PersonsCellPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsCellPhone.StatusType
            };
            ret.Add(attrIsAbout_PersonCellPhone);

            #endregion

            #region IsAbout_CostCentre

            curField = TranslationCaseFields.IsAbout_CostCentre.ToString();
            var attrIsAbout_CostCentre = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_CostCentre.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_CostCentre.StatusType
            };
            ret.Add(attrIsAbout_CostCentre);

            #endregion

            #region IsAbout_Place

            curField = TranslationCaseFields.IsAbout_Place.ToString();
            var attrIsAbout_Place = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_Place.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_Place.StatusType
            };
            ret.Add(attrIsAbout_Place);

            #endregion

            #region IsAbout_UserCode

            curField = TranslationCaseFields.IsAbout_UserCode.ToString();
            var attrIsAbout_UserCode = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_UserCode.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_UserCode.StatusType
            };
            ret.Add(attrIsAbout_UserCode);

            #endregion            

            #region IsAbout_Region

            curField = TranslationCaseFields.IsAbout_Region_Id.ToString();
            var attrIsAbout_Region = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.IsAbout_Regions.DefaultItem,
                Selected = basicInformation.IsAbout_Regions.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_Regions.StatusType,
                Items = basicInformation.IsAbout_Regions.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode, CaseRuleMode.TemplateUserChangeMode,
                                      CaseRuleMode.CaseNewTemplateMode, CaseRuleMode.CaseInheritTemplateMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.IsAbout_Department_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),                        
                        ShowAllIfKeyIsNull = true
                    }
                }
            };
            ret.Add(attrIsAbout_Region);

            #endregion

            #region IsAbout_Department

            curField = TranslationCaseFields.IsAbout_Department_Id.ToString();
            var attrIsAbout_Department = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.IsAbout_Departments.DefaultItem,
                Selected = basicInformation.IsAbout_Departments.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_Departments.StatusType,
                Items = basicInformation.IsAbout_Departments.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode, CaseRuleMode.TemplateUserChangeMode,
                                      CaseRuleMode.CaseNewTemplateMode, CaseRuleMode.CaseInheritTemplateMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.IsAbout_OU_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    }
                }
            };
            ret.Add(attrIsAbout_Department);

            #endregion

            #region IsAbout_OU

            curField = TranslationCaseFields.IsAbout_OU_Id.ToString();
            var attrIsAbout_OU = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.IsAbout_OUs.DefaultItem,
                Selected = basicInformation.IsAbout_OUs.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_OUs.StatusType,
                Items = basicInformation.IsAbout_OUs.Items
            };
            ret.Add(attrIsAbout_OU);

            #endregion

            #endregion

            // *** Computer Info ***
            #region Computer Info

            #region InventoryNumber

            curField = TranslationCaseFields.InventoryNumber.ToString();
            var attrInventoryNumber = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InventoryNumber.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InventoryNumber.StatusType
            };
            ret.Add(attrInventoryNumber);

            #endregion

            #region InventoryType

            curField = TranslationCaseFields.ComputerType_Id.ToString();
            var attrInventoryType = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InventoryType.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InventoryType.StatusType
            };
            ret.Add(attrInventoryType);

            #endregion 

            #region InventoryLocation

            curField = TranslationCaseFields.InventoryLocation.ToString();
            var attrInventoryLocation = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InventoryLocation.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InventoryLocation.StatusType
            };
            ret.Add(attrInventoryLocation);

            #endregion

            #endregion

            // *** Case Info ***
            #region Case Info

            #region RegistrationSource

            curField = TranslationCaseFields.RegistrationSourceCustomer.ToString();
            var attrRegistrationSource = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.RegistrationSources.DefaultItem,
                Selected = basicInformation.RegistrationSources.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.RegistrationSources.StatusType,
                Items = basicInformation.RegistrationSources.Items
            };
            ret.Add(attrRegistrationSource);

            #endregion

            #region CaseType

            curField = TranslationCaseFields.CaseType_Id.ToString();
            var attrCaseType = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TreeButtonSelect,
                DefaultItem = basicInformation.CaseTypes.DefaultItem,
                Selected = basicInformation.CaseTypes.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CaseTypes.StatusType,
                Items = basicInformation.CaseTypes.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.Performer_User_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.WorkingGroup_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }
                }

            };
            ret.Add(attrCaseType);

            #endregion 

            #region ProductArea

            curField = TranslationCaseFields.ProductArea_Id.ToString();
            var attrProductArea = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TreeButtonSelect,
                DefaultItem = basicInformation.ProductAreas.DefaultItem,
                Selected = basicInformation.ProductAreas.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ProductAreas.StatusType,
                Items = basicInformation.ProductAreas.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.WorkingGroup_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },

                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 1,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.Priority_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }
                }
            };
            ret.Add(attrProductArea);

            #endregion 

            #region System

            curField = TranslationCaseFields.System_Id.ToString();
            var attrSystem = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Systems.DefaultItem,
                Selected = basicInformation.Systems.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Systems.StatusType,
                Items = basicInformation.Systems.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.Urgency_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    }                 
                }
            };
            ret.Add(attrSystem);

            #endregion

            #region Urgency

            curField = TranslationCaseFields.Urgency_Id.ToString();
            var attrUrgency = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Urgencies.DefaultItem,
                Selected = basicInformation.Urgencies.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Urgencies.StatusType,
                Items = basicInformation.Urgencies.Items,
                Relations = new List<FieldRelation> {                                      
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Priority_Impact_Urgent.ToString(),
                        DataStore1 = TranslationCaseFields.Impact_Id.ToString(),
                        DataStore2 = TranslationCaseFields.Urgency_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.Priority_Id.ToString()
                    }
                }
            };
            ret.Add(attrUrgency);

            #endregion

            #region Impact

            curField = TranslationCaseFields.Impact_Id.ToString();
            var attrImpact = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Impacts.DefaultItem,
                Selected = basicInformation.Impacts.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Impacts.StatusType,
                Items = basicInformation.Impacts.Items,
                Relations = new List<FieldRelation> {                   
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Priority_Impact_Urgent.ToString(),
                        DataStore1 = TranslationCaseFields.Impact_Id.ToString(),
                        DataStore2 = TranslationCaseFields.Urgency_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.Priority_Id.ToString()
                    }
                }
            };
            ret.Add(attrImpact);

            #endregion

            #region Category

            curField = TranslationCaseFields.Category_Id.ToString();
            var attrCategory = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Categories.DefaultItem,
                Selected = basicInformation.Categories.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Categories.StatusType,
                Items = basicInformation.Categories.Items
            };
            ret.Add(attrCategory);

            #endregion

            #region Supplier

            curField = TranslationCaseFields.Supplier_Id.ToString();
            var attrSupplier = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Supliers.DefaultItem,
                Selected = basicInformation.Supliers.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Supliers.StatusType,
                Items = basicInformation.Supliers.Items
            };
            ret.Add(attrSupplier);

            #endregion

            #region InvoiceNumber

            curField = TranslationCaseFields.InvoiceNumber.ToString();
            var attrInvoiceNumber = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InvoiceNumber.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InvoiceNumber.StatusType
            };
            ret.Add(attrInvoiceNumber);

            #endregion

            #region ReferenceNumber

            curField = TranslationCaseFields.ReferenceNumber.ToString();
            var attrReferenceNumber = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ReferenceNumber.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ReferenceNumber.StatusType
            };
            ret.Add(attrReferenceNumber);

            #endregion

            #region Caption

            curField = TranslationCaseFields.Caption.ToString();
            var attrCaption = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Caption.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Caption.StatusType
            };
            ret.Add(attrCaption);

            #endregion

            #region Description

            curField = TranslationCaseFields.Description.ToString();
            var attrDescription = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Description.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InvoiceNumber.StatusType
            };
            ret.Add(attrDescription);

            #endregion

            #region Miscellaneous

            curField = TranslationCaseFields.Miscellaneous.ToString();
            var attrMiscellaneous = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Miscellaneous.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Miscellaneous.StatusType
            };
            ret.Add(attrMiscellaneous);

            #endregion

            #region ContactBeforeAction

            curField = TranslationCaseFields.ContactBeforeAction.ToString();
            var attrContactBeforeAction = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ContactBeforeAction.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ContactBeforeAction.StatusType
            };
            ret.Add(attrContactBeforeAction);

            #endregion

            #region SMS

            curField = TranslationCaseFields.SMS.ToString();
            var attrSMS = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.SMS.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.SMS.StatusType
            };
            ret.Add(attrSMS);

            #endregion

            #region Available

            curField = TranslationCaseFields.Available.ToString();
            var attrAvailable = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Available.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Available.StatusType
            };
            ret.Add(attrAvailable);

            #endregion

            #region Cost

            curField = TranslationCaseFields.Cost.ToString();
            var attrCost = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Cost.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Cost.StatusType
            };
            ret.Add(attrCost);

            curField = TranslationCaseFields.Cost.ToString();
            var attrOtherCost = new FieldAttributeModel()
            {
                FieldId = "OtherCost",
                FieldName = "OtherCost",
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.OtherCost.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.OtherCost.StatusType
            };
            ret.Add(attrOtherCost);

            curField = TranslationCaseFields.Cost.ToString();
            var attrCurrency = new FieldAttributeModel()
            {
                FieldId = "Currency",
                FieldName = "Currency",
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Currency.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Currency.StatusType
            };
            ret.Add(attrCurrency);
            #endregion

            #region CaseFile

            curField = TranslationCaseFields.Filename.ToString();
            var attrFilename = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.ButtonField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CaseFile.StatusType
            };
            ret.Add(attrFilename);

            #endregion

            #region AgreedDate

            curField = TranslationCaseFields.AgreedDate.ToString();
            var attrAgreedDate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.AgreedDate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.AgreedDate.StatusType
            };
            ret.Add(attrAgreedDate);

            #endregion
            #endregion

            // *** Other Info ***
            #region Other Info

            #region WorkingGroup

            curField = TranslationCaseFields.WorkingGroup_Id.ToString();
            var attrWorkingGroup = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.WorkingGroups.DefaultItem,
                Selected = basicInformation.WorkingGroups.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.WorkingGroups.StatusType,
                Items = basicInformation.WorkingGroups.Items,
                Relations = new List<FieldRelation> {                    
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.StateSecondary_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }
                }
            };

            if (customerSettings != null && customerSettings.DontConnectUserToWorkingGroup == 0)
            {
                attrWorkingGroup.Relations.Add(
                                                new FieldRelation(CaseRuleMode.CaseUserChangeMode, CaseRuleMode.TemplateUserChangeMode,
                                                                  CaseRuleMode.CaseNewTemplateMode, CaseRuleMode.CaseInheritTemplateMode)
                                                {
                                                    SequenceNo = 0,
                                                    RelationType = RelationType.ManyToMany.ToInt(),
                                                    ActionType = RelationActionType.ListPopulator.ToInt(),
                                                    FieldId = TranslationCaseFields.Performer_User_Id.ToString(),
                                                    ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),
                                                    ShowRunTimeCurrentValue = ruleMode == CaseRuleMode.TemplateUserChangeMode,
                                                    ShowAllIfKeyIsNull = true                                                                                                                                               
                                                });
            }            

            ret.Add(attrWorkingGroup);

            #endregion

            #region Performer_User

            curField = TranslationCaseFields.Performer_User_Id.ToString();
            var attrPerformer_User = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Administrators.DefaultItem,
                Selected = basicInformation.Administrators.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Administrators.StatusType,
                Items = basicInformation.Administrators.Items
            };
            ret.Add(attrPerformer_User);

            #endregion 

            #region Priority

            curField = TranslationCaseFields.Priority_Id.ToString();
            var attrPriority = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Priorities.DefaultItem,
                Selected = basicInformation.Priorities.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Priorities.StatusType,
                Items = basicInformation.Priorities.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                            SequenceNo = 0,
                            RelationType = RelationType.OneToOne.ToInt(),
                            ActionType = RelationActionType.ValueSetter.ToInt(),
                            FieldId = TranslationCaseFields.tblLog_Text_External.ToString(),
                            ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },

                    new FieldRelation(CaseRuleMode.CaseUserChangeMode,
                                      CaseRuleMode.CaseNewTemplateMode, CaseRuleMode.CaseInheritTemplateMode) {
                        SequenceNo = 1,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Department_WatchDate.ToString(),
                        DataStore1 = TranslationCaseFields.Department_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.WatchDate.ToString(),
                        StaticMessage = "<b> " + Translation.Get(TranslationCaseFields.WatchDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + " </b> " + 
                                        Translation.GetCoreTextTranslation("beräknas enligt kalendern"),
                        Conditions =  new List<FieldRelationCondition> {                            
                            new FieldRelationCondition(TranslationCaseFields.Department_Id.ToString(), ForeignKeyNum.FKeyNum0, ConditionOperator.HasValue),
                            new FieldRelationCondition(TranslationCaseFields.Priority_Id.ToString(), ForeignKeyNum.FKeyNum2, ConditionOperator.Equal, "0")
                        }
                    }
                }
            };
            ret.Add(attrPriority);

            #endregion

            #region Status

            curField = TranslationCaseFields.Status_Id.ToString();
            var attrStatus = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Status.DefaultItem,
                Selected = basicInformation.Status.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Status.StatusType,
                Items = basicInformation.Status.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.WorkingGroup_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },

                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 1,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.StateSecondary_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }
                }
            };
            ret.Add(attrStatus);

            #endregion

            #region SubStatus

            curField = TranslationCaseFields.StateSecondary_Id.ToString();
            var attrStateSecondary = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.SubStatus.DefaultItem,
                Selected = basicInformation.SubStatus.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.SubStatus.StatusType,
                Items = basicInformation.SubStatus.Items,
                Relations = new List<FieldRelation> {

                    /* Disabled because it makes cycle */
                    //new FieldRelation() {
                    //    SequenceNo = 0,
                    //    RelationType = RelationType.OneToOne.ToInt(),
                    //    ActionType = RelationActionType.ValueSetter.ToInt(),
                    //    FieldId = TranslationCaseFields.WorkingGroup_Id.ToString(),
                    //    ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    //},

                    new FieldRelation(CaseRuleMode.CaseUserChangeMode,
                                      CaseRuleMode.CaseNewTemplateMode, CaseRuleMode.CaseInheritTemplateMode) {
                        SequenceNo = 0,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Department_WatchDate.ToString(),
                        DataStore1 = TranslationCaseFields.Department_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.WatchDate.ToString(),                        
                        Conditions =  new List<FieldRelationCondition> {
                            new FieldRelationCondition(TranslationCaseFields.StateSecondary_Id.ToString(), ForeignKeyNum.FKeyNum3, ConditionOperator.NotEqual, "0"),
                            new FieldRelationCondition(TranslationCaseFields.Department_Id.ToString(), ForeignKeyNum.FKeyNum0, ConditionOperator.HasValue),
                            new FieldRelationCondition(TranslationCaseFields.Priority_Id.ToString(), ForeignKeyNum.FKeyNum2, ConditionOperator.Equal, "0")
                        }
                    },

                    new FieldRelation(CaseRuleMode.CaseUserChangeMode) {
                        SequenceNo = 1,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.MailToNotifier.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }                    
                }
            };
            ret.Add(attrStateSecondary);

            #endregion

            #region Project

            curField = TranslationCaseFields.Project.ToString();
            var attrProject = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Projects.DefaultItem,
                Selected = basicInformation.Projects.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Projects.StatusType,
                Items = basicInformation.Projects.Items
            };
            ret.Add(attrProject);

            #endregion

            #region Problem

            curField = TranslationCaseFields.Problem.ToString();
            var attrProblem = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Problems.DefaultItem,
                Selected = basicInformation.Problems.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Problems.StatusType,
                Items = basicInformation.Problems.Items
            };
            ret.Add(attrProblem);

            #endregion

            #region CausingPart

            curField = TranslationCaseFields.CausingPart.ToString();
            var attrCausingPart = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.CausingParts.DefaultItem,
                Selected = basicInformation.CausingParts.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CausingParts.StatusType,
                Items = basicInformation.CausingParts.Items
            };
            ret.Add(attrCausingPart);

            #endregion

            #region Change

            curField = TranslationCaseFields.Change.ToString();
            var attrChange = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Changes.DefaultItem,
                Selected = basicInformation.Changes.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Changes.StatusType,
                Items = basicInformation.Changes.Items
            };
            ret.Add(attrChange);

            #endregion

            #region PlanDate

            curField = TranslationCaseFields.PlanDate.ToString();
            var attrPlanDate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PlanDate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PlanDate.StatusType
            };
            ret.Add(attrPlanDate);

            #endregion

            #region WatchDate

            curField = TranslationCaseFields.WatchDate.ToString();
            var attrWatchDate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.WatchDate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.WatchDate.StatusType
            };
            ret.Add(attrWatchDate);

            #endregion

            #region Verified

            curField = TranslationCaseFields.Verified.ToString();
            var attrVerified = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Verified.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Verified.StatusType
            };
            ret.Add(attrVerified);

            #endregion

            #region VerifiedDescription

            curField = TranslationCaseFields.VerifiedDescription.ToString();
            var attrVerifiedDescription = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.VerifiedDescription.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.VerifiedDescription.StatusType
            };
            ret.Add(attrVerifiedDescription);

            #endregion

            #region SolutionRate

            curField = TranslationCaseFields.SolutionRate.ToString();
            var attrSolutionRate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.SolutionRate.DefaultItem,
                Selected = basicInformation.SolutionRate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.SolutionRate.StatusType,
                Items = basicInformation.SolutionRate.Items
            };
            ret.Add(attrSolutionRate);

            #endregion

            #endregion

            // *** Log Info ***
            #region Log Info

            #region ExternalLog

            curField = TranslationCaseFields.tblLog_Text_External.ToString();
            var attrExternalLog = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ExternalLog.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ExternalLog.StatusType
            };
            ret.Add(attrExternalLog);

            #endregion

            #region Internal Log

            curField = TranslationCaseFields.tblLog_Text_Internal.ToString();
            var attrInternalLog = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InternalLog.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InternalLog.StatusType
            };
            ret.Add(attrInternalLog);

            #endregion 

            #region FinishingDescription

            curField = TranslationCaseFields.FinishingDescription.ToString();
            var attrFinishingDescription = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.FinishingDescription.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.FinishingDescription.StatusType
            };
            ret.Add(attrFinishingDescription);

            #endregion

            #region Log File

            curField = TranslationCaseFields.tblLog_Filename.ToString();
            var attrLogFile = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.ButtonField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.LogFile.StatusType
            };
            ret.Add(attrLogFile);

            #endregion

            #region Log Internal File

            curField = TranslationCaseFields.tblLog_Filename_Internal.ToString();
            var attrLogInternalFile = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.ButtonField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.LogFileInternal.StatusType
            };
            ret.Add(attrLogInternalFile);

            #endregion

            #region FinishingDate

            curField = TranslationCaseFields.FinishingDate.ToString();
            var attrFinishingDate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.FinishingDate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.FinishingDate.StatusType
            };
            ret.Add(attrFinishingDate);

            #endregion

            #region ClosingReason

            curField = TranslationCaseFields.ClosingReason.ToString();
            var attrClosingReason = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TreeButtonSelect,
                DefaultItem = basicInformation.ClosingReason.DefaultItem,
                Selected = basicInformation.ClosingReason.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ClosingReason.StatusType,
                Items = basicInformation.ClosingReason.Items
            };
            ret.Add(attrClosingReason);

            #endregion

            #endregion

            // *** Virtual Data ***
            #region Virtual Data

            #region Priority_Impact_Urgent

            curField = VirtualFields.Priority_Impact_Urgent.ToString();
            var resultField = TranslationCaseFields.Priority_Id.ToString(); 

            var attrVirtual1 = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(resultField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = true,
                IsAvailableOnSelfService = true,
                IsMandatory = false,
                Items = basicInformation.Priority_Impact_Urgent.Items,
                StatusType = CaseFieldStatusType.Readonly
            };
            ret.Add(attrVirtual1);

            #endregion

            #region Department_WatchDate

            curField = VirtualFields.Department_WatchDate.ToString();
            var resultField_WD = TranslationCaseFields.WatchDate.ToString();

            var attrVirtual2 = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(resultField_WD, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = true,
                IsAvailableOnSelfService = true,
                IsMandatory = false,
                Items = basicInformation.Department_WatchDate.Items,
                StatusType = CaseFieldStatusType.Readonly
            };
            ret.Add(attrVirtual2);

            #endregion

            #endregion

            return ret;

        }        
    }
}