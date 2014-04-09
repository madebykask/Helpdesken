$(function () {
    dhHelpdesk.change = function(parameters) {
        return new Change(parameters);
    };
});

function fillAnalyzeInviteToCabTextArea(sendToEmails) {
    var emailsText = '';

    for (var i = 0; i < sendToEmails.length; i++) {
        if (i != 0) {
            emailsText += '\n';
        }

        emailsText += sendToEmails[i];
    }

    $('#analyze_invite_to_cab_textarea').val(emailsText);
}

function fillAnalyzeSendToEmailsTextArea(sendToEmails) {
    var emailsText = '';

    for (var i = 0; i < sendToEmails.length; i++) {
        if (i != 0) {
            emailsText += '\n';
        }

        emailsText += sendToEmails[i];
    }

    $('#analyze_send_to_emails_textarea').val(emailsText);
}

function fillImplementationSendToEmailsTextArea(sendToEmails) {
    var emailsText = '';

    for (var i = 0; i < sendToEmails.length; i++) {
        if (i != 0) {
            emailsText += '\n';
        }

        emailsText += sendToEmails[i];
    }

    $('#implementation_send_to_emails_textarea').val(emailsText);
}

function fillEvaluationSendToEmailsTextArea(sendToEmails) {
    var emailsText = '';

    for (var i = 0; i < sendToEmails.length; i++) {
        if (i != 0) {
            emailsText += '\n';
        }

        emailsText += sendToEmails[i];
    }

    $('#evaluation_send_to_emails_textarea').val(emailsText);
}

function fillLogSendToEmailsTextArea(sendToEmails) {
    var emailsText = '';

    for (var i = 0; i < sendToEmails.length; i++) {
        if (i != 0) {
            emailsText += '\n';
        }

        emailsText += sendToEmails[i];
    }

    $('#log_send_to_emails_textarea').val(emailsText);
}

function Change(parameters) {
    this.parameters = parameters;
    var self = this;

    $('#save_and_close_new_change_button').click(function() {
        $('#new_change_form').submit();
        window.location.href = self.parameters.indexUrl;
    });

    $('#save_and_close_button').click(function() {
        $('#change_form').submit();
        window.location.href = self.parameters.indexUrl;
    });

    $('#delete_button').click(function() {
        $.post(self.parameters.deleteChangeUrl, { id: self.parameters.id });
        window.location.href = self.parameters.indexUrl;
    });

    $('#registration_files_uploader').pluploadQueue({
        url: self.parameters.uploadFileUrl,
        multipart_params: { changeId: self.parameters.id, subtopic: self.parameters.registrationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#registration_files_container').html(responseContent.response);
            }
        }
    });

    $('#analyze_files_uploader').pluploadQueue({
        url: self.parameters.uploadFileUrl,
        multipart_params: { changeId: self.parameters.id, subtopic: self.parameters.analyzeSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#analyze_files_container').html(responseContent.response);
            }
        }
    });

    $('#implementation_files_uploader').pluploadQueue({
        url: self.parameters.uploadFileUrl,
        multipart_params: { changeId: self.parameters.id, subtopic: self.parameters.implementationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#implementation_files_container').html(responseContent.response);
            }
        }
    });

    $('#evaluation_files_uploader').pluploadQueue({
        url: self.parameters.uploadFileUrl,
        multipart_params: { changeId: self.parameters.id, subtopic: self.parameters.evaluationSubtopic },
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

    $('#evaluation_send_to_button').button().click(function() {
        $('#evaluation_send_to_dialog').dialog('open');
    });

    $('#implementation_send_to_button').button().click(function() {
        $('#implementation_send_to_dialog').dialog('open');
    });

    $('#log_send_to_button').button().click(function () {
        $('#log_send_to_dialog').dialog('open');
    });

    $('#analyze_invite_to_cab_button').button().click(function() {
        $('#analyze_invite_to_cab_dialog').dialog('open');
    });

    $('#registration_approval_dropdown').change(function() {
        var selectedValue = $(this).val();
        if (selectedValue == self.parameters.registrationRejectValue) {
            $('#registration_reject_explanation_textarea').show();
        } else {
            $('#registration_reject_explanation_textarea').hide();
        }
    });

    $('#analyze_approval_dropdown').change(function() {
        var selectedValue = $(this).val();
        if (selectedValue == self.parameters.analyzeRejectValue) {
            $('#analyze_reject_explanation_textarea').show();
        } else {
            $('#analyze_reject_explanation_textarea').hide();
        }
    });
}

Change.prototype.deleteFile = function(subtopic, fileName, filesContainerId) {
    $.post(this.parameters.deleteFileUrl, { changeId: this.parameters.id, subtopic: subtopic, fileName: fileName }, function(markup) {
        $('#' + filesContainerId).html(markup);
    });
};

Change.prototype.deleteLog = function(subtopic, logId, logsContainerId) {
    $.post(this.parameters.deleteLogUrl, { changeId: this.parameters.id, subtopic: subtopic, logId: logId }, function(markup) {
        $('#' + logsContainerId).html(markup);
    });
};