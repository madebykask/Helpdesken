using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers.Api
{
	//[CustomAuthorize(Roles = "3,4")]
	public class FeedbackApiController : BaseApiController
    {
	    [HttpGet]
	    public object Test()
	    {
		    return new { Time =  DateTime.Now};
	    }
    }
}
