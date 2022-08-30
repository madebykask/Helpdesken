'use strict';

/**
* Initializator for Log form and case-log related controls
*/

function LogInitForm() {



    var $finishTypeContainer = $('#divFinishingType');
    var $finishTypeId = $("#CaseLog_FinishingType");
    var $finishTypeBreadcrubs = $("#divBreadcrumbs_FinishingType");
    var $finishDate = $('#CaseLog_FinishingDate');
    var $txtInformPerformer = $('#txtInformPerformer');
    var $caseLogTextInternal = $('#CaseLog_TextInternal');
    var $caseLogTextExternal = $('#CaseLog_TextExternal');
    var $initiatorEmail = $('#case__PersonsEmail')
    var $caseLogSendMailAboutCaseToPerformer = $('#CaseLog_SendMailAboutCaseToPerformer');
    var caseLogEmailRecepientsInternalLogTo = 'fake_CaseLog_EmailRecepientsInternalLogTo';
    var caseLogEmailRecepientsInternalLogCc = 'fake_CaseLog_EmailRecepientsInternalLogCc';
    var hasCaseLogTextInternalEmailsTo = false;
    var hasCaseLogTextInternalEmailsCc = false;
    
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

    $($caseLogTextExternal).bind('input propertychange', function () {
        var informNotifier = $('#CaseLog_SendMailAboutCaseToNotifier');
  
        informNotifier.removeAttr('checked');
        if (this.value.length) {
            $('#CaseLog_SendMailAboutCaseToNotifier:not(:disabled)').attr('checked', 'checked');
            var to = $initiatorEmail.val();
            var extras = $("#extraEmailsInput").val().replaceAll(";", " ").replaceAll(to, "");
            if (to != '') {
                $("#emailsTo").html(to);
                $("#emailsTo").css('display', 'inline');
                $("#emailsToTitle").css('display', 'inline');
            }
            if (extras != '') {
                $('#extraEmailsInputShow').text(extras);
                $('#extraEmailsInputShow').show();
                $("#ccTitle").show();
            }
             

        }
    });

    $($caseLogTextInternal).bind('input propertychange', function () {
        var informPerformer = $('#CaseLog_SendMailAboutCaseToPerformer');
        informPerformer.removeAttr('checked');

        $($txtInformPerformer).css('display', 'none');
        $('#CaseLog_TextInternal').prop('required', false);
        if (this.value.length || (hasCaseLogTextInternalEmailsTo || hasCaseLogTextInternalEmailsCc)) {

            $($txtInformPerformer).css('display', 'inline');
            $('#CaseLog_TextInternal').prop('required', true);
            
            $('#CaseLog_SendMailAboutCaseToPerformer:not(:disabled)').attr('checked', 'checked');
        }
    });

    $('#CaseLog_SendMailAboutCaseToPerformer').on('change', function (e) {
        if (e.currentTarget.checked) {
            $($txtInformPerformer).css('display', 'inline');
            $('#CaseLog_TextInternal').prop('required', true);
        }
        else {
            $($txtInformPerformer).css('display', 'none');
            $('#CaseLog_TextInternal').prop('required', false);
        }
    });

    const observerAttributes = {
        attributes: true,
        characterData: true,
        childList: true,
        subtree: true,
        attributeOldValue: true,
        characterDataOldValue: true
    }

    const observer = new MutationObserver(function (mutations_list) {

        mutations_list.forEach(function (mutation) {
            var target = mutations_list[0].target.id;
            $('#CaseLog_TextInternal').prop('required', false);

            if (target === caseLogEmailRecepientsInternalLogTo) hasCaseLogTextInternalEmailsTo = false;
            if (target === caseLogEmailRecepientsInternalLogCc) hasCaseLogTextInternalEmailsCc = false;

            mutation.addedNodes.forEach(function (added_node) {
                if (added_node.className === 'case-email-selected') {
                    if (target === caseLogEmailRecepientsInternalLogTo) hasCaseLogTextInternalEmailsTo = true;
                    
                    if (target === caseLogEmailRecepientsInternalLogCc) hasCaseLogTextInternalEmailsCc = true;                   
                    
                    //observer.disconnect();
                }
            });            
        });
        
        if (hasCaseLogTextInternalEmailsTo || hasCaseLogTextInternalEmailsCc || $caseLogSendMailAboutCaseToPerformer[0].checked) {
               
            $('#CaseLog_TextInternal').prop('required', true);
        }
    });

    observer.observe(document.querySelector('#' + caseLogEmailRecepientsInternalLogTo), observerAttributes);

    observer.observe(document.querySelector('#' + caseLogEmailRecepientsInternalLogCc), observerAttributes);

    bindDeleteLogFileBehaviorToDeleteButtons();
    bindDeleteLogFileBehaviorToDeleteButtons(true);

    $("#btnCaseCharge").on('click', function (ev) {
        window.caseChargeObj.show();
    });

    $caseLogSendMailAboutCaseToPerformer.change(function (e) {
        if (hasCaseLogTextInternalEmailsTo || hasCaseLogTextInternalEmailsCc || e.target.checked) {

            $('#CaseLog_TextInternal').prop('required', true);
        }
    });


}
