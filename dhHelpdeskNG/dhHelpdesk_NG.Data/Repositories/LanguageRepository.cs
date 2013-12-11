using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface ILanguageRepository : IRepository<Language>
	{
	}

	public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
	{
		public LanguageRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
