﻿"use strict";

function EditPage() {};

var caseButtonsToLock = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
                          '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                          '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton');


/*** CONST BEGIN ***/
EditPage.prototype.DELETE_CASE_URL = '/Cases/DeleteCase';
EditPage.prototype.NEW_CLOSE_CASE_URL = '/Cases/NewAndClose';
EditPage.prototype.EDIT_CASE_URL = '/Cases/Edit';
EditPage.prototype.SAVE_GOTO_PARENT_CASE_URL = '/Cases/NewAndGotoParentCase';
EditPage.prototype.SAVE_ADD_CASE_URL = '/Cases/NewAndAddCase';
EditPage.prototype.CASE_OVERVIEW_URL = '/Cases';

EditPage.prototype.CHLID_CASES_TAB = 'childcases-tab';

EditPage.prototype.CASE_IN_IDLE = 'case_in_idle';
EditPage.prototype.CASE_IN_SAVING = 'case_in_saving';

/*** CONST END ***/

/*** Variables ***/
EditPage.prototype.Contains_ExtendedCase = false;
EditPage.prototype.Contains_Eform = false;
EditPage.prototype.Case_Field_Ids = null;

/**
* @private
* @param { Number } deptId
*/
EditPage.prototype.fetchWatchDateByDept = function (deptId) {
    var me = this;
    if (deptId == null) {
        return;
    }

    $.getJSON(
        '/cases/GetWatchDateByDepartment',
        { 'departmentId': deptId, 'myTime' : Date.now },
        function (response) {
            if (response.result === 'success') {
                if (response.data != null) {
                    var dt = new Date(parseInt(response.data.replace("/Date(", "").replace(")/", ""), 10));                                        
                    me.$watchDate.datepicker('update', dt);

                    var readOnly = $(me.$watchDateEdit).attr("readonly");
                    if (readOnly != undefined && readOnly.toLowerCase() == 'readonly') {
                        var dateText = dt.format('yyyy-MM-dd');
                        me.$watchDateEdit.val(dateText);
                    }
                }               
            }
        });
};

EditPage.prototype.ReExtendCaseLock = function () {
    var me = this;
    var _parameters = me.p;
    $.post(_parameters.caseLockExtender, { lockGuid: _parameters.caseLockGuid, extendValue: _parameters.extendValue },
        function (data) {
            if (data == false) {
                clearInterval(me.timerId);
            }
        });
};

EditPage.prototype.resetSaving = function () {
    var me = this;
    me.setCaseStatus(me.CASE_IN_IDLE);
};

EditPage.prototype.isProductAreaValid = function () {
    var me = this;
    var res = me.$productAreaChildObj.val();
    if (res == '1')
        return false;
    else
        return true;
};


EditPage.prototype.getValidationErrorMessage = function (extraMessage) {
    var me = this;
    var validationMessages = me.p.casesScopeInitParameters.validationMessages || '';
    var requiredFieldsMessage = me.p.casesScopeInitParameters.requiredFieldsMessage || '';
    var mandatoryFieldsText = me.p.casesScopeInitParameters.mandatoryFieldsText || '';
    var messages = [requiredFieldsMessage, '<br />', mandatoryFieldsText, ':'];
    $('label.error').each(function (key, el) {
        var errorText;
        if ($(el).css('display') === 'none') {
            return true;
        }
        errorText = $(el).text();
        $.each(validationMessages, function (index, validationMessage) {
            errorText = '<br />' + '[' + dhHelpdesk.cases.utils.replaceAll(errorText, validationMessage, '').trim() + ']';
        });
        messages.push(errorText);
    });    

    messages.push(extraMessage);

    return messages.join('');
};

EditPage.prototype.getDate = function (val) {
    if (val == undefined || val == null || val == "")
        return null;
    else {
        var dateStr = val.split(' ');
        if (dateStr.length > 0)
            return new Date(dateStr[0]);
        else
            return null;
    }
};

EditPage.prototype.isFormValid = function() {
    var me = this;
    $('#btnAddCaseFile').removeClass('error');

    if (!me.isProductAreaValid()) {
        me.$productAreaObj.addClass("error");
        dhHelpdesk.cases.utils.showError(me.productAreaErrorMessage);
        return false;
    }            
            
    var curFinishDate = $('#' + me.p.caseFieldIds.FinishingDate).val();
    if (curFinishDate != undefined && curFinishDate != '') {
        var regDate = me.getDate(me.p.caseRegDate);            
        var finishDate = me.getDate(curFinishDate);
        if (regDate > finishDate) {
            dhHelpdesk.cases.utils.showError(me.p.finishingDateMessage);
            $('#' + me.p.caseFieldIds.FinishingDate).addClass("error");
            return false;
        };        
    }
    
    var isCaseFileValid = true;
    var err = '';
    if (me.isCaseFileMandatory != 0) {        
        var caseFiles = $('#case_files_table tbody tr');
        if (caseFiles != null && caseFiles != undefined) {
            if (caseFiles.length < 1) {
                $('#btnAddCaseFile').addClass('error');
                isCaseFileValid = false;
                err = '<br />' + '[' + me.p.caseFileCaption + ']';                
            }
        }
    }

    if (!me.$form.valid()) {
        if (isCaseFileValid)
            dhHelpdesk.cases.utils.showError(me.getValidationErrorMessage());
        else
            dhHelpdesk.cases.utils.showError(me.getValidationErrorMessage(err));
        return false;
    } else {
        if (!isCaseFileValid) {
            dhHelpdesk.cases.utils.showError(me.getValidationErrorMessage(err));
            return false;
        }
    }
    
    return true;
};

EditPage.prototype.primaryValidation = function (submitUrl) {
    var me = this;

    var finishDate = $('#CaseLog_FinishingDate').val();

    /* Check FinishigTime */
    if (me.CaseWillFinish() && finishDate != null && finishDate != undefined) {        
        $.get('/Cases/IsFinishingDateValid/', { changedTime: me.p.caseChangedTime, finishingTime: finishDate, myTime: Date.now }, function (res) {
            if (res != null && res) {
                me.startSaveProcess(me, submitUrl);
            }
            else {
                dhHelpdesk.cases.utils.showError(me.p.finishingDateMessage2);
            }
        });
    } else {
        me.startSaveProcess(me, submitUrl);
    }
    
}

EditPage.prototype.startSaveProcess = function (sender, submitUrl) {
    //Check if there is Order which is not invoiced yet
    var me = sender;

    if (me.invoiceIsActive && me.CaseWillFinish()) {
        $.get('/CaseInvoice/IsThereNotSentOrder/', { caseId: me.p.currentCaseId, myTime: Date.now }, function (res) {
            if (res != null && res) {
                dhHelpdesk.cases.utils.showError(me.p.invoicePreventsToCloseCaseMessage);
            }
            else {
                me.checkAndSave(submitUrl);
            }
        });
    }
    else {
        me.checkAndSave(submitUrl);
    }
}

EditPage.prototype.checkAndSave = function (submitUrl) {
    var me = this;
    var action = submitUrl || me.EDIT_CASE_URL;
    if (me._inSaving) {
        return false;
    }

    me.setCaseStatus(me.CASE_IN_SAVING);    
    var params = me.p;
    if (params.preventToSaveCaseWithInactiveValue != null && params.preventToSaveCaseWithInactiveValue == 1) {
        if (me.$form == undefined || me.$form.index(0) == -1) {
            ShowToastMessage("Case form is not valid!", "error", true);
            return;
        }

        try 
        {
            var form = me.$form[0];
            var fieldIds = params.caseFieldIds;
            var fieldsToCheck = {
                CustomerId: (form.elements[fieldIds.CustomerId] == undefined) ? null : form.elements[fieldIds.CustomerId].value,
                RegionId: (form.elements[fieldIds.RegionId] == undefined) ? null : form.elements[fieldIds.RegionId].value,
                DepartmentId: (form.elements[fieldIds.DepartmentId] == undefined) ? null : form.elements[fieldIds.DepartmentId].value,
                OUId: (form.elements[fieldIds.OUId] == undefined) ? null : form.elements[fieldIds.OUId].value,
                SourceId: (form.elements[fieldIds.SourceId] == undefined) ? null : form.elements[fieldIds.SourceId].value,
                CaseTypeId: (form.elements[fieldIds.CaseTypeId] == undefined) ? null : form.elements[fieldIds.CaseTypeId].value,
                ProductAreaId: (form.elements[fieldIds.ProductAreaId] == undefined) ? null : form.elements[fieldIds.ProductAreaId].value,
                CategoryId: (form.elements[fieldIds.CategoryId] == undefined) ? null : form.elements[fieldIds.CategoryId].value,
                SupplierId: (form.elements[fieldIds.SupplierId] == undefined) ? null : form.elements[fieldIds.SupplierId].value,
                WorkingGroupId: (form.elements[fieldIds.WorkingGroupId] == undefined) ? null : form.elements[fieldIds.WorkingGroupId].value,
                ResponsibleId: (form.elements[fieldIds.ResponsibleId] == undefined) ? null : form.elements[fieldIds.ResponsibleId].value,
                AdministratorId: (form.elements[fieldIds.AdministratorId] == undefined) ? null : form.elements[fieldIds.AdministratorId].value,
                PriorityId: (form.elements[fieldIds.PriorityId] == undefined) ? null : form.elements[fieldIds.PriorityId].value,
                StatusId: (form.elements[fieldIds.StatusId] == undefined) ? null : form.elements[fieldIds.StatusId].value,
                SubStatusId: (form.elements[fieldIds.SubStatusId] == undefined) ? null : form.elements[fieldIds.SubStatusId].value,
                CausingPartId: (form.elements[fieldIds.CausingPartId] == undefined) ? null : form.elements[fieldIds.CausingPartId].value,
                ClosingReasonId: (form.elements[fieldIds.ClosingReasonId] == undefined) ? null : form.elements[fieldIds.ClosingReasonId].value
            }
        }        
        catch (exception) {
            ShowToastMessage("An error has occurred in field validation: " + exception.message, "error", true);
            me.setCaseStatus(me.CASE_IN_IDLE);
            return;
        }        

        $.ajax({
            type: "POST",
            data: JSON.stringify(fieldsToCheck),
            url: params.caseActiveDataChecker,
            contentType: "application/json",
            success: function (data) {
                if (data == 'valid') {                    
                    return me.doSave(submitUrl);
                } else {
                    //Case has inactive value(s)                    
                    ShowToastMessage(data, "error", true);
                    me.setCaseStatus(me.CASE_IN_IDLE);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {                                
                ShowToastMessage(params.checkInactiveDataErrorMessage + " <br/> \"" + thrownError + "\"", "error", true);
                me.setCaseStatus(me.CASE_IN_IDLE);
                return;
            }
        });        
    }
    else
        return me.doSave(submitUrl);
    
}

EditPage.prototype.doSave = function(submitUrl) {
    var me = this;
    var action = submitUrl || '/Cases/Edit';
    me.$form.attr("action", action);
    if (me.isFormValid()) {
        if (me.case.isNew()) {
            me.stopCaseLockTimer();
            me.$form.submit();
            return false;
        } 

        me.stopCaseLockTimer();
        $.post(window.parameters.caseLockChecker, {
                caseId: me.p.currentCaseId,
                caseChangedTime: me.p.caseChangedTime,
                lockGuid: me.p.caseLockGuid
            },
            function (data) {
                if (data == true) {                    
                    me.$form.submit();
                } else {
                    //Case is Locked
                    ShowToastMessage(me.p.saveLockedCaseMessage, "error", true);
                    me.setCaseStatus(me.CASE_IN_IDLE);
                }
            });
    } else {
        me.setCaseStatus(me.CASE_IN_IDLE);
    }
    return false;
};

EditPage.prototype.stopCaseLockTimer = function () {
    var me = this;
    if (me.timerId != undefined)
        clearInterval(me.timerId);    
};

EditPage.prototype.setCaseStatus = function (status) {
    var me = this;
    switch (status)
    {
        case me.CASE_IN_IDLE:
            me._inSaving = false;
            me.$buttonsToDisable.removeClass('disabled');
            caseButtons.css("pointer-events", "");
            $(templateQuickButtonIndicator).css("display", "none");
            return true;
    
        case me.CASE_IN_SAVING:
            me._inSaving = true;        
            me.$buttonsToDisable.addClass('disabled');
            caseButtons.css("pointer-events", "none");
            $(templateQuickButtonIndicator).css("display", "block");
            return true;

        default:
            ShowToastMessage("Case status is not defined!", "error", true);
            return false;
    }    
};

EditPage.prototype.confirmDialog = function (d, onOk, onCancel)
{
    var title = dhHelpdesk.CaseArticles.translate("Meddelande");
    var ok = dhHelpdesk.CaseArticles.translate("Ok");
    var decline = dhHelpdesk.CaseArticles.translate("Avbryt");
    return d.dialog({
        title: title,
        buttons: [
            {
                text: ok,
                click: function(){
                    onOk();
                    d.dialog("close");
                },
                class: 'btn'
            },
            {
                text: decline,
                click: function(){
                    onCancel();
                    d.dialog("close");
                },
                class: 'btn'
            }
        ],
        modal: true
    });
};

EditPage.prototype.CaseWillFinish = function ()
{
    if ($('#CaseLog_FinishingDate').val() != '' || $('#CaseLog_FinishingType').val() != '') {
        return true;
    }
    else {
        return false;
    }
};

EditPage.prototype.onSaveYes = function () {    
    var me = this;
    var c = me.case;
    var url = me.EDIT_CASE_URL;
    if (c.isNew() && c.isChildCase()) {
        url = me.SAVE_GOTO_PARENT_CASE_URL;
    }
    return me.primaryValidation(url);
};

EditPage.prototype.onSaveAndNewYes = function (){
    var me = this;
    return me.primaryValidation(me.SAVE_ADD_CASE_URL);
};

EditPage.prototype.onSaveAndCloseYes = function () {
    var me = this;
    return me.primaryValidation(me.NEW_CLOSE_CASE_URL);
};


EditPage.prototype.ReturnFalse = function () {
    return false;
};

EditPage.prototype.onSaveClick = function () {
    var me = this;
    var c = me.case;
    var url = me.EDIT_CASE_URL;
    if (c.isNew() && c.isChildCase()) {
        url = me.SAVE_GOTO_PARENT_CASE_URL;
    }
    
    return me.primaryValidation(url);
};

EditPage.prototype.onSaveAndCloseClick = function () {
    var me = this;
    return me.primaryValidation(me.NEW_CLOSE_CASE_URL);
};

EditPage.prototype.onSaveAndNewClick = function () {
    var me = this;
    return me.primaryValidation(me.SAVE_ADD_CASE_URL);
};

EditPage.prototype.canDeleteCase = function () {
    var me = this;
    return !(me.$btnDelete.hasClass('disabled') || me.case.isAnyNotClosedChild());
};

EditPage.prototype.showDeleteConfirmationDlg = function () {
    var me = this;
    if (!me.canDeleteCase()) {
        return false;
    }
    me.deleteDlg.show();
    return false;
};

EditPage.prototype.MakeDeleteParams = function (c) {
    var me = this;
    var res = {
        caseId: c.id
    };

    if (c.customerId != 0) {
        res.customerId = c.customerId;
    }

    if (c.parentCaseId != 0) {
        res.parentCaseId = c.parentCaseId;
    }

    if (me.p.backUrl != null) {
        res.backUrl = me.p.backUrl;
    }

    return res;
};

/**
* @param { Case } c
*/
EditPage.prototype.doDeleteCase = function(c) {
    var me = this;
    var $form = $(['<form action="', me.DELETE_CASE_URL, '?', $.param(me.MakeDeleteParams(c)), '" method="post"></form>'].join(String.EMPTY));
    $('body').append($form);
    me.stopCaseLockTimer();
    $form.submit();
};

EditPage.prototype.onDeleteDlgClick = function (res) {
    var me = this;
    if (res === ConfirmationDialog.NO) {
        me.deleteDlg.hide();
        return false;
    }

    if (!me.canDeleteCase()) {
        return false;
    }

    $.post(_parameters.caseLockChecker,
        {
            caseId: _parameters.currentCaseId,
            caseChangedTime: _parameters.caseChangedTime,
            lockGuid: _parameters.caseLockGuid
        })
        .done(callAsMe(function(data) {
            me.showDeleteConfirmationDlg();
            if (data == true) {
                me.doDeleteCase(me.case);
            } else {
                ShowToastMessage(_parameters.deleteLockedCaseMessage, "error", true);
            }
        }));
    return true;
};

EditPage.prototype.InitDeleteConfirmationDlg = function() {
    var me = this;
    // Delete case confirmation dialogue
    var dlgOptions = {
        btnYesText: $("#DeleteDialogDeleteButtonText").val(),
        btnNoText: $("#DeleteDialogCancelButtonText").val(),
        onClick: Utils.callAsMe(me.onDeleteDlgClick, me),
        dlgText: me.$btnDelete.attr('deleteDialogText'),
    };
    if (me.$btnDelete.attr("buttonTypes") === 'YesNo') {
        dlgOptions.btnYesText = $("#DeleteDialogYesButtonText").val();
        dlgOptions.btnNoText = $("#DeleteDialogNoButtonText").val();
    }
    return CreateInstance(ConfirmationDialog, dlgOptions);
};

EditPage.prototype.InitLeaveConfirmationDlg = function () {
    var me = this;
    // Delete case confirmation dialogue
    var dlgOptions = {
        btnYesText: $("#DeleteDialogYesButtonText").val(),
        btnNoText: $("#DeleteDialogNoButtonText").val(),
        dlgText: me.p.lostChangesConfirmation
    };
    
    return CreateInstance(ConfirmationDialog, dlgOptions);
};

EditPage.prototype.onPageLeave = function(ev) {
    var me = this;
    var gotoUrl = ev.target;
    if (me.formOnBootValues !== me.$form.serialize()) {
        me.leaveDlg.show().done(function (result) {
            me.leaveDlg.hide();
            if (result === ConfirmationDialog.YES) {
                me.stopCaseLockTimer();
                window.location.href = gotoUrl;
            }
        });
        ev.preventDefault();
        return false;
    }
};

EditPage.prototype.getTokenIfNeeded = function () {
    var needToGetToken = false;
    var _tokenData = localStorage.getItem("TokenData");

    if (_tokenData !== null) {
        var _details = JSON.parse(_tokenData);
        if (_details != undefined) {
            needToGetToken = _details.AccessToken === '' || _details.RefreshToken === '';
        }
    } else {
        needToGetToken = true;
    }

    if (needToGetToken) {
        $.get(_parameters.getTokenUrl,
            {
                curTime: Date.now()
            },
            function (res) {
                if (res != null) {
                    if (res.result) {
                        localStorage.removeItem("TokenData");
                        var newTokenData = { 'AccessToken': res.accessToken, 'RefreshToken': res.refreshToken };
                        localStorage.setItem("TokenData", JSON.stringify(newTokenData));
                    }
                }
            }
         );
    }
}

EditPage.prototype.onCloseClick = function(ev) {
    var me = this;
    var c = me.case;
    var url;
    if (c.isChildCase() && c.isNew()) {
        url = me.EDIT_CASE_URL + '/' + c.parentCaseId;
    } else {
        url = me.CASE_OVERVIEW_URL;
    }

    me.stopCaseLockTimer();
    window.location.href = url;
    return false;
};

EditPage.prototype.refreshCaseInfo = function (updatedInfo) {
    if (updatedInfo == null)
        return;

    var self = this;
    var _caseFields = self.Case_Field_Ids;
    
    self.changeCaseButtonsState(false);

    $('#' + _caseFields.ReportedBy).val(updatedInfo.ReportedBy);
    $('#' + _caseFields.PersonsName).val(updatedInfo.PersonsName);
    $('#' + _caseFields.PersonsPhone).val(updatedInfo.PersonsPhone);

    $('#' + _caseFields.CaseTypeId).val(updatedInfo.CaseType_Id).change();

    $('#' + _caseFields.ProductAreaId).val(updatedInfo.ProductArea_Id).change();
    $('#' + _caseFields.WorkingGroupId).val(updatedInfo.WorkingGroup_Id).change();
    $('#' + _caseFields.WorkingGroupName).val(updatedInfo.WorkingGroupName);
    $('#' + _caseFields.PriorityId).val(updatedInfo.Priority_Id).change();

    $('#' + _caseFields.PlanDate).datepicker({
        format: updatedInfo.DateFormat.toLowerCase(),
        autoclose: true
    }).datepicker('setDate', updatedInfo.PlanDateJS);

    $('#' + _caseFields.WatchDate).datepicker({
        format: updatedInfo.DateFormat.toLowerCase(),
        autoclose: true
    }).datepicker('setDate', updatedInfo.WatchDateJS);

    $("#CaseTemplate_Department_Id").val();
    $("#CaseTemplate_OU_Id").val();

    if (updatedInfo.Department_Id != null && updatedInfo.Department_Id != 0) {
        $("#CaseTemplate_Department_Id").val(updatedInfo.Department_Id);
    }

    if (updatedInfo.OU_Id != null && updatedInfo.OU_Id != 0) {
        $("#CaseTemplate_OU_Id").val(updatedInfo.OU_Id);
    }

    $('#' + _caseFields.Region_Id).val(updatedInfo.Region_Id).change();
    $('#' + _caseFields.RegionName).val(updatedInfo.RegionName);
    $('#' + _caseFields.DepartmentName).val(updatedInfo.DepartmentName);
    $('#' + _caseFields.OUName).val(updatedInfo.OUName);

    $.get('/Cases/GetCaseFilesJS', { caseId: updatedInfo.Id, now: Date.now() }, function (data) {
        $('#divCaseFiles').html(data);
        $(document).trigger("OnUploadedCaseFileRendered", []);
        bindDeleteCaseFileBehaviorToDeleteButtons();
    });

    $.get('/Cases/GetCaseInputModelForLog', { caseId: updatedInfo.Id, now: Date.now() }, function (data) {
        $('#logtab').html(data);
    }).done(function () {

    });

    $.get('/Cases/GetCaseInputModelForHistory', { caseId: updatedInfo.Id, now: Date.now() }, function (data) {
        $('#' + _caseFields.StatusId).val(updatedInfo.Status_Id).change();
        $('#' + _caseFields.StatusName).val(updatedInfo.StatusName).change();
        $('#' + _caseFields.SubStatusId).val(updatedInfo.StateSecondary_Id).change();
        $(".readonlySubstate").val(updatedInfo.StateSecondary_Id);
        $('#' + _caseFields.SubStatusName).val(updatedInfo.SubStateName);
        $('#historytab').html(data);
    }).done(function () {
        self.changeCaseButtonsState(true);
    });
}

EditPage.prototype.changeCaseButtonsState = function(state) {
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

EditPage.prototype.init = function (p) {
    var self = this;
    self._inSaving = false;
    self.p = p;
    EditPage.prototype.Case_Field_Ids = p.caseFieldIds;
    
    this.Contains_ExtendedCase = self.p.containsExtendedCase;
    this.Contains_Eform = self.p.containsEForm;    

    /// controls binding
    self.$form = $('#target');
    self.$watchDateChangers = $('.departments-list, #case__Priority_Id, #case__StateSecondary_Id');
    self.$department = $('.departments-list');
    self.$SLASelect = $('#case__Priority_Id');
    self.$SLAText = $('#case__Priority_Id');    
    self.$watchDateEdit = $('#case__WatchDate');
    self.$watchDate = $('#divCase__WatchDate');      
    self.$buttonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
                             '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                             '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton');

    self.$productAreaObj = $('#divProductArea');
    self.$productAreaChildObj = $('#ProductAreaHasChild');
    self.productAreaErrorMessage = self.p.productAreaErrorMessage;
    self.$moveCaseButton = $("#btnMoveCase");    
    self.$btnSave = $('.btn.save');
    self.$btnSaveClose = $('.btn.save-close');
    self.$btnSaveNew = $('.btn.save-new');
    self.$btnDelete = $('.caseDeleteDialog.btn');
    self.$btnClose = $('.btn.close-page');
    self.deleteDlg = self.InitDeleteConfirmationDlg();
    self.leaveDlg = self.InitLeaveConfirmationDlg();
    ///////////////////////     events binding      /////////////////////////////////
    self.$btnDelete.on('click', callAsMe(self.showDeleteConfirmationDlg, self));
    self.$btnClose.on('click', Utils.callAsMe(self.onCloseClick, self));
    self.$btnSave.on('click', Utils.callAsMe(self.onSaveClick, self));
    self.$btnSaveClose.on('click', Utils.callAsMe(self.onSaveAndCloseClick, self));
    self.$btnSaveNew.on('click', Utils.callAsMe(self.onSaveAndNewClick, self));

    self.$btnPrint = $('.btn.print-case');    
    self.$printArea = $('#CasePrintArea');
    self.$printDialog = $('#PrintCaseDialog');
    self.$descriptionDialog = $('#caseDescriptionPreview');
    self.$caseDescriptionEl = $(".case-description")
    self.$caseTab = $("#myTab li a");//$('#case-tab');
    self.$activeTabHolder = $('#ActiveTab');

    self.isCaseFileMandatory = self.p.isCaseFileMandatory;

    var invoiceElm = $('#CustomerSettings_ModuleCaseInvoice').val();
    self.invoiceIsActive = invoiceElm != undefined && invoiceElm != null && invoiceElm.toString().toLowerCase() == 'true';

    self.$watchDateChangers.on('change', function () {        
        var deptId = parseInt(self.$department.val(), 10);
        var SLA = parseInt(self.$SLASelect.find('option:selected').attr('data-sla'), 10);
        if (isNaN(SLA)) {            
            SLA = parseInt(self.$SLAText.attr('data-sla'), 10);
        }
        if (this.id == "case__StateSecondary_Id") {
            $.post('/Cases/ChangeStateSecondary', { 'id': $(this).val() }, function (data) {
                if (data.ReCalculateWatchDate == 1) {
                    if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
                        return self.fetchWatchDateByDept.call(self, deptId);
                    }
                }
            }, 'json');
            return;
        }

        if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
            return self.fetchWatchDateByDept.call(self, deptId);
        }
        //else {
        //    if (self.$watchDateEdit.val() == '')
        //        self.$watchDate.datepicker('update', '');
        //}

        return false;
    });
    
    self.$moveCaseButton.click(function (e) {
        e.preventDefault();
        $.post(p.caseLockChecker,
            {
                caseId: p.currentCaseId,
                caseChangedTime: p.caseChangedTime,
                lockGuid: p.caseLockGuid
            },
            function (data) {
                if (data == true) {
                    window.moveCase(p.currentCaseId);
                } else {
                    ShowToastMessage(p.moveLockedCaseMessage, "error", true);
                }
        });
    });
    
    $('.date').each(function () {
        var $this = $(this);
        var errorLabel = $this.find('label.error:visible');
        if (errorLabel.length > 0) {
            var calendarIcon = $this.find('.add-on');
            if (calendarIcon.length > 0) {
                errorLabel.detach().insertAfter(calendarIcon);
            }
        }
    });

    $('.lang.dropdown-submenu a').on('click', Utils.callAsMe(self.onPageLeave, self));             

    self.$btnPrint.click(function (e) {
            
        /* Setup Print Page 
        try {
            var regpath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\PageSetup\\" + "Print_Background";
            var oWSS = new ActiveXObject("WScript.Shell");
            oWSS.RegWrite(regpath, "yes", "REG_SZ");


            var regpath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\PageSetup\\" + "Shrink_To_Fit";
            var oWSS = new ActiveXObject("WScript.Shell");
            oWSS.RegWrite(regpath, "no", "REG_SZ");
        }
        catch (err) {            
        }*/
        
        $.get("/Cases/ShowCasePrintPreview/",
                {
                    caseId: p.currentCaseId,
                    caseNumber: p.currentCaseNumber,
                    curTime: new Date().getTime()
                },                

                function (_reportPresentation) {
                    self.$printArea.html(_reportPresentation);
                                        
                    $('#PrintCaseDialog').draggable({
                        handle: ".modal-header"
                    });
                    
                    /* show true if you need to show case print preview*/
                    $('#PrintCaseDialog').modal({
                        "backdrop": "static",
                        "keyboard": true,
                        "show": false
                    });
                   
                    //var _iframe = $("#caseReportContainer").find("iframe");
                    //if (_iframe != null && _iframe != undefined) {
                    //    update_iFrame(_iframe.attr("id"));                        
                    //}
                }
             );       
    });
    
    self.$descriptionDialog.click(function () {
        var description = self.$caseDescriptionEl
            .attr("value")
            .replace(/\r\n|\r|\n/g, "<br />");

        var d = $(document.createElement("div"))
            .html(description)
            .dialog({
                title:
                    self.p.descriptionText,
                modal: true,
                width: 600,
                height: 400,
                close: function () {
                    d.dialog("destroy");
                }
            });
    });

    self.$caseDescriptionEl.keyup(function (ev) {
        var visibility = 'visible';
        if (ev.target.value.length == 0) {
            visibility = 'hidden';
        }
        self.$descriptionDialog.css('visibility', visibility);
    });

    /*Enable if you have case print preview*/
    //var update_iFrame = function (iframeId) {
    //    setTimeout(function () {           
    //        var elm = document.getElementById(iframeId);
    //        if (elm != null && elm != undefined) {
    //            var newH = $(elm).height() + 50;
    //            $(elm).css({ height: newH.toString() + "px" });
    //        }            
    //    }, 3000);
    //}

    //////// event bind end ///////////
    /*
        window.parameters.currentCaseId,
        customerId: window.parameters.customerId,
        parentCaseId: window.parameters.parentCaseId
    */
    self.case = new Case({
        id: parseInt(p.currentCaseId, 10),
        customerId: parseInt(p.customerId, 10),
        parentCaseId: parseInt(p.parentCaseId, 10),
        isAnyNotClosedChild: p.isAnyNotClosedChild === 'True'
    });

    if (self.case.isAnyNotClosedChild()) {
        self.$btnDelete.addClass('disabled');
    }

    if (p.currentCaseId > 0 && self.p.timerInterval > 0) {
        self.timerId = setInterval(callAsMe(self.ReExtendCaseLock, self), self.p.timerInterval * 1000);
    }

    self.formOnBootValues = self.$form.serialize();

    self.ExternalInvoice = new ExternalInvoice({
        requiredMessage: p.casesScopeInitParameters.mandatoryFieldsText,
        mustBeNumberMessage: p.casesScopeInitParameters.formatFieldsText + ": #.##"
    });
        
    self.$caseTab.click(function (ev) {
        if (this.Contains_ExtendedCase) {
            self.$activeTabHolder.val('case-tab');
        }

        if (this.Contains_Eform) {
            if (ev.target.className.indexOf("case") >= 0) {
                if ($(".secnav").hasClass("hide")) {
                    $(".secnav").fadeIn(300, function () {
                        $(".secnav").removeClass("hide");
                    });
                }

                $.ajax({
                    url: "/Cases/GetCaseInfo",
                    type: "GET",
                    data: { caseId: self.p.currentCaseId, curTime: Date.now }
                })
                .done(function (result) {
                    if (result.needUpdate) {
                        if (result.shouldReload) {
                            location.reload(true);
                        } else {
                            self.refreshCaseInfo(result.newData);
                        }
                    }
                });
            }

            if (ev.target.className.indexOf("eform") >= 0) {
                var secnav = $(".secnav");
                if (secnav.hasClass("hide") == false) {
                    secnav.fadeOut(300, function () {
                        secnav.addClass("hide");
                    });
                }
            }
        }
    });

    self.$caseTab.on('shown', function (e) {
        window.scrollTo(0, 0);
    });

    /* Temporary disabled because Extended Case will not use token */
    //self.getTokenIfNeeded();
};


