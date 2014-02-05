namespace dhHelpdesk_NG.Service.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Common.Enums;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Service.BusinessLogic.Changes;
    using dhHelpdesk_NG.Service.BusinessModelFactories.Changes;

    public class ChangeService : IChangeService
    {
        private readonly IChangeRepository changeRepository;

        private readonly IDepartmentRepository departmentRepository;

        private readonly IUserRepository userRepository;

        private readonly IChangeObjectRepository changeObjectRepository;

        private readonly IChangeStatusRepository changeStatusRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly ILanguageRepository languageRepository;

        private readonly IChangeFieldSettingRepository changeFieldSettingRepository;

        private readonly IChangeContactRepository changeContactRepository;

        private readonly ISystemRepository systemRepository;

        private readonly IChangeAggregateFactory changeAggregateFactory;

        private readonly IChangeCategoryRepository changeCategoryRepository;

        private readonly IChangePriorityRepository changePriorityRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly IChangeImplementationStatusRepository changeImplementationStatusRepository;

        private readonly IChangeHistoryRepository changeHistoryRepository;

        private readonly IChangeEmailLogRepository changeEmailLogRepository;

        private readonly IUpdatedChangeFactory updatedChangeFactory;

        private readonly IChangeGroupRepository changeGroupRepository;

        private readonly INewChangeFactory newChangeFactory;

        private readonly IChangeLogRepository changeLogRepository;

        private readonly IHistoriesComparator historiesComparator;

        private readonly IChangeFileRepository changeFileRepository;

        private readonly IChangeChangeRepository changeChangeRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IEmailGroupRepository emailGroupRepository;

        private readonly IEmailGroupEmailRepository emailGroupEmailRepository;

        public ChangeService(
            IChangeRepository changeRepository,
            IChangeFieldSettingRepository changeFieldSettingRepository,
            ILanguageRepository languageRepository,
            IUserRepository userRepository,
            IWorkingGroupRepository workingGroupRepository,
            IChangeObjectRepository changeObjectRepository,
            IChangeStatusRepository changeStatusRepository,
            IChangeContactRepository changeContactRepository, 
            IChangeAggregateFactory changeAggregateFactory, 
            IDepartmentRepository departmentRepository,
            ISystemRepository systemRepository,
            IChangeCategoryRepository changeCategoryRepository,
            IChangePriorityRepository changePriorityRepository,
            ICurrencyRepository currencyRepository, 
            IChangeImplementationStatusRepository changeImplementationStatusRepository, 
            IChangeHistoryRepository changeHistoryRepository,
            IChangeEmailLogRepository changeEmailLogRepository, 
            IUpdatedChangeFactory updatedChangeFactory, 
            IChangeGroupRepository changeGroupRepository, 
            INewChangeFactory newChangeFactory,
            IChangeLogRepository changeLogRepository,
            IHistoriesComparator historiesComparator,
            IChangeFileRepository changeFileRepository,
            IChangeChangeRepository changeChangeRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IEmailGroupEmailRepository emailGroupEmailRepository, 
            IEmailGroupRepository emailGroupRepository)
        {
            this.changeRepository = changeRepository;
            this.changeFieldSettingRepository = changeFieldSettingRepository;
            this.languageRepository = languageRepository;
            this.userRepository = userRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.changeObjectRepository = changeObjectRepository;
            this.changeStatusRepository = changeStatusRepository;
            this.changeContactRepository = changeContactRepository;
            this.changeAggregateFactory = changeAggregateFactory;
            this.departmentRepository = departmentRepository;
            this.systemRepository = systemRepository;
            this.changeCategoryRepository = changeCategoryRepository;
            this.changePriorityRepository = changePriorityRepository;
            this.currencyRepository = currencyRepository;
            this.changeImplementationStatusRepository = changeImplementationStatusRepository;
            this.changeHistoryRepository = changeHistoryRepository;
            this.changeEmailLogRepository = changeEmailLogRepository;
            this.updatedChangeFactory = updatedChangeFactory;
            this.changeGroupRepository = changeGroupRepository;
            this.newChangeFactory = newChangeFactory;
            this.changeLogRepository = changeLogRepository;
            this.historiesComparator = historiesComparator;
            this.changeFileRepository = changeFileRepository;
            this.changeChangeRepository = changeChangeRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.emailGroupEmailRepository = emailGroupEmailRepository;
            this.emailGroupRepository = emailGroupRepository;
        }

        public List<ItemOverviewDto> FindActiveAdministratorOverviews(int customerId)
        {
            return this.userRepository.FindActiveOverviews(customerId);
        }

        public ChangeAggregate FindChange(int changeId)
        {
            var change = this.changeRepository.FindById(changeId);
            var contacts = this.changeContactRepository.FindByChangeId(changeId);
            var historyDifferences = new List<HistoriesDifference>();

            var histories = this.changeHistoryRepository.FindByChangeId(changeId);
            var historyIds = histories.Select(i => i.Id).ToList();
            var logs = this.changeLogRepository.FindOverviewsByHistoryIds(historyIds);
            var emailLogs = this.changeEmailLogRepository.FindOverviewsByHistoryIds(historyIds);

            History previousHistory = null;

            foreach (var history in histories)
            {
                var historyLog = logs.SingleOrDefault(l => l.HistoryId == history.Id);
                var historyEmailLogs = emailLogs.Where(l => l.HistoryId == history.Id).ToList();

                var difference = this.historiesComparator.Compare(
                    previousHistory,
                    history,
                    historyLog,
                    historyEmailLogs);

                if (difference != null)
                {
                    historyDifferences.Add(difference);
                }
                
                previousHistory = history;
            }

            return this.changeAggregateFactory.Create(change, contacts, historyDifferences);
        }

        public ChangeOptionalData FindNewChangeOptionalData(int customerId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var statuses = this.changeStatusRepository.FindOverviews(customerId);
            var systems = this.systemRepository.FindOverviews(customerId);
            var objects = this.changeObjectRepository.FindOverviews(customerId);
            
            var workingGroups = this.workingGroupRepository.FindActiveOverviews(customerId);
            var workingGroupIds = workingGroups.Select(g => int.Parse(g.Value)).ToList();
            var workingGroupsUserIds = this.userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
            var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
            var userIdsWithEmails = this.userRepository.FindUsersEmails(userIds);

            var workingGroupsWithEmails = new List<GroupWithEmails>(workingGroups.Count);

            foreach (var workingGroup in workingGroups)
            {
                var groupId = int.Parse(workingGroup.Value);
                var groupUserIdsWithEmails = workingGroupsUserIds.Single(g => g.WorkingGroupId == groupId);

                var groupEmails =
                    userIdsWithEmails.Where(e => groupUserIdsWithEmails.UserIds.Contains(e.ItemId))
                        .Select(e => e.Email)
                        .ToList();

                var groupWithEmails = new GroupWithEmails(groupId, workingGroup.Name, groupEmails);
                workingGroupsWithEmails.Add(groupWithEmails);
            }
            
            var users = this.userRepository.FindActiveOverviews(customerId);
            var administrators = users;
            var changeGroups = this.changeGroupRepository.FindOverviews(customerId);
            var owners = changeGroups;
            var processesAffected = changeGroups;
            var categories = this.changeCategoryRepository.FindOverviews(customerId);
            var relatedChanges = this.changeRepository.FindOverviews(customerId);
            var priorities = this.changePriorityRepository.FindOverviews(customerId);
            var responsibles = users;
            var currencies = this.currencyRepository.FindOverviews();

            var emailGroups = this.emailGroupRepository.FindActiveOverviews(customerId);
            var emailGroupIds = emailGroups.Select(g => int.Parse(g.Value)).ToList();
            var emailGroupsEmails = this.emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

            var emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

            foreach (var emailGroup in emailGroups)
            {
                var groupId = int.Parse(emailGroup.Value);
                var groupEmails = emailGroupsEmails.Single(e => e.ItemId == groupId).Emails;
                var groupWithEmails = new GroupWithEmails(groupId, emailGroup.Name, groupEmails);

                emailGroupsWithEmails.Add(groupWithEmails);
            }

            var implementationStatuses = this.changeImplementationStatusRepository.FindOverviews(customerId);

            return new ChangeOptionalData(
                departments,
                statuses,
                systems,
                objects,
                workingGroupsWithEmails,
                administrators,
                owners,
                processesAffected,
                categories,
                relatedChanges,
                priorities,
                responsibles,
                currencies,
                emailGroupsWithEmails,
                implementationStatuses);
        }

        public ChangeOptionalData FindChangeOptionalData(int customerId, int changeId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var statuses = this.changeStatusRepository.FindOverviews(customerId);
            var systems = this.systemRepository.FindOverviews(customerId);
            var objects = this.changeObjectRepository.FindOverviews(customerId);

            var workingGroups = this.workingGroupRepository.FindActiveOverviews(customerId);
            var workingGroupIds = workingGroups.Select(g => int.Parse(g.Value)).ToList();
            var workingGroupsUserIds = this.userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
            var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
            var userIdsWithEmails = this.userRepository.FindUsersEmails(userIds);

            var workingGroupsWithEmails = new List<GroupWithEmails>(workingGroups.Count);

            foreach (var workingGroup in workingGroups)
            {
                var groupId = int.Parse(workingGroup.Value);
                var groupUserIdsWithEmails = workingGroupsUserIds.Single(g => g.WorkingGroupId == groupId);

                var groupEmails =
                    userIdsWithEmails.Where(e => groupUserIdsWithEmails.UserIds.Contains(e.ItemId))
                        .Select(e => e.Email)
                        .ToList();

                var groupWithEmails = new GroupWithEmails(groupId, workingGroup.Name, groupEmails);
                workingGroupsWithEmails.Add(groupWithEmails);
            }

            var users = this.userRepository.FindActiveOverviews(customerId);
            var administrators = users;
            var changeGroups = this.changeGroupRepository.FindOverviews(customerId);
            var owners = changeGroups;
            var processesAffected = changeGroups;
            var categories = this.changeCategoryRepository.FindOverviews(customerId);
            var relatedChanges = this.changeRepository.FindOverviewsExcludeChange(customerId, changeId);
            var priorities = this.changePriorityRepository.FindOverviews(customerId);
            var responsibles = users;
            var currencies = this.currencyRepository.FindOverviews();

            var emailGroups = this.emailGroupRepository.FindActiveOverviews(customerId);
            var emailGroupIds = emailGroups.Select(g => int.Parse(g.Value)).ToList();
            var emailGroupsEmails = this.emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

            var emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

            foreach (var emailGroup in emailGroups)
            {
                var groupId = int.Parse(emailGroup.Value);
                var groupEmails = emailGroupsEmails.Single(e => e.ItemId == groupId).Emails;
                var groupWithEmails = new GroupWithEmails(groupId, emailGroup.Name, groupEmails);

                emailGroupsWithEmails.Add(groupWithEmails);
            }

            var implementationStatuses = this.changeImplementationStatusRepository.FindOverviews(customerId);

            return new ChangeOptionalData(
                departments,
                statuses,
                systems,
                objects,
                workingGroupsWithEmails,
                administrators,
                owners,
                processesAffected,
                categories,
                relatedChanges,
                priorities,
                responsibles,
                currencies,
                emailGroupsWithEmails,
                implementationStatuses);
        }

        public void DeleteChange(int changeId)
        {
            var historyIds = this.changeHistoryRepository.FindIdsByChangeId(changeId);
            historyIds.ForEach(i => this.changeEmailLogRepository.DeleteByHistoryId(i));
            this.changeEmailLogRepository.Commit();

            this.changeHistoryRepository.DeleteByChangeId(changeId);
            this.changeHistoryRepository.Commit();

            this.changeChangeRepository.DeleteReferencesToChange(changeId);
            this.changeChangeRepository.Commit();

            this.changeRepository.DeleteById(changeId);
            this.changeRepository.Commit();
        }

        public List<ItemOverviewDto> FindActiveWorkingGroupOverviews(int customerId)
        {
            return this.workingGroupRepository.FindActiveOverviews(customerId);
        }

        public SearchResultDto SearchDetailedChangeOverviews(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            Data.Enums.Changes.ChangeStatus status,
            int selectCount)
        {
            return this.changeRepository.SearchOverviews(
                customerId,
                statusIds,
                objectIds,
                ownerIds,
                workingGroupIds,
                administratorIds,
                pharse,
                status,
                selectCount);
        }

        public FieldOverviewSettings FindFieldOverviewSettings(int customerId, int languageId)
        {
            var languageTextId = this.languageRepository.FindLanguageIdById(languageId);
            
            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    return this.changeFieldSettingRepository.FindSwedishByCustomerId(customerId);
                case LanguageTextId.English:
                    return this.changeFieldSettingRepository.FindEnglishByCustomerId(customerId);
                default:
                    throw new ArgumentOutOfRangeException("languageTextId", languageTextId);
            }
        }

        public List<Log> FindLogs(int changeId, Subtopic subtopic, List<int> excludeIds)
        {
            return this.changeLogRepository.FindLogsByChangeIdAndSubtopic(changeId, subtopic, excludeIds);
        }

        public void DeleteFile(int changeId, Subtopic subtopic, string fileName)
        {
            this.changeFileRepository.Delete(changeId, subtopic, fileName);
            this.changeFileRepository.Commit();
        }

        public List<string> FindFileNames(int changeId, Subtopic subtopic)
        {
            return this.changeFileRepository.FindFileNamesByChangeIdAndSubtopic(changeId, subtopic);
        }

        public List<string> FindFileNamesExcludeSpecified(int changeId, Subtopic subtopic, List<string> excludeFiles)
        {
            return this.changeFileRepository.FindFileNamesExcludeSpecified(changeId, subtopic, excludeFiles);
        }

        public void AddFile(NewFile file)
        {
            this.changeFileRepository.AddFile(file);
            this.changeFileRepository.Commit();
        }

        public bool FileExists(int changeId, Subtopic subtopic, string fileName)
        {
            return this.changeFileRepository.FileExists(changeId, subtopic, fileName);
        }

        public byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName)
        {
            return this.changeFileRepository.GetFileContent(changeId, subtopic, fileName);
        }

        public void AddChange(NewChangeAggregate newChange)
        {
            var change = this.newChangeFactory.Create(newChange);

            this.changeRepository.AddChange(change);
            this.changeRepository.Commit();

            this.changeChangeRepository.AddRelatedChanges(change.Id, newChange.Analyze.RelatedChangeIds);
            this.changeChangeRepository.Commit();

            foreach (var attachedFile in newChange.Registration.AttachedFiles)
            {
                attachedFile.ChangeId = change.Id;
                this.changeFileRepository.AddFile(attachedFile);
            }

            foreach (var attachedFile in newChange.Analyze.AttachedFiles)
            {
                attachedFile.ChangeId = change.Id;
                this.changeFileRepository.AddFile(attachedFile);
            }

            foreach (var attachedFile in newChange.Implementation.AttachedFiles)
            {
                attachedFile.ChangeId = change.Id;
                this.changeFileRepository.AddFile(attachedFile);
            }

            foreach (var attachedFile in newChange.Evaluation.AttachedFiles)
            {
                attachedFile.ChangeId = change.Id;
                this.changeFileRepository.AddFile(attachedFile);
            }

            this.changeFileRepository.Commit();
        }

        public void UpdateChange(UpdatedChangeAggregate updatedChange)
        {
            var change = this.updatedChangeFactory.Create(updatedChange);

            this.changeLogRepository.DeleteByIds(updatedChange.DeletedLogIds);
            this.changeLogRepository.Commit();

            this.changeChangeRepository.UpdateRelatedChanges(updatedChange.Id, updatedChange.Analyze.RelatedChangeIds);
            this.changeChangeRepository.Commit();

            this.changeRepository.Update(change);
            this.changeRepository.Commit();
        }

        public SearchFieldSettings FindSearchFieldSettings(int customerId)
        {
            return new SearchFieldSettings(
                new FieldOverviewSetting(true, "Statuses"),
                new FieldOverviewSetting(true, "Objects"),
                new FieldOverviewSetting(true, "ADministrators"),
                new FieldOverviewSetting(true, "gfgdfg"));
        }

        public void UpdateSettings(UpdatedFieldSettingsDto updatedSettings)
        {
            this.changeFieldSettingRepository.UpdateSettings(updatedSettings);
            this.changeFieldSettingRepository.Commit();
        }

        public FieldSettings FindSettings(int customerId, int languageId)
        {
            return this.changeFieldSettingRepository.FindByCustomerIdAndLanguageId(customerId, languageId);
        }

        public List<ItemOverviewDto> FindStatusOverviews(int customerId)
        {
            return this.changeStatusRepository.FindOverviews(customerId);
        }

        public List<ItemOverviewDto> FindObjectOverviews(int customerId)
        {
            return this.changeObjectRepository.FindOverviews(customerId);
        }

        public IList<ChangeEntity> GetChanges(int customerId)
        {
            return this.changeRepository.GetChanges(customerId).OrderBy(x => x.OrdererName).ToList();
        }
    }
}
