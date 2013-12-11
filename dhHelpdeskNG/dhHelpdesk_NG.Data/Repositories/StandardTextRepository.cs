using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface IStandardTextRepository : IRepository<StandardText>
	{
	}

	public class StandardTextRepository : RepositoryBase<StandardText>, IStandardTextRepository
	{
		public StandardTextRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
