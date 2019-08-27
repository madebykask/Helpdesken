using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using DH.Helpdesk.WebApi.Infrastructure.ActionResults;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace DH.Helpdesk.WebApi.Infrastructure
{
    public abstract class BaseApiController : ApiController
    {
        protected int UserId
        {
            get
            {
                if (!User.Identity.IsAuthenticated) throw new Exception("UserId is null. User is not authenticated.");

                var userIdStr =  User.Identity.GetUserId();

                if(string.IsNullOrWhiteSpace(userIdStr)) throw new Exception("No UserID claim found.");

                return int.Parse(userIdStr);
            }
        }

        protected string UserName
        {
            get
            {
                if (!User.Identity.IsAuthenticated) throw new Exception("UserId is null. User is not authenticated.");

                var userNameStr =  User.Identity.GetUserName();

                if(string.IsNullOrWhiteSpace(userNameStr)) throw new Exception("No UserName claim found.");

                return userNameStr;
            }
        }

        protected string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }

            return null;
        }

        protected IHttpActionResult Forbidden(string msg)
        {
            return new ForbiddenResult(Request, msg);
        }

        protected T SendResponse<T>(T response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (statusCode != HttpStatusCode.OK)
            {
                var badResponse =
                    new HttpResponseMessage(statusCode)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
                    };

                throw new HttpResponseException(badResponse);
            }
            return response;
        }

    }
}