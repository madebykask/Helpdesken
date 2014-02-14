namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerFieldSettingsRepository : Repository, IComputerFieldSettingsRepository
    {
        public ComputerFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}