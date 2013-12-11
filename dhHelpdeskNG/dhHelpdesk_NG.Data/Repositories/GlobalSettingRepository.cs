using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface IGlobalSettingRepository : IRepository<GlobalSetting>
	{
	}
	public class GlobalSettingRepository : RepositoryBase<GlobalSetting>, IGlobalSettingRepository
	{
		public GlobalSettingRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
