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
        List<ItemOverviewDto> FindOverviews();
    }

    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviews()
        {
            var currencies = this.DataContext.Currencies.Select(c => new { c.Code, c.Id }).ToList();
         
            return
                currencies.Select(
                    c => new ItemOverviewDto { Name = c.Code, Value = c.Id.ToString(CultureInfo.InvariantCulture) })
                    .ToList();
        }
    }
}