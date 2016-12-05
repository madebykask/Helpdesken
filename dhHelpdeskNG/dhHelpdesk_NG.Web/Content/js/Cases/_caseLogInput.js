'use strict';

/**
* Initializator for Log form and case-log related controls
*/
function LogInitForm() {
    var $finishTypeContainer = $('#divFinishingType');
    var $finishTypeId = $("#CaseLog_FinishingType");
    var $finishTypeBreadcrubs = $("#divBreadcrumbs_FinishingType");
    var $finishDate = $('#CaseLog_FinishingDate');

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

    $("#CaseLog_SendMailAboutLog").change(function () {
        if ($(this).attr('readonly') != null) {
            return false;
        }
        $('#divEmailRecepientsInternalLog').hide();
        if (this.checked) {
            $("#divSendToDialogCase").dialog("option", "width", 450);
            $("#divSendToDialogCase").dialog("option", "height", 550);
            $("#divSendToDialogCase").dialog("open");
        }
    });
//
//    $("#caseLogSendToMailAboutLogTo").click(function () {
//        $("#SendIntLogCase").dialog("option", "width", 350);
//        $("#SendIntLogCase").dialog("option", "height", 350);
//        $("#SendIntLogCase").dialog("option", "dialogType", 1);
//        $("#SendIntLogCase").dialog("open");
//        var existEmails = $("#CaseLog_EmailRecepientsInternalLogTo").val();
//        $("#casesIntLogSendInput").val(existEmails);
//        $("#casesIntLogSendInput").focus();
//    });
//
//    $("#caseLogSendCcMailAboutLogCc").click(function () {
//        $("#SendIntLogCase").dialog("option", "width", 350);
//        $("#SendIntLogCase").dialog("option", "height", 350);
//        $("#SendIntLogCase").dialog("option", "dialogType", 2);
//        $("#SendIntLogCase").dialog("open");
//        var existEmails = $("#CaseLog_EmailRecepientsInternalLogCc").val();
//        $("#casesIntLogSendInput").val(existEmails);
//        $("#casesIntLogSendInput").focus();
//    });

    $('#CaseLog_TextExternal').bind('input propertychange', function () {
        var informNotifier = $('#CaseLog_SendMailAboutCaseToNotifier');
        var isInformNotifierBehavior = informNotifier.attr("InformNotifierBehavior");
        if (isInformNotifierBehavior == "false") {
            return;
        }

        informNotifier.removeAttr('checked');
        if (this.value.length) {
            $('#CaseLog_SendMailAboutCaseToNotifier:not(:disabled)').attr('checked', 'checked');
        }
    });

    bindDeleteLogFileBehaviorToDeleteButtons();
}
