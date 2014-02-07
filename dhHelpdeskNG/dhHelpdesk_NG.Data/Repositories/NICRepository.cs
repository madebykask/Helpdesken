namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface INICRepository : IRepository<NIC>
	{
	}

	public class NICRepository : RepositoryBase<NIC>, INICRepository
	{
		public NICRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
