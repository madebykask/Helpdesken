namespace DH.Helpdesk.NewSelfService
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
       
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Stylesheet
                     
            bundles.Add(new StyleBundle("~/content/css/selfservice").Include(
                            "~/Content/css/jquery.toastmessage.css",
                            "~/Content/css/bootstrap.css",          
                            "~/Content/css/selfservice.css"));

            bundles.Add(new StyleBundle("~/img-profile/profile").Include(
                            "~/img-profile/profile.css"));

            bundles.Add(new StyleBundle("~/Content/css/popup").Include(
                            "~/Content/css/*.css",
                            "~/Content/themes/base/minified/jquery-ui.min.css",
                            "~/Content/js/jquery.plupload.queue/css/jquery.plupload.queue.css"));

            bundles.Add(new StyleBundle("~/Content/bundles/css").Include(
                           "~/Content/css/*.css",
                           "~/Content/themes/base/minified/jquery-ui.min.css",
                           "~/Content/js/jquery.plupload.queue/css/jquery.plupload.queue.css"));
            
            #endregion

            #region Scripts

            bundles.Add(new ScriptBundle("~/content/js/jquery").Include(
                            "~/Content/js/jquery.js",
                            "~/Content/js/jquery-1.11.0.min.js",
                            "~/Content/js/jquery-ui-1.9.2.min.js",
                            "~/Content/js/jquery.validate.js",
                            "~/Content/js/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/content/js/bootstrap").Include(
                            "~/Content/js/bootstrap.js",
                            "~/Content/js/bootstrap-multiselect.js",
                            "~/Content/js/bootstrap-datepicker.js",

                            "~/Content/js/plupload.full.min.js",                            
                            "~/Content/js/MicrosoftAjax.js",
                            "~/Content/js/MicrosoftMvcAjax.js",
                            "~/Content/js/typeahead.js",
                            "~/Content/js/jquery.toastmessage.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                          "~/Content/js/plupload.full.min.js",
                          "~/Content/js/jquery.plupload.queue/jquery.plupload.queue.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/Shared/_layout").Include(
                          "~/Content/js/Shared/sortby.js",
                          "~/Content/js/Shared/app.layout.js"));
                    
            #endregion
        }
    }
}