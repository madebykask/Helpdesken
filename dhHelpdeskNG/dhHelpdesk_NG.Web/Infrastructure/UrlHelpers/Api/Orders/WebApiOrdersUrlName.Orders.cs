using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Areas.Orders.Controllers.WebApi;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api.Orders
{
    public partial class WebApiOrdersUrlName
    {
        public static string Name => "Orders";

        public class Orders : WebApiUrlNameBase.BaseNameHelper<OrdersApiController>
        {
            public static string SearchDepartmentsByRegionId
            {
                get { return GetAction(e => e.SearchDepartmentsByRegionId(null)); }
            }

        }
    }
}