using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
