namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;

    public class CurrencyController : BaseAdminController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(
            ICurrencyService currencyService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._currencyService = currencyService;
        }

        public ActionResult Index()
        {
            var currencies = this._currencyService.GetCurrencies();
            
            return this.View(currencies);
        }

        public ActionResult New()
        {
            return this.View(new Currency());
        }

        [HttpPost]
        public ActionResult New(Currency currency)
        {
            if (this.ModelState.IsValid)
            {
                this._currencyService.NewCurrency(currency);
                this._currencyService.Commit();

                return this.RedirectToAction("index", "currency", new { area = "admin" });
            }

            return this.View(currency);
        }

        public ActionResult Edit(int id)
        {
            var currency = this._currencyService.GetCurrency(id);

            if (currency == null)                
                return new HttpNotFoundResult("No currency found...");

            return this.View(currency);
        }

        [HttpPost]
        public ActionResult Edit(Currency currency)
        {
            if (this.ModelState.IsValid)
            {
                this._currencyService.UpdateCurrency(currency);
                this._currencyService.Commit();

                return this.RedirectToAction("index", "currency", new { area = "admin" });
            }

            return this.View(currency);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var currency = this._currencyService.GetCurrency(id);

            if (currency != null)
            {
                this._currencyService.DeleteCurrency(currency);
                this._currencyService.Commit();
            }

            return this.RedirectToAction("index", "currency", new { area = "admin" });
        }
    }
}
