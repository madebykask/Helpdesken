using System.IO;
using System.Web;
using System.Web.Mvc;
using ECT.Core;
using ECT.Core.Cache;
using ECT.Model.Abstract;

namespace ECT.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string UploadPath;
        private readonly IUserRepository _userRepository;
        protected string XmlPath;

        public BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            UploadPath = Server.MapPath("~/App_Data/Uploads");

            var rd = filterContext.HttpContext.Request.RequestContext.RouteData;
            XmlPath = rd.GetRequiredString("controller");

            if(rd.DataTokens["area"] != null)
                XmlPath = rd.DataTokens["area"] + "/" + XmlPath + ".xml";
            else
                XmlPath = XmlPath + ".xml";

            if(!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["clearsession"]))
                Session.Clear();

            if(!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["clearcache"]))
            {
                var cache = new CacheProvider();
                cache.InvalidateAll();
            }

            if(SessionFacade.User == null)
            {
                var identity = User.Identity.Name;
                var userId = filterContext.RequestContext.HttpContext.Request.QueryString["userId"];

                var user = _userRepository.Get(identity, !string.IsNullOrEmpty(userId) ? userId : null);

                if(user != null)
                    SessionFacade.User = user;
                else
                    throw new HttpException(401, "Unauthorized access");
            }

            if(!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["language"]) && SessionFacade.User != null)
                SessionFacade.User.Language = filterContext.RequestContext.HttpContext.Request.QueryString["language"].ToLower();

            base.OnAuthorization(filterContext);
        }

        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial ? ViewEngines.Engines.FindPartialView(ControllerContext, viewName) : ViewEngines.Engines.FindView(ControllerContext, viewName, null);

            if(viewResult == null || (viewResult != null && viewResult.View == null))
                throw new FileNotFoundException("View could not be found");

            ViewData.Model = model;
            using(var sw = new StringWriter())
            {
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected RedirectToRouteResult RedirectToActionWithMessage(string actionName, string controllerName, string message)
        {
            ViewData[Constants.ViewData.Message] = message;
            return RedirectToAction(actionName, controllerName);
        }
    }
}
