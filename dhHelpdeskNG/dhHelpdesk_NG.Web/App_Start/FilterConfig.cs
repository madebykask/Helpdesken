using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace DH.Helpdesk.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
#if !DEBUG
            filters.Add(new CustomHandleErrorAttribute());
#endif
		}

		public static void RegisterWebApiGlobalFilters(HttpFilterCollection filters)
		{
			//filters.Add(new MdPortalWebApiErrorFilter());
			//filters.Add(new ValidationActionFilter());
			//filters.Add(new WebApiAuthorizeAttribute());
			//filters.Add(new Web.Common.Attributes.WebApi.ValidateAntiForgeryTokenAttribute());
		}


	}
}