namespace DH.Helpdesk.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Common.Enums;

    public class ErrorController : Controller
    {        
        private readonly IMasterDataService _masterDataService;

        

        public ErrorController(IMasterDataService masterDataService)
        {
            this._masterDataService = masterDataService;
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
            var errorMessage = "Error 404: Page was not found!";
            /*var curLanguageId = GetMessageLanguageId();

            if (curLanguageId > 0)
            {                
                var text = this._masterDataService.GetTranslationTexts()
                                                  .Where(t => t.Type == TranslationType.ErrorText && t.TextToTranslate == "404 Error")
                                                  .FirstOrDefault();
                if (text != null)
                {
                    var textForCurrentLang = text.TextTranslations.Where(tt => tt.Language_Id == curLanguageId).FirstOrDefault();
                    if (textForCurrentLang != null)
                        errorMessage = textForCurrentLang.TextTranslated;
                    else
                    {
                        var firstTranslation = text.TextTranslations.FirstOrDefault();
                        if (firstTranslation != null)
                            errorMessage = firstTranslation.TextTranslated;
                    }
                }
            }
            */
            ViewBag.ErrorMessage = errorMessage;
           // Response.StatusCode = 404;
            return this.View();
        }

        //private int GetMessageLanguageId()
        //{
        //    var ret = 0;
        //    var globalSetting = this._masterDataService.GetGlobalSettings().FirstOrDefault();

        //    if (globalSetting != null)
        //    {                
        //        if (SessionFacade.CurrentLanguageId > 0)
        //            ret = SessionFacade.CurrentLanguageId;
        //        else
        //            ret = globalSetting.DefaultLanguage_Id;
        //    }

        //    return ret;
        //}

        public ViewResult BusinessLogicError()
        {
            return this.View();
        }

        public ViewResult Forbidden()
        {
            //Response.StatusCode = 403; // Forbidden 
            return this.View("Unathorized");
        }
        
        public ViewResult Unathorized()
            {
            //Response.StatusCode = 401; // unauthorized
            return this.View();
        }
    }
}
