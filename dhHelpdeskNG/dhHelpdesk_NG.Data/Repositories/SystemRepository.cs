using dhHelpdesk_NG.Data.Infrastructure;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public interface ISystemRepository : IRepository<Domain.System>
    {
        List<ItemOverviewDto> FindOverviews(int customerId);
    }

	public class SystemRepository : RepositoryBase<Domain.System>, ISystemRepository
	{
		public SystemRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}

	    public List<ItemOverviewDto> FindOverviews(int customerId)
	    {
	        var systems =
	            this.DataContext.Systems.Where(s => s.Customer_Id == customerId)
	                .Select(s => new { s.SystemName, s.Id })
	                .ToList();

	        return
	            systems.Select(
	                s => new ItemOverviewDto { Name = s.SystemName, Value = s.Id.ToString(CultureInfo.InvariantCulture) })
	                .ToList();
	    }
	}
}
