using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using DH.Helpdesk.WebApi.Infrastructure.Config.Authentication;
using Microsoft.AspNet.Identity;

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

    }
}