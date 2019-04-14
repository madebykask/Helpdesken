namespace DH.Helpdesk.Web
{
    using System.Web.Optimization;

    public partial class BundleConfig
    {
        public struct StylesNames
        {
        }

        public partial struct ScriptNames
        {
            public const string DynamicCase = "~/bundles/dynamic-case";
            public const string AttributesValidation = "~/bundles/jqueryattrval";
            public const string InventoryUserSearch = ("~/bundles/inventory/inventorysearch");
            public const string CaseIntLogEmailSearch = ("~/bundles/case/caseintlogemailsearch");
            public const string UserSearchCommon = ("~/bundles/common/usersearchcommon");
            public const string CaseCharge = ("~/bundles/case/CaseCharge");
            public const string Select2 = "~/bundles/select2";
            public const string CaseConnectToParent = "~/bundles/case/caseconnecttoparent";
            public const string FeedbackStatisticsCases = "~/bundles/case/feedbackstatisticscases";
            public const string QuickLinks = "~/bundles/admin/quicklinks";
            public const string OrderTypes = "~/bundles/admin/ordertypes/index";
            public const string FeedbackEdit = "~/bundles/feedback/feedbackedit";
            public const string CaseAttachExistingFiles = "~/bundles/case/attachexfile";
            public const string ConfirmationDialog = "~/bundles/confirmdialog";

            //advanced search 
            public const string AdvancedSearchPage = "~/bundles/advancedsearch";
            
            //inventory
            public const string InventoryOverview = "~/bundles/inventory/overview";
            public const string InventoryRelatedCases = "~/bundles/inventory/relatedcases";
            public const string InventoryFiles = "~/bundles/inventory/files";
        }


        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new StyleBundle("~/img-profile/css").Include(
                            "~/img-profile/profile.css"));

            bundles.Add(new StyleBundle("~/Content/bundles/css").Include(
                            "~/Content/css/*.css",
                            "~/Content/themes/base/minified/jquery-ui.min.css",
                            "~/Content/font-awesome.min.css",
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
                            "~/Content/js/DynamicCase/container.js"));

            bundles.Add(new ScriptBundle("~/Content/js/iframeResizer").Include(
                            "~/Content/js/DynamicCase/iframeResizer.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                            "~/Content/js/Shared/commonUtils.js",
                            "~/Content/js/Shared/errors.js",
                            "~/Content/js/Shared/jquery.customAjax.js",

#if DEBUG
                            "~/Scripts/jquery-1.8.3.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.js",
                            "~/Content/js/additional-methods.js",
#else
                            "~/Scripts/jquery-1.8.3.min.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.min.js",
                            "~/Content/js/additional-methods.min.js",
#endif
                "~/Content/js/jquery.unobtrusive-ajax.min.js",
                "~/Content/js/jquery.validate.unobtrusive.min.js",
#if DEBUG
 "~/Scripts/jquery-ui-1.9.2.js",
                            "~/Content/js/chosen.jquery.js",
#else
                            "~/Scripts/jquery-ui-1.9.2.min.js",
                            "~/Content/js/chosen.jquery.min.js",
#endif
                            "~/Content/js/bootstrap.js",
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
                            "~/Content/js/Shared/custom.validation.reuiredifnotempty.js",
                            "~/Content/js/Shared/sortby.js",
                            "~/Content/js/jquery.toastmessage.js",
                            "~/Content/js/bootstrap-switch.min.js",
                            "~/Content/js/dhHelpdesk-head.js",
                            "~/Content/js/ui/dh.ui.hierarchylist.js",
                            "~/Content/js/jquery.dataTables.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-typing").Include(
                            "~/Content/js/jquery.typing-0.2.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dhhelpdesk").Include(
                            "~/Content/js/dhHelpdesk.js",
                            "~/Content/js/snippets/clickmultimenu_dropdown.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/common/login").Include(
                            "~/Content/js/Shared/errors.js",
                            "~/Scripts/jquery-1.8.3.min.js",
                            "~/Content/js/jquery.validate.min.js",
                            "~/Content/js/jquery.toastmessage.js",
                            "~/Content/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/common/popup").Include(
                            "~/Content/js/Shared/errors.js",
                            "~/Content/js/Shared/jquery.customAjax.js",
                            "~/Scripts/jquery-1.8.3.min.js",
                            "~/Content/js/jquery.unobtrusive-ajax.min.js",
                            "~/Content/js/jquery.validate.min.js",
                            "~/Content/js/additional-methods.min.js",
                            "~/Content/js/jquery.validate.unobtrusive.min.js",
                            "~/Content/js/jquery-ui-1.9.2.min.js",
                            "~/Content/js/jquery.toastmessage.js",
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

            bundles.Add(new ScriptBundle("~/bundles/licensesedit").Include(
                            "~/Content/js/Licenses/license.editlogic.js"));

            bundles.Add(new ScriptBundle("~/bundles/licenses/products").Include(
                            "~/Content/js/Licenses/products.js"));

            bundles.Add(new ScriptBundle("~/bundles/modules").Include(
                            "~/Content/js/Users/modules.js"));

            bundles.Add(new ScriptBundle(ScriptNames.Select2).Include(
                            "~/Content/js/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/invoices").Include(                                                      
                            "~/Content/js/bootstrap-multiselect.js",
                            "~/Content/js/chosen.jquery.min.js",           
                            "~/Content/js/Invoice/invoice.js"));

            bundles.Add(new ScriptBundle("~/bundles/cases/index").Include(
                "~/Scripts/jquery.cookie.js",
                "~/Content/js/Cases/components/Utils.js",
                "~/Content/js/Cases/components/BaseField.js",
                "~/Content/js/Cases/components/DropdownButtonField.js",
                "~/Content/js/Cases/components/JQueryChosenField.js",
                "~/Content/js/Cases/components/DateField.js",
                "~/Content/js/Cases/components/FilterForm.js",
                "~/Content/js/Cases/index.logic.js",
                "~/Content/js/Cases/index.settings.js"));

            //todo: to be deleted after new is complete
            bundles.Add(new ScriptBundle("~/bundles/advancedsearch/specialfilter").Include(
                "~/Content/js/AdvancedSearch/index.specialfilter.js"));

            bundles.Add(new ScriptBundle("~/bundles/advancedsearch/index").Include(
                "~/Content/js/AdvancedSearch/index.logic.js"));

            bundles.Add(new ScriptBundle(ScriptNames.AdvancedSearchPage).Include(
                "~/Content/js/AdvancedSearch/advancedSearchPage.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/contract").Include(
                "~/Content/js/contract/contractIndex.js",
                "~/Content/js/contract/contractCases.js"));

            bundles.Add(new ScriptBundle("~/bundles/faqs").Include(
                "~/Content/js/faqs/faqsPage.js"));

            #region Case editing
            bundles.Add(new StyleBundle("~/cases/dynamic-cases").Include(
                         "~/Content/css/custom/dynamic-cases.css"));

            bundles.Add(new ScriptBundle("~/bundles/cases/new").Include(
                "~/Content/js/Cases/caseInitForm.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/cases/edit").Include(
                "~/Content/js/Cases/caseInitForm.js",
                "~/Content/js/snippets/tabSwitchByHashTag.js"));

            bundles.Add(new ScriptBundle("~/bundles/Cases/_CaseLogInput").Include(
                "~/Content/js/Cases/_caseLogInput.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/cases/_input").Include(
                    "~/Content/js/Cases/components/ConfirmationDialog.js",
                    "~/Content/js/Cases/models/Case.js",
                    "~/Content/js/Cases/components/Utils.js",
                    "~/Content/js/Cases/components/EditPage.js",
                    "~/Content/js/Cases/_input.js",
                    "~/Content/js/snippets/dropdown_fix.js",
                    "~/Content/js/Cases/case.templates.js",
                    "~/Content/js/jsrender.min.js",
                    "~/Content/js/Cases/externalInvoice.js"));

            bundles.Add(new ScriptBundle("~/bundles/cases/_caseLogFiles").Include("~/Content/js/Cases/_caseLogFiles.js"));
            bundles.Add(
                new ScriptBundle("~/bundles/Cases/_ChildCases").Include(
                    "~/Content/js/Cases/_childCases.js"));

            bundles.Add(new ScriptBundle("~/bundles/cases/editLog").Include("~/Content/js/Cases/editLog.logic.js"));
            bundles.Add(new ScriptBundle(ScriptNames.CaseCharge).Include("~/Content/js/Cases/Dialogs/CaseCharge.js"));

            #endregion

            bundles.Add(new ScriptBundle("~/bundles/casetemplates/edit").Include(
                "~/Content/js/CaseTemplates/edit.logic.js"));

            bundles.Add(new ScriptBundle("~/bundles/casetemplates/index").Include(
                "~/Content/js/CaseTemplates/index.logic.js"));

            bundles.Add(new ScriptBundle("~/bundles/caserules/logic").Include(
                "~/Content/js/CaseRules/case.rule.logics.js"));


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
                            "~/Content/js/Notifiers/edit.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/contracts/edit").Include(
               "~/Content/js/Contract/edit.js"));


            bundles.Add(new ScriptBundle("~/bundles/changepassword").Include(
                "~/Content/js/ChangePassword/PasswordValidator.js",
                "~/Content/js/ChangePassword/_changePasswordDialog.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
#if DEBUG
                "~/Scripts/knockout-3.4.0.debug.js"
#else
                "~/Scripts/knockout-3.4.0.js"
#endif
                ));

            #region Admin scripts

            bundles.Add(new ScriptBundle("~/bundles/report").Include(
                            "~/Areas/Reports/Content/js/reportViewer.logic.js",
                            "~/Areas/Reports/Content/js/reports.js"));

            bundles.Add(new ScriptBundle("~/bundles/admininvoice").Include(
                           "~/Areas/admin/Content/js/invoice/invoicearticleIndex.js",
                           "~/Areas/admin/Content/js/invoice/invoicearticleproductareaInput.js"));

            bundles.Add(new ScriptBundle("~/bundles/admindataprivacy").Include(
                    "~/Areas/Admin/Content/js/DataPrivacy/_dataPrivacyForm.js",
                    "~/Content/js/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/common/admin").Include(
                           "~/Content/js/Shared/errors.js",
                           "~/Content/js/Shared/jquery.customAjax.js",
                           "~/Scripts/jquery-1.8.3.min.js",
                           "~/Scripts/jquery-ui-1.9.2.min.js",
                           "~/Content/js/bootstrap.js",
                           "~/Content/js/chosen.jquery.min.js",
                           "~/Content/js/bootstrap-multiselect.js",
                           "~/Content/js/bootstrap-datepicker.js",
                           "~/Content/js/bootstrap-tagsinput.js",
                        #if DEBUG
                            "~/Content/js/jquery.validate.js",
                            "~/Content/js/additional-methods.js",
#else
                            "~/Content/js/jquery.validate.min.js",
                            "~/Content/js/additional-methods.min.js",
#endif
                            "~/Content/js/plupload.full.min.js",
                           "~/Content/js/jquery.plupload.queue/jquery.plupload.queue.js",
                           "~/Content/js/dhHelpdesk-head.js",
                           "~/Content/js/jquery.form.min.js",
                           "~/Content/js/jquery.toastmessage.js",
                           "~/Content/js/bootstrap-switch.min.js",
                           "~/Content/js/jquery.dataTables.min.js")); //To apply changes on tinymce use>>   ?cachebuster=123

            bundles.Add(new ScriptBundle("~/bundles/admin/users").Include(
                            "~/Areas/Admin/Content/js/Users/user.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/users/_input").Include(
                            "~/Areas/Admin/Content/js/Users/_input.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/casetype/_input").Include(
                            "~/Areas/Admin/Content/js/CaseType/_input.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/users/index_lockedcases").Include(
                           "~/Areas/Admin/Content/js/Users/index.lockedcase.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/users/index").Include(
                           "~/Scripts/jquery.cookie.js",
                           "~/Areas/Admin/Content/js/Users/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/customerOverview").Include(
                            "~/Areas/Admin/Content/js/CaseOverview/customerOverview.js",
                            "~/Areas/Admin/Content/js/Common/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/customerCaseSettings").Include(
                            "~/Areas/Admin/Content/js/Customer/customerCaseSettings.js",
                            "~/Areas/Admin/Content/js/Common/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/ProductArea/index").Include(
                "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
                "~/Areas/Admin/Content/js/ProductArea/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/CaseType/index").Include(
                "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
                "~/Areas/Admin/Content/js/CaseType/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/WorkingGroup/index").Include(
              "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
              "~/Areas/Admin/Content/js/WorkingGroup/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/CausingPart/index").Include(
             "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
             "~/Areas/Admin/Content/js/CausingPart/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/Category/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/Category/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/Supplier/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/Supplier/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/Priority/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/Priority/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/Status/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/Status/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/StateSecondary/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/StateSecondary/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/RegistrationSourceCustomer/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/RegistrationSourceCustomer/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/Region/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/Region/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/Department/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/Department/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/OU/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/OU/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/StandardText/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/StandardText/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/FinishingCause/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/FinishingCause/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/DailyReportSubject/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/DailyReportSubject/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/OperationObject/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/OperationObject/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/OperationLogCategory/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/OperationLogCategory/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/EMailGroup/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/EMailGroup/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/Program/index").Include(
            "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
            "~/Areas/Admin/Content/js/Program/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/cases/relatedCases").Include(
                            "~/Content/js/Cases/relatedCases.logic.js"));

            bundles.Add(new ScriptBundle("~/bundles/cases/caseByIds").Include(
                            "~/Content/js/Cases/caseByIds.logic.js"));

            bundles.Add(new ScriptBundle("~/bundles/businessrule/businessRules").Include(
                "~/Content/js/jsrender.min.js",
                "~/Areas/Admin/Content/js/BusinessRule/businessRuleInput.js",
                "~/Content/js/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle(ScriptNames.QuickLinks).Include(
                "~/Areas/Admin/Content/js/QuickLinks/index.logic.js"));

            bundles.Add(new ScriptBundle(ScriptNames.OrderTypes).Include(
                "~/Areas/Admin/Content/js/Common/ToggableInactiveList.js",
                "~/Areas/Admin/Content/js/OrderType/index.logic.js"));

            #endregion

            bundles.Add(new ScriptBundle(ScriptNames.AttributesValidation).Include(
                "~/Content/js/Shared/custom.validation.reuiredifnotempty.js"));
            bundles.Add(new ScriptBundle(ScriptNames.InventoryUserSearch).Include(
                "~/Content/js/Inventory/inventory.search.js"));
            bundles.Add(new ScriptBundle(ScriptNames.CaseIntLogEmailSearch).Include(
                "~/Content/js/Cases/Dialogs/_caseIntLogSearch.js",
                "~/Content/js/Common/userSearchCommon.js"));
            bundles.Add(new ScriptBundle(ScriptNames.UserSearchCommon).Include(
                "~/Content/js/Common/userSearchCommon.js"));
            bundles.Add(new ScriptBundle(ScriptNames.CaseConnectToParent).Include(
                "~/Content/js/Cases/Dialogs/_caseConnectToParent.js",
                "~/Content/js/Cases/components/FilterForm.js",
                "~/Content/js/Cases/components/BaseField.js",
                "~/Content/js/Cases/components/JQueryChosenField.js",
                "~/Content/js/Cases/components/DateField.js",
                "~/Content/js/Cases/components/JQueryChosenField.js",
                "~/Content/js/Cases/components/DropdownButtonField.js"));
            bundles.Add(new ScriptBundle(ScriptNames.FeedbackStatisticsCases).Include(
                "~/Content/js/Feedback/feedbackStatisticsCases.js",
                "~/Content/js/Cases/components/Utils.js"));
            bundles.Add(new ScriptBundle(ScriptNames.FeedbackEdit).Include(
                "~/Content/js/Feedback/feedback.edit.js"));
            bundles.Add(new ScriptBundle(ScriptNames.CaseAttachExistingFiles).Include(
                "~/Content/js/Cases/Dialogs/_caseAttachExistFile.js"));
            bundles.Add(new ScriptBundle(ScriptNames.ConfirmationDialog).Include(
                "~/Content/js/Cases/components/ConfirmationDialog.js",
                "~/Content/js/Cases/components/Utils.js"));

            bundles.Add(new ScriptBundle(ScriptNames.InventoryOverview).Include(
                    "~/Areas/Inventory/Content/js/inventoryOverview.js"));
            bundles.Add(new ScriptBundle(ScriptNames.InventoryRelatedCases).Include(
                    "~/Areas/Inventory/Content/js/relatedCases.js"));
            bundles.Add(new ScriptBundle(ScriptNames.InventoryFiles).Include(
                "~/Areas/Inventory/Content/js/InventoryFiles.js"));



            RegisterOrdersAreaBundles(bundles);
            RegisterInvoicesAreaBundles(bundles);

        }

    }
}