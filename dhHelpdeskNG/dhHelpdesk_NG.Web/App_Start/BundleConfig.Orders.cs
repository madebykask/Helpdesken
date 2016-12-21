using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc.Orders;

namespace DH.Helpdesk.Web
{
    public partial class BundleConfig
    {
        public partial struct ScriptNames
        {
            public struct Orders
            {
                public const string EditOrder = ("~/bundles/orders/orderedit");
            }
        }

        public static void RegisterOrdersAreaBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(ScriptNames.Orders.EditOrder).Include(
                        "~/Areas/" + MvcOrdersUrlName.Name + "/Content/js/Order/order.edit.js"));

        }
    }
}