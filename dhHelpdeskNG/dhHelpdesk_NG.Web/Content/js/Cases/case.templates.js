"use strict";

function SetValueIfElVisible($el, val, opt, forceApply) {
    opt = opt || { doOverwrite: false, doNotTriggerEvent: false };
    if ($el.length && (isFieldVisible($el) || (forceApply && val != null && val !== ''))) {
        if ($el.val() === "" || opt.doOverwrite) {
            $el.val(val);
            if (!opt.doNotTriggerEvent) {
               $el.trigger('change');
            }
        }
    }
}

function SetSingleSelectValueIfElVisible($el, val, opt) {
    opt = opt || { doOverwrite: false, doNotTriggerEvent: false };
    if ($el.length && isFieldVisible($el)) {
        if ($el.val() === "" || opt.doOverwrite) {
            $el.val(val);
            $el.trigger("chosen:updated");
        }
    }
}

function SetDateValueIfElVisible($el, val, opt, format) {
    opt = opt || { doOverwrite: false, doNotTriggerEvent: false };
    if ($el.length && isFieldVisible($el)) {
        if ($el.val() === "" || opt.doOverwrite) {
            $el.datepicker({
                format: format.toLowerCase(),
                autoclose: true
            }).datepicker('setDate', val);
           
            const readOnly = $el.attr("readonly");            
            if (readOnly != undefined && readOnly.toLowerCase() === 'readonly') {                
                $el.val(val);
            }

            if (!opt.doNotTriggerEvent) {
                $el.trigger('change');
            }            
        }
    }
}

function SetValueToBtnGroup(domContainer, domText, domValue, value, doOverwrite) {
    var newValue = value || '';
    var $domValue = $(domValue);
    var oldValue = $domValue.val();
    var el = $(domContainer).find('a[value="' + newValue + '"]');
    if (el.length && (doOverwrite || oldValue === '')) {
        $(domText).text(getBreadcrumbs(el));
        $domValue.val(newValue);
        if (oldValue !== newValue) {
            $domValue.trigger('change');
        }
    }
}

function SetCheckboxValueIfElVisible($el, val, doNotTriggerEvent) {
    if ($el.length && isFieldVisible($el)) {
        $el.attr('checked', val);
        if (!doNotTriggerEvent) {
            $el.trigger('change');
        }
    }
}

function SetBootstrapSwitchIfElVisible($el, val, doNotTriggerEvent) {
    if ($el.length && isFieldVisible($el)) {
        $el.bootstrapSwitch('state', val.toLowerCase() === 'true');
        if (!doNotTriggerEvent) {
            $el.trigger('change');
        }
    }
}

function IsWillBeOverwrittenByValue(domEl, domValue, val) {
    const $el = $(domEl);
    return $el.length && isFieldVisible($el) && $(domValue).val() != '' && $(domValue).val() != val;
}

function IsWillBeOverwritten(fieldId, val) {
    
    switch (fieldId) {
        case 'PersonsName':
            return IsWillBeOverwrittenByValue('#case__PersonsName', '#case__PersonsName', val);
            break;
        case 'PersonsPhone':
            return IsWillBeOverwrittenByValue('#case__PersonsPhone', '#case__PersonsPhone', val);
            break;
        case 'PersonsCellPhone':
            return IsWillBeOverwrittenByValue('#case__PersonsCellphone', '#case__PersonsCellphone', val);
            break;
        case 'CaseType_Id':
            return IsWillBeOverwrittenByValue('#divCaseType', '#case__CaseType_Id', val);
            break;
        case 'Category_Id':
            return IsWillBeOverwrittenByValue('#divCategory', '#case__Category_Id', val);
            break;
        case 'ReportedBy':
            return IsWillBeOverwrittenByValue('#case__ReportedBy', '#case__ReportedBy', val);
            break;
        case 'Region_Id':
            return IsWillBeOverwrittenByValue('#case__Region_Id', '#case__Region_Id', val);
            break;
        case 'Department_Id':
            return IsWillBeOverwrittenByValue('#case__Department_Id', '#case__Department_Id', val);
            break;
        case 'OU_Id':
            return IsWillBeOverwrittenByValue('#case__Ou_Id', '#case__Ou_Id', val);
            break;
        case 'CostCentre':
            return IsWillBeOverwrittenByValue('#case__CostCentre', '#case__CostCentre', val);
            break;
        case 'PersonsEmail':
            return IsWillBeOverwrittenByValue('#case__PersonsEmail', '#case__PersonsEmail', val);
            break;
        case 'Place':
            return IsWillBeOverwrittenByValue('#case__Place', '#case__Place', val);
            break;
        case 'UserCode':
            return IsWillBeOverwrittenByValue('#case__UserCode', '#case__UserCode', val);
            break;
        case 'UpdateNotifierInformation':
            return false;
            break;
        case 'NoMailToNotifier':
            return false;
            break;

        case 'IsAbout_ReportedBy':
            return IsWillBeOverwrittenByValue('#case__IsAbout_ReportedBy', '#case__IsAbout_ReportedBy', val);
            break;

        case 'IsAbout_PersonsName':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Person_Name', '#case__IsAbout_Person_Name', val);
            break;

        case 'IsAbout_PersonsEmail':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Person_Email', '#case__IsAbout_Person_Email', val);
            break;

        case 'IsAbout_PersonsPhone':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Person_Phone', '#case__IsAbout_Person_Phone', val);
            break;

        case 'IsAbout_PersonsCellPhone':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Person_Cellphone', '#case__IsAbout_Person_Cellphone', val);
            break;
       
        case 'IsAbout_Region_Id':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Region_Id', '#case__IsAbout_Region_Id', val);
            break;

        case 'IsAbout_Department_Id':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Department_Id', '#case__IsAbout_Department_Id', val);
            break;

        case 'IsAbout_OU_Id':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Ou_Id', '#case__IsAbout_Ou_Id', val);
            break;

        case 'IsAbout_CostCentre':
            return IsWillBeOverwrittenByValue('#case__IsAbout_CostCentre', '#case__IsAbout_CostCentre', val);
            break;
        
        case 'IsAbout_Place':
            return IsWillBeOverwrittenByValue('#case__IsAbout_Place', '#case__IsAbout_Place', val);
            break;

        case 'IsAbout_UserCode':
            return IsWillBeOverwrittenByValue('#case__IsAbout_UserCode', '#case__IsAbout_UserCode', val);
            break;

        case 'InventoryLocation':
            return IsWillBeOverwrittenByValue('#case__InventoryLocation', '#case__InventoryLocation', val);
            break;
        case 'InventoryNumber':
            return IsWillBeOverwrittenByValue('#case__InventoryNumber', '#case__InventoryNumber', val);
            break;
        case 'InventoryType':
            return IsWillBeOverwrittenByValue('#case__InventoryType', '#case__InventoryType', val);
            break;


        case 'InvoiceNumber':
            return IsWillBeOverwrittenByValue('#case__InvoiceNumber', '#case__InvoiceNumber', val);
            break;

        case 'System_Id':
            return IsWillBeOverwrittenByValue('#case__System_Id', '#case__System_Id', val);
            break;
            
        case 'Urgency_Id':
            return IsWillBeOverwrittenByValue('#case__Urgency_Id', '#case__Urgency_Id', val);
            break;

        case 'Impact_Id':
            return IsWillBeOverwrittenByValue('#case__Impact_Id', '#case__Impact_Id', val);
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

        case 'SMS':
            return IsWillBeOverwrittenByValue('#case__SMS', '#case__SMS', val);
            break;

        case 'Available':
            return IsWillBeOverwrittenByValue('#case__Available', '#case__Available', val);
            break;

        case 'Cost':
            return IsWillBeOverwrittenByValue('#case__Cost', '#case__Cost', val);
            break;

        case 'OtherCost':
            return IsWillBeOverwrittenByValue('#case__OtherCost', '#case__OtherCost', val);
            break;

        case 'Currency':
            return IsWillBeOverwrittenByValue('#case__Currency', '#case__Currency', val);
            break;

        case 'Problem_Id':
            return IsWillBeOverwrittenByValue('#case__Problem_Id', '#case__Problem_Id', val);
            break;

        case 'PlanDate':
            return IsWillBeOverwrittenByValue('#case__PlanDate', '#case__PlanDate', val);
            break;

        case 'AgreedDate':
            return IsWillBeOverwrittenByValue('#case__AgreedDate', '#case__AgreedDate', val);
            break;

        case 'VerifiedDescription':
            return IsWillBeOverwrittenByValue('#case__VerifiedDescription', '#case__VerifiedDescription', val);
            break;

        case 'SolutionRate':
            return IsWillBeOverwrittenByValue('#case__SolutionRate', '#case__SolutionRate', val);
            break;

        case 'Verified':
            return IsWillBeOverwrittenByValue('#case__Verified', '#case__Verified', val);
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
            me.dlg = $('#overwriteDlg');
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

var finalActionId;

var caseButtons = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
                    '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                    '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton, #btnGo, #steps');

var templateQuickButtonIndicator = '#TemplateQuickButtonIndicator';

function SaveTemplateValue(ctrlId, data, fieldName) {
   if (data[fieldName] != undefined && data[fieldName] != null) {
       $(ctrlId).val(data[fieldName]);
   } else {
       $(ctrlId).val('');
   }
}

var ApplyTemplate = function (data, doOverwrite) {

    changeCaseButtonsState(false);
    var dateFormat = data['dateFormat'];
    $('#CaseTemplate_ExternalLogNote').val('True');

    // save template values to hidden fields to make them avaialbe for UI change handlers
    SaveTemplateValue('#CaseTemplate_Performer_Id', data, 'PerformerUser_Id');
    SaveTemplateValue('#CaseTemplate_WorkingGroup_Id', data, 'CaseWorkingGroup_Id');
    SaveTemplateValue('#CaseTemplate_Priority_Id', data, 'Priority_Id');
    SaveTemplateValue('#CaseTemplate_StateSecondary_Id', data, 'StateSecondary_Id');

    SaveTemplateValue('#CaseTemplate_Department_Id', data, 'Department_Id');
    SaveTemplateValue('#CaseTemplate_OU_Id', data, 'OU_Id');
    SaveTemplateValue('#CaseTemplate_IsAbout_Department_Id', data, 'IsAbout_Department_Id');
    SaveTemplateValue('#CaseTemplate_IsAbout_OU_Id', data, 'IsAbout_OU_Id');

    var el = null;
    var val = null;
    var cfg = { doOverwrite: doOverwrite, doNotTriggerEvent: false };

    // NOTE: using 'if' instead of 'switch/case' guarantees required processing order.

    //region
    if (!isNullOrEmpty(data.Region_Id)) {
        val = data.Region_Id || '';
        el = $('#case__Region_Id');
        
        if (!isNullOrEmpty(data.Department_Id)) {
            cfg.doNotTriggerEvent = false;
        }

        SetValueIfElVisible(el, val, cfg);
        cfg.doNotTriggerEvent = false;
    }

    //department
    if (!isNullOrEmpty(data.Department_Id)) {
        val = data.Department_Id || '';
        el = $('#case__Department_Id');
        
        if (!isNullOrEmpty(data.Region_Id)) {
            cfg.doNotTriggerEvent = true;
        }

        SetValueIfElVisible(el, val, cfg);
        cfg.doNotTriggerEvent = false;
    }

    // OU
    if (!isNullOrEmpty(data.OU_Id)) {
        val = data.OU_Id || '';
        el = $('#case__Ou_Id');
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.CostCentre)) {
        val = data.CostCentre || '';
        el = $("#case__CostCentre");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.PersonsName)) {
        val = data.PersonsName || '';
        el = $('#case__PersonsName');
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.PersonsPhone)) {
        val = data.PersonsPhone || '';

        el = $('#case__PersonsPhone');
        SetValueIfElVisible(el, val, cfg);
    }
    if (!isNullOrEmpty(data.PersonsCellPhone)) {
        val = data.PersonsCellPhone || '';

        el = $('#case__PersonsCellphone');
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Category_Id)) {
        val = data.Category_Id || '';
        SetValueToBtnGroup('#divCategory', "#divBreadcrumbs_Category", "#case__Category_Id", val, doOverwrite);
    }
    if (!isNullOrEmpty(data.ReportedBy)) {
        val = data.ReportedBy || '';
        el = $('#case__ReportedBy');
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.PersonsEmail)) {
        val = data.PersonsEmail || '';
        el = $("#case__PersonsEmail");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Place)) {
        val = data.Place || '';
        el = $("#case__Place");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.UserCode)) {
        val = data.UserCode || '';
        el = $("#case__UserCode");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.UpdateNotifierInformation)) {
        val = data.UpdateNotifierInformation || '';
        el = $("#UpdateNotifierInformation");
        SetBootstrapSwitchIfElVisible(el, val);
    }
    
    if (!isNullOrEmpty(data.NoMailToNotifier)) {
        val = data.NoMailToNotifier || false;
        var $el = $("#sendMailToNotifier");
        
        $("#dontSendMailToNotifier").val(val);
        if (val) {
            $("#autoCheckNotifyerCheckbox").val('false');
            $("#sendMailToNotifier").prop('checked', false);
            $el.trigger('change');
        }
        else {
            $("#autoCheckNotifyerCheckbox").val('true');
            $("#sendMailToNotifier").prop('checked', true);
            $el.trigger('change');
        }
    }

    if (!isNullOrEmpty(data.IsAbout_PersonsName)) {
        val = data.IsAbout_PersonsName || '';
        el = $('#case__IsAbout_Person_Name');
        SetValueIfElVisible(el, val, cfg, true);
    }
    if (!isNullOrEmpty(data.IsAbout_PersonsPhone)) {
        val = data.IsAbout_PersonsPhone || '';
        el = $('#case__IsAbout_Person_Phone');
        SetValueIfElVisible(el, val, cfg, true);
    }
    if (!isNullOrEmpty(data.IsAbout_PersonsCellPhone)) {
        val = data.IsAbout_PersonsCellPhone || '';
        el = $('#case__IsAbout_Person_Cellphone');
        SetValueIfElVisible(el, val, cfg, true);
    }

    if (!isNullOrEmpty(data.IsAbout_ReportedBy)) {
        val = data.IsAbout_ReportedBy || '';
        el = $('#case__IsAbout_ReportedBy');
        SetValueIfElVisible(el, val, cfg, true);
    }

    if (!isNullOrEmpty(data.IsAbout_Region_Id)) {
        val = data.IsAbout_Region_Id || '';
        el = $('#case__IsAbout_Region_Id');
        
        if (!isNullOrEmpty(data.IsAbout_Department_Id)) {
            cfg.doNotTriggerEvent = false;
        }
        
        SetValueIfElVisible(el, val, cfg, true);
        cfg.doNotTriggerEvent = false;
    }

    if (!isNullOrEmpty(data.IsAbout_Department_Id)) {
        val = data.IsAbout_Department_Id || '';
        el = $('#case__IsAbout_Department_Id');
        
        if (!isNullOrEmpty(data.IsAbout_Region_Id)) {
            cfg.doNotTriggerEvent = true;
        } 

        SetValueIfElVisible(el, val, cfg, true);
        cfg.doNotTriggerEvent = false;
    }

    if (!isNullOrEmpty(data.IsAbout_OU_Id)) {
        val = data.IsAbout_OU_Id || '';
        el = $('#case__IsAbout_Ou_Id');
        SetValueIfElVisible(el, val, cfg, true);
    }

    if (!isNullOrEmpty(data.IsAbout_CostCentre)) {
        val = data.IsAbout_CostCentre || '';
        el = $("#case__IsAbout_CostCentre");
        SetValueIfElVisible(el, val, cfg, true);
    }

    if (!isNullOrEmpty(data.IsAbout_PersonsEmail)) {
        val = data.IsAbout_PersonsEmail || '';
        el = $("#case__IsAbout_Person_Email");
        SetValueIfElVisible(el, val, cfg, true);
    }

    if (!isNullOrEmpty(data.IsAbout_Place)) {
        val = data.IsAbout_Place || '';
        el = $("#case__IsAbout_Place");
        SetValueIfElVisible(el, val, cfg, true);
    }

    if (!isNullOrEmpty(data.IsAbout_UserCode)) {
        val = data.IsAbout_UserCode || '';
        el = $("#case__IsAbout_UserCode");
        SetValueIfElVisible(el, val, cfg, true);
    }

    if (!isNullOrEmpty(data.Priority_Id)) {
        val = data.Priority_Id || '';
        el = $("#case__Priority_Id");
        SetValueIfElVisible(el, val, cfg);
        if (el && (el.val() == "" || cfg.doOverwrite)) {
            //Todo: refactor
            //if connected to workflow we need to set the value
            if ($('#steps').length) {
                el.val(val);
            }
        }
    }

    if (!isNullOrEmpty(data.PerformerUser_Id)) {
        val = data.PerformerUser_Id || '';
        el = $('#Performer_Id');
        SetValueIfElVisible(el, val, cfg);
    }
    
    if (!isNullOrEmpty(data.CaseWorkingGroup_Id)) {
        val = data.CaseWorkingGroup_Id || '';
        el = $("#case__WorkingGroup_Id");
        $("#case__WorkingGroup_Id").val("");
        //#13311(redmine) Case template_list of administrators doesn't narrows depending on the choice of working group
        //cfg['doNotTriggerEvent'] = true;
        $('#Performer_Id').one('applyValue', function () {
            $(this).val(data.PerformerUser_Id);
        });
        SetValueIfElVisible(el, val, cfg);
        if (el && (el.val() == "" || cfg.doOverwrite)) {

            //Todo: refactor
            //if connected to workflow we need to set the value
            if ($('#steps').length) {
                el.val(val);
            }
        }
    }

    if (!isNullOrEmpty(data.CaseType_Id)) {
        val = data.CaseType_Id || '';
        SetValueToBtnGroup('#divCaseType', "#divBreadcrumbs_CaseType", "#case__CaseType_Id", val, doOverwrite);
    }

    if (!isNullOrEmpty(data.ProductArea_Id)) {
        val = data.ProductArea_Id || '';
        SetValueToBtnGroup('#divProductArea', "#divBreadcrumbs_ProductArea", "#case__ProductArea_Id", val, doOverwrite);
    }
    
    if (!isNullOrEmpty(data.Status_Id)) {
        val = data.Status_Id || '';
        el = $("#case__Status_Id");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.StateSecondary_Id)) {
        val = data.StateSecondary_Id || '';
        el = $("#case__StateSecondary_Id");

        SetValueIfElVisible(el, val, cfg);

        if (el && (el.val() == "" || cfg.doOverwrite)) {
            $(".readonlySubstate").val(val);
            //Todo: refactor
            //if connected to workflow we need to set the value
            if ($('#steps').length) {
                el.val(val);
            }
        }
    }

    if (!isNullOrEmpty(data.InventoryLocation)) {
        val = data.InventoryLocation || '';
        el = $("#case__InventoryLocation");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.InventoryNumber)) {
        val = data.InventoryNumber || '';
        el = $("#case__InventoryNumber");
        SetValueIfElVisible(el, val, cfg);
    }
    if (!isNullOrEmpty(data.InventoryType)) {
        val = data.InventoryType || '';
        el = $("#case__InventoryType");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.InvoiceNumber)) {
        val = data.InvoiceNumber || '';
        el = $("#case__InvoiceNumber");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.ReferenceNumber)) {
        val = data.ReferenceNumber || '';
        el = $("#case__ReferenceNumber");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.System_Id)) {
        val = data.System_Id || '';
        el = $("#case__System_Id");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Urgency_Id)) {
        val = data.Urgency_Id || '';
        el = $("#case__Urgency_Id");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Impact_Id)) {
        val = data.Impact_Id || '';
        el = $("#case__Impact_Id");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.WatchDate)) {
        val = data.WatchDate || '';
        el = $("#case__WatchDate");
        SetDateValueIfElVisible(el, val, cfg, dateFormat);
    }

    if (!isNullOrEmpty(data.Caption)) {
        val = data.Caption || '';
        el = $("#case__Caption");
        SetValueIfElVisible(el, val, cfg);
    }
    if (!isNullOrEmpty(data.Description)) {
        val = data.Description || '';
        if (val !== '') {
            $(".summernotedesc").summernote("code", val.replace(/\r\n|\r|\n/g, "<br />"));
            $("#spanDesc").text(val);
        }
    }
    if (!isNullOrEmpty(data.Miscellaneous)) {
        val = data.Miscellaneous || '';
        el = $("#case__Miscellaneous");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Project_Id)) {
        val = data.Project_Id || '';
        el = $("#case__Project_Id");
        SetValueIfElVisible(el, val, cfg);
    }
    if (!isNullOrEmpty(data.Text_External)) {
        val = data.Text_External || '';
        if (val !== '') {
            $(".summernoteexternal").summernote("code", val.replace(/\r\n|\r|\n/g, "<br />"));
        }
    }
    if (!isNullOrEmpty(data.Text_Internal)) {
        val = data.Text_Internal || '';
        if (val !== '') {
            $(".summernoteinternal").summernote("code", val.replace(/\r\n|\r|\n/g, "<br />"));
        }
    }
    if (!isNullOrEmpty(data.FinishingCause_Id)) {
        val = data.FinishingCause_Id || '';
        SetValueToBtnGroup('#divFinishingType', "#divBreadcrumbs_FinishingType", "#CaseLog_FinishingType", val, doOverwrite);
    }
    if (!isNullOrEmpty(data.RegistrationSource)) {
        val = data.RegistrationSource || '';
        el = $("#CustomerRegistrationSourceId");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.CausingPartId)) {
        val = data.CausingPartId || '';
        el = $("#case__CausingPartId");
        SetSingleSelectValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.SMS)) {
        val = data.SMS || false;
        el = $("#case__SMS");
        SetCheckboxValueIfElVisible(el, val);
    }

    if (!isNullOrEmpty(data.Available)) {
        val = data.Available || '';
        el = $("#case__Available");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Cost)) {
        val = data.Cost || '';
        el = $("#case__Cost");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.OtherCost)) {
        val = data.OtherCost || '';
        el = $("#case__OtherCost");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Currency)) {
        val = data.Currency || '';
        el = $("#case__Currency");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Problem_Id)) {
        val = data.Problem_Id || '';
        el = $("#case__Problem_Id");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.PlanDate)) {
        val = data.PlanDate || '';
        el = $("#case__PlanDate");
        SetDateValueIfElVisible(el, val, cfg, dateFormat);
    }

    if (!isNullOrEmpty(data.AgreedDate)) {
        val = data.AgreedDate || '';
        el = $("#case__AgreedDate");
        SetDateValueIfElVisible(el, val, cfg, dateFormat);
    }

    if (!isNullOrEmpty(data.VerifiedDescription)) {
        val = data.VerifiedDescription || '';
        el = $("#case__VerifiedDescription");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.SolutionRate)) {
        val = data.SolutionRate || '';
        el = $("#case__SolutionRate");
        SetValueIfElVisible(el, val, cfg);
    }

    if (!isNullOrEmpty(data.Verified)) {
        val = data.Verified || false;
        el = $("#case__Verified");
        SetCheckboxValueIfElVisible(el, val);
    }
    

    //Extended Case
    if (data.extendedCases) {
        el = $("#CaseSolution_Id");
        el.val(data.Id);

        var lastECId = '';
        var lastECPath = '';
        var lastECGuid = '';
        var lastECLanguage = 1;
        for (var i = 0; i < data.extendedCases.length ; i++) {
            var ex = data.extendedCases[i];
            var newTab =
            "<li data-active-tab='extendedcase-tab' >" +
                " <a href='#container_" + ex.Id + "' id='extendedcase-tab' class='extendedcase' " +
                  "data-id='" + ex.Id + "' data-state=''>" +
                   ex.Name +
                   "<i id='exTabIndicator_" + ex.Id + "' class='tab-indicator' style='display:none'> &nbsp;</i>" +
                "</a>" +
            "</li>";

            if ($("#dynamicCaseTabContainer").length > 0)
            { $(newTab).insertAfter($("#dynamicCaseTabContainer")); }
            else if ($("#mergedCaseTabContainer").length > 0) { $(newTab).insertAfter($("#mergedCaseTabContainer")); }
            else if ($("#childCaseTabContainer").length > 0)
            { $(newTab).insertAfter($("#childCaseTabContainer")); }
            else if ($("#caseTabContainer").length > 0)
            { $(newTab).insertAfter($("#caseTabContainer")); }

            $('#tabsArea a[href="#container_' + ex.Id + '"]').click(function (e) {
                e.preventDefault();
                $(this).tab('show');
            })

            var headLine = ex.Name;
            var statusBar = [];
            var inputs = document.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].name.indexOf('StatusBar') == 0) {
                    statusBar.push(inputs[i]);
                }
            }
            var items = "";
            if (statusBar.length > 0) {
                for (var i = 0; i < statusBar.length; i++) {
                    items += statusBar[i].value;
                }
            }
            var newContent =
                "<div id='container_" + ex.Id + "' class='extended-case-tab tab-pane tab-pane-border'>" +
                    "<div class='container' id='exCaseContent'>" +
                        "<h4 style='font-weight:normal !important; font-size:11px !important;'>" +
                         "<strong>" + headLine + "</strong>" +
                         (statusBar.length > 0 ? " <text> | </text>" : " <text>&nbsp;</text>") +
                         items +
                        "</h4>"
            "</div>" +
        "</div>";

            $("#mainTabContainer").append(newContent);

            lastECId = ex.Id;
            lastECPath = ex.Path;
            lastECGuid = ex.ExtendedCaseGuid;
            lastECLanguage = ex.LanguageId;
        }

        setTimeout(function () {
            EditPage.prototype.Current_EC_FormId = lastECId;
            //debug Mode
            EditPage.prototype.Current_EC_Path = lastECPath;
            //EditPage.prototype.Current_EC_Path = "http://localhost:8099";
            EditPage.prototype.Current_EC_Guid = lastECGuid;
            EditPage.prototype.Current_EC_LanguageId = lastECLanguage;
            EditPage.prototype.loadExtendedCaseIfNeeded();
        }, 1000);
    }// End if extendedcase   
    //else {
    //    if ($("#extendedcase-tab").length > 0)
    //        $("#extendedcase-tab").remove();
    //}
  
    //reset case template values after loading template data. Added 3sec delay in case some UI events are not complete...
    setTimeout(function() {
            $("#CaseTemplate_Performer_Id").val("");
            $('#CaseTemplate_WorkingGroup_Id').val("");
            $('#CaseTemplate_Priority_Id').val("");
            $('#CaseTemplate_StateSecondary_Id').val("");
            $('#CaseTemplate_Department_Id').val("");
            $('#CaseTemplate_OU_Id').val("");
            $('#CaseTemplate_IsAbout_Department_Id').val("");
            $('#CaseTemplate_IsAbout_OU_Id').val("");
        },
        3000);

    //TODO: As there are fragmented functiond to execute the Case rules (some here and some are in others .js file) 
    //      we call the FinalAction function after 3 seconds. 
    //      After refactoring CaseTemplate this fuction should be call Sync.

    if (finalActionId != null) {
        changeCaseButtonsState(false);
        setTimeout(runFinalAction, 3000, false);
    } else {
        setTimeout(runFinalAction, 500, false);
    }    
    
}

function isNullOrEmpty(val) {
    if (val === null || val === '')
        return true;
    else
        return false;
}

function IsValueApplicableFor(templateFieldId, val) {
    if (val == null || val === "") {
        return false;
    }
    
    switch (templateFieldId) {
        case 'PersonsName':
            return $('#case__PersonsName').is(":visible") && $('#case__PersonsName').find('a[value="' + val + '"]').length != 0;
            break;
        case 'PersonsPhone':
            return $('#case__PersonsPhone').is(":visible") && $('#case__PersonsPhone').find('a[value="' + val + '"]').length != 0;
            break;
        case 'case__PersonsCellphone':
            return $('#case__PersonsCellphone').is(":visible") && $('#case__PersonsCellphone').find('a[value="' + val + '"]').length != 0;
            break;            
        case 'CaseType_Id':
            return $('#divCaseType').is(":visible") && $('#divCaseType').find('a[value="' + val + '"]').length != 0;
            break;
        case 'Category_Id':
            return $('#case__Category_Id').is(":visible");
            break;
        case 'ReportedBy':
            return $('#case__ReportedBy').is(":visible");
            break;

        case 'Region_Id':
            return $('#case__Region_Id').is(':visible');
            break;

        case 'Department_Id':
            return $('#case__Department_Id').is(':visible');
            break;

        case 'OU_Id':
            return $('#case__Ou_Id').is(':visible');
            break;

        case 'CostCentre':
            return $("#case__CostCentre").is(':visible');
            break;

        case 'PersonsEmail':
            return $("#case__PersonsEmail").is(':visible');
            break;

        case 'Place':
            return $("#case__Place").is(':visible');
            break;

        case 'UserCode':
            return $("#case__UserCode").is(':visible');
            break;

        case 'UpdateNotifierInformation':
            return true;
            break;

        case 'NoMailToNotifier':
            return true;
            break;

        case 'IsAbout_PersonsName':
            return $('#case__IsAbout_Person_Name').is(":visible") && $('#case__IsAbout_Person_Name').find('a[value="' + val + '"]').length != 0;
            break;
        case 'IsAbout_PersonsPhone':
            return $('#case__IsAbout_Person_Phone').is(":visible") && $('#case__IsAbout_Person_Phone').find('a[value="' + val + '"]').length != 0;
            break;
        case 'IsAbout_PersonsCellPhone':
            return $('#case__IsAbout_Person_Cellphone').is(":visible") && $('#case__IsAbout_Person_Cellphone').find('a[value="' + val + '"]').length != 0;
            break;               
        case 'IsAbout_ReportedBy':
            return $('#case__IsAbout_ReportedBy').is(":visible");
            break;

        case 'IsAbout_Region_Id':
            return $('#case__IsAbout_Region_Id').is(':visible');
            break;

        case 'IsAbout_Department_Id':
            return $('#case__IsAbout_Department_Id').is(':visible');
            break;

        case 'IsAbout_OU_Id':
            return $('#case__IsAbout_Ou_Id').is(':visible');
            break;

        case 'IsAbout_CostCentre':
            return $("#case__IsAbout_CostCentre").is(':visible');
            break;

        case 'IsAbout_PersonsEmail':
            return $("#case__IsAbout_Person_Email").is(':visible');
            break;

        case 'PlaceIsAbout_Place':
            return $("#case__IsAbout_Place").is(':visible');
            break;

        case 'IsAbout_UserCode':
            return $("#case__IsAbout_UserCode").is(':visible');
            break;

        case 'InventoryLocation':
            return $("#case__InventoryLocation").is(':visible');
            break;
        case 'InventoryNumber':
            return $("#case__InventoryNumber").is(':visible');
            break;
        case 'InventoryType':
            return $("#case__InventoryType").is(':visible');
            break;

        case 'InvoiceNumber':
            return $("#case__InvoiceNumber").is(':visible');
            break;
            
        case 'ReferenceNumber':
            return $("#case__ReferenceNumber").is(':visible');
            break;

        case 'System_Id':
            return $("#case__System_Id").is(':visible');
            break;

        case 'Urgency_Id':
            return $("#case__Urgency_Id").is(':visible');
            break;
            
        case 'Impact_Id':
            return $("#case__Impact_Id").is(':visible');
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

        case 'SMS':
            return $("#case__SMS").is(':visible');
            break;

        case 'Available':
            return $("#case__Available").is(':visible');
            break;
         
        case 'Cost':
            return $("#case__Cost").is(':visible');
            break;

        case 'OtherCost':
            return $("#case__OtherCost").is(':visible');
            break;

        case 'Currency':
            return $("#case__Currency").is(':visible');
            break;

        case 'Problem_Id':
            return $("#case__Problem_Id").is(':visible');
            break;

        case 'PlanDate':
            return $("#case__PlanDate").is(':visible');
            break;
        
        case 'AgreedDate':
            return $("#case__AgreedDate").is(':visible');
            break;

        case 'VerifiedDescription':
            return $("#case__VerifiedDescription").is(':visible');
            break;

        case 'SolutionRate':
            return $("#case__SolutionRate").is(':visible');
            break;

        case 'Verified':
            return $("#case__Verified").is(':visible');
            break;
    }
    return false;
}

function LoadTemplate(id) {
    debugger
    var caseInvoiceIsActive = false;
    var curCaseId = $('#case__Id').val();
    if ($('#CustomerSettings_ModuleCaseInvoice') != undefined)
        caseInvoiceIsActive = $('#CustomerSettings_ModuleCaseInvoice').val().toLowerCase() == 'true';
    
    if (caseInvoiceIsActive) {
        $.get('/CaseInvoice/IsThereNotSentOrder/', { caseId: curCaseId, myTime: Date.now }, function (res) {
            if (res != null && res) {
                var mes = window.parameters.caseTemplateChangeMessage || '';
                ShowToastMessage(mes, 'warning', false);                
            }
            else {
                GetTemplateData(id).done(function (template) {
                    if (template.SplitToCaseSolution_Id != null) {
                        $("#SplitToCaseSolution_Id").val(template.SplitToCaseSolution_Id);
                    }

                    if (template.Text_External != "") {
                        $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', true);
                    }
                });
            }
        });
    }
    else {
        GetTemplateData(id).done(function (template) {
            if (template.SplitToCaseSolution_Id != null) {
                $("#SplitToCaseSolution_Id").val(template.SplitToCaseSolution_Id);
            }

            if (template.Text_External != "") {
                $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', true);
            }
        });
    }
    
}

function GetTemplateData(id) {

    var data = { 'id': id, myTime: Date.now };
    return $.get('/CaseSolution/GetTemplate', data, function (caseTemplate) {

            finalActionId = caseTemplate["SaveAndClose"];
            var showOverwriteWarning = false;
            if (!caseTemplate || (caseTemplate.extendedCases && $("#extendedcase-tab").length > 0)) {
                //alert("Already exists")
                return;
            }

            for (var field in caseTemplate) {
                if (caseTemplate.hasOwnProperty(field)) {
                    if (window.IsValueApplicableFor(field, caseTemplate[field]) &&
                        window.IsWillBeOverwritten(field, caseTemplate[field])) {
                        showOverwriteWarning = true;
                        break;
                    }
                }
            }

            var overwriteDirectly = caseTemplate["OverWritePopUp"];            
            if (overwriteDirectly != undefined && overwriteDirectly !== 0)
                window.ApplyTemplate(caseTemplate, true);
            else {
                if (showOverwriteWarning) {
                    window.overwriteWarning.show(caseTemplate);
                } else {
                    window.ApplyTemplate(caseTemplate);
                }
            }
 
            return caseTemplate;
        }
    );
}

function runFinalAction(forceRun) {    
    /*TODO: Start to run just after Invoices is loaded to Case. After refactoring to run Sync, 
            we should just run the fuction without any timer */

    var indicator = $("#invoiceButtonIndicator");   

    if (!forceRun && indicator != null && indicator != undefined && indicator.is(':visible')) {        
        setTimeout(runFinalAction, 1500, false);        
        return;
    } else {
        if (!forceRun) {
            setTimeout(runFinalAction, 500, true);
            return;
        }

        changeCaseButtonsState(true);
        if (finalActionId == null) {
            // do nothing
        }        
        else if (finalActionId == 0)
            $("#case-action-save").trigger('click');
        else if (finalActionId == 1)
            $("#case-action-save-and-close").trigger('click');
    }    
}

function changeCaseButtonsState(state) {
    if (state) {
        caseButtons.removeClass('disabled');
        caseButtons.css("pointer-events", "");
        $(templateQuickButtonIndicator).css("display", "none");
    }
    else {
        caseButtons.addClass("disabled");
        caseButtons.css("pointer-events", "none");
        $(templateQuickButtonIndicator).css("display", "block");
    }
}


function isFieldVisible($el) {
    
    if ($el.length === 0) return false;

    const $row = $el.closest('tr');
    if ($row.length && $row[0].style.display === 'none') return false;

    const style = getComputedStyle($el.data('chosen') ? $el.data('chosen').container[0] : $el[0]);
    if (style.display === 'none') return false;
    if (style.visibility !== 'visible') return false;
    
    return true;
}
