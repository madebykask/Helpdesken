namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
