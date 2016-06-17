"use strict";

function SetValueIfElVisible(el, val, opt) {
    opt = opt || { doOverwrite: false, doNotTriggerEvent: false };
    if (el && $(el).is(':visible')) {
        if (el.val() == "" || opt.doOverwrite) {
            $(el).val(val);
            if (!opt.doNotTriggerEvent) {
                $(el).trigger('change');
            }
            if (el.selector == "#case__WorkingGroup_Id") {
                $("#CaseTemplate_WorkingGroup_Id").val(val);
            }
        }
    }
}

function SetSingleSelectValueIfElVisible(el, val, opt) {
    opt = opt || { doOverwrite: false, doNotTriggerEvent: false };   
        if (el.val() == "" || opt.doOverwrite) {            
            $(el).val(val);
            $(el).trigger("chosen:updated");            
        }    
}

function SetDateValueIfElVisible(el, val, opt, format) {
    opt = opt || { doOverwrite: false, doNotTriggerEvent: false };
    if (el && $(el).is(':visible')) {
        if (el.val() == "" || opt.doOverwrite) {            
            $(el).datepicker({
                format: format.toLowerCase(),
                autoclose: true
            }).datepicker('setDate', val);
           
            if (!opt.doNotTriggerEvent) {
                $(el).trigger('change');                
            }            
        }
    }
}

function SetValueToBtnGroup(domContainer, domText, domValue, value, doOverwrite) {
    var $domValue = $(domValue);
    var oldValue = $domValue.val();
    var el = $(domContainer).find('a[value="' + value + '"]');
    if (el && (doOverwrite || oldValue == '')) {
        $(domText).text(getBreadcrumbs(el));
        $domValue.val(value);
        if (oldValue !== value) {
            $domValue.trigger('change');
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

function IsWillBeOverwrittenByValue(domVisible, domValue, val) {
    return $(domVisible).is(':visible') && $(domValue).val() != '' && $(domValue).val() != val;
}

function IsWillBeOverwritten(fieldId, val) {
    switch (fieldId) {
        case 'CaseType_Id':
            return IsWillBeOverwrittenByValue('#divCaseType', '#case__CaseType_Id', val);
            break;
        case 'Category_Id':
            return IsWillBeOverwrittenByValue('#case__Category_Id', '#case__Category_Id', val);
            break;
        case 'ReportedBy':
            return IsWillBeOverwrittenByValue('#case__ReportedBy', '#case__ReportedBy', val);
            break;
        case 'Department_Id':
            return IsWillBeOverwrittenByValue('#case__Department_Id', '#case__Department_Id', val);
            break;
        case 'PersonsEmail':
            return IsWillBeOverwrittenByValue('#case__PersonsEmail', '#case__PersonsEmail', val);
            break;
        case 'NoMailToNotifier':
            return false;
            break;
        case 'WatchDate':
            return IsWillBeOverwrittenByValue('#case__WatchDate', '#case__WatchDate', val);
            break;
        case 'ProductArea_Id':
            return IsWillBeOverwrittenByValue('#divProductArea', '#case__ProductArea_Id', val);
            break;
        case 'Caption':
            return IsWillBeOverwrittenByValue('#case__Caption', '#case__Caption', val);
            break;
        case 'Description':
            return IsWillBeOverwrittenByValue('#case__Description', '#case__Description', val);
            break;
        case 'Miscellaneous':
            return IsWillBeOverwrittenByValue('#case__Miscellaneous', '#case__Miscellaneous', val);
            break;
        case 'CaseWorkingGroup_Id':
            return IsWillBeOverwrittenByValue('#case__WorkingGroup_Id', '#case__WorkingGroup_Id', val);
            break;
        case 'PerformerUser_Id':
            return IsWillBeOverwrittenByValue('#Performer_Id', '#Performer_Id', val);
            break;
        case 'Priority_Id':
            return IsWillBeOverwrittenByValue('#case__Priority_Id', '#case__Priority_Id', val);
            break;
        case 'Project_Id':
            return IsWillBeOverwrittenByValue('#case__Project_Id', '#case__Project_Id', val);
            break;
        case 'Text_External':
            return IsWillBeOverwrittenByValue("#CaseLog_TextExternal", "#CaseLog_TextExternal", val);
            break;
        case 'Text_Internal':
            return IsWillBeOverwrittenByValue("#CaseLog_TextInternal", "#CaseLog_TextInternal", val);
            break;
        case 'FinishingCause_Id':
            return IsWillBeOverwrittenByValue('#divFinishingType', "#CaseLog_FinishingType", val);
            break;
        case 'RegistrationSource':
            return IsWillBeOverwrittenByValue('#CustomerRegistrationSourceId', "#CustomerRegistrationSourceId", val);
            break;
        case 'Status_Id':
            return IsWillBeOverwrittenByValue('#case__Status_Id', '#case__Status_Id', val);
            break;
        case 'CausingPartId':
            return IsWillBeOverwrittenByValue('#case__CausingPartId', '#case__CausingPartId', val);
            break;
        case 'StateSecondary_Id':
            return IsWillBeOverwrittenByValue('#case__StateSecondary_Id', '#case__StateSecondary_Id', val);
            break;
    }
    return false;
}

var overwriteWarning = {
    dlg: null,
    caseTemplateData: null,
    show: function (data) {
        var me = window.overwriteWarning;
        me.caseTemplateData = data;
        if (me.dlg == null) {
            me.dlg = $('#overwriteDlg')
            $(me.dlg).find('button.btn-ok').on('click', function () {
                me.dlg.modal('hide');
                window.ApplyTemplate(me.caseTemplateData, true);
            });
            $(me.dlg).find('button.btn-cancel').on('click', function () {
                me.dlg.modal('hide');
                window.ApplyTemplate(me.caseTemplateData);
            });
        }

        me.dlg.modal({
            "backdrop": "static",
            "keyboard": true,
            "show": true
        });
    }
};

var ApplyTemplate = function (data, doOverwrite) {
    var cfg = { doOverwrite: doOverwrite };
    var dateFormat = data["dateFormat"];
    for (var fieldId in data) {
        var val = data[fieldId];
        var el;
        if (val != null && val !== '') {
            switch (fieldId) {
                case 'CaseType_Id':
                    SetValueToBtnGroup('#divCaseType', "#divBreadcrumbs_CaseType", "#case__CaseType_Id", val, doOverwrite);
                    break;
                case 'Category_Id':
                    el = $('#case__Category_Id');
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'ReportedBy':
                    el = $('#case__ReportedBy');
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Department_Id':
                    el = $('#case__Department_Id');
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'PersonsEmail':
                    el = $("#case__PersonsEmail");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'NoMailToNotifier':
                    el = $("#CaseMailSetting_DontSendMailToNotifier");
                    SetCheckboxValueIfElVisible(el, val);
                    break;
                case 'WatchDate':
                    el = $("#case__WatchDate");
                    SetDateValueIfElVisible(el, val, cfg, dateFormat);
                    break;
                case 'ProductArea_Id':
                    SetValueToBtnGroup('#divProductArea', "#divBreadcrumbs_ProductArea", "#case__ProductArea_Id", val, doOverwrite);
                    break;
                case 'Caption':
                    el = $("#case__Caption");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Description':
                    el = $("#case__Description");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Miscellaneous':
                    el = $("#case__Miscellaneous");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'CaseWorkingGroup_Id':
                    el = $("#case__WorkingGroup_Id");
                    $("#case__WorkingGroup_Id").val("");
                    //#13311(redmine) Case template_list of administrators doesn´t narrows depending on the choice of working group
                    //cfg['doNotTriggerEvent'] = true;
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'PerformerUser_Id':
                    el = $('#Performer_Id');
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Priority_Id':
                    el = $("#case__Priority_Id");
                    //Diabled to show WatchDate 
                    //cfg['doNotTriggerEvent'] = true;
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Project_Id':
                    el = $("#case__Project_Id");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Text_External':
                    el = $("#CaseLog_TextExternal");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Text_Internal':
                    el = $("#CaseLog_TextInternal");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'FinishingCause_Id':
                    SetValueToBtnGroup('#divFinishingType', "#divBreadcrumbs_FinishingType", "#CaseLog_FinishingType", val, doOverwrite);
                    break;
                case 'RegistrationSource':
                    el = $("#CustomerRegistrationSourceId");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'StateSecondary_Id':
                    el = $("#case__StateSecondary_Id");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'Status_Id':
                    el = $("#case__Status_Id");
                    SetValueIfElVisible(el, val, cfg);
                    break;
                case 'CausingPartId':
                    el = $("#case__CausingPartId").chosen();
                    SetSingleSelectValueIfElVisible(el, val, cfg);
                    break;
            }
        }
    }
}

function IsValueApplicableFor(templateFieldId, val) {
    if (val == null || val === "") {
        return false;
    }

    switch (templateFieldId) {
        case 'CaseType_Id':
            return $('#divCaseType').is(":visible") && $('#divCaseType').find('a[value="' + val + '"]').length != 0;
            break;
        case 'Category_Id':
            return $('#case__Category_Id').is(":visible");
            break;
        case 'ReportedBy':
            return $('#case__ReportedBy').is(":visible");
            break;
        case 'Department_Id':
            return $('#case__Department_Id');
            break;
        case 'PersonsEmail':
            return $("#case__PersonsEmail").is(':visible');
            break;
        case 'NoMailToNotifier':
            return true;
            break;
        case 'WatchDate':
            return $("#case__WatchDate").is(':visible');
            break;          
        case 'ProductArea_Id':
            return $('#divProductArea').is(':visible') && $('#divProductArea').find('a[value="' + val + '"]').length != 0;
            break;
        case 'Caption':
            return $("#case__Caption").is(':visible');
            break;
        case 'Description':
            return $("#case__Description").is(':visible');
            break;
        case 'Miscellaneous':
            return $("#case__Miscellaneous").is(':visible');
            break;
        case 'CaseWorkingGroup_Id':
            return $("#case__WorkingGroup_Id").is(':visible');
            break;
        case 'PerformerUser_Id':
            return $('#Performer_Id').is(':visible');
            break;
        case 'Priority_Id':
            return $("#case__Priority_Id").is(':visible');
            break;
        case 'Project_Id':
            return $("#case__Project_Id").is(':visible');
            break;
        case 'Text_External':
            return $("#CaseLog_TextExternal").is(':visible');
            break;
        case 'Text_Internal':
            return $("#CaseLog_TextInternal").is(':visible');
            break;
        case 'FinishingCause_Id':
            return $("#divFinishingType").is(':visible') && $("#divFinishingType").find('a[value="' + val + '"]');
            break;
        case 'RegistrationSource':
            return $("#CustomerRegistrationSourceId").is(':visible') && $("#CustomerRegistrationSourceId").find('a[value="' + val + '"]');
            break;
        case 'StateSecondary_Id':
            return $("#case__StateSecondary_Id").is(':visible');
            break;
        case 'Status_Id':
            return $("#case__Status_Id").is(':visible');
            break;
        case 'CausingPartId':
            return $("#case__CausingPartId").is(':visible');
            break;
    }
    return false;
}

function LoadTemplate(id) {
    var caseInvoiceIsActive = false;
    var curCaseId = $('#case__Id').val();
    if ($('#CustomerSettings_ModuleCaseInvoice') != undefined)
        caseInvoiceIsActive = $('#CustomerSettings_ModuleCaseInvoice').val().toLowerCase() == 'true';

    if (caseInvoiceIsActive) {
        $.get('/Invoice/IsThereNotSentOrder/', { caseId: curCaseId, myTime: Date.now }, function (res) {
            if (res != null && res) {
                var mes = window.parameters.caseTemplateChangeMessage || '';
                ShowToastMessage(mes, 'warning', false);                
            }
            else {
                GetTemplateData(id)
            }
        });
    }
    else {
        GetTemplateData(id)
    }
}

function GetTemplateData(id) {
    $.get('/CaseSolution/GetTemplate',
        { 'id': id, myTime: Date.now },
        function (caseTemplate) {

            var showOverwriteWarning = false;
            if (!caseTemplate) {
                return;
            }

            for (var field in caseTemplate) {
                if (window.IsValueApplicableFor(field, caseTemplate[field]) && window.IsWillBeOverwritten(field, caseTemplate[field])) {
                    showOverwriteWarning = true;
                    break;
                }
            }

            if (showOverwriteWarning) {
                window.overwriteWarning.show(caseTemplate);
            } else {                
                window.ApplyTemplate(caseTemplate);
            }
        }
    );
}
