namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IDivisionRepository : IRepository<Division>
    {
        List<ItemOverviewDto> FindByCustomerId(int customerId);
    }

    public class DivisionRepository : RepositoryBase<Division>, IDivisionRepository
    {
        public DivisionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindByCustomerId(int customerId)
        {
            var divisionOverviews =
                this.DataContext.Divisions.Where(d => d.Customer_Id == customerId)
                    .Select(d => new { d.Id, d.Name })
                    .ToList();

            return
                divisionOverviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Id.ToString(CultureInfo.InvariantCulture) })
                                 .ToList();
        }
    }
}
