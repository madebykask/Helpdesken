using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IGlobalSettingRepository : IRepository<GlobalSetting>
    {
        GlobalSetting Get();
        Task<GlobalSetting> GetAsync();
    }

	public class GlobalSettingRepository : RepositoryBase<GlobalSetting>, IGlobalSettingRepository
	{
		public GlobalSettingRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}

	    public GlobalSetting Get()
	    {
	        return Table.FirstOrDefault();
	    }

	    public Task<GlobalSetting> GetAsync()
	    {
	        return Table.FirstOrDefaultAsync();
	    }
	}
}
