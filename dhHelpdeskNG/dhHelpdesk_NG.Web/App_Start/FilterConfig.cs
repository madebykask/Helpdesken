using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.Attributes.Api;

namespace DH.Helpdesk.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
            filters.Add(new CustomHandleErrorAttribute());
		}

		public static void RegisterWebApiGlobalFilters(HttpFilterCollection filters)
		{
			filters.Add(new CustomApiErrorFilter());
			filters.Add(new ValidationApiActionFilter());
			filters.Add(new WebApiAuthorizeAttribute());
			filters.Add(new SessionApiRequiredAttribute());
			filters.Add(new ValidateApiAntiForgeryTokenAttribute());
		}


	}
}