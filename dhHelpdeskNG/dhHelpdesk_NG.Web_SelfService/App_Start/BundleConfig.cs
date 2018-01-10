using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService
{
    using System.Web.Optimization;

    public static partial class BundleConfig
    {
        public partial struct StyleNames
        {
            public const string Common = "~/Content/css/common";
        }

        public partial struct ScriptNames
        {
            public const string Orders = "~/bundles/orders/index";
            public const string EditOrder = "~/bundles/orders/orderedit";
            public const string CasesSearch = "~/bundles/cases/search";
        }
       
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            SetOptimizations();

            #region Stylesheet
            bundles.Add(new StyleBundle("~/img-profile/css").Include(
                           "~/img-profile/profile.css"));

            bundles.Add(new StyleBundle(StyleNames.Common).Include(
                            "~/Content/css/*.css",
                            "~/Content/themes/base/minified/jquery-ui.min.css",
                            "~/Content/js/jquery.plupload.queue/css/jquery.plupload.queue.css"));     
            #endregion

            #region Scripts

            bundles.Add(new ScriptBundle("~/content/js/jquery").Include(
                            "~/Scripts/jquery-1.8.3.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.js",
                            "~/Content/js/jquery.validate.unobtrusive.min.js",
                            "~/Content/js/Shared/custom.validation.requiredfrom.js",
                            "~/Content/js/Shared/custom.validation.maxlengthfrom.js",
                            "~/Scripts/jquery-ui-1.9.2.js",
                            "~/Content/js/chosen.jquery.js"
                            ));

            bundles.Add(new ScriptBundle("~/content/js/bootstrap").Include(
                            "~/Content/js/bootstrap.js",
                            "~/Content/js/bootstrap-multiselect.js",
                            "~/Content/js/bootstrap-datepicker.js",
                            "~/Content/js/plupload.full.min.js",                            
                            "~/Content/js/MicrosoftAjax.js",
                            "~/Content/js/MicrosoftMvcAjax.js",
                            "~/Content/js/typeahead.js",
                            "~/Content/js/jquery.toastmessage.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                          "~/Content/js/plupload.full.min.js",
                          "~/Content/js/jquery.plupload.queue/jquery.plupload.queue.min.js",
                          "~/Content/js/jquery.dataTables.js",
                          "~/Content/js/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Content/js/Shared/_layout").Include(
                          "~/Content/js/Shared/sortby.js",
                          "~/Content/js/Shared/app.layout.js"));

            bundles.Add(new ScriptBundle("~/Content/js/Case/edit").Include(                          
                          "~/Content/js/Case/edit.js"));

            bundles.Add(new ScriptBundle("~/Content/js/Case/extendedCase").Include(
                          "~/Content/js/iframeResizer.js",
                          "~/Content/js/Case/case.extended.js"));

            bundles.Add(new ScriptBundle("~/Content/js/Case/log").Include(
                          "~/Content/js/Case/case.log.js"));

            bundles.Add(new ScriptBundle("~/Content/js/Faq/index").Include(
                         "~/Content/js/Faq/faq.js"));

            bundles.Add(new ScriptBundle("~/Content/js/helpdesk").Include(
                         "~/Content/js/dhHelpdesk.js"));

            bundles.Add(new ScriptBundle(ScriptNames.CasesSearch).Include(
               "~/Content/js/Case/caseSearch.js"));

            bundles.Add(new ScriptBundle("~/bundles/case/caseaddfollowerssearch").Include(
               "~/Content/js/Case/_caseAddFollowersSearch.js",
               "~/Content/js/Case/_caseUserSearchCommon.js"));

            bundles.Add(new ScriptBundle(ScriptNames.Orders).Include(
                "~/Content/js/Orders/orders.list.js"));

            bundles.Add(new ScriptBundle(ScriptNames.EditOrder).Include(
                "~/Content/js/Orders/order.edit.js"));

            #endregion
        }

        private static void SetOptimizations()
        {
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}