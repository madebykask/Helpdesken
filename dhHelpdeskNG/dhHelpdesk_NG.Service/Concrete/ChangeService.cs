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
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Service.AggregateDataLoader.Changes;
    using dhHelpdesk_NG.Service.BusinessLogic.Changes;
    using dhHelpdesk_NG.Service.BusinessModelFactories.Changes;

    using Log = dhHelpdesk_NG.DTO.DTOs.Changes.Output.Log;

    public class ChangeService : IChangeService
    {
        private readonly IChangeRepository changeRepository;

        private readonly IUserRepository userRepository;

        private readonly IChangeObjectRepository changeObjectRepository;

        private readonly IChangeStatusRepository changeStatusRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly ILanguageRepository languageRepository;

        private readonly IChangeFieldSettingRepository changeFieldSettingRepository;

        private readonly IChangeAggregateFactory changeAggregateFactory;

        private readonly IChangeHistoryRepository changeHistoryRepository;

        private readonly IChangeEmailLogRepository changeEmailLogRepository;

        private readonly IUpdatedChangeFactory updatedChangeFactory;

        private readonly INewChangeFactory newChangeFactory;

        private readonly IChangeLogRepository changeLogRepository;

        private readonly IChangeFileRepository changeFileRepository;

        private readonly IChangeChangeRepository changeChangeRepository;

        private readonly IChangeLogic changeLogic;

        private readonly IChangeAggregateDataLoader changeAggregateDataLoader;

        private readonly INewChangeOptionsDataLoader newChangeOptionsDataLoader;

        private readonly IChangeOptionsDataLoader changeOptionsDataLoader;

        public ChangeService(
            IChangeRepository changeRepository,
            IChangeFieldSettingRepository changeFieldSettingRepository,
            ILanguageRepository languageRepository,
            IUserRepository userRepository,
            IWorkingGroupRepository workingGroupRepository,
            IChangeObjectRepository changeObjectRepository,
            IChangeStatusRepository changeStatusRepository,
            IChangeAggregateFactory changeAggregateFactory, 
            IChangeHistoryRepository changeHistoryRepository,
            IChangeEmailLogRepository changeEmailLogRepository, 
            IUpdatedChangeFactory updatedChangeFactory, 
            INewChangeFactory newChangeFactory,
            IChangeLogRepository changeLogRepository,
            IChangeFileRepository changeFileRepository,
            IChangeChangeRepository changeChangeRepository,
            IChangeLogic changeLogic,
            IChangeAggregateDataLoader changeAggregateDataLoader,
            INewChangeOptionsDataLoader newChangeOptionsDataLoader, 
            IChangeOptionsDataLoader changeOptionsDataLoader)
        {
            this.changeRepository = changeRepository;
            this.changeFieldSettingRepository = changeFieldSettingRepository;
            this.languageRepository = languageRepository;
            this.userRepository = userRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.changeObjectRepository = changeObjectRepository;
            this.changeStatusRepository = changeStatusRepository;
            this.changeAggregateFactory = changeAggregateFactory;
            this.changeHistoryRepository = changeHistoryRepository;
            this.changeEmailLogRepository = changeEmailLogRepository;
            this.updatedChangeFactory = updatedChangeFactory;
            this.newChangeFactory = newChangeFactory;
            this.changeLogRepository = changeLogRepository;
            this.changeFileRepository = changeFileRepository;
            this.changeChangeRepository = changeChangeRepository;
            this.changeLogic = changeLogic;
            this.changeAggregateDataLoader = changeAggregateDataLoader;
            this.newChangeOptionsDataLoader = newChangeOptionsDataLoader;
            this.changeOptionsDataLoader = changeOptionsDataLoader;
        }

        public List<ItemOverviewDto> FindActiveAdministratorOverviews(int customerId)
        {
            return this.userRepository.FindActiveOverviews(customerId);
        }

        public ChangeAggregate FindChange(int changeId)
        {
            var aggregateData = this.changeAggregateDataLoader.Load(changeId);
            
            var historyDifferences = this.changeLogic.AnalyzeDifference(
                aggregateData.Histories,
                aggregateData.Logs,
                aggregateData.EmailLogs);

            return this.changeAggregateFactory.Create(aggregateData.Change, aggregateData.Contacts, historyDifferences);
        }

        public ChangeEditOptions FindNewChangeOptionalData(int customerId)
        {
            var optionsData = this.newChangeOptionsDataLoader.Load(customerId);

            return new ChangeEditOptions(
                optionsData.Departments,
                optionsData.Statuses,
                optionsData.Systems,
                optionsData.Objects,
                optionsData.WorkingGroups,
                optionsData.Users,
                optionsData.ChangeGroups,
                optionsData.ChangeGroups,
                optionsData.Categories,
                optionsData.RelatedChanges,
                optionsData.Priorities,
                optionsData.Users,
                optionsData.Currencies,
                optionsData.EmailGroups,
                optionsData.ImplementationStatuses);
        }

        public ChangeEditOptions FindChangeOptionalData(int customerId, int changeId, ChangeEditSettings editSettings)
        {
            var optionsData = this.changeOptionsDataLoader.Load(customerId, changeId);

            return new ChangeEditOptions(
                optionsData.Departments,
                optionsData.Statuses,
                optionsData.Systems,
                optionsData.Objects,
                optionsData.WorkingGroups,
                optionsData.Users,
                optionsData.ChangeGroups,
                optionsData.ChangeGroups,
                optionsData.Categories,
                optionsData.RelatedChanges,
                optionsData.Priorities,
                optionsData.Users,
                optionsData.Currencies,
                optionsData.EmailGroups,
                optionsData.ImplementationStatuses);
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

        public SearchResultDto SearchDetailedChangeOverviews(SearchParameters parameters)
        {
            return this.changeRepository.SearchOverviews(parameters);
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

        public ChangeEditSettings FindChangeEditSettings(int customerId, int languageId)
        {
            return this.changeFieldSettingRepository.FindChangeEditSettings(customerId, languageId);
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

        public List<string> FindFileNames(int changeId, Subtopic subtopic, List<string> excludeFiles)
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
