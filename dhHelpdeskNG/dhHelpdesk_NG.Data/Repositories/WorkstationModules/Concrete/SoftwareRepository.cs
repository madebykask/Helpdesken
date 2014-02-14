namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class SoftwareRepository : Repository, ISoftwareRepository
    {
        public SoftwareRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
