using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace DH.Helpdesk.WebApi.Infrastructure
{
    public abstract class BaseApiController : ApiController
    {
        protected int UserId
        {
            get
            {
                if (!User.Identity.IsAuthenticated) throw new Exception("UserId is null. User is not authenticated.");

                var claimsIdentity = (ClaimsIdentity) User.Identity; //TODO: Move Claims to context
                var userIdStr = claimsIdentity.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

                return int.Parse(userIdStr);

            }
        }
    }
}