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
        { 'departmentId': deptId },
        function (response) {
            if (response.result === 'success') {
                if (response.data != null) {
                    var dt = new Date(parseInt(response.data.replace("/Date(", "").replace(")/", ""), 10));
                    me.$watchDate.datepicker('update', dt);
                } else {
                    me.$watchDate.datepicker('update', '');
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


EditPage.prototype.getValidationErrorMessage = function () {
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
    return messages.join('');
};

EditPage.prototype.isFormValid = function() {
    var me = this;

    if (!me.isProductAreaValid()) {
        me.$productAreaObj.addClass("error");
        dhHelpdesk.cases.utils.showError(me.productAreaErrorMessage);
        return false;
    }    

    if (!me.$form.valid()) {
        dhHelpdesk.cases.utils.showError(me.getValidationErrorMessage());
        return false;
    }
    
    return true;
};

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
        var form = me.$form[0];
        var fieldsToCheck = {
            CustomerId: form.elements[params.caseFieldNames.CustomerId].value,
            RegionId: form.elements[params.caseFieldNames.RegionId].value,
            DepartmentId: form.elements[params.caseFieldNames.DepartmentId].value,
            OUId: form.elements[params.caseFieldNames.OUId].value,
            SourceId: form.elements[params.caseFieldNames.SourceId].value,
            CaseTypeId: form.elements[params.caseFieldNames.CaseTypeId].value,
            ProductAreaId: form.elements[params.caseFieldNames.ProductAreaId].value,
            CategoryId: form.elements[params.caseFieldNames.CategoryId].value,
            SupplierId: form.elements[params.caseFieldNames.SupplierId].value,
            WorkingGroupId: form.elements[params.caseFieldNames.WorkingGroupId].value,
            ResponsibleId: form.elements[params.caseFieldNames.ResponsibleId].value,
            AdministratorId: form.elements[params.caseFieldNames.AdministratorId].value,
            PriorityId: form.elements[params.caseFieldNames.PriorityId].value,
            StatusId: form.elements[params.caseFieldNames.StatusId].value,
            SubStatusId: form.elements[params.caseFieldNames.SubStatusId].value,
            CausingPartId: form.elements[params.caseFieldNames.CausingPartId].value,
            ClosingReasonId: form.elements[params.caseFieldNames.ClosingReasonId].value
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
                ShowToastMessage("Error in check inactive items! <br/> \"" + thrownError + "\"", "error", true);
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
            me.$form.submit();
            return false;
        } 

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

EditPage.prototype.setCaseStatus = function (status) {
    var me = this;
    switch (status)
    {
        case me.CASE_IN_IDLE:
            me._inSaving = false;
            me.$buttonsToDisable.removeClass('disabled');
            return true;
    
        case me.CASE_IN_SAVING:
            me._inSaving = true;        
            me.$buttonsToDisable.addClass('disabled');
            return true;

        default:
            ShowToastMessage("Case status is not defined!", "error", true);
            return false;
    }    
};


EditPage.prototype.onSaveClick = function () {
    var me = this;
    var c = me.case;
    var url = me.EDIT_CASE_URL;
    if (c.isNew() && c.isChildCase()) {
        url = me.SAVE_GOTO_PARENT_CASE_URL;
    }
    
    return me.checkAndSave(url);
};

EditPage.prototype.onSaveAndCloseClick = function () {
    var me = this;
    return me.checkAndSave(me.NEW_CLOSE_CASE_URL);    
};

EditPage.prototype.onSaveAndNewClick = function () {
    var me = this;
    return me.checkAndSave(me.SAVE_ADD_CASE_URL);    
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

EditPage.prototype.MakeDeleteParams = function(c) {
    var res = {
        caseId: c.id
    };
    if (c.customerId != 0) {
        res.customerId = c.customerId;
    }
    if (c.parentCaseId != 0) {
        res.parentCaseId = c.parentCaseId;
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
    me.$watchDateChangers = $('.departments-list, #case__Priority_Id');
    me.$department = $('.departments-list');
    me.$SLASelect = $('#case__Priority_Id');
    me.$SLAInput = $('input.sla-value');
    me.$watchDate = $('#divCase__WatchDate');
    me.$buttonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new');
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

    me.$watchDateChangers.on('change', function () {
        var deptId = parseInt(me.$department.val(), 10);
        var SLA = parseInt(me.$SLASelect.find('option:selected').attr('data-sla'), 10);
        if (isNaN(SLA)) {
            SLA = parseInt(me.$SLAInput.attr('data-sla'), 10);
        }

        if (!isNaN(deptId) && (!isNaN(SLA) && SLA === 0)) {
            return me.fetchWatchDateByDept.call(me, deptId);
        } else {
            me.$watchDate.datepicker('update', '');
        }

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

    if (p.currentCaseId > 0) {
        me.timerId = setInterval(callAsMe(me.ReExtendCaseLock, me), me.p.extendValue * 1000);
    }

    me.formOnBootValues = me.$form.serialize();
};