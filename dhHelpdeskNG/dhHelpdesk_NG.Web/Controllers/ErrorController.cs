namespace DH.Helpdesk.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class ErrorController : Controller
    {
        private readonly ITextTranslationService _textTranslationService;

        public ErrorController(ITextTranslationService textTranslationService)
        {
            this._textTranslationService = textTranslationService;
        }

        public ActionResult Index()
        {
            return this.View("Error");
        }

        public ActionResult NotFound()
        {
            
            return this.View("Error");
        }

        public ActionResult NotFound404()
        {
            /// 999 is for errorcodes
            var text = _textTranslationService.GetAllTexts(999,null).Where(x=>x.TextToTranslate == "404 Error").FirstOrDefault();
            Response.StatusCode = 404;
            ViewBag.ErrorMessage = text.TextTranslated;
            return this.View();
        }

        public ViewResult BusinessLogicError()
        {
            return this.View();
        }

        public ViewResult Forbidden()
        {
            Response.StatusCode = 403; // Forbidden 
            return this.View("Unathorized");
        }
        

        public ViewResult Unathorized()
        {
            Response.StatusCode = 401; // unauthorized
            return this.View();
        }
    }
}
