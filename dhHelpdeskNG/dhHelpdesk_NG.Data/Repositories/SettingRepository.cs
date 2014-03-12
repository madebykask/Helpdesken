namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ISettingRepository : IRepository<Setting>
	{
        Setting GetCustomerSetting(int id);
	}

	public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
	{
		public SettingRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}

        public Setting GetCustomerSetting(int id)
        {
            return this.Get(x => x.Customer_Id == id);
        }

	}
}
