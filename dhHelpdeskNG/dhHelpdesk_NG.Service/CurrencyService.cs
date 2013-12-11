using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICurrencyService
    {
        IDictionary<string, string> Validate(Currency currencyToValidate);

        IList<Currency> GetCurrencies();

        Currency GetCurrency(int id);

        void DeleteCurrency(Currency currency);
        void NewCurrency(Currency currency);
        void UpdateCurrency(Currency currency);
        void Commit();
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(
            ICurrencyRepository currencyRepository,
            IUnitOfWork unitOfWork)
        {
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(Currency currencyToValidate)
        {
            if (currencyToValidate == null)
                throw new ArgumentNullException("currencytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<Currency> GetCurrencies()
        {
            return _currencyRepository.GetAll().OrderBy(x => x.Code).ToList();
        }

        public Currency GetCurrency(int id)
        {
            return _currencyRepository.Get(x => x.Id == id);
        }

        public void DeleteCurrency(Currency currency)
        {
            _currencyRepository.Delete(currency);
        }

        public void NewCurrency(Currency currency)
        {
            currency.ChangedDate = DateTime.UtcNow;
            _currencyRepository.Add(currency);
        }

        public void UpdateCurrency(Currency currency)
        {
            currency.ChangedDate = DateTime.UtcNow;
            _currencyRepository.Update(currency);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
