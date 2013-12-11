using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICountryService
    {
        IList<Country> GetCountries(int customerId);
    }

    public class CountryService : ICountryService
    {
        public readonly ICountryRepository _countryRepository;

        public CountryService(
            ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public IList<Country> GetCountries(int customerId)
        {
            return _countryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }
    }
}
