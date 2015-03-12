using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.NewSelfService.Models.Error;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        [HttpGet]
        public ActionResult Index(int errorCode = -1, string message = "")
        {
            
            var model = new ErrorModel()
            {
                ErrorCode = errorCode,
                ErrorMessage = message
            };

            ViewBag.HasError = "true";
            return View(model);
        }

    }
}
