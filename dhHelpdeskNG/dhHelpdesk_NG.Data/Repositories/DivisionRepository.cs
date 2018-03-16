namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IDivisionRepository : IRepository<Division>
    {
        List<ItemOverview> FindByCustomerId(int customerId);
        int GetDivisionIdByName(string division, int customerId);
    }

    public class DivisionRepository : RepositoryBase<Division>, IDivisionRepository
    {
        public DivisionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindByCustomerId(int customerId)
        {
            var divisionOverviews =
                this.DataContext.Divisions.Where(d => d.Customer_Id == customerId)
                    .Select(d => new { d.Id, d.Name })
                    .ToList();

            return
                divisionOverviews.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture)))
                    .OrderBy(x => x.Name).ToList();
        }

        public int GetDivisionIdByName(string division , int customerId)
        {
            return this.DataContext.Divisions.Where(d => d.Name == division & d.Customer_Id == customerId)
                    .Select(d => d.Id).FirstOrDefault();
        }
    }
}
