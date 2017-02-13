using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Areas.Orders.Controllers;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc.Orders
{
    public partial class MvcOrdersUrlName
    {

        public class Orders : MvcUrlNameBase.BaseNameHelper<OrdersController>
        {
            public static string Index
            {
                get { return GetAction(e => e.Index()); }
            }

        }
    }
}