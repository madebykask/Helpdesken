using System.Web.Optimization;

namespace ECT.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/assets/js/bundle").Include(
                        "~/assets/js/jquery-1.11.0.js",
                        "~/assets/js/bootstrap.js",
                        "~/assets/js/bootstrap-datepicker.js",
                        //"~/assets/js/locales/bootstrap-datepicker.*",
                        "~/assets/js/prettify.js",
                        //"~/assets/js/jquery.validate.js",
                        //"~/assets/js/jquery.validate.additional-methods.js",
                        "~/assets/plupload/plupload.full.js",
                        "~/assets/js/ect.js"));

            bundles.Add(new StyleBundle("~/assets/css/bundle").Include(
                "~/assets/css/bootstrap.css",
                "~/assets/css/datepicker.css",
                "~/assets/css/ect.css"));          
        }
    }
}