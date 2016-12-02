"use strict";

function EditPage() {};

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
            
    var curFinishDate = $('#' + me.p.caseFieldNames.FinishingDate).val();
    if (curFinishDate != undefined && curFinishDate != '') {
        var regDate = me.getDate(me.p.caseRegDate);            
        var finishDate = me.getDate(curFinishDate);
        if (regDate > finishDate) {
            dhHelpdesk.cases.utils.showError(me.p.finishingDateMessage);
            $('#' + me.p.caseFieldNames.FinishingDate).addClass("error");
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

    /* Check FinishigTime */
    if (me.CaseWillFinish()) {
        var finishDate =  $('#CaseLog_FinishingDate').val();
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
            var fieldsToCheck = {
                CustomerId: (form.elements[params.caseFieldNames.CustomerId] == undefined) ? null : form.elements[params.caseFieldNames.CustomerId].value,
                RegionId: (form.elements[params.caseFieldNames.RegionId] == undefined) ? null : form.elements[params.caseFieldNames.RegionId].value,
                DepartmentId: (form.elements[params.caseFieldNames.DepartmentId] == undefined) ? null : form.elements[params.caseFieldNames.DepartmentId].value,
                OUId: (form.elements[params.caseFieldNames.OUId] == undefined) ? null : form.elements[params.caseFieldNames.OUId].value,
                SourceId: (form.elements[params.caseFieldNames.SourceId] == undefined) ? null : form.elements[params.caseFieldNames.SourceId].value,
                CaseTypeId: (form.elements[params.caseFieldNames.CaseTypeId] == undefined) ? null : form.elements[params.caseFieldNames.CaseTypeId].value,
                ProductAreaId: (form.elements[params.caseFieldNames.ProductAreaId] == undefined) ? null : form.elements[params.caseFieldNames.ProductAreaId].value,
                CategoryId: (form.elements[params.caseFieldNames.CategoryId] == undefined) ? null : form.elements[params.caseFieldNames.CategoryId].value,
                SupplierId: (form.elements[params.caseFieldNames.SupplierId] == undefined) ? null : form.elements[params.caseFieldNames.SupplierId].value,
                WorkingGroupId: (form.elements[params.caseFieldNames.WorkingGroupId] == undefined) ? null : form.elements[params.caseFieldNames.WorkingGroupId].value,
                ResponsibleId: (form.elements[params.caseFieldNames.ResponsibleId] == undefined) ? null : form.elements[params.caseFieldNames.ResponsibleId].value,
                AdministratorId: (form.elements[params.caseFieldNames.AdministratorId] == undefined) ? null : form.elements[params.caseFieldNames.AdministratorId].value,
                PriorityId: (form.elements[params.caseFieldNames.PriorityId] == undefined) ? null : form.elements[params.caseFieldNames.PriorityId].value,
                StatusId: (form.elements[params.caseFieldNames.StatusId] == undefined) ? null : form.elements[params.caseFieldNames.StatusId].value,
                SubStatusId: (form.elements[params.caseFieldNames.SubStatusId] == undefined) ? null : form.elements[params.caseFieldNames.SubStatusId].value,
                CausingPartId: (form.elements[params.caseFieldNames.CausingPartId] == undefined) ? null : form.elements[params.caseFieldNames.CausingPartId].value,
                ClosingReasonId: (form.elements[params.caseFieldNames.ClosingReasonId] == undefined) ? null : form.elements[params.caseFieldNames.ClosingReasonId].value
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

/**
* @private
*/
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

/**
* @private
*/
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

/**
* Page initialization 
*/
EditPage.prototype.init = function (p) {
    var me = this;
    me._inSaving = false;
    me.p = p;
    /// controls binding
    me.$form = $('#target');
    me.$watchDateChangers = $('.departments-list, #case__Priority_Id, #case__StateSecondary_Id');
    me.$department = $('.departments-list');
    me.$SLASelect = $('#case__Priority_Id');
    me.$SLAText = $('#case__Priority_Id');    
    me.$watchDateEdit = $('#case__WatchDate');
    me.$watchDate = $('#divCase__WatchDate');      
    me.$buttonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new, .btn.caseDeleteDialog, ' +
                             '#case-action-close, #divActionMenu, #btnActionMenu, #divCaseTemplate, #btnCaseTemplateTree, .btn.print-case,' +
                             '.btn.show-inventory, .btn.previous-case, .btn.next-case, .btn.templateQuickButton');

    me.$productAreaObj = $('#divProductArea');
    me.$productAreaChildObj = $('#ProductAreaHasChild');
    me.productAreaErrorMessage = me.p.productAreaErrorMessage;
    me.$moveCaseButton = $("#btnMoveCase");    
    me.$btnSave = $('.btn.save');
    me.$btnSaveClose = $('.btn.save-close');
    me.$btnSaveNew = $('.btn.save-new');
    me.$btnDelete = $('.caseDeleteDialog.btn');
    me.$btnClose = $('.btn.close-page');
    me.deleteDlg = me.InitDeleteConfirmationDlg();
    me.leaveDlg = me.InitLeaveConfirmationDlg();
    ///////////////////////     events binding      /////////////////////////////////
    me.$btnDelete.on('click', callAsMe(me.showDeleteConfirmationDlg, me));
    me.$btnClose.on('click', Utils.callAsMe(me.onCloseClick, me));
    me.$btnSave.on('click', Utils.callAsMe(me.onSaveClick, me));
    me.$btnSaveClose.on('click', Utils.callAsMe(me.onSaveAndCloseClick, me));
    me.$btnSaveNew.on('click', Utils.callAsMe(me.onSaveAndNewClick, me));

    me.$btnPrint = $('.btn.print-case');    
    me.$printArea = $('#CasePrintArea');
    me.$printDialog = $('#PrintCaseDialog');

    me.isCaseFileMandatory = me.p.isCaseFileMandatory;

    var invoiceElm = $('#CustomerSettings_ModuleCaseInvoice').val();
    me.invoiceIsActive = invoiceElm != undefined && invoiceElm != null && invoiceElm.toString().toLowerCase() == 'true';

    me.$watchDateChangers.on('change', function () {        
        var deptId = parseInt(me.$department.val(), 10);
        var SLA = parseInt(me.$SLASelect.find('option:selected').attr('data-sla'), 10);
        if (isNaN(SLA)) {            
            SLA = parseInt(me.$SLAText.attr('data-sla'), 10);
        }
        if (this.id == "case__StateSecondary_Id") {
            $.post('/Cases/ChangeStateSecondary', { 'id': $(this).val() }, function (data) {
                if (data.ReCalculateWatchDate == 1) {
                    if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
                        return me.fetchWatchDateByDept.call(me, deptId);
                    }
                }
            }, 'json');
            return;
        }

        if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
            return me.fetchWatchDateByDept.call(me, deptId);
        }
        //else {
        //    if (me.$watchDateEdit.val() == '')
        //        me.$watchDate.datepicker('update', '');
        //}

        return false;
    });
    
    me.$moveCaseButton.click(function (e) {
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

    $('.lang.dropdown-submenu a').on('click', Utils.callAsMe(me.onPageLeave, me));             

    me.$btnPrint.click(function (e) {
            
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
                    me.$printArea.html(_reportPresentation);
                                        
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
    me.case = new Case({
        id: parseInt(p.currentCaseId, 10),
        customerId: parseInt(p.customerId, 10),
        parentCaseId: parseInt(p.parentCaseId, 10),
        isAnyNotClosedChild: p.isAnyNotClosedChild === 'True'
    });

    if (me.case.isAnyNotClosedChild()) {
        me.$btnDelete.addClass('disabled');
    }

    if (p.currentCaseId > 0 && me.p.timerInterval > 0) {
        me.timerId = setInterval(callAsMe(me.ReExtendCaseLock, me), me.p.timerInterval * 1000);
    }

    me.formOnBootValues = me.$form.serialize();

    me.ExternalInvoice = new ExternalInvoice({
        requiredMessage: p.casesScopeInitParameters.mandatoryFieldsText,
        mustBeNumberMessage: p.casesScopeInitParameters.formatFieldsText + ": #.##"
    });
};