using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

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
