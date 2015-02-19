namespace DH.Helpdesk.Web
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public struct ScriptNames
        {
            public const string DynamicCase = "~/bundles/dynamic-case";
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/img-profile/css").Include(
                            "~/img-profile/profile.css"));

            bundles.Add(new StyleBundle("~/Content/bundles/css").Include(
                            "~/Content/css/*.css",
                            "~/Content/themes/base/minified/jquery-ui.min.css",                            
                            "~/Content/js/jquery.plupload.queue/css/jquery.plupload.queue.css"));

            bundles.Add(new StyleBundle("~/Content/css/admin").Include(
                            "~/Content/css/*.css",
                            "~/Content/js/jquery.plupload.queue/css/jquery.plupload.queue.css"));

            bundles.Add(new StyleBundle("~/Content/css/login").Include(
                            "~/Content/css/*.css"));

            bundles.Add(new StyleBundle("~/Content/css/popup").Include(
                            "~/Content/css/*.css",
                            "~/Content/themes/base/minified/jquery-ui.min.css",
                            "~/Content/js/jquery.plupload.queue/css/jquery.plupload.queue.css"));

            bundles.Add(new ScriptBundle(ScriptNames.DynamicCase).Include(
                            "~/Content/js/DynamicCase/iframeResizer.js",
                            "~/Content/js/DynamicCase/container.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                            "~/Content/js/jquery-1.8.3.min.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.min.js",
                            "~/Content/js/jquery.validate.unobtrusive.min.js",
                            "~/Content/js/jquery-ui-1.9.2.min.js",
                            "~/Content/js/bootstrap.js",
                            "~/Content/js/chosen.jquery.min.js",
                            "~/Content/js/bootstrap-multiselect.js",
                            "~/Content/js/bootstrap-datepicker.js",
                            "~/Content/js/bootstrap-timepicker.min.js",
                            "~/Content/js/bootstrap-tagsinput.js",
                            "~/Content/js/plupload.full.min.js",
                            "~/Content/js/jquery.plupload.queue/jquery.plupload.queue.min.js",                            
                            "~/Content/js/MicrosoftAjax.js",
                            "~/Content/js/MicrosoftMvcAjax.js",
                            "~/Content/js/jsrender.min.js",
                            "~/Content/js/Shared/custom.validation.maxlengthfrom.notrequired.js",
                            "~/Content/js/Shared/custom.validation.requiredfrom.js",
                            "~/Content/js/Shared/custom.validation.maxlengthfrom.js",
                            "~/Content/js/Shared/sortby.js",
                            "~/Content/js/jquery.toastmessage.js",
                            "~/Content/js/bootstrap-switch.min.js",
                            "~/Content/js/dhHelpdesk-head.js",
                            "~/Content/js/ui/dh.ui.hierarchylist.js",
                            "~/Content/js/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-typing").Include(
                            "~/Content/js/jquery.typing-0.2.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dhhelpdesk").Include(
                            "~/Content/js/dhHelpdesk.js"));

            bundles.Add(new ScriptBundle("~/bundles/common/admin").Include(
                            "~/Content/js/jquery-1.8.3.min.js",
                            "~/Content/js/bootstrap.js",
                            "~/Content/js/chosen.jquery.min.js",
                            "~/Content/js/bootstrap-multiselect.js",
                            "~/Content/js/bootstrap-datepicker.js",
                            "~/Content/js/bootstrap-tagsinput.js",
                            "~/Content/js/jquery.validate.min.js",
                            "~/Content/js/plupload.full.min.js",
                            "~/Content/js/jquery.plupload.queue/jquery.plupload.queue.js",
                            "~/Content/js/jquery-ui-1.9.2.min.js",
                            "~/Content/js/dhHelpdesk-head.js",
                            "~/Content/js/jquery.form.min.js",
                            "~/Content/js/jquery.toastmessage.js",
                            "~/Content/js/bootstrap-switch.min.js",
                            "~/Content/js/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/common/login").Include(
                            "~/Content/js/jquery-1.8.3.min.js",
                            "~/Content/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/common/popup").Include(
                            "~/Content/js/jquery-1.8.3.min.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.min.js",
                            "~/Content/js/jquery.validate.unobtrusive.min.js",
                            "~/Content/js/jquery-ui-1.9.2.min.js",
                            "~/Content/js/bootstrap.js",
                            "~/Content/js/chosen.jquery.min.js",
                            "~/Content/js/bootstrap-multiselect.js",
                            "~/Content/js/bootstrap-datepicker.js",
                            "~/Content/js/bootstrap-tagsinput.js",
                            "~/Content/js/MicrosoftAjax.js",
                            "~/Content/js/MicrosoftMvcAjax.js",
                            "~/Content/js/Shared/custom.validation.maxlengthfrom.notrequired.js",
                            "~/Content/js/Shared/custom.validation.requiredfrom.js",
                            "~/Content/js/Shared/custom.validation.maxlengthfrom.js",
                            "~/Content/js/Shared/sortby.js",
                            "~/Content/js/bootstrap-switch.min.js",
                            "~/Content/js/jquery.dataTables.min.js",
                            "~/Content/js/plupload.full.min.js",
                            "~/Content/js/jquery.plupload.queue/jquery.plupload.queue.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/licenses").Include(
                            "~/Content/js/Licenses/license.js"));

            bundles.Add(new ScriptBundle("~/bundles/licenses/products").Include(
                            "~/Content/js/Licenses/products.js"));

            bundles.Add(new ScriptBundle("~/bundles/modules").Include(
                            "~/Content/js/Users/modules.js"));

            bundles.Add(new ScriptBundle("~/bundles/invoices").Include(
                            "~/Content/js/Invoice/invoice.js"));

            bundles.Add(new ScriptBundle("~/bundles/cases/index").Include("~/Content/js/Cases/index.logic.js"));
            bundles.Add(
                new ScriptBundle("~/bundles/cases/edit").Include(
                    "~/Content/js/Cases/edit.logic.js",
                            "~/Content/js/Cases/case.templates.js"));

            bundles.Add(new ScriptBundle("~/bundles/casetemplates/edit").Include(
                "~/Content/js/CaseTemplates/edit.logic.js"));


            bundles.Add(new ScriptBundle("~/bundles/changes/change").Include(
                            "~/Content/js/Changes/change.js"));

            bundles.Add(new ScriptBundle("~/bundles/changes/inventory").Include(
                            "~/Content/js/Changes/inventoryDialog.js"));

            bundles.Add(new ScriptBundle("~/bundles/changes/index").Include(
                            "~/Content/js/Changes/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/changes/search").Include(
                            "~/Content/js/Changes/search.js"));

            bundles.Add(new ScriptBundle("~/bundles/changes/settings").Include(
                            "~/Content/js/Changes/settings.js"));

            bundles.Add(new ScriptBundle("~/bundles/inventory/reports").Include(
                            "~/Content/js/Inventory/reports.js"));

            bundles.Add(new ScriptBundle("~/bundles/inventory/place-cascading").Include(
                            "~/Content/js/Inventory/place-cascading.js"));

            bundles.Add(new ScriptBundle("~/bundles/inventory/settings").Include(
                            "~/Content/js/Inventory/settings.js"));

            bundles.Add(new ScriptBundle("~/bundles/notifiers/notifier").Include(
                            "~/Content/js/Notifiers/notifier.js"));

            bundles.Add(new ScriptBundle("~/bundles/notifiers/index").Include(
                            "~/Content/js/bootstrap-switch.min.js",
                            "~/Content/js/Notifiers/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/notifiers/notifiers").Include(
                            "~/Content/js/bootstrap-switch.min.js",
                            "~/Content/js/Notifiers/notifiers.js"));

            bundles.Add(new ScriptBundle("~/bundles/notifiers/settings").Include(
                            "~/Content/js/bootstrap-switch.min.js",
                            "~/Content/js/Notifiers/settings.js"));

            bundles.Add(new ScriptBundle("~/bundles/orders/index").Include(
                            "~/Content/js/Orders/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/orders/settings").Include(
                            "~/Content/js/Orders/settings.js"));

            bundles.Add(new ScriptBundle("~/bundles/orders/order").Include(
                            "~/Content/js/Orders/order.js"));

            bundles.Add(new ScriptBundle("~/bundles/orderaccounts/order").Include(
                "~/Content/js/Account/order.js"));

            bundles.Add(new ScriptBundle("~/bundles/report").Include(
                            "~/Areas/Reports/Content/js/reports.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                            "~/Content/js/lib/angular-1.3.11/angular.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-animate.js",
                            "~/Content/js/lib/angular-1.3.11/angular-aria.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-cookies.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-loader.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-messages.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-mocks.js",
                            "~/Content/js/lib/angular-1.3.11/angular-resource.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-route.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-sanitize.min.js",
                            "~/Content/js/lib/angular-1.3.11/angular-scenario.js",
                            "~/Content/js/lib/angular-1.3.11/angular-touch.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/users").Include(
                            "~/Areas/Admin/Content/js/Users/user.js"));
        }
    }
}