function applyPageBehavior(parameters) {
    $('#save_and_close_button').click(function() {
        $('#change_form').submit();
        window.location.href = parameters.indexUrl;
    });

    $('#delete_button').click(function() {
        $.post(parameters.deleteChangeUrl, { id: parameters.id });
    });

    $('#registration_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.registrationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#registration_files_container').html(responseContent.response);
            }
        }
    });

    $('#analyze_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.analyzeSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#analyze_files_container').html(responseContent.response);
            }
        }
    });

    $('#implementation_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.implementationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#implementation_files_container').html(responseContent.response);
            }
        }
    });

    $('#evaluation_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.evaluationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#evaluation_files_container').html(responseContent.response);
            }
        }
    });

    $('#analyze_send_to_button').button().click(function() {
        $('#analyze_send_to_dialog').dialog('open');
    });

    $('#analyze_invite_to_cab_button').button().click(function() {
        $('#analyze_invite_to_cab_dialog').dialog('open');
    });

    $('#implementation_send_to_button').button().click(function() {
        $('#implementation_send_to_dialog').dialog('open');
    });

    $('#evaluation_send_to_button').button().click(function() {
        $('#evaluation_send_to_dialog').dialog('open');
    });

    $('#log_send_to_button').button().click(function() {
        $('#log_send_to_dialog').dialog('open');
    });

    $('#registration_approval_dropdown').change(function() {
        var selectedValue = $(this).val();
        if (selectedValue == parameters.registrationRejectValue) {
            $('#registration_reject_explanation_textarea').show();
        } else {
            $('#registration_reject_explanation_textarea').hide();
        }
    });

    $('#analyze_approval_dropdown').change(function() {
        var selectedValue = $(this).val();
        if (selectedValue == parameters.analyzeRejectValue) {
            $('#analyze_reject_explanation_textarea').show();
        } else {
            $('#analyze_reject_explanation_textarea').hide();
        }
    });

    window.deleteFile = function(subtopic, fileName, filesContainerId) {
        $.post(parameters.deleteFileUrl, { changeId: parameters.id, subtopic: subtopic, fileName: fileName }, function(markup) {
            $('#' + filesContainerId).html(markup);
        });
    };

    window.deleteLog = function(subtopic, logId, logsContainerId) {
        $.post(parameters.deleteLogUrl, { changeId: parameters.id, subtopic: subtopic, logId: logId }, function(markup) {
            $('#' + logsContainerId).html(markup);
        });
    };

    window.fillEmailsTextArea = function (textAreaId, emails) {
        var emailsText = '';

        for (var i = 0; i < emails.length; i++) {
            if (i != 0) {
                emailsText += '\n';
            }

            emailsText += emails[i];
        }

        $('#' + textAreaId).val(emailsText);
    };

    window.fillLogSendLogNoteToEmailsTextArea = function (emails) {
        this.fillEmailsTextArea('log_send_to_emails_textarea', emails);
    };
}