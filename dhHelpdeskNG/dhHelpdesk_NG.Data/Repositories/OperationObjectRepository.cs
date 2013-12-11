using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface IOperationObjectRepository : IRepository<OperationObject>
	{
	}

	public class OperationObjectRepository : RepositoryBase<OperationObject>, IOperationObjectRepository
	{
		public OperationObjectRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
