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
            IChangeChangeRepository changeChangeRepository)
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
            var implementationStatuses = this.changeImplementationStatusRepository.FindOverviews(customerId);

            return new ChangeOptionalData(
                departments,
                statuses,
                systems,
                objects,
                workingGroups,
                administrators,
                owners,
                processesAffected,
                categories,
                relatedChanges,
                priorities,
                responsibles,
                currencies,
                implementationStatuses);
        }

        public ChangeOptionalData FindChangeOptionalData(int customerId, int changeId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var statuses = this.changeStatusRepository.FindOverviews(customerId);
            var systems = this.systemRepository.FindOverviews(customerId);
            var objects = this.changeObjectRepository.FindOverviews(customerId);
            var workingGroups = this.workingGroupRepository.FindActiveOverviews(customerId);
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
            var implementationStatuses = this.changeImplementationStatusRepository.FindOverviews(customerId);

            return new ChangeOptionalData(
                departments,
                statuses,
                systems,
                objects,
                workingGroups,
                administrators,
                owners,
                processesAffected,
                categories,
                relatedChanges,
                priorities,
                responsibles,
                currencies,
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

        public FieldOverviewSettingsDto FindFieldOverviewSettings(int customerId, int languageId)
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

        public void DeleteFile(int changeId, Subtopic subtopic, string fileName)
        {
            this.changeFileRepository.Delete(changeId, subtopic, fileName);
            this.changeFileRepository.Commit();
        }

        public List<string> FindFileNames(int changeId, Subtopic subtopic)
        {
            return this.changeFileRepository.FindFileNamesByChangeIdAndSubtopic(changeId, subtopic);
        }

        public void AddFile(NewChangeFile file)
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
            
            this.changeChangeRepository.UpdateRelatedChanges(updatedChange.Id, updatedChange.Analyze.RelatedChangeIds);
            this.changeChangeRepository.Commit();

            this.changeRepository.Update(change);
            this.changeRepository.Commit();
        }

        public SearchFieldSettingsDto FindSearchFieldSettings(int customerId)
        {
            return new SearchFieldSettingsDto(
                new FieldOverviewSettingDto(true, "Statuses"),
                new FieldOverviewSettingDto(true, "Objects"),
                new FieldOverviewSettingDto(true, "ADministrators"),
                new FieldOverviewSettingDto(true, "gfgdfg"));
        }

        public void UpdateSettings(UpdatedFieldSettingsDto updatedSettings)
        {
            this.changeFieldSettingRepository.UpdateSettings(updatedSettings);
            this.changeFieldSettingRepository.Commit();
        }

        public FieldSettingsDto FindSettings(int customerId, int languageId)
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
