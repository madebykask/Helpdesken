namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
