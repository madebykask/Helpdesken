namespace DH.Helpdesk.SelfService
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
       
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Stylesheet
            bundles.Add(new StyleBundle("~/img-profile/css").Include(
                           "~/img-profile/profile.css"));

            bundles.Add(new StyleBundle("~/Content/bundles/css").Include(
                            "~/Content/css/*.css",
                            "~/Content/themes/base/minified/jquery-ui.min.css",
                            "~/Content/js/jquery.plupload.queue/css/jquery.plupload.queue.css"));     
            #endregion

            #region Scripts

            bundles.Add(new ScriptBundle("~/content/js/jquery").Include(
                            #if DEBUG
                            "~/Scripts/jquery-1.8.3.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.js",
                            #else
                            "~/Scripts/jquery-1.8.3.min.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.min.js",
                            #endif
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.unobtrusive.min.js",
                            #if DEBUG
                            "~/Scripts/jquery-ui-1.9.2.js",
                            "~/Content/js/chosen.jquery.js"
                            #else
                            "~/Scripts/jquery-ui-1.9.2.min.js",
                            "~/Content/js/chosen.jquery.min.js"
                            #endif
                            ));

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

            bundles.Add(new ScriptBundle("~/Content/js/Case/edit").Include(
                          "~/Content/js/Case/edit.js"));

            bundles.Add(new ScriptBundle("~/Content/js/Case/log").Include(
                          "~/Content/js/Case/case.log.js"));

            #endregion
        }
    }
}