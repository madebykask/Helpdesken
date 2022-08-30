namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public CurrencyService(
            ICurrencyRepository currencyRepository,
            IUnitOfWork unitOfWork)
        {
            this._currencyRepository = currencyRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> Validate(Currency currencyToValidate)
        {
            if (currencyToValidate == null)
                throw new ArgumentNullException("currencytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<Currency> GetCurrencies()
        {
            return this._currencyRepository.GetAll().OrderBy(x => x.Code).ToList();
        }

        public Currency GetCurrency(int id)
        {
            return this._currencyRepository.Get(x => x.Id == id);
        }

        public void DeleteCurrency(Currency currency)
        {
            this._currencyRepository.Delete(currency);
        }

        public void NewCurrency(Currency currency)
        {
            currency.ChangedDate = DateTime.UtcNow;
            this._currencyRepository.Add(currency);
        }

        public void UpdateCurrency(Currency currency)
        {
            currency.ChangedDate = DateTime.UtcNow;
            this._currencyRepository.Update(currency);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
