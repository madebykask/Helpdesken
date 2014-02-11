using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Web.Models.Questionnire;
namespace DH.Helpdesk.Web.Controllers
{
    public class QuestionnireController : Controller
    {
        //
        // GET: /Questionnire/

        public ActionResult Index()
        {
            return View();
            
        }

    }
}
