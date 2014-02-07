namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ISettingRepository : IRepository<Setting>
	{
	}

	public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
	{
		public SettingRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
