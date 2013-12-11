using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        void ResetDefault(int exclude);
    }

    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude)
        {
            foreach (Country obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }
    }
}
