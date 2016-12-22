using System.Web.Mvc;
using ECT.Model.Abstract;
using ECT.Web.Controllers;

namespace ECT.Web.Areas.Poland.Controllers
{
    public class DeadController : BaseController
    {
        public DeadController(IUserRepository userRepository)
            : base(userRepository)
        {}

        public ActionResult Index()
        {
            return View();
        }
    }
}
