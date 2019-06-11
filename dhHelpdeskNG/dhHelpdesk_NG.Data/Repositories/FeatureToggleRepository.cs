using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories
{
	public interface IFeatureToggleRepository
	{
		FeatureToggle GetByStrongName(string strongName);
	}

	public class FeatureToggleRepository : RepositoryBase<FeatureToggle>, IFeatureToggleRepository
	{
		public FeatureToggleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{
		}

		public FeatureToggle GetByStrongName(string strongName)
		{
			var featureToogle = Table.SingleOrDefault(o => o.StrongName == strongName);
			return featureToogle;
		}
	}
}
