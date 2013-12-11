using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public class TranslationRepository : RepositoryBase<Translation>, ITranslationRepository
	{
		public TranslationRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{

		}
	}

	public interface ITranslationRepository : IRepository<Translation>
	{

	}
}
