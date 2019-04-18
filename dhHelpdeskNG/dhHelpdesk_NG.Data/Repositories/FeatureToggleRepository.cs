using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories
{
	public interface IFeatureToggleRepository
	{
		FeatureToggle GetByStrongName(string strongName);
	}

	public class FeatureToggleRepository : Repository<FeatureToggle>, IFeatureToggleRepository
	{
		public FeatureToggleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{

		}
		public FeatureToggle GetByStrongName(string strongName)
		{
			var featureToogle = this.DbSet.SingleOrDefault(o => o.StrongName == strongName);
			return featureToogle;
		}
	}
}
