using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

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