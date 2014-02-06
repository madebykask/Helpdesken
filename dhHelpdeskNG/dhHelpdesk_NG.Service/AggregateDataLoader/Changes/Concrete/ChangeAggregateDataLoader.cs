namespace dhHelpdesk_NG.Service.AggregateDataLoader.Changes.Concrete
{
    using System.Linq;

    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Service.AggregateData.Changes;

    public sealed class ChangeAggregateDataLoader : IChangeAggregateDataLoader
    {
        private readonly IChangeRepository changeRepository;

        private readonly IChangeContactRepository changeContactRepository;

        private readonly IChangeHistoryRepository changeHistoryRepository;

        private readonly IChangeLogRepository changeLogRepository;

        private readonly IChangeEmailLogRepository changeEmailLogRepository;

        public ChangeAggregateDataLoader(
            IChangeRepository changeRepository,
            IChangeContactRepository changeContactRepository,
            IChangeHistoryRepository changeHistoryRepository,
            IChangeLogRepository changeLogRepository,
            IChangeEmailLogRepository changeEmailLogRepository)
        {
            this.changeRepository = changeRepository;
            this.changeContactRepository = changeContactRepository;
            this.changeHistoryRepository = changeHistoryRepository;
            this.changeLogRepository = changeLogRepository;
            this.changeEmailLogRepository = changeEmailLogRepository;
        }

        public ChangeAggregateData Load(int changeId)
        {
            var change = this.changeRepository.FindById(changeId);
            var contacts = this.changeContactRepository.FindByChangeId(changeId);

            var histories = this.changeHistoryRepository.FindByChangeId(changeId);
            var historyIds = histories.Select(i => i.Id).ToList();
            var logs = this.changeLogRepository.FindOverviewsByHistoryIds(historyIds);
            var emailLogs = this.changeEmailLogRepository.FindOverviewsByHistoryIds(historyIds);

            return new ChangeAggregateData(change, contacts, histories, logs, emailLogs);
        }
    }
}
