namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
            this._countryRepository = countryRepository;
        }

        public IList<Country> GetCountries(int customerId)
        {
            return this._countryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }
    }
}
