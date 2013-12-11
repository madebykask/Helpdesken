using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(
            ICurrencyService currencyService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _currencyService = currencyService;
        }

        public ActionResult Index()
        {
            var currencies = _currencyService.GetCurrencies();
            
            return View(currencies);
        }

        public ActionResult New()
        {
            return View(new Currency());
        }

        [HttpPost]
        public ActionResult New(Currency currency)
        {
            if (ModelState.IsValid)
            {
                _currencyService.NewCurrency(currency);
                _currencyService.Commit();

                return RedirectToAction("index", "currency", new { area = "admin" });
            }

            return View(currency);
        }

        public ActionResult Edit(int id)
        {
            var currency = _currencyService.GetCurrency(id);

            if (currency == null)                
                return new HttpNotFoundResult("No currency found...");

            return View(currency);
        }

        [HttpPost]
        public ActionResult Edit(Currency currency)
        {
            if (ModelState.IsValid)
            {
                _currencyService.UpdateCurrency(currency);
                _currencyService.Commit();

                return RedirectToAction("index", "currency", new { area = "admin" });
            }

            return View(currency);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var currency = _currencyService.GetCurrency(id);

            if (currency != null)
            {
                _currencyService.DeleteCurrency(currency);
                _currencyService.Commit();
            }

            return RedirectToAction("index", "currency", new { area = "admin" });
        }
    }
}
