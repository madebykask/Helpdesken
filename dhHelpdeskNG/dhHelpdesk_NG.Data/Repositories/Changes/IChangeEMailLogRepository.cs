namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;

    public interface IChangeEmailLogRepository : IRepository<ChangeEmailLogEntity>
    {
        void DeleteByHistoryId(int historyId);
    }
}