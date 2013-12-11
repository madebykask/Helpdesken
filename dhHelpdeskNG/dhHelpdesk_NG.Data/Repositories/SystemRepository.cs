using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface ISystemRepository : IRepository<Domain.System>
	{
	}

	public class SystemRepository : RepositoryBase<Domain.System>, ISystemRepository
	{
		public SystemRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
