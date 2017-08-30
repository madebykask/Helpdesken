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
//        var isInformNotifierBehavior = informNotifier.attr("InformNotifierBehavior");
//        if (isInformNotifierBehavior == "false") {
//            return;
//        }

        informNotifier.removeAttr('checked');
        if (this.value.length) {
            $('#CaseLog_SendMailAboutCaseToNotifier:not(:disabled)').attr('checked', 'checked');
        }
    });

    bindDeleteLogFileBehaviorToDeleteButtons();

    $("#btnCaseCharge").on('click', function (ev) {
        window.caseChargeObj.show();
    });

    $('#log-action-save').on('click',function(e){
      
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
                $('#CaseLog_SendMailAboutCaseToNotifier').prop('checked', false);
                $('#CaseLog_SendMailAboutCaseToNotifier').attr('disabled', false);
            }
            // set workinggroup id
            var exists = $('#case__WorkingGroup_Id option[value=' + data.WorkingGroup_Id + ']').length;
            if (exists > 0 && data.WorkingGroup_Id > 0) {
                $("#case__WorkingGroup_Id").val(data.WorkingGroup_Id);
            }
        }, 'json');
    });
}
