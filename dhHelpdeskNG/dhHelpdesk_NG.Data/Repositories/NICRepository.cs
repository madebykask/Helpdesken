using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
