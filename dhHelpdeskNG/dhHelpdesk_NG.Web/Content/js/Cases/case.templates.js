"use strict";

function SetValueIfElVisible(el, val, doNotTriggerEvent) {
    if (el && $(el).is(':visible')) {
        $(el).val(val);
        if (!doNotTriggerEvent) {
            $(el).trigger('change');
        }
    }
}

function SetCheckboxValueIfElVisible(el, val, doNotTriggerEvent) {
    if (el && $(el).is(':visible')) {
        $(el).attr('checked', val);
        if (!doNotTriggerEvent) {
            $(el).trigger('change');
        }
    }
}


function LoadTemplate(id) {
    $.post('/CaseSolution/GetTemplate?id=',
        {'id': id},
        function (resp) {
            if (!resp) {
                return;
            }
            for (var fieldId in resp) {
                var val = resp[fieldId];
                var el;
                if (val != null && val !== '') {
                    switch (fieldId) {
                        case 'CaseType_Id':
                            el = $('#divCaseType').find('a[value="' + val + '"]');
                            if (el) {
                                $("#divBreadcrumbs_CaseType").text(getBreadcrumbs(el));
                                $("#case__CaseType_Id").val(val).trigger('change');
                            }
                            break;
                        case 'Category_Id':
                            el = $('#case__Category_Id');
                            SetValueIfElVisible(el, val);
                            break;
                        case 'ReportedBy':
                            el = $('#case__ReportedBy');
                            SetValueIfElVisible(el, val);
                            break;
                        case 'Department_Id':
                            el = $('#case__Department_Id');
                            SetValueIfElVisible(el, val);
                            break;
                        case 'NoMailToNotifier':
                            el = $("#CaseMailSetting_DontSendMailToNotifier");
                            SetCheckboxValueIfElVisible(el, val);
                            break;
                        case 'ProductArea_Id':
                            el = $('#divProductArea').find('a[value="' + val + '"]');
                            if (el) {
                                $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(el));
                                $("#case__ProductArea_Id").val(val).trigger('change');
                            }
                            break;
                        case 'Caption':
                            el = $("#case__Caption");
                            SetValueIfElVisible(el, val);
                            break;
                        case 'Description':
                            el = $("#case__Description");
                            SetValueIfElVisible(el, val);
                            break;
                        case 'Miscellaneous':
                            el = $("#case__Miscellaneous");
                            SetValueIfElVisible(el, val);
                            break;
                        case 'CaseWorkingGroup_Id':
                            el = $("#case__WorkingGroup_Id");
                            SetValueIfElVisible(el, val, true);
                            break;
                        case 'PerformerUser_Id':
                            el = $('#case__Performer_User_Id');
                            SetValueIfElVisible(el, val);
                            break;
                        case 'Priority_Id':
                            el = $("#case__Priority_Id");
                            SetValueIfElVisible(el, val, true);
                            break;
                        case 'Project_Id':
                            el = $("#case__Project_Id");
                            SetValueIfElVisible(el, val);
                            break;
                        case 'Text_External':
                            el = $("#CaseLog_TextExternal");
                            SetValueIfElVisible(el, val);
                            break;
                        case 'Text_Internal':
                            el = $("#CaseLog_TextInternal");
                            SetValueIfElVisible(el, val);
                            break;
                        case 'FinishingCause_Id':
                            el = $("#divFinishingType").find('a[value="' + val + '"]');
                            if (el) {
                                $("#divBreadcrumbs_FinishingType").text(getBreadcrumbs(el));
                                $("#CaseLog_FinishingType").val(val).trigger('change');
                            }
                            break;
                    }
                }
            }
        }
    );
}
