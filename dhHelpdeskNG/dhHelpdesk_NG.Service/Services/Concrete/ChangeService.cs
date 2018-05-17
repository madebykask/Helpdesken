namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums;
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Dal.Repositories.Inventory;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelMappers;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Changes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Customers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Customers;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Response.Changes;

    using Log = DH.Helpdesk.BusinessData.Models.Changes.Output.Log;

    public sealed class ChangeService : IChangeService
    {
        #region Fields

        private readonly List<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>> changeAuditors; 

        private readonly IChangeCategoryRepository changeCategoryRepository;

        private readonly IChangeChangeGroupRepository changeChangeGroupRepository;

        private readonly IChangeChangeRepository changeChangeRepository;

        private readonly IChangeDepartmentRepository changeDepartmentRepository;

        private readonly IChangeEmailLogRepository changeEmailLogRepository;

        private readonly IChangeFieldSettingRepository changeFieldSettingRepository;

        private readonly IChangeFileRepository changeFileRepository;

        private readonly IChangeGroupRepository changeGroupRepository;

        private readonly IChangeHistoryRepository changeHistoryRepository;

        private readonly IChangeImplementationStatusRepository changeImplementationStatusRepository;

        private readonly IChangeLogRepository changeLogRepository;

        private readonly IChangeLogic changeLogic;

        private readonly IChangeObjectRepository changeObjectRepository;

        private readonly IChangePriorityRepository changePriorityRepository;

        private readonly IChangeRepository changeRepository;

        private readonly IChangeRestorer changeRestorer;

        private readonly IChangeStatusRepository changeStatusRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly IDepartmentRepository departmentRepository;

        private readonly IEmailGroupEmailRepository emailGroupEmailRepository;

        private readonly IEmailGroupRepository emailGroupRepository;

        private readonly ILanguageRepository languageRepository;

        private readonly ISystemRepository systemRepository;

        private readonly IUpdateChangeRequestValidator updateChangeRequestValidator;

        private readonly IUserRepository userRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IChangeContactRepository changeContactRepository;

        private readonly IBusinessModelsMapper<UpdateChangeRequest, History> changeToChangeHistoryMapper;

        private readonly IExcelFileComposer excelFileComposer;

        private readonly IBusinessModelsMapper<ChangeOverviewSettings, List<ExcelTableHeader>>
            changeOverviewSettingsToExcelHeadersMapper;

        private readonly IBusinessModelsMapper<ChangeDetailedOverview, BusinessItem> changeToBusinessItemMapper;

        private readonly IExportFileNameFormatter exportFileNameFormatter;

        private readonly IInventoryTypeRepository inventoryTypeRepository;

        private readonly IChangeCouncilRepository changeCouncilRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

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
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.changeCategoryRepository = changeCategoryRepository;
            this.changeChangeGroupRepository = changeChangeGroupRepository;
            this.changeChangeRepository = changeChangeRepository;
            this.changeDepartmentRepository = changeDepartmentRepository;
            this.changeEmailLogRepository = changeEmailLogRepository;
            this.changeFieldSettingRepository = changeFieldSettingRepository;
            this.changeFileRepository = changeFileRepository;
            this.changeGroupRepository = changeGroupRepository;
            this.changeHistoryRepository = changeHistoryRepository;
            this.changeImplementationStatusRepository = changeImplementationStatusRepository;
            this.changeLogRepository = changeLogRepository;
            this.changeLogic = changeLogic;
            this.changeObjectRepository = changeObjectRepository;
            this.changePriorityRepository = changePriorityRepository;
            this.changeRepository = changeRepository;
            this.changeStatusRepository = changeStatusRepository;
            this.currencyRepository = currencyRepository;
            this.departmentRepository = departmentRepository;
            this.emailGroupEmailRepository = emailGroupEmailRepository;
            this.emailGroupRepository = emailGroupRepository;
            this.languageRepository = languageRepository;
            this.systemRepository = systemRepository;
            this.userRepository = userRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.updateChangeRequestValidator = updateChangeRequestValidator;
            this.changeRestorer = changeRestorer;
            this.changeContactRepository = changeContactRepository;
            this.changeToChangeHistoryMapper = changeToChangeHistoryMapper;
            this.changeAuditors = changeAuditors;
            this.excelFileComposer = excelFileComposer;
            this.changeOverviewSettingsToExcelHeadersMapper = changeOverviewSettingsToExcelHeadersMapper;
            this.changeToBusinessItemMapper = changeToBusinessItemMapper;
            this.exportFileNameFormatter = exportFileNameFormatter;
            this.inventoryTypeRepository = inventoryTypeRepository;
            this.changeCouncilRepository = changeCouncilRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion

        #region Public Methods and Operators

        public ExcelFile ExportChangesToExcelFile(SearchParameters parameters, OperationContext context)
        {
            var changes = this.changeRepository.Search(parameters);
            var overviewSettings = this.GetChangeOverviewSettings(parameters.CustomerId, context.LanguageId, false);

            var affectedProcesses = this.changeGroupRepository.FindOverviews(context.CustomerId);
            overviewSettings.Registration.AffectedProcessesOverviews = affectedProcesses;
            List<ExcelTableHeader> headers = this.changeOverviewSettingsToExcelHeadersMapper.Map(overviewSettings);
            List<BusinessItem> businessItems = changes.Changes.Select(
                                c =>
                                {
                                    c.Registration.AffectedProcessesOverviews = affectedProcesses;
                                    return this.changeToBusinessItemMapper.Map(c);
                                }).ToList();

            const string WorksheetName = "Changes";
            var content = this.excelFileComposer.Compose(headers, businessItems, WorksheetName);

            var fileName = this.exportFileNameFormatter.Format("Changes", "xlsx");
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

            this.changeRepository.AddChange(request.Change);
            this.changeRepository.Commit();

            request.Contacts.ForEach(c => c.ChangeId = request.Change.Id);
            this.changeContactRepository.AddContacts(request.Contacts);
            this.changeContactRepository.Commit();

            this.changeChangeGroupRepository.AddChangeProcesses(request.Change.Id, request.AffectedProcessIds);
            this.changeChangeGroupRepository.Commit();

            this.changeDepartmentRepository.AddChangeDepartments(request.Change.Id, request.AffectedDepartmentIds);
            this.changeDepartmentRepository.Commit();

            request.NewFiles.ForEach(f => f.ChangeId = request.Change.Id);
            this.changeFileRepository.AddFiles(request.NewFiles);
            this.changeFileRepository.Commit();

            foreach (var newLog in request.NewLogs)
            {
                newLog.ChangeId = request.Change.Id;
                newLog.CreatedByUserId = request.Context.UserId;
                newLog.CreatedDateAndTime = request.Context.DateAndTime;
            }

            this.changeLogRepository.AddLogs(request.NewLogs);
            this.changeLogRepository.Commit();
        }

        public void DeleteChange(int changeId)
        {
            this.changeLogRepository.DeleteByChangeId(changeId);
            this.changeLogRepository.Commit();

            var historyIds = this.changeHistoryRepository.FindIdsByChangeId(changeId);

            this.changeCouncilRepository.DeleteByChangeId(changeId);
            this.changeContactRepository.Commit();

            this.changeEmailLogRepository.DeleteByHistoryIds(historyIds);
            this.changeEmailLogRepository.Commit();

            this.changeHistoryRepository.DeleteByChangeId(changeId);
            this.changeHistoryRepository.Commit();

            this.changeChangeRepository.DeleteReferencesToChange(changeId);
            this.changeChangeRepository.Commit();
            
            this.changeChangeGroupRepository.ResetChangeRelatedProcesses(changeId);
            this.changeChangeGroupRepository.Commit();

            this.changeDepartmentRepository.ResetChangeRelatedDepartments(changeId);
            this.changeDepartmentRepository.Commit();

            this.changeFileRepository.DeleteChangeFiles(changeId);
            this.changeFileRepository.Commit();

            this.changeContactRepository.DeleteChangeContacts(changeId);
            this.changeContactRepository.Commit();

            this.changeRepository.DeleteById(changeId);
            this.changeRepository.Commit();
        }

        public bool FileExists(int changeId, Subtopic subtopic, string fileName)
        {
            return this.changeFileRepository.FileExists(changeId, subtopic, fileName);
        }

        public FindChangeResponse FindChange(int changeId, OperationContext context)
        {
            var change = this.changeRepository.FindById(changeId);
            if (change == null)
            {
                return null;
            }

            var contacts = this.changeContactRepository.FindChangeContacts(changeId);
            var affectedProcessIds = this.changeChangeGroupRepository.FindProcessIdsByChangeId(changeId);
            var affectedDepartmentIds = this.changeDepartmentRepository.FindDepartmentIdsByChangeId(changeId);
            var relatedChangeIds = this.changeChangeRepository.FindRelatedChangeIdsByChangeId(changeId);
            var files = this.changeFileRepository.FindFilesByChangeId(changeId);
            var logs = this.changeLogRepository.FindLogsByChangeId(changeId);

            var histories = this.changeHistoryRepository.FindHistoriesByChangeId(changeId);
            var historyIds = histories.Select(i => i.Id).ToList();
            var logOverviews = this.changeLogRepository.FindOverviewsByHistoryIds(historyIds);
            var emailLogs = this.changeEmailLogRepository.FindOverviewsByHistoryIds(historyIds);

            var historyDifferences = this.changeLogic.AnalyzeHistoriesDifferences(histories, logOverviews, emailLogs);

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

            var settings = this.GetChangeEditSettings(context.CustomerId, context.LanguageId);
            var options = this.GetChangeEditData(changeId, settings, context);

            return new FindChangeResponse(
                editData,
                settings,
                options);
        }

        public List<string> FindChangeFileNamesExcludeDeleted(int changeId, Subtopic subtopic, List<string> excludeFiles)
        {
            return this.changeFileRepository.FindFileNamesExcludeSpecified(changeId, subtopic, excludeFiles);
        }

        public List<Log> FindChangeLogsExcludeSpecified(int changeId, Subtopic subtopic, List<int> excludeLogIds)
        {
            return this.changeLogRepository.FindLogsExcludeSpecified(changeId, subtopic, excludeLogIds);
        }

        public GetSettingsResponse GetSettings(int languageId, OperationContext context)
        {
            var languageTextId = this.languageRepository.GetLanguageTextIdById(languageId);
            ChangeFieldSettings settings;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    settings = this.changeFieldSettingRepository.GetSwedishFieldSettings(context.CustomerId);
                    break;
                case LanguageTextId.English:
                    settings = this.changeFieldSettingRepository.GetEnglishFieldSettings(context.CustomerId);
                    break;
                default:
                    settings = this.changeFieldSettingRepository.GetEnglishFieldSettings(context.CustomerId);
                    break;
            }

            var languagesEntity = this.languageRepository.GetActiveLanguages();            
            var languages = this.languageRepository.FindActiveOverviewsByIds(languagesEntity.Select(l=> l.Id).ToList());

            return new GetSettingsResponse(settings, languages);
        }

        public ChangeEditOptions GetChangeEditData(int changeId, ChangeEditSettings settings, OperationContext context)
        {
            var editData = this.GetChangeEditDataCore(context.CustomerId, context.LanguageId, settings);
            var relatedChanges = this.changeRepository.FindOverviewsExcludeSpecified(context.CustomerId, changeId);
            editData.RelatedChanges = relatedChanges
                                        .Select(c => new ItemOverview(string.Format("#{0} - {1}", c.Value, c.Name), c.Value))
                                        .ToList();
            return editData;
        }

        public ChangeEditSettings GetChangeEditSettings(int customerId, int languageId)
        {
            var languageTextId = this.languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.GetSwedishEditSettings(customerId);
                case LanguageTextId.English:
                    return this.changeFieldSettingRepository.GetEnglishEditSettings(customerId);
                default:
                    return this.changeFieldSettingRepository.GetEnglishEditSettings(customerId);
            }
        }

        public ChangeOverviewSettings GetChangeOverviewSettings(int customerId, int languageId, bool onlyListSettings)
        {
            var languageTextId = this.languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.GetSwedishOverviewSettings(customerId, onlyListSettings);
                case LanguageTextId.English:
                    return this.changeFieldSettingRepository.GetEnglishOverviewSettings(customerId, onlyListSettings);
                default:
                    return this.changeFieldSettingRepository.GetEnglishOverviewSettings(customerId, onlyListSettings);
            }
        }

        public IList<ChangeOverview> GetChanges(int customerId)
        {
            return this.changeRepository.GetChanges(customerId).ToList();
        }

        public byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName)
        {
            return this.changeFileRepository.GetFileContent(changeId, subtopic, fileName);
        }

        public GetNewChangeEditDataResponse GetNewChangeEditData(OperationContext context)
        {
            var settings = this.GetChangeEditSettings(context.CustomerId, context.LanguageId);
            var editData = this.GetChangeEditDataCore(context.CustomerId, context.LanguageId, settings);
            var relatedChanges = this.changeRepository.FindOverviews(context.CustomerId);
            editData.RelatedChanges = relatedChanges;

            return new GetNewChangeEditDataResponse(settings, editData);
        }

        public GetSearchDataResponse GetSearchData(OperationContext context)
        {
            var statuses = this.changeStatusRepository.FindOverviews(context.CustomerId);
            var objects = this.changeObjectRepository.FindOverviews(context.CustomerId);
            var changeGroups = this.changeGroupRepository.FindOverviews(context.CustomerId);
            var owners = changeGroups;
            var affectedProcesses = changeGroups;
            var workingGroups = this.workingGroupRepository.FindActiveOverviews(context.CustomerId);
            var administrators = this.userRepository.FindActiveOverviews(context.CustomerId);
            var responsibles = this.userRepository.FindActiveOverviews(context.CustomerId);

            var settings = this.GetSearchSettings(context.CustomerId, context.LanguageId);
            var options = new SearchOptions(statuses, objects, owners, affectedProcesses, workingGroups, administrators, responsibles);

            return new GetSearchDataResponse(settings, options);
        }

        public SearchSettings GetSearchSettings(int customerId, int languageId)
        {
            var languageTextId = this.languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.GetSwedishSearchSettings(customerId);
                    
                default:
                    return this.changeFieldSettingRepository.GetEnglishSearchSettings(customerId);                    
            }
        }

        public SearchResponse Search(SearchParameters parameters, OperationContext context)
        {
            var result = this.changeRepository.Search(parameters);
            var settings = this.GetChangeOverviewSettings(context.CustomerId, context.LanguageId, true);
            return new SearchResponse(result, settings);
        }

        public void UpdateChange(UpdateChangeRequest request)
        {
            var existingChange = this.changeRepository.GetById(request.Change.Id);
            var processingSettings = this.changeFieldSettingRepository.GetProcessingSettings(request.Context.CustomerId);
            this.changeRestorer.Restore(request.Change, existingChange, processingSettings);

            this.updateChangeRequestValidator.Validate(request, existingChange, processingSettings);

            this.changeRepository.Update(request.Change);
            this.changeRepository.Commit();

            var newContacts = request.Contacts.Where(c => c.State == ModelStates.Created).ToList();
            newContacts.ForEach(c => c.ChangeId = request.Change.Id);
            this.changeContactRepository.AddContacts(newContacts);
            var updatedContact = request.Contacts.Where(c => c.State == ModelStates.Updated).ToList();
            this.changeContactRepository.UpdateContacts(updatedContact);
            this.changeContactRepository.Commit();

            this.changeChangeGroupRepository.UpdateChangeProcesses(request.Change.Id, request.AffectedProcessIds);
            this.changeChangeGroupRepository.Commit();

            this.changeDepartmentRepository.UpdateChangeDepartments(request.Change.Id, request.AffectedDepartmentIds);
            this.changeDepartmentRepository.Commit();

            this.changeChangeRepository.UpdateRelatedChanges(request.Change.Id, request.RelatedChangeIds);
            this.changeChangeRepository.Commit();

            request.DeletedFiles.ForEach(f => f.ChangeId = request.Change.Id);
            this.changeFileRepository.DeleteFiles(request.DeletedFiles);
            this.changeFileRepository.Commit();

            request.NewFiles.ForEach(f => f.ChangeId = request.Change.Id);
            this.changeFileRepository.AddFiles(request.NewFiles);
            this.changeFileRepository.Commit();

            this.changeLogRepository.DeleteByIds(request.DeletedLogIds);
            this.changeLogRepository.Commit();

            var history = this.changeToChangeHistoryMapper.Map(request);
            this.changeHistoryRepository.AddHistory(history);
            this.changeHistoryRepository.Commit();

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

            this.changeLogRepository.AddLogs(logsForSave);
            this.changeLogRepository.Commit();

            var auditOptionalData = new ChangeAuditData(history.Id, existingChange);
            this.changeAuditors.ForEach(a => a.Audit(request, auditOptionalData));
        }

        public void UpdateSettings(ChangeFieldSettings settings)
        {
            this.changeFieldSettingRepository.UpdateSettings(settings);
            this.changeFieldSettingRepository.Commit();
        }

        public ChangeOverview GetChangeOverview(int id)
        {
            return this.changeRepository.GetChangeOverview(id);
        }

        public List<CustomerChange> GetCustomersChanges(int[] customersIds)
        {
            return this.changeRepository.GetCustomersChanges(customersIds);
        }

        public CustomerChanges[] GetCustomerChanges(int[] customerIds, int userId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var customerRepository = uow.GetRepository<Customer>();

                var customersHaveResponsible = new List<int>();
                foreach (int customerId in customerIds)                
                {
                   var curSettings = this.GetChangeEditSettings(customerId, LanguageIds.Swedish);
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
                departments = this.departmentRepository.FindActiveOverviews(customerId);
            }

            if (settings.General.Status.Show)
            {
                statuses = this.changeStatusRepository.FindOverviews(customerId);
            }

            if (settings.General.System.Show)
            {
                systems = this.systemRepository.FindOverviews(customerId);
            }

            if (settings.General.Object.Show)
            {
                objects = this.changeObjectRepository.FindOverviews(customerId);
            }

            if (settings.General.Inventory.Show)
            {
                inventoryTypesWithInventories =
                    this.inventoryTypeRepository.FindInventoryTypesWithInventories(customerId, languageId);
            }

            if (settings.General.WorkingGroup.Show)
            {
                workingGroups = this.workingGroupRepository.FindActiveOverviews(customerId);
            }

            if (settings.General.Administrator.Show || settings.Analyze.Responsible.Show)
            {
                users = this.userRepository.FindUsersWithPermissionsForCustomers(new[] { customerId });
            }

            if (settings.Registration.Owner.Show || settings.Registration.AffectedProcesses.Show)
            {
                changeGroups = this.changeGroupRepository.FindOverviews(customerId);
            }

            if (settings.Analyze.Category.Show)
            {
                categories = this.changeCategoryRepository.FindOverviews(customerId);
            }

            if (settings.Analyze.Priority.Show)
            {
                priorities = this.changePriorityRepository.FindOverviews(customerId);
            }

            if (settings.Analyze.Cost.Show || settings.Analyze.YearlyCost.Show)
            {
                currencies = this.currencyRepository.FindOverviews();
            }

            if (settings.Implementation.Status.Show)
            {
                implementationStatuses = this.changeImplementationStatusRepository.FindOverviews(customerId);
            }

            if (settings.General.Administrator.Show)
            {
                administrators = this.userRepository.FindUsersWithPermissionsForCustomers(new[] { customerId });
            }

            if (settings.Analyze.Logs.Show || settings.Implementation.Logs.Show || settings.Evaluation.Logs.Show)
            {
                var workingGroupOverviews = this.workingGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var workingGroupIds = workingGroupOverviews.Select(g => g.Id).ToList();
                var workingGroupsUserIds = this.userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
                var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
                var userIdsWithEmails = this.userRepository.FindUsersEmails(userIds);

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

                var emailGroups = this.emailGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var emailGroupIds = emailGroups.Select(g => g.Id).ToList();
                var emailGroupsEmails = this.emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

                emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

                foreach (var emailGroup in emailGroups)
                {
                    var groupEmails = emailGroupsEmails.Single(e => e.ItemId == emailGroup.Id).Emails;
                    var groupWithEmails = new GroupWithEmails(emailGroup.Id, emailGroup.Name, groupEmails);

                    emailGroupsWithEmails.Add(groupWithEmails);
                }

                administratorsWithEmails = this.userRepository.FindActiveUsersIncludeEmails(customerId);
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