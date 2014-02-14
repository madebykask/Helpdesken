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

        string GetCurrencyCodeById(int currencyId);

        int GetCurrencyIdByCode(string code);
    }

    public sealed class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews()
        {
            var currencies = this.DataContext.Currencies.Select(c => new { c.Id, c.Code }).ToList();

            return
                currencies.Select(c => new ItemOverview(c.Code, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public string GetCurrencyCodeById(int currencyId)
        {
            return this.DataContext.Currencies.Where(c => c.Id == currencyId).Select(c => c.Code).Single();
        }

        public int GetCurrencyIdByCode(string code)
        {
            return this.DataContext.Currencies.Where(c => c.Code == code).Select(c => c.Id).Single();
        }
    }
}