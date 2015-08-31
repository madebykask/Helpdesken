"use strict";

function EditPage() { };

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
    var message = requiredFieldsMessage + '<br />' + mandatoryFieldsText + ':';
    $('label.error:visible').each(function (key, value) {
        var errorText = $(value).text();
        $.each(validationMessages, function (index, validationMessage) {
            errorText = '<br />' + '[' + dhHelpdesk.cases.utils.replaceAll(errorText, validationMessage, '').trim() + ']';
        });
        message += errorText;
    });
    return message;
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
    var action = submitUrl || '/Cases/Edit';
    if (me._inSaving) {
        return false;
    }
    me._inSaving = true;
    me.$buttonsToDisable.addClass('disabled');
    me.$form.attr("action", action);
    if (me.isFormValid()) {
        $.post(window.parameters.caseLockChecker, {
                caseId: me.p.currentCaseId,
                lockGuid: me.p.caseLockGuid
            },
            function(data) {
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
    return me.doSave('/Cases/NewAndClose');
};

EditPage.prototype.onSaveAndNewClick = function () {
    var me = this;
    return me.doSave('/Cases/NewAndAddCase');
};

/**
* Page initialization 
*/
EditPage.prototype.init = function (_parameters) {
    var me = this;
    me._inSaving = false;
    me.p = _parameters;
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
    ///////////////////////     events binding      /////////////////////////////////
    $('.btn.save').on('click', function () {
        return me.onSaveClick.call(me);
    });

    $('.btn.save-close').on('click', function () {
        return me.onSaveAndCloseClick.call(me);
    });

    $('.btn.save-new').on('click', function () {
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
        $.post(_parameters.caseLockChecker,
            { caseId: _parameters.currentCaseId, lockGuid: _parameters.caseLockGuid },
            function (data) {
                if (data == true) {
                    window.moveCase(_parameters.currentCaseId);
                } else {
                    ShowToastMessage(_parameters.moveLockedCaseMessage, "error", true);
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
    // Timer for case in edit mode (every 1 minutes)
    if (_parameters.currentCaseId > 0) {
        me.timerId = setInterval(callAsMe(me.ReExtendCaseLock, me), 60000);
    }
};