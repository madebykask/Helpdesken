function applyOrderBehavior(parameters) {
    if (!parameters.id) throw new Error('id must be specified.');
    if (!parameters.deleteOrderUrl) throw new Error('deleteOrderUrl must be specified.');
    if (!parameters.uploadFileUrl) throw new Error('uploadFileUrl must be specified.');
    if (!parameters.deleteFileUrl) throw new Error('deleteFileUrl must be specified.');
    if (!parameters.deleteLogUrl) throw new Error('deleteLogUrl must be specified.');
    if (!parameters.logSubtopic) throw new Error('logSubtopic must be specified.');
    if (!parameters.fileNameSubtopic) throw new Error('fileNameSubtopic must be specified.');

    var fileUploadWhiteList = parameters.fileUploadWhiteList;

    var isFileInWhiteList = function (filename, whiteList) {
        if (filename.indexOf('.') !== -1) {
            var extension = filename.split('.').reverse()[0];
            if (whiteList.indexOf(extension) >= 0)
                return true;
        }
        else {
            if (whiteList.indexOf('') >= 0)
                return true;
        }
        return false;
    }

    $('#fileName_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { entityId: parameters.id, subtopic: parameters.fileNameSubtopic },
        max_file_size: '10mb',
        multi_selection: false,
        max_files: 1,
        multiple_queues: true,

        init: {
            FilesAdded: function (up, files) {
                if (fileUploadWhiteList != null) {
                    var whiteList = fileUploadWhiteList;

                    files.forEach(function (e) {
                        if (!isFileInWhiteList(e.name, whiteList)) {
                            up.removeFile(e);
                            alert(e.name + ' does not have a valid extension.'); // TODO: translate
                        }
                    })

                }

                if (up.files.length >= up.settings.max_files) {
                    up.splice(up.settings.max_files);
                    if (typeof up.settings.browse_button === "string") {
                        $('#' + up.settings.browse_button).hide();
                    } else {
                        $(up.settings.browse_button).hide();
                    }
                    $('#fileName_files_uploader_popup').find("input[type='file']")
                        .prop('disabled', true).parent().css("z-index", -10000);
                }
            },
            FilesRemoved: function(up, files) {
                if (up.files.length < up.settings.max_files) {
                    if (typeof up.settings.browse_button === "string") {
                        $('#' + up.settings.browse_button).show();
                    } else {
                        $(up.settings.browse_button).show();
                    }
                    $('#fileName_files_uploader_popup').find("input[type='file']")
                        .prop('disabled', false).parent().css("z-index", 0);
                }
            },
            FileUploaded: function (uploader, uploadedFile, responseContent) {
                $('#file_container').html(responseContent.response);
            }
        }
    });

    $('#log_send_to_button').button().click(function () {
        $('#log_send_to_dialog').dialog('open');
    });

    window.deleteFile = function (subtopic, fileName, filesContainerId) {
        $.post(parameters.deleteFileUrl, { entityId: parameters.id, subtopic: subtopic, fileName: fileName }, function (markup) {
            $('#' + filesContainerId).html(markup);
        });
    };

    window.deleteLog = function (subtopic, logId, logsContainerId) {
        $.post(parameters.deleteLogUrl, { orderId: parameters.id, subtopic: subtopic, logId: logId }, function (markup) {
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


(function ($) {
    $.validator.setDefaults({
        ignore: ""
    });
}(jQuery));