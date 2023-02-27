using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DH.Helpdesk.WebApi.Infrastructure.Attributes
{
    public class AuthorizeApiAttribute: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var tokenHasExpired = false;
            var owinContext = actionContext.Request.GetOwinContext();
            if (owinContext != null)
            {
                tokenHasExpired = owinContext.Environment.ContainsKey("oauth.token_expired");
            }

            if (tokenHasExpired)
            {
                actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                    new
                    {
                        error = "invalid_token",
                        error_message = "The Token has expired"
                    });
            }
            else
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
                                error_message = "The Secret Key is invalid"
                            });

                    }
                }
                else
                {
                    //TODO: Add messages for no permisssions, roles
                    actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                        new
                        {

                            error = "invalid_request",
                            error_message = "The Token is invalid"
                        });
            }

        }
        }
    }

    public class AuthenticationFailureMessage : HttpResponseMessage
    {
        public AuthenticationFailureMessage(string reasonPhrase, HttpRequestMessage request, object responseMessage)
            : base(HttpStatusCode.Unauthorized)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

            Content = new ObjectContent<object>(responseMessage, jsonFormatter);            
            RequestMessage = request;
            ReasonPhrase = reasonPhrase;            
        }
    }

}