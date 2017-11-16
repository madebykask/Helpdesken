'use strict';

/**
* Initializator for Log form and case-log related controls
*/
function LogInitForm() {

    var utils = {
        okText: '',
        cancelText: '',
        yesText: '',
        noText: '',

        init: function (okText, cancelText, yesText, noText) {
            utils.okText = okText;
            utils.cancelText = cancelText;
            utils.yesText = yesText;
            utils.noText = noText;
        },

        showMessage: function (message, type) {
            $().toastmessage('showToast', {
                text: utils.replaceAll(message, '|', '<br />'),
                sticky: false,
                position: 'top-center',
                type: type || 'notice',
                closeText: '',
                stayTime: 10000,
                inEffectDuration: 1000,
                width: 700
            });
        },

        showWarning: function (message) {
            utils.showMessage(message, 'warning');
        },

        showError: function (message) {
            utils.showMessage(message, 'error');
        },

        replaceAll: function (string, omit, place, prevstring) {
            if (prevstring && string === prevstring)
                return string;
            prevstring = string.replace(omit, place);
            return utils.replaceAll(prevstring, omit, place, string);
        }
    }

    var $form = $('#target');
    var $finishTypeContainer = $('#divFinishingType');
    var $finishTypeId = $("#CaseLog_FinishingType");
    var $finishTypeBreadcrubs = $("#divBreadcrumbs_FinishingType");
    var $finishDate = $('#CaseLog_FinishingDate');
    var EDIT_LOG_URL = '/Cases/EditLog';

    var getValidationErrorMessage = function (extraMessage) {
        var validationMessages = window.parameters.validationMessages || '';
        var requiredFieldsMessage = window.parameters.requiredFieldsMessage || '';
        var mandatoryFieldsText = window.parameters.mandatoryFieldsText || '';
        var messages = [requiredFieldsMessage, '<br />', mandatoryFieldsText, ':'];
        $("label.error").each(function (key, el) {
            if ($(el).css('display') === 'none') {
                return true;
            }
            var errorText = $(el).text();
            $.each(validationMessages, function (index, validationMessage) {
                errorText = '<br />' + '[' + utils.replaceAll(errorText, validationMessage, '').trim() + ']';
            });
            messages.push(errorText);
        });

        messages.push(extraMessage);

        return messages.join('');
    };

    var isFormValid = function () {
        if (!$form.valid()) {
            utils.showError(getValidationErrorMessage());
            return false;
        }
        return true;
    };

    $finishTypeContainer.find('ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var value = $(this).attr('value');
        $finishTypeBreadcrubs.text(window.getBreadcrumbs(this));
        $finishTypeId.val(value).trigger('change');
    });

    $finishTypeId.on('change', function (ev) {
        var value = $(ev.target).val();
        if (value == '' || value === undefined) {
            $finishDate.val('');
        } else {
            if ($finishDate.val() == '') {
                $finishDate.val(window.today());
            }
        }
    });

    $('#CaseLog_TextExternal').bind('input propertychange', function () {
        var informNotifier = $('#CaseLog_SendMailAboutCaseToNotifier');

        informNotifier.removeAttr('checked');
        if (this.value.length) {
            $('#CaseLog_SendMailAboutCaseToNotifier:not(:disabled)').attr('checked', 'checked');
        }
    });

    bindDeleteLogFileBehaviorToDeleteButtons();

    $("#btnCaseCharge").on('click', function (ev) {
        window.caseChargeObj.show();
    });

    $('#log-action-save').on('click',function(e) {
        isFormValid();
    });

    $('#case__StateSecondary_Id').change(function () {
        $('#CaseLog_SendMailAboutCaseToNotifier').removeAttr('disabled');
        var curVal = $('#case__StateSecondary_Id').val();
        $('#case__StateSecondary_Id option[value=' + curVal + ']').attr('selected', 'selected');
        $.post('/Cases/ChangeStateSecondary', { 'id': $(this).val() }, function (data) {
            // disable send mail checkbox
            if (data.NoMailToNotifier == 1) {
                $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', false);
                $('#CaseLog_SendMailAboutCaseToNotifier').attr('disabled', true);
            }
            else {
                if ($('#CaseLog_TextExternal').val() == '') {
                    $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', false);
                    $('#CaseLog_SendMailAboutCaseToNotifier').attr('disabled', false);
                } else {

                    $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', true);
                    $('#CaseLog_SendMailAboutCaseToNotifier').attr('disabled', false);
                }
                
            }
            // set workinggroup id
            var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
            if (exists > 0 && data.WorkingGroup_Id > 0) {
                $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
            }
        }, 'json');
    });
}
