namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
