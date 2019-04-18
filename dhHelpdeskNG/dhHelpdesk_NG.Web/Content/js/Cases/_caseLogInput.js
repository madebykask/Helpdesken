'use strict';

/**
* Initializator for Log form and case-log related controls
*/
function LogInitForm() {

    var $finishTypeContainer = $('#divFinishingType');
    var $finishTypeId = $("#CaseLog_FinishingType");
    var $finishTypeBreadcrubs = $("#divBreadcrumbs_FinishingType");
    var $finishDate = $('#CaseLog_FinishingDate');
    var EDIT_LOG_URL = '/Cases/EditLog';
    
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
}
