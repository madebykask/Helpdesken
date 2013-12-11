using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

    public interface IDivisionRepository : IRepository<Division>
    {
        List<DivisionOverviewDto> FindByCustomerId(int customerId);
    }

    public class DivisionRepository : RepositoryBase<Division>, IDivisionRepository
    {
        public DivisionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DivisionOverviewDto> FindByCustomerId(int customerId)
        {
            return
                this.DataContext.Divisions.Where(d => d.Customer_Id == customerId)
                    .Select(d => new DivisionOverviewDto { Id = d.Id, Name = d.Name})
                    .ToList();
        }
    }
}
