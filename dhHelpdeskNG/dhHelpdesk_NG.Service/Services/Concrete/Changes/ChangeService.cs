using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Services.Services.Concrete.Changes;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums;
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using BusinessData.Models.Shared;
    using BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Enums;
    using Dal.NewInfrastructure;
    using Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using Dal.Repositories.Inventory;
    using Domain;
    using DH.Helpdesk.Domain.Changes;
    using BusinessLogic.BusinessModelAuditors;
    using BusinessLogic.BusinessModelExport;
    using BusinessLogic.BusinessModelExport.ExcelExport;
    using BusinessLogic.BusinessModelMappers;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Changes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Changes;
    using BusinessLogic.Mappers.Customers;
    using BusinessLogic.Specifications.Customers;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Response.Changes;

    using Log = DH.Helpdesk.BusinessData.Models.Changes.Output.Log;

    public sealed class ChangeService : BaseChangesService, IChangeService
    {
        #region Fields

        private readonly List<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>> _changeAuditors; 

        private readonly IChangeCategoryRepository _changeCategoryRepository;

        private readonly IChangeChangeGroupRepository _changeChangeGroupRepository;

        private readonly IChangeChangeRepository _changeChangeRepository;

        private readonly IChangeDepartmentRepository _changeDepartmentRepository;

        private readonly IChangeEmailLogRepository _changeEmailLogRepository;

        private readonly IChangeFieldSettingRepository _changeFieldSettingRepository;

        private readonly IChangeFileRepository _changeFileRepository;

        private readonly IChangeGroupRepository _changeGroupRepository;

        private readonly IChangeHistoryRepository _changeHistoryRepository;

        private readonly IChangeImplementationStatusRepository _changeImplementationStatusRepository;

        private readonly IChangeLogRepository _changeLogRepository;

        private readonly IChangeLogic _changeLogic;

        private readonly IChangeObjectRepository _changeObjectRepository;

        private readonly IChangePriorityRepository _changePriorityRepository;

        private readonly IChangeRestorer _changeRestorer;

        private readonly IChangeStatusRepository _changeStatusRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly IDepartmentRepository _departmentRepository;

        private readonly IEmailGroupEmailRepository _emailGroupEmailRepository;

        private readonly IEmailGroupRepository _emailGroupRepository;

        private readonly ILanguageRepository _languageRepository;

        private readonly ISystemRepository _systemRepository;

        private readonly IUpdateChangeRequestValidator _updateChangeRequestValidator;

        private readonly IUserRepository _userRepository;

        private readonly IUserWorkingGroupRepository _userWorkingGroupRepository;

        private readonly IWorkingGroupRepository _workingGroupRepository;

        private readonly IChangeContactRepository _changeContactRepository;

        private readonly IBusinessModelsMapper<UpdateChangeRequest, History> _changeToChangeHistoryMapper;

        private readonly IExcelFileComposer _excelFileComposer;

        private readonly IBusinessModelsMapper<ChangeOverviewSettings, List<ExcelTableHeader>>
            _changeOverviewSettingsToExcelHeadersMapper;

        private readonly IEntityToBusinessModelMapper<ChangeEntity, Change> _changeEntityToChangeMapper;

        private readonly IBusinessModelsMapper<ChangeDetailedOverview, BusinessItem> _changeToBusinessItemMapper;
        private readonly IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> _updatedChangeToChangeEntityMapper;

        private readonly IExportFileNameFormatter _exportFileNameFormatter;

        private readonly IInventoryTypeRepository _inventoryTypeRepository;

        private readonly IChangeCouncilRepository _changeCouncilRepository;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        #endregion

        #region Constructors and Destructors

        public ChangeService(
            IChangeCategoryRepository changeCategoryRepository,
            IChangeChangeGroupRepository changeChangeGroupRepository,
            IChangeChangeRepository changeChangeRepository,
            IChangeDepartmentRepository changeDepartmentRepository,
            IChangeEmailLogRepository changeEmailLogRepository,
            IChangeFieldSettingRepository changeFieldSettingRepository,
            IChangeFileRepository changeFileRepository,
            IChangeGroupRepository changeGroupRepository,
            IChangeHistoryRepository changeHistoryRepository,
            IChangeImplementationStatusRepository changeImplementationStatusRepository,
            IChangeLogRepository changeLogRepository,
            IChangeLogic changeLogic,
            IChangeObjectRepository changeObjectRepository,
            IChangePriorityRepository changePriorityRepository,
            IChangeRepository changeRepository,
            IChangeStatusRepository changeStatusRepository,
            ICurrencyRepository currencyRepository,
            IDepartmentRepository departmentRepository,
            IEmailGroupEmailRepository emailGroupEmailRepository,
            IEmailGroupRepository emailGroupRepository,
            ILanguageRepository languageRepository,
            ISystemRepository systemRepository,
            IUserRepository userRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUpdateChangeRequestValidator updateChangeRequestValidator,
            IChangeRestorer changeRestorer,
            IChangeContactRepository changeContactRepository,
            IBusinessModelsMapper<UpdateChangeRequest, History> changeToChangeHistoryMapper,
            List<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>> changeAuditors,
            IExcelFileComposer excelFileComposer,
            IBusinessModelsMapper<ChangeOverviewSettings, List<ExcelTableHeader>> changeOverviewSettingsToExcelHeadersMapper,
            IBusinessModelsMapper<ChangeDetailedOverview, BusinessItem> changeToBusinessItemMapper,
            IExportFileNameFormatter exportFileNameFormatter,
            IInventoryTypeRepository inventoryTypeRepository,
            IChangeCouncilRepository changeCouncilRepository, 
            IUnitOfWorkFactory unitOfWorkFactory, IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper, IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper) : base(changeRepository)
        {
            
            _changeCategoryRepository = changeCategoryRepository;
            _changeChangeGroupRepository = changeChangeGroupRepository;
            _changeChangeRepository = changeChangeRepository;
            _changeDepartmentRepository = changeDepartmentRepository;
            _changeEmailLogRepository = changeEmailLogRepository;
            _changeFieldSettingRepository = changeFieldSettingRepository;
            _changeFileRepository = changeFileRepository;
            _changeGroupRepository = changeGroupRepository;
            _changeHistoryRepository = changeHistoryRepository;
            _changeImplementationStatusRepository = changeImplementationStatusRepository;
            _changeLogRepository = changeLogRepository;
            _changeLogic = changeLogic;
            _changeObjectRepository = changeObjectRepository;
            _changePriorityRepository = changePriorityRepository;
            _changeStatusRepository = changeStatusRepository;
            _currencyRepository = currencyRepository;
            _departmentRepository = departmentRepository;
            _emailGroupEmailRepository = emailGroupEmailRepository;
            _emailGroupRepository = emailGroupRepository;
            _languageRepository = languageRepository;
            _systemRepository = systemRepository;
            _userRepository = userRepository;
            _userWorkingGroupRepository = userWorkingGroupRepository;
            _workingGroupRepository = workingGroupRepository;
            _updateChangeRequestValidator = updateChangeRequestValidator;
            _changeRestorer = changeRestorer;
            _changeContactRepository = changeContactRepository;
            _changeToChangeHistoryMapper = changeToChangeHistoryMapper;
            _changeAuditors = changeAuditors;
            _excelFileComposer = excelFileComposer;
            _changeOverviewSettingsToExcelHeadersMapper = changeOverviewSettingsToExcelHeadersMapper;
            _changeToBusinessItemMapper = changeToBusinessItemMapper;
            _exportFileNameFormatter = exportFileNameFormatter;
            _inventoryTypeRepository = inventoryTypeRepository;
            _changeCouncilRepository = changeCouncilRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _updatedChangeToChangeEntityMapper = updatedChangeToChangeEntityMapper;
            _changeEntityToChangeMapper = changeEntityToChangeMapper;
        }

        #endregion

        #region Public Methods and Operators

        public ExcelFile ExportChangesToExcelFile(SearchParameters parameters, OperationContext context)
        {
            var changes = _changeRepository.Search(parameters);
            var overviewSettings = GetChangeOverviewSettings(parameters.CustomerId, context.LanguageId, false);

            var affectedProcesses = _changeGroupRepository.FindOverviews(context.CustomerId);
            overviewSettings.Registration.AffectedProcessesOverviews = affectedProcesses;
            List<ExcelTableHeader> headers = _changeOverviewSettingsToExcelHeadersMapper.Map(overviewSettings);
            List<BusinessItem> businessItems = changes.Changes.Select(
                                c =>
                                {
                                    c.Registration.AffectedProcessesOverviews = affectedProcesses;
                                    return _changeToBusinessItemMapper.Map(c);
                                }).ToList();

            const string WorksheetName = "Changes";
            var content = _excelFileComposer.Compose(headers, businessItems, WorksheetName);

            var fileName = _exportFileNameFormatter.Format("Changes", "xlsx");
            return new ExcelFile(content, fileName);
        }

        public void AddChange(NewChangeRequest request)
        {
            if (request.Change.Orderer == null)
            {
                request.Change.InitializeOrdererPartWithDefaultValues();
            }

            if (request.Change.General == null)
            {
                request.Change.InitializeGeneralPartWithDefaultValues(request.Context);
            }

            request.Change.InitializeAnalyzePartWithDefaultValues();
            request.Change.InitializeImplementationPartWithDefautValues();
            request.Change.InitializeEvaluationPathWithDefaultValues();

            _changeRepository.AddChange(request.Change);
            _changeRepository.Commit();

            request.Contacts.ForEach(c => c.ChangeId = request.Change.Id);
            _changeContactRepository.AddContacts(request.Contacts);
            _changeContactRepository.Commit();

            _changeChangeGroupRepository.AddChangeProcesses(request.Change.Id, request.AffectedProcessIds);
            _changeChangeGroupRepository.Commit();

            _changeDepartmentRepository.AddChangeDepartments(request.Change.Id, request.AffectedDepartmentIds);
            _changeDepartmentRepository.Commit();

            request.NewFiles.ForEach(f => f.ChangeId = request.Change.Id);
            _changeFileRepository.AddFiles(request.NewFiles);
            _changeFileRepository.Commit();

            foreach (var newLog in request.NewLogs)
            {
                newLog.ChangeId = request.Change.Id;
                newLog.CreatedByUserId = request.Context.UserId;
                newLog.CreatedDateAndTime = request.Context.DateAndTime;
            }

            _changeLogRepository.AddLogs(request.NewLogs);
            _changeLogRepository.Commit();
        }

        public void DeleteChange(int changeId)
        {
            _changeLogRepository.DeleteByChangeId(changeId);
            _changeLogRepository.Commit();

            var historyIds = _changeHistoryRepository.FindIdsByChangeId(changeId);

            _changeCouncilRepository.DeleteByChangeId(changeId);
            _changeContactRepository.Commit();

            _changeEmailLogRepository.DeleteByHistoryIds(historyIds);
            _changeEmailLogRepository.Commit();

            _changeHistoryRepository.DeleteByChangeId(changeId);
            _changeHistoryRepository.Commit();

            _changeChangeRepository.DeleteReferencesToChange(changeId);
            _changeChangeRepository.Commit();
            
            _changeChangeGroupRepository.ResetChangeRelatedProcesses(changeId);
            _changeChangeGroupRepository.Commit();

            _changeDepartmentRepository.ResetChangeRelatedDepartments(changeId);
            _changeDepartmentRepository.Commit();

            _changeFileRepository.DeleteChangeFiles(changeId);
            _changeFileRepository.Commit();

            _changeContactRepository.DeleteChangeContacts(changeId);
            _changeContactRepository.Commit();

            _changeRepository.DeleteById(changeId);
            _changeRepository.Commit();
        }

        public bool FileExists(int changeId, Subtopic subtopic, string fileName)
        {
            return _changeFileRepository.FileExists(changeId, subtopic, fileName);
        }

        public FindChangeResponse FindChange(int changeId, OperationContext context)
        {
            var change = _changeEntityToChangeMapper.Map(_changeRepository.FindById(changeId));
            if (change == null)
            {
                return null;
            }

            var contacts = _changeContactRepository.FindChangeContacts(changeId);
            var affectedProcessIds = _changeChangeGroupRepository.FindProcessIdsByChangeId(changeId);
            var affectedDepartmentIds = _changeDepartmentRepository.FindDepartmentIdsByChangeId(changeId);
            var relatedChangeIds = _changeChangeRepository.FindRelatedChangeIdsByChangeId(changeId);
            var files = _changeFileRepository.FindFilesByChangeId(changeId);
            var logs = _changeLogRepository.FindLogsByChangeId(changeId);

            var histories = _changeHistoryRepository.FindHistoriesByChangeId(changeId);
            var historyIds = histories.Select(i => i.Id).ToList();
            var logOverviews = _changeLogRepository.FindOverviewsByHistoryIds(historyIds);
            var emailLogs = _changeEmailLogRepository.FindOverviewsByHistoryIds(historyIds);

            var historyDifferences = _changeLogic.AnalyzeHistoriesDifferences(histories, logOverviews, emailLogs);

            var editData = new ChangeEditData
                           {
                               Change = change,
                               Contacts = contacts,
                               AffectedDepartmentIds = affectedDepartmentIds,
                               AffectedProcessIds = affectedProcessIds,
                               Files = files,
                               Histories = historyDifferences,
                               Logs = logs,
                               RelatedChangeIds = relatedChangeIds
                           };

            var settings = GetChangeEditSettings(context.CustomerId, context.LanguageId);
            var options = GetChangeEditData(changeId, settings, context);

            return new FindChangeResponse(
                editData,
                settings,
                options);
        }

        public List<string> FindChangeFileNamesExcludeDeleted(int changeId, Subtopic subtopic, List<string> excludeFiles)
        {
            return _changeFileRepository.FindFileNamesExcludeSpecified(changeId, subtopic, excludeFiles);
        }

        public List<Log> FindChangeLogsExcludeSpecified(int changeId, Subtopic subtopic, List<int> excludeLogIds)
        {
            return _changeLogRepository.FindLogsExcludeSpecified(changeId, subtopic, excludeLogIds);
        }

        public GetSettingsResponse GetSettings(int languageId, OperationContext context)
        {
            var languageTextId = _languageRepository.GetLanguageTextIdById(languageId);
            ChangeFieldSettings settings;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    settings = _changeFieldSettingRepository.GetSwedishFieldSettings(context.CustomerId);
                    break;
                case LanguageTextId.English:
                    settings = _changeFieldSettingRepository.GetEnglishFieldSettings(context.CustomerId);
                    break;
                default:
                    settings = _changeFieldSettingRepository.GetEnglishFieldSettings(context.CustomerId);
                    break;
            }

            var languagesEntity = _languageRepository.GetActiveLanguages();            
            var languages = _languageRepository.FindActiveOverviewsByIds(languagesEntity.Select(l=> l.Id).ToList());

            return new GetSettingsResponse(settings, languages);
        }

        public ChangeEditOptions GetChangeEditData(int changeId, ChangeEditSettings settings, OperationContext context)
        {
            var editData = GetChangeEditDataCore(context.CustomerId, context.LanguageId, settings);
            var relatedChanges = _changeRepository.FindOverviewsExcludeSpecified(context.CustomerId, changeId);
            editData.RelatedChanges = relatedChanges
                                        .Select(c => new ItemOverview(string.Format("#{0} - {1}", c.Value, c.Name), c.Value))
                                        .ToList();
            return editData;
        }

        public ChangeEditSettings GetChangeEditSettings(int customerId, int languageId)
        {
            var languageTextId = _languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return _changeFieldSettingRepository.GetSwedishEditSettings(customerId);
                case LanguageTextId.English:
                    return _changeFieldSettingRepository.GetEnglishEditSettings(customerId);
                default:
                    return _changeFieldSettingRepository.GetEnglishEditSettings(customerId);
            }
        }
        

        public byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName)
        {
            return _changeFileRepository.GetFileContent(changeId, subtopic, fileName);
        }

        public GetNewChangeEditDataResponse GetNewChangeEditData(OperationContext context)
        {
            var settings = GetChangeEditSettings(context.CustomerId, context.LanguageId);
            var editData = GetChangeEditDataCore(context.CustomerId, context.LanguageId, settings);
            var relatedChanges = _changeRepository.FindOverviews(context.CustomerId);
            editData.RelatedChanges = relatedChanges;

            return new GetNewChangeEditDataResponse(settings, editData);
        }

        public GetSearchDataResponse GetSearchData(OperationContext context)
        {
            var statuses = _changeStatusRepository.FindOverviews(context.CustomerId);
            var objects = _changeObjectRepository.FindOverviews(context.CustomerId);
            var changeGroups = _changeGroupRepository.FindOverviews(context.CustomerId);
            var owners = changeGroups;
            var affectedProcesses = changeGroups;
            var workingGroups = _workingGroupRepository.FindActiveOverviews(context.CustomerId);
            var administrators = _userRepository.FindUsersWithPermissionsForCustomers(new[] { context.CustomerId });
            var responsibles = _userRepository.FindUsersWithPermissionsForCustomers(new[] { context.CustomerId });

            var settings = GetSearchSettings(context.CustomerId, context.LanguageId);
            var options = new SearchOptions(statuses, objects, owners, affectedProcesses, workingGroups, administrators, responsibles);

            return new GetSearchDataResponse(settings, options);
        }

        public SearchResponse Search(SearchParameters parameters, OperationContext context)
        {
            var result = _changeRepository.Search(parameters);
            var settings = GetChangeOverviewSettings(context.CustomerId, context.LanguageId, true);
            return new SearchResponse(result, settings);
        }

        public void UpdateChange(UpdateChangeRequest request)
        {
            var existingChange = _changeEntityToChangeMapper.Map(_changeRepository.GetById(request.Change.Id));
            var processingSettings = _changeFieldSettingRepository.GetProcessingSettings(request.Context.CustomerId);
            _changeRestorer.Restore(request.Change, existingChange, processingSettings);

            _updateChangeRequestValidator.Validate(request, existingChange, processingSettings);

            var changeEntity = _changeRepository.FindById(request.Change.Id);
            _updatedChangeToChangeEntityMapper.Map(request.Change, changeEntity);
            _changeRepository.Commit();

            var newContacts = request.Contacts.Where(c => c.State == ModelStates.Created).ToList();
            newContacts.ForEach(c => c.ChangeId = request.Change.Id);
            _changeContactRepository.AddContacts(newContacts);
            var updatedContact = request.Contacts.Where(c => c.State == ModelStates.Updated).ToList();
            _changeContactRepository.UpdateContacts(updatedContact);
            _changeContactRepository.Commit();

            _changeChangeGroupRepository.UpdateChangeProcesses(request.Change.Id, request.AffectedProcessIds);
            _changeChangeGroupRepository.Commit();

            _changeDepartmentRepository.UpdateChangeDepartments(request.Change.Id, request.AffectedDepartmentIds);
            _changeDepartmentRepository.Commit();

            _changeChangeRepository.UpdateRelatedChanges(request.Change.Id, request.RelatedChangeIds);
            _changeChangeRepository.Commit();

            request.DeletedFiles.ForEach(f => f.ChangeId = request.Change.Id);
            _changeFileRepository.DeleteFiles(request.DeletedFiles);
            _changeFileRepository.Commit();

            request.NewFiles.ForEach(f => f.ChangeId = request.Change.Id);
            _changeFileRepository.AddFiles(request.NewFiles);
            _changeFileRepository.Commit();

            _changeLogRepository.DeleteByIds(request.DeletedLogIds);
            _changeLogRepository.Commit();

            var history = _changeToChangeHistoryMapper.Map(request);
            _changeHistoryRepository.AddHistory(history);
            _changeHistoryRepository.Commit();

            foreach (var newLog in request.NewLogs)
            {
                newLog.ChangeId = request.Change.Id;
                newLog.ChangeHistoryId = history.Id;
                newLog.CreatedByUserId = request.Context.UserId;
                newLog.CreatedDateAndTime = request.Context.DateAndTime;
            }

            var logsForSave = request.NewLogs.Any(l => l.Subtopic == Subtopic.Analyze)
                                  ? request.NewLogs.Where(l => l.Subtopic != Subtopic.InviteToCab).ToList()
                                  : request.NewLogs;

            _changeLogRepository.AddLogs(logsForSave);
            _changeLogRepository.Commit();

            var auditOptionalData = new ChangeAuditData(history.Id, existingChange);
            _changeAuditors.ForEach(a => a.Audit(request, auditOptionalData));
        }

        public void UpdateSettings(ChangeFieldSettings settings)
        {
            _changeFieldSettingRepository.UpdateSettings(settings);
            _changeFieldSettingRepository.Commit();
        }

        public ChangeOverview GetChangeOverview(int id)
        {
            return _changeRepository.GetChangeOverview(id);
        }

        public CustomerChanges[] GetCustomerChanges(int[] customerIds, int userId)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var customerRepository = uow.GetRepository<Customer>();

                var customersHaveResponsible = new List<int>();
                foreach (int customerId in customerIds)                
                {
                   var curSettings = GetChangeEditSettings(customerId, LanguageIds.Swedish);
                   if (curSettings.Analyze.Responsible.Show)
                        customersHaveResponsible.Add(customerId);
                }                                

                var customerChanges = customerRepository.GetAll()
                                    .GetByIds(customerIds)
                                    .MapToCustomerChanges(userId, customersHaveResponsible);

                return customerChanges;
            }
        }

        #endregion

        #region Methods

        private ChangeOverviewSettings GetChangeOverviewSettings(int customerId, int languageId, bool onlyListSettings)
        {
            var languageTextId = _languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return _changeFieldSettingRepository.GetSwedishOverviewSettings(customerId, onlyListSettings);
                case LanguageTextId.English:
                    return _changeFieldSettingRepository.GetEnglishOverviewSettings(customerId, onlyListSettings);
                default:
                    return _changeFieldSettingRepository.GetEnglishOverviewSettings(customerId, onlyListSettings);
            }
        }

        private SearchSettings GetSearchSettings(int customerId, int languageId)
        {
            var languageTextId = _languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return _changeFieldSettingRepository.GetSwedishSearchSettings(customerId);
                    
                default:
                    return _changeFieldSettingRepository.GetEnglishSearchSettings(customerId);                    
            }
        }

        private ChangeEditOptions GetChangeEditDataCore(int customerId, int languageId, ChangeEditSettings settings)
        {
            List<ItemOverview> departments = null;
            List<ItemOverview> statuses = null;
            List<ItemOverview> systems = null;
            List<ItemOverview> objects = null;
            List<InventoryTypeWithInventories> inventoryTypesWithInventories = null;
            List<ItemOverview> workingGroups = null;
            List<ItemOverview> users = null;
            List<ItemOverview> changeGroups = null;
            List<ItemOverview> categories = null;
            List<ItemOverview> priorities = null;
            List<ItemOverview> currencies = null;
            List<ItemOverview> implementationStatuses = null;
            List<ItemOverview> administrators = null;

            List<GroupWithEmails> workingGroupsWithEmails = null;
            List<GroupWithEmails> emailGroupsWithEmails = null;
            List<ItemOverview> administratorsWithEmails = null;

            if (settings.Orderer.Department.Show || settings.Registration.AffectedDepartments.Show)
            {
                departments = _departmentRepository.FindActiveOverviews(customerId);
            }

            if (settings.General.Status.Show)
            {
                statuses = _changeStatusRepository.FindOverviews(customerId);
            }

            if (settings.General.System.Show)
            {
                systems = _systemRepository.FindOverviews(customerId);
            }

            if (settings.General.Object.Show)
            {
                objects = _changeObjectRepository.FindOverviews(customerId);
            }

            if (settings.General.Inventory.Show)
            {
                inventoryTypesWithInventories =
                    _inventoryTypeRepository.FindInventoryTypesWithInventories(customerId, languageId);
            }

            if (settings.General.WorkingGroup.Show)
            {
                workingGroups = _workingGroupRepository.FindActiveOverviews(customerId);
            }

            if (settings.General.Administrator.Show || settings.Analyze.Responsible.Show)
            {
                users = _userRepository.FindUsersWithPermissionsForCustomers(new[] { customerId });
            }

            if (settings.Registration.Owner.Show || settings.Registration.AffectedProcesses.Show)
            {
                changeGroups = _changeGroupRepository.FindOverviews(customerId);
            }

            if (settings.Analyze.Category.Show)
            {
                categories = _changeCategoryRepository.FindOverviews(customerId);
            }

            if (settings.Analyze.Priority.Show)
            {
                priorities = _changePriorityRepository.FindOverviews(customerId);
            }

            if (settings.Analyze.Cost.Show || settings.Analyze.YearlyCost.Show)
            {
                currencies = _currencyRepository.FindOverviews();
            }

            if (settings.Implementation.Status.Show)
            {
                implementationStatuses = _changeImplementationStatusRepository.FindOverviews(customerId);
            }

            if (settings.General.Administrator.Show)
            {
                administrators = _userRepository.FindUsersWithPermissionsForCustomers(new[] { customerId });
            }

            if (settings.Analyze.Logs.Show || settings.Implementation.Logs.Show || settings.Evaluation.Logs.Show)
            {
                var workingGroupOverviews = _workingGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var workingGroupIds = workingGroupOverviews.Select(g => g.Id).ToList();
                var workingGroupsUserIds = _userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
                var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
                var userIdsWithEmails = _userRepository.FindUsersEmails(userIds);

                workingGroupsWithEmails = new List<GroupWithEmails>(workingGroupOverviews.Count);

                foreach (var workingGroupOverview in workingGroupOverviews)
                {
                    var groupUserIdsWithEmails =
                        workingGroupsUserIds.Single(g => g.WorkingGroupId == workingGroupOverview.Id);

                    var groupEmails =
                        userIdsWithEmails.Where(e => groupUserIdsWithEmails.UserIds.Contains(e.ItemId))
                            .Select(e => e.Email)
                            .ToList();

                    var groupWithEmails = new GroupWithEmails(
                        workingGroupOverview.Id,
                        workingGroupOverview.Name,
                        groupEmails);

                    workingGroupsWithEmails.Add(groupWithEmails);
                }

                var emailGroups = _emailGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var emailGroupIds = emailGroups.Select(g => g.Id).ToList();
                var emailGroupsEmails = _emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

                emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

                foreach (var emailGroup in emailGroups)
                {
                    var groupEmails = emailGroupsEmails.Single(e => e.ItemId == emailGroup.Id).Emails;
                    var groupWithEmails = new GroupWithEmails(emailGroup.Id, emailGroup.Name, groupEmails);

                    emailGroupsWithEmails.Add(groupWithEmails);
                }

                administratorsWithEmails = _userRepository.FindActiveUsersIncludeEmails(customerId);
            }

            return new ChangeEditOptions(
                departments,
                statuses,
                systems,
                objects,
                inventoryTypesWithInventories,
                workingGroups,
                workingGroupsWithEmails,
                administratorsWithEmails,
                changeGroups,
                changeGroups,
                departments,
                categories,
                priorities,
                users,
                currencies,
                emailGroupsWithEmails,
                implementationStatuses,
                administrators);
        }

        #endregion
    }
}