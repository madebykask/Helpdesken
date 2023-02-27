using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

public class SecretKeyHeaderAttribute : ActionFilterAttribute
{
    private readonly string _expectedSecretKey;

    public SecretKeyHeaderAttribute()
    {
        _expectedSecretKey = ConfigurationManager.AppSettings["SharePointSecretKey"]; 
    }

    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        if (actionContext.ControllerContext.Request.Headers.Contains("secretAppKey"))
        {
            //Get the value of key sekretAppKey from header
            var keyfromRequest = actionContext.ControllerContext.Request.Headers
            .GetValues("secretAppKey")
            .FirstOrDefault();
            var _expectedSecretKey = ConfigurationManager.AppSettings["SharePointSecretKey"];
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