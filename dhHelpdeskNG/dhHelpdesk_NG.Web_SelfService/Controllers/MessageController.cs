using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Models.Message;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Controllers
{
    
    public class MessageController : Controller
    {       
        public MessageController()
        {            
        
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new MessageDialogModel();
            if (SessionFacade.LastMessageDialog != null)
            {
                model = SessionFacade.LastMessageDialog;
            }

            return View(model);
        }                      
    }
}
