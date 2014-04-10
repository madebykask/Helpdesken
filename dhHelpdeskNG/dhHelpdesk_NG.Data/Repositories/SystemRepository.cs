namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Systems.Output;
    using DH.Helpdesk.Dal.Infrastructure;

    public interface ISystemRepository : IRepository<Domain.System>
    {
        List<ItemOverview> FindOverviews(int customerId);

        string GetSystemName(int systemId);

        /// <summary>
        /// The get system overview.
        /// </summary>
        /// <param name="system">
        /// The system.
        /// </param>
        /// <returns>
        /// The <see cref="SystemOverview"/>.
        /// </returns>
        SystemOverview GetSystemOverview(int system);
    }

	public sealed class SystemRepository : RepositoryBase<Domain.System>, ISystemRepository
	{
		public SystemRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}

	    public List<ItemOverview> FindOverviews(int customerId)
	    {
	        var systems =
	            this.DataContext.Systems.Where(s => s.Customer_Id == customerId)
	                .Select(s => new { s.SystemName, s.Id })
	                .ToList();

	        return
	            systems.Select(s => new ItemOverview(s.SystemName, s.Id.ToString(CultureInfo.InvariantCulture))).ToList();
	    }

	    public string GetSystemName(int systemId)
	    {
	        return this.DataContext.Systems.Where(s => s.Id == systemId).Select(s => s.SystemName).Single();
	    }

	    /// <summary>
	    /// The get system overview.
	    /// </summary>
	    /// <param name="system">
	    /// The system.
	    /// </param>
	    /// <returns>
	    /// The <see cref="SystemOverview"/>.
	    /// </returns>
	    public SystemOverview GetSystemOverview(int system)
	    {
	        return this.GetAll()
                .Where(s => s.Id == system)
                .Select(s => new SystemOverview()
                                 {
                                     Id = s.Id,
                                     Name = s.SystemName
                                 })
                .FirstOrDefault();
	    }
	}
}
