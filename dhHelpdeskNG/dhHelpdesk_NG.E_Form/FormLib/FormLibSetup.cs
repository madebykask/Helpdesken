using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DH.Helpdesk.EForm.FormLib
{
    public class FormLibSetup
    {
        public static void Setup()
        {
            SetupControllers();
            SetupViewEngine();
            SetupBundles();
        }

        public static void SetupRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = false;
            routes.IgnoreRoute("Static/{*pathInfo}");
        }

        private static void SetupViewEngine()
        {
            ViewEngines.Engines.Add(new FormLibViewEngine());
        }

        private static void SetupControllers()
        {
            ControllerBuilder.Current.DefaultNamespaces.Add("DH.Helpdesk.EForm.FormLib.Controllers");
        }

        private static void SetupBundles()
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/FormLibContent/assets/js/bundle").Include(
                        "~/FormLibContent/assets/js/jquery-1.11.0.js",
                        "~/FormLibContent/assets/js/bootstrap.js",
                        "~/FormLibContent/assets/js/bootstrap-datepicker.js",
                        "~/FormLibContent/assets/js/prettify.js",
                        "~/FormLibContent/assets/plupload/plupload.full.js",
                        "~/FormLibContent/assets/selectize/js/standalone/selectize.js",
                        "~/FormLibContent/assets/selectize/js/es5-shim.js",
                        "~/FormLibContent/assets/js/norway.js",
                        "~/FormLibContent/Assets/js/integration.js",
                        "~/FormLibContent/assets/js/spin.js",
                        "~/FormLibContent/assets/js/ect.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/FormLibContent/assets/css/bundle").Include(
                "~/FormLibContent/assets/css/bootstrap.css",
                "~/FormLibContent/assets/css/datepicker.css",
                "~/FormLibContent/assets/selectize/css/selectize.bootstrap2.css",
                "~/FormLibContent/assets/css/ect.css"));

            BundleTable.EnableOptimizations = true;

#if DEBUG
            BundleTable.EnableOptimizations = false;
#endif
        }
    }
}