function applyChangeBehavior(parameters) {
    if (!parameters.id) throw new Error('id must be specified.');
    if (!parameters.deleteChangeUrl) throw new Error('deleteChangeUrl must be specified.');
    if (!parameters.uploadFileUrl) throw new Error('uploadFileUrl must be specified.');
    if (!parameters.deleteFileUrl) throw new Error('deleteFileUrl must be specified.');
    if (!parameters.deleteLogUrl) throw new Error('deleteLogUrl must be specified.');
    if (!parameters.registrationSubtopic) throw new Error('registrationSubtopic must be specified.');
    if (!parameters.analyzeSubtopic) throw new Error('analyzeSubtopic must be specified.');
    if (!parameters.implementationSubtopic) throw new Error('implementationSubtopic must be specified.');
    if (!parameters.evaluationSubtopic) throw new Error('evaluationSubtopic must be specified.');
    if (!parameters.registrationRejectValue) throw new Error('registrationRejectValue must be specified.');
    if (!parameters.analyzeRejectValue) throw new Error('analyzeRejectValue must be specified.');
    var fileUploadWhiteList = parameters.fileUploadWhiteList;
    var invalidFileExtensionText = parameters.invalidFileExtensionText;

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
    };

    $('#general_inventory_dialog_open_button').click(function() {
        $('#general_inventory_dialog').dialog('open');
    });

    $('#registration_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.registrationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#registration_files_container').html(responseContent.response);
            },
            FilesAdded: function (up, files) {
                if (fileUploadWhiteList != null) {
                    var whiteList = fileUploadWhiteList;

                    files.forEach(function (e) {
                        if (!isFileInWhiteList(e.name, whiteList)) {
                            up.removeFile(e);
                            alert(e.name + ' ' + invalidFileExtensionText);
                        }
                    })

                }
            },
        }
    });

    $('#analyze_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.analyzeSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#analyze_files_container').html(responseContent.response);
            },
            FilesAdded: function (up, files) {
                if (fileUploadWhiteList != null) {
                    var whiteList = fileUploadWhiteList;

                    files.forEach(function (e) {
                        if (!isFileInWhiteList(e.name, whiteList)) {
                            up.removeFile(e);
                            alert(e.name + ' ' + invalidFileExtensionText);
                        }
                    })

                }
            },
        }
    });

    $('#implementation_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.implementationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#implementation_files_container').html(responseContent.response);
            },
            FilesAdded: function (up, files) {
                if (fileUploadWhiteList != null) {
                    var whiteList = fileUploadWhiteList;

                    files.forEach(function (e) {
                        if (!isFileInWhiteList(e.name, whiteList)) {
                            up.removeFile(e);
                            alert(e.name + ' ' + invalidFileExtensionText);
                        }
                    })

                }
            },
        }
    });

    $('#evaluation_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { changeId: parameters.id, subtopic: parameters.evaluationSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#evaluation_files_container').html(responseContent.response);
            },
            FilesAdded: function (up, files) {
                if (fileUploadWhiteList != null) {
                    var whiteList = fileUploadWhiteList;

                    files.forEach(function (e) {
                        if (!isFileInWhiteList(e.name, whiteList)) {
                            up.removeFile(e);
                            alert(e.name + ' ' + invalidFileExtensionText);
                        }
                    })

                }
            },
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

    window.fillEmailsTextArea = function(textAreaId, emails) {
        var emailsText = '';

        for (var i = 0; i < emails.length; i++) {
            if (i != 0) {
                emailsText += '\n';
            }

            emailsText += emails[i];
        }

        $('#' + textAreaId).val(emailsText);
    };

    window.fillLogSendLogNoteToEmailsTextArea = function(emails) {
        this.fillEmailsTextArea('log_send_to_emails_textarea', emails);
    };

    window.fillAnalyzeSendLogNoteToEmailsTextArea = function(emails) {
        this.fillEmailsTextArea('analyze_send_to_emails_textarea', emails);
    };

    window.fillAnalyzeInviteToCabEmailsTextArea = function(emails) {
        this.fillEmailsTextArea('analyze_invite_to_cab_emails_textarea', emails);
    };

    window.fillImplementationSendLogNoteToEmailsTextArea = function(emails) {
        this.fillEmailsTextArea('implementation_send_to_emails_textarea', emails);
    };

    window.fillEvaluationSendLogNoteToEmailsTextArea = function(emails) {
        this.fillEmailsTextArea('evaluation_send_to_emails_textarea', emails);
    };

    function getChangeComputerUserSearchOptions() {
        
        var options = {
            items: 20,
            minLength: 2,

            source: function (query, process) {
                return $.ajax({
                    url: '/cases/search_user',
                    type: 'post',
                    data: { query: query, customerId: $('#change_customerId').val() },
                    dataType: 'json',
                    success: function (result) {
                        
                        var resultList = jQuery.map(result, function (item) {
                            var aItem = {
                                id: item.Id
                                        , num: item.UserId
                                        , name: item.SurName + ' ' + item.FirstName
                                        , email: item.Email
                                        , place: item.Location
                                        , phone: item.Phone
                                        , usercode: item.UserCode
                                        , cellphone: item.CellPhone
                                        , regionid: item.Region_Id
                                        , departmentid: item.Department_Id
                                        , ouid: item.OU_Id
                            };
                            return JSON.stringify(aItem);
                        });

                        return process(resultList);
                    }
                });
            },

            matcher: function (obj) {
                var item = JSON.parse(obj);
                return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
                    || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                    || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                    || ~item.email.toLowerCase().indexOf(this.query.toLowerCase());
            },

            sorter: function (items) {
                var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                while (aItem = items.shift()) {
                    var item = JSON.parse(aItem);
                    if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                    else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                    else caseInsensitive.push(JSON.stringify(item));
                }

                return beginswith.concat(caseSensitive, caseInsensitive);
            },

            highlighter: function (obj) {
                var item = JSON.parse(obj);
                var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email;

                return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                    return '<strong>' + match + '</strong>';
                });
            },

            updater: function (obj) {
                var item = JSON.parse(obj);
                $('#change_orderer_id').val(item.num);
                $('#change_orderer_name').val(item.name);
                $('#change_orderer_email').val(item.email);
                $('#change_orderer_phone').val(item.phone);
                $('#change_orderer_cellphone').val(item.cellphone);
                $('#change_orderer_department').val(item.departmentid);
                return item.num;
            }
        };

        return options;
    }

    $('#change_orderer_id').typeahead(getChangeComputerUserSearchOptions());
}

(function ($) {
    $.validator.setDefaults({
        ignore: ""
    });
}(jQuery));