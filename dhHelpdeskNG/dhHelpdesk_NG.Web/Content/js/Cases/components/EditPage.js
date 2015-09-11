"use strict";

function EditPage() {
    this.DELETE_CASE_URL = '/Cases/DeleteCase';
    this.NEW_CLOSE_CASE_URL = '/Cases/NewAndClose';
    this.EDIT_CASE_URL = '/Cases/Edit';
    this.SAVE_ADD_CASE_URL = '/Cases/NewAndAddCase';
};

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
    me._inSaving = false;
    me.$buttonsToDisable.removeClass('disabled');
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

EditPage.prototype.doSave = function(submitUrl) {
    var me = this;
    var action = submitUrl || me.EDIT_CASE_URL;
    if (me._inSaving) {
        return false;
    }
    me._inSaving = true;
    me.$buttonsToDisable.addClass('disabled');
    me.$form.attr("action", action);
    if (me.isFormValid()) {
        if (me.case.isNew()) {
            me.$form.submit();
            return false;
        } 

        $.post(window.parameters.caseLockChecker, {
                caseId: me.p.currentCaseId,
                lockGuid: me.p.caseLockGuid
            },
            function (data) {
                if (data == true) {
                    me.$form.submit();
                } else {
                    //Case is Locked
                    ShowToastMessage(me.p.saveLockedCaseMessage, "error", true);
                    me.$buttonsToDisable.removeClass('disabled');
                    me._inSaving = false;
                }
            });
    } else {
        me._inSaving = false;
        me.$buttonsToDisable.removeClass('disabled');
    }
    return false;
};

EditPage.prototype.onSaveClick = function () {
    var me = this;
    return me.doSave();
};

EditPage.prototype.onSaveAndCloseClick = function () {
    var me = this;
    return me.doSave(me.NEW_CLOSE_CASE_URL);
};

EditPage.prototype.onSaveAndNewClick = function () {
    var me = this;
    return me.doSave(me.SAVE_ADD_CASE_URL);
};

EditPage.prototype.canDeleteCase = function () {
    var me = this;
    return !(me.$btnDelete.hasClass('disabled') || me.case.isAnyNotClosedChild());
};


EditPage.prototype.closeDeleteConfirmationDlg = function () {
    var me = this;
    me.deleteDlg.hide();
    return me.deleteDlg;
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

EditPage.prototype.onDeleteDlgClick = function () {
    var me = this;
    if (!me.canDeleteCase()) {
        return false;
    }
    $.post(_parameters.caseLockChecker, { caseId: _parameters.currentCaseId, lockGuid: _parameters.caseLockGuid })
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
* @param { Case c }
*/
EditPage.prototype.initDeleteConfirmationDlg = function (c) {
    var me = this;
    // Delete case confirmation dialogue
    var deleteDlgOptions = {
        btnYesText: $("#DeleteDialogDeleteButtonText").val(),
        btnNoText: $("#DeleteDialogCancelButtonText").val(),
        onYesClick: callAsMe(me.onDeleteDlgClick, me),
        onNoClick: callAsMe(me.closeDeleteConfirmationDlg, me),
        dlgText: me.$btnDelete.attr('deleteDialogText'),
//        dlgAction: me.DELETE_CASE_URL + '?' + $.param(deleteActionData)
    };
    if (me.$btnDelete.attr("buttonTypes") === 'YesNo') {
        deleteDlgOptions.btnYesText = $("#DeleteDialogYesButtonText").val();
        deleteDlgOptions.btnNoText = $("#DeleteDialogNoButtonText").val();
    }
    return CreateInstance(ConfirmationDialog, deleteDlgOptions);
}

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
    me.$buttonsToDisable = $('.btn.save, .btn.save-close, .btn.save-new');
    me.$btnSave = $('.btn.save');
    me.$btnSaveClose = $('.btn.save-close');
    me.$btnSaveNew = $('.btn.save-new');
    me.$btnDelete = $('.caseDeleteDialog.btn');
    me.deleteDlg = me.initDeleteConfirmationDlg();
    ///////////////////////     events binding      /////////////////////////////////
    me.$btnDelete.on('click', callAsMe(me.showDeleteConfirmationDlg, me));
    me.$btnSave.on('click', function () {
        return me.onSaveClick.call(me);
    });
    me.$btnSaveClose.on('click', function () {
        return me.onSaveAndCloseClick.call(me);
    });
    me.$btnSaveNew.on('click', function () {
        return me.onSaveAndNewClick.call(me);
    });

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
            { caseId: p.currentCaseId, lockGuid: p.caseLockGuid },
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
    //////// init actions end ///////////
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
};