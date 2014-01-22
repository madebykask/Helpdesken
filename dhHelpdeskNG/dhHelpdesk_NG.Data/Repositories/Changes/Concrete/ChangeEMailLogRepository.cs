namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeEmailLogRepository : RepositoryBase<ChangeEmailLogEntity>, IChangeEmailLogRepository
    {
        public ChangeEmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByHistoryId(int historyId)
        {
            var logs = this.DataContext.ChangeEMailLogs.Where(l => l.ChangeHistory_Id == historyId).ToList();
            logs.ForEach(l => this.DataContext.ChangeEMailLogs.Remove(l));
        }
    }
}