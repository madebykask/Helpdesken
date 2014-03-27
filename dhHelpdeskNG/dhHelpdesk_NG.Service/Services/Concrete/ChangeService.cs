namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Requests.Changes;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Changes;
    using DH.Helpdesk.Services.Restorers.Changes;
    using DH.Helpdesk.Services.Validators.Changes;

    public sealed class ChangeService : IChangeService
    {
        #region Fields

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

        private readonly IChangeStatusRepository changeStatusRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly IDepartmentRepository departmentRepository;

        private readonly IEmailGroupEmailRepository emailGroupEmailRepository;

        private readonly IEmailGroupRepository emailGroupRepository;

        private readonly ILanguageRepository languageRepository;

        private readonly ISystemRepository systemRepository;

        private readonly IUserRepository userRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IUpdateChangeRequestValidator updateChangeRequestValidator;

        private readonly IChangeRestorer changeRestorer;

        private readonly IChangeEmailService changeEmailService;

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
            IChangeEmailService changeEmailService)
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
            this.changeEmailService = changeEmailService;
        }

        #endregion

        #region Public Methods and Operators

        public void AddChange(NewChangeRequest request)
        {
            request.Change.Analyze = NewAnalyzeFields.CreateDefault();
            request.Change.Implementation = NewImplementationFields.CreateDefault();
            request.Change.Evaluation = NewEvaluationFields.CreateDefault();

            this.changeRepository.AddChange(request.Change);
            this.changeRepository.Commit();

            this.changeChangeGroupRepository.AddChangeProcesses(request.Change.Id, request.AffectedProcessIds);
            this.changeChangeGroupRepository.Commit();

            this.changeDepartmentRepository.AddChangeDepartments(request.Change.Id, request.AffectedDepartmentIds);
            this.changeDepartmentRepository.Commit();

            request.NewFiles.ForEach(f => f.ChangeId = request.Change.Id);
            this.changeFileRepository.AddFiles(request.NewFiles);
            this.changeFileRepository.Commit();
        }

        public void DeleteChange(int changeId)
        {
            var historyIds = this.changeHistoryRepository.FindIdsByChangeId(changeId);

            this.changeEmailLogRepository.DeleteByHistoryIds(historyIds);
            this.changeEmailLogRepository.Commit();

            this.changeHistoryRepository.DeleteByChangeId(changeId);
            this.changeHistoryRepository.Commit();

            this.changeChangeRepository.DeleteReferencesToChange(changeId);
            this.changeChangeRepository.Commit();

            this.changeRepository.DeleteById(changeId);
            this.changeRepository.Commit();
        }

        public bool FileExists(int changeId, Subtopic subtopic, string fileName)
        {
            return this.changeFileRepository.FileExists(changeId, subtopic, fileName);
        }

        public FindChangeResponse FindChange(int changeId)
        {
            var change = this.changeRepository.FindById(changeId);
            if (change == null)
            {
                return null;
            }

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

            return new FindChangeResponse(
                change,
                affectedProcessIds,
                affectedDepartmentIds,
                relatedChangeIds,
                files,
                logs,
                historyDifferences);
        }

        public List<string> FindFileNames(int changeId, Subtopic subtopic, List<string> excludeFiles)
        {
            return this.changeFileRepository.FindFileNamesExcludeSpecified(changeId, subtopic, excludeFiles);
        }

        public List<Log> FindLogs(int changeId, Subtopic subtopic, List<int> excludeLogIds)
        {
            return this.changeLogRepository.FindLogsExcludeSpecified(changeId, subtopic, excludeLogIds);
        }

        public ChangeFieldSettings FindSettings(int customerId, int languageId)
        {
            var languageTextId = this.languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.GetSwedishFieldSettings(customerId);
                case LanguageTextId.English:
                    return this.changeFieldSettingRepository.GetEnglishFieldSettings(customerId);
                default:
                    throw new ArgumentOutOfRangeException("languageId");
            }
        }

        public SearchSettings GetSearchSettings(int customerId, int languageId)
        {
            var languageTextId = this.languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.GetSwedishSearchSettings(customerId);
                case LanguageTextId.English:
                    return this.changeFieldSettingRepository.GetEnglishSearchSettings(customerId);
                default:
                    throw new ArgumentOutOfRangeException("languageId");
            }
        }

        public SearchData GetSearchData(int customerId)
        {
            var statuses = this.changeStatusRepository.FindOverviews(customerId);
            var objects = this.changeObjectRepository.FindOverviews(customerId);
            var changeGroups = this.changeGroupRepository.FindOverviews(customerId);
            var owners = changeGroups;
            var affectedProcesses = changeGroups;
            var workingGroups = this.workingGroupRepository.FindActiveOverviews(customerId);
            var administrators = this.userRepository.FindActiveOverviews(customerId);

            return new SearchData(statuses, objects, owners, affectedProcesses, workingGroups, administrators);
        }

        public ChangeEditData GetChangeEditData(int changeId, int customerId, ChangeEditSettings settings)
        {
            var editData = this.GetChangeEditDataCore(customerId, settings);
            var relatedChanges = this.changeRepository.FindOverviewsExcludeSpecified(customerId, changeId);
            editData.RelatedChanges = relatedChanges;

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
                    throw new ArgumentOutOfRangeException("languageId");
            }
        }

        public ChangeOverviewSettings GetChangeOverviewSettings(int customerId, int languageId)
        {
            var languageTextId = this.languageRepository.GetLanguageTextIdById(languageId);

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.GetSwedishOverviewSettings(customerId);
                case LanguageTextId.English:
                    return this.changeFieldSettingRepository.GetEnglishOverviewSettings(customerId);
                default:
                    throw new ArgumentOutOfRangeException("languageId");
            }
        }

        public IList<ChangeEntity> GetChanges(int customerId)
        {
            return this.changeRepository.GetChanges(customerId).OrderBy(x => x.OrdererName).ToList();
        }

        public byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName)
        {
            return this.changeFileRepository.GetFileContent(changeId, subtopic, fileName);
        }

        public ChangeEditData GetNewChangeEditData(int customerId, ChangeEditSettings settings)
        {
            var editData = this.GetChangeEditDataCore(customerId, settings);
            var relatedChanges = this.changeRepository.FindOverviews(customerId);
            editData.RelatedChanges = relatedChanges;

            return editData;
        }

        public SearchResult Search(SearchParameters parameters)
        {
            return this.changeRepository.Search(parameters);
        }

        public void UpdateChange(UpdateChangeRequest request)
        {
            var existingChange = this.changeRepository.GetById(request.Change.Id);
            var processingSettings = this.changeFieldSettingRepository.GetProcessingSettings(request.CustomerId);
            this.changeRestorer.Restore(request.Change, existingChange, processingSettings);
            this.updateChangeRequestValidator.Validate(request, existingChange, processingSettings);

            this.changeRepository.Update(request.Change);
            this.changeRepository.Commit();

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

            if (request.AnalyzeNewLog != null && request.AnalyzeNewLog.Emails.Any())
            {
//                this.changeEmailService.SendInternalLogNoteTo(null, request.AnalyzeNewLog.Text, request.AnalyzeNewLog.Emails);
            }
        }

        public void UpdateSettings(ChangeFieldSettings settings)
        {
            this.changeFieldSettingRepository.UpdateSettings(settings);
            this.changeFieldSettingRepository.Commit();
        }

        #endregion

        #region Methods

        private ChangeEditData GetChangeEditDataCore(int customerId, ChangeEditSettings settings)
        {
            List<ItemOverview> departments = null;
            List<ItemOverview> statuses = null;
            List<ItemOverview> systems = null;
            List<ItemOverview> objects = null;
            List<ItemOverview> workingGroups = null;
            List<ItemOverview> users = null;
            List<ItemOverview> changeGroups = null;
            List<ItemOverview> categories = null;
            List<ItemOverview> priorities = null;
            List<ItemOverview> currencies = null;
            List<ItemOverview> implementationStatuses = null;

            List<GroupWithEmails> workingGroupsWithEmails = null;
            List<GroupWithEmails> emailGroupsWithEmails = null;

            if (settings.Orderer.Department.Show)
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

            if (settings.General.WorkingGroup.Show)
            {
                workingGroups = this.workingGroupRepository.FindActiveOverviews(customerId);
            }

            if (settings.General.Administrator.Show)
            {
                users = this.userRepository.FindActiveOverviews(customerId);
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
                        workingGroupOverview.Id, workingGroupOverview.Name, groupEmails);

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
            }

            return new ChangeEditData(
                departments,
                statuses,
                systems,
                objects,
                workingGroups,
                workingGroupsWithEmails,
                users,
                changeGroups,
                changeGroups,
                departments,
                categories,
                priorities,
                users,
                currencies,
                emailGroupsWithEmails,
                implementationStatuses);
        }

        #endregion
    }
}