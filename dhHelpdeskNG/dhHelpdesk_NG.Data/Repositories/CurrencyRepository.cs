namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICurrencyRepository : IRepository<Currency>
    {
        List<ItemOverview> FindOverviews();
    }

    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews()
        {
            var currencies = this.DataContext.Currencies.Select(c => new { c.Code, c.Id }).ToList();

            return
                currencies.Select(c => new ItemOverview(c.Code, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }
    }
}