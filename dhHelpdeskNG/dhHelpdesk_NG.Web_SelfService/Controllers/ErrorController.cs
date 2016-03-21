using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.SelfService.Models.Error;
using DH.Helpdesk.SelfService.Infrastructure;

namespace DH.Helpdesk.SelfService.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        [HttpGet]        
        public ActionResult Index() 
        {            
            var model = new Error();
            if (SessionFacade.LastError != null)
            {
                model.ErrorCode = SessionFacade.LastError.ErrorCode.ToString();
                model.ErrorMessage = SessionFacade.LastError.Message;               
            };

            model.BackURL = SessionFacade.LastCorrectUrl;

            
            return View(model);
        }

    }
}
