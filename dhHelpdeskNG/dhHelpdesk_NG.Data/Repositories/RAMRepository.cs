namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.WorkstationModules;

    public interface IRAMRepository : IRepository<RAM>
	{
	}

	public class RAMRepository : RepositoryBase<RAM>, IRAMRepository
	{
		public RAMRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
