namespace dhHelpdesk_NG.Service.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Common.Enums;
    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Domain.Changes;
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
            INewChangeFactory newChangeFactory)
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
        }

        public List<ItemOverviewDto> FindActiveAdministratorOverviews(int customerId)
        {
            return this.userRepository.FindActiveOverviews(customerId);
        }

        public ChangeAggregate FindChange(int changeId)
        {
            var change = this.changeRepository.FindById(changeId);
            var contacts = this.changeContactRepository.FindByChangeId(changeId);

            return this.changeAggregateFactory.Create(change, contacts);
        }

        public ChangeOptionalData FindChangeOptionalData(int customerId)
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

        public void AddChange(NewChangeAggregate newChange)
        {
            var change = this.newChangeFactory.Create(newChange);
            throw new NotImplementedException();
        }

        public void UpdateChange(UpdatedChangeAggregate updatedChange)
        {
            var change = this.updatedChangeFactory.Create(updatedChange);

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
