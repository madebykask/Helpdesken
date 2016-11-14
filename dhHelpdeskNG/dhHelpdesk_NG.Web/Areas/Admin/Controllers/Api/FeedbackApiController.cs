using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Attributes.Api;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers.Api
{
	[PermissionsApi(Roles = "3,4")]
	public class FeedbackApiController : BaseApiController
    {
	    [HttpGet]
	    public async Task<object> Test()
	    {
		    return new { Time =  DateTime.Now, Name = SessionFacade.CurrentUser.FirstName };
	    }

		[HttpPost]
		public async Task<object> TestPost(TestModel model)
		{
			return new { Time = DateTime.Now, Name = SessionFacade.CurrentUser.FirstName };
		}

		public class TestModel
		{
			[Required]
			public string Required { get; set; }
		}
	}
}
