using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.NewSelfService.Models.Error;
using DH.Helpdesk.NewSelfService.Infrastructure;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        [HttpGet]        
        public ActionResult Index() 
        {            
            var model = new Error()
            {
                ErrorCode = SessionFacade.LastError.ErrorCode.ToString(),
                ErrorMessage = SessionFacade.LastError.Message,
                BackURL = SessionFacade.LastCorrectUrl
            };
            
            ViewBag.HasError = "true";
            return View(model);
        }

    }
}
