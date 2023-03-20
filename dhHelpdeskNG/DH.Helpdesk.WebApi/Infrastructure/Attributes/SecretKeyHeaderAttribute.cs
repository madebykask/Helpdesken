using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

public class WebpartSecretKeyHeaderAttribute : ActionFilterAttribute
{
    private readonly string _expectedSecretKey;

    public WebpartSecretKeyHeaderAttribute()
    {
        _expectedSecretKey = ConfigurationManager.AppSettings["WebpartSecretKey"];
    }

    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        if (actionContext.ControllerContext.Request.Headers.Contains("webpartsecretkey"))
        {
            //Get the value of key sekretAppKey from header
            var keyfromRequest = actionContext.ControllerContext.Request.Headers
            .GetValues("webpartsecretkey")
            .FirstOrDefault();
            var _expectedSecretKey = ConfigurationManager.AppSettings["WebpartSecretKey"];
            if (keyfromRequest == _expectedSecretKey)
            {
                return;
            }
            else
            {
                actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                    new
                    {
                        error = "invalid_request",
                        error_message = "The Secret key is invalid"
                    });

            }
        }

        base.OnActionExecuting(actionContext);
    }
}