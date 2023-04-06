var LogModes = {
    CaseSection: 0,
    Communication: 1
};

$(function () {

    (function ($) {
        var params = window.caseLogsParameters;
        window.selfService = window.selfService || {};
        window.selfService.caseLog = window.selfService.caseLog || new CaseLog(params);
        var fileUploadWhiteList = window.appParameters.fileUploadWhiteList;
        var invalidFileExtensionText = window.appParameters.invalidFileExtensionText;
        
        var uploadLogFileUrl = params.uploadLogFileUrl;
        var logFileKey = params.logFileKey;
        var caseId = params.caseId || 0;
        var fileAlreadyExistsMsg = params.fileAlreadyExistsMsg;
        
        var alreadyExistFileIds = [];

        var isFileInWhiteList = function (filename, whiteList) {
            if (filename.indexOf('.') !== -1) {
                var extension = filename.split('.').reverse()[0].toLowerCase();
                if (whiteList.indexOf(extension) >= 0)
                    return true;
            }
            else {
                if (whiteList.indexOf('') >= 0)
                    return true;
            }
            return false;
        }

        PluploadTranslation($("#caseLog_languageId").val());

        var $fileUploader = $('#Log_upload_files_popup').find('#file_uploader');

        $fileUploader.pluploadQueue({
            runtimes: 'html5,html4',
            url: uploadLogFileUrl,
            // request params
            multipart_params: {
                Id: logFileKey,
                caseId: caseId,
                myTime: Date.now()
            },
            max_file_size: '10mb',

            init: {
                FileUploaded: function () {
                    window.selfService.caseLog.reloadLogFiles();
                },

                UploadComplete: function (up, file) {
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");
                    up.refresh();
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

                Error: function (uploader, e) {
                    if (e.status != 409) {
                        console.error(e);
                        return;
                    }
                    alreadyExistFileIds.push(e.file.id);
                },

                StateChanged: function (uploader) {
                    if (uploader.state != plupload.STOPPED) {
                        return;
                    }

                    for (var i = 0; i < alreadyExistFileIds.length; i++) {
                        var fileId = alreadyExistFileIds[i];
                        $fileUploader.find('ul[class="plupload_filelist"] li[id="' + fileId + '"] div[class="plupload_file_action"] a').prop('title', fileAlreadyExistsMsg);
                    }

                    alreadyExistFileIds = [];
                    uploader.refresh();
                }
            }
        });

        $('#Log_upload_files_popup').on('shown.bs.modal', function () {
            //refresh required to make file open dlg work correctly 
            var $fileUploader = $('#Log_upload_files_popup').find('#file_uploader');
            $fileUploader.pluploadQueue().refresh();
        });

        $('#Log_upload_files_popup').on('hidden.bs.modal', function () {
            var $fileUploader = $('#Log_upload_files_popup').find('#file_uploader');
            if ($fileUploader && $fileUploader.length) {
                if ($fileUploader.pluploadQueue().files.length > 0) {
                    if ($fileUploader.pluploadQueue().state == plupload.UPLOADING)
                        $fileUploader.pluploadQueue().stop();

                    while ($fileUploader.pluploadQueue().files.length > 0) {
                        $fileUploader.pluploadQueue().removeFile($fileUploader.pluploadQueue().files[0]);
                    }
                    $fileUploader.pluploadQueue().refresh();
                }
            }
        });

        /*********** Paste image from clipboard (Log) ************/
        $("#addLogFileFromClipboard").on('click', function (e) {
            e.preventDefault();
            var opt = {
                refreshCallback: function (res) {
                    selfService.caseLog.reloadLogFiles();
                },
                fileKey: logFileKey,
                submitUrl: uploadLogFileUrl,
                uploadParams: {
                    caseId: caseId
                }
            };
            var clipboardFileUpload = new ClipboardFileUpload(opt);
            clipboardFileUpload.show();
            return false;
        });
        /*****************************************************/

        function CaseLog(params) {

            var logFileKey = params.logFileKey;
            var getLogFilesUrl = params.getLogFilesUrl;
            var deleteLogFileUrl = params.deleteLogFileUrl;
            var downloadLogFileUrl = params.downloadLogFileUrl;
            var downloadLogFileParamUrl = params.downloadLogFileParamUrl;
            var saveLogMessageUrl = params.saveLogMessageUrl;
            var caseId = params.caseId;
            var caseDetailsUrl = params.caseDetailsUrl;
            var logMandatoryText = params.logMandatoryText;
            var currentLogMode = params.logNotesMode || 0;
            var isCaseFinished = params.isFinished;
            var caseEmailGuid = params.caseEmailGuid;

            //private 
            function refreshLogFilesTable($table, files) {
                $table.empty();

                var filesMarkup = buildFilesMarkup(files) || '';
                var markup = '<tbody>' + filesMarkup + '</tbody>';
                $(markup).appendTo($table);

                bindDeleteLogFileToDeleteButtons($table);
            }

            //public
            this.init = function () {
                //console.log('>>> Current mode: ' + (currentLogMode || 'undefined'));
                this._elements = getLogNoteElementsForMode(currentLogMode);
                changeState.call(this, false);
            };

            //public
            this.reloadLogFiles = function () {
                var self = this;
                var data = {
                    id: logFileKey,
                    myTime: Date.now(),
                    caseId: caseId
                };
                $.get(getLogFilesUrl, $.param(data), function (files) {
                    refreshLogFilesTable(self._elements.logFilesTable, files);
                });
            };

            //public
            this.saveLogMessage = function (src) {
                var self = this;
                var isPopup = src && src === "popup";
                //console.log('>>> IsPopup: ' + (isPopup ? 'true' : 'false'));

                $("#popupError").css("display", "none");

                var note = self._elements.logNoteInput.val() || '';
                var templateId = self._elements.stepsInput.val() || 0;
                if (note === '') {
                    if (isPopup) {
                        $("#popupError").text(logMandatoryText);
                        $("#popupError").css("display", "block");
                    }
                    else {
                        ShowToastMessage(logMandatoryText, "warning", false);
                    }
                } else {
                    changeState.call(self, true);
                    $.post(saveLogMessageUrl, { caseId: caseId, note: note, logFileGuid: logFileKey, templateId: templateId },
                        function(res) {
                            if (isPopup) {
                                if (caseEmailGuid != null && caseEmailGuid !== '') {
                                    window.location.href = caseDetailsUrl.indexOf(caseEmailGuid) >= 0 ? caseDetailsUrl : (caseDetailsUrl + '/' + caseEmailGuid);
                                }
                                else
                                    window.location.href = caseDetailsUrl.indexOf(caseId) >= 0 ? caseDetailsUrl : (caseDetailsUrl + '/' + caseId);
                           }
                            else {
                                self._elements.logNotesDiv.html(res.replace(/\r\n|\r|\n/g, "<br />"));
                                self._elements.logNoteInput.val('');
                                self.reloadLogFiles();
                                changeState.call(self, false);
                                window.parent.location.reload(true);
                            }
                        }).fail(function(e) {
                            console.error(e);
                            ShowToastMessage('Unknown error.', 'error', false);
                            changeState.call(self, false);
                        });
                }
            }            

            //private
            function getLogNoteElementsForMode(mode) {
                // if isCaseFinished then use popup elements
                var $parent =
                    isCaseFinished
                        ? $("#logNotesSectionPopup")
                        : Number(mode) === LogModes.Communication
                            ? $('#communicationPanel')
                            : $('#logNotesSection');

                //note: _Communication and _CaseReceipt partial views has same ids for controls below. Its safe since both views cannot be used together.              
                var expand = $("#btnOpenLog").hasClass("expand");
                if (expand) {                    
                    $(".mapButtonclose").trigger("click");                    
                }

                return {
                    logFilesTable: $('#LogFile_table', $parent),    
                    logNoteInput: $('#logNote', $parent),
                    stepsInput: $('#steps', $parent),
                    logNotesDiv: $('#CaseLogPartial', $parent),
                    sendButton: $('#btnSendLog', $parent),
                    sendInidicator: $('#sendLogIndicator', $parent),
                    btnLoadFromClipboard: $('#addLogFileFromClipboard', $parent),
                    btnUploadFile: $('#btnUploadFile', $parent)
                };
            };

            //private
            function buildFilesMarkup(files) {
                var fileMarkup = '';
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    var urlQsParams = (downloadLogFileParamUrl || '').length ? downloadLogFileParamUrl + '&fileName=' + encodeURIComponent(file) : 'fileName=' + encodeURIComponent(file);
                    var row =
                        '<tr>' +
                            '   <td><i class="glyphicon glyphicon-file">&nbsp;</i><a style="color:blue" href="' + downloadLogFileUrl + '?' + urlQsParams + '">' + file + '</a></td>' +
                            '   <td class="del"><a id="delete_file_button_' + i + '" class="btn btn-default btn-xs"><span class="glyphicon glyphicon-remove"></span></a></td>' +
                        '</tr>';
                    fileMarkup += row;
                }

                return fileMarkup;
            }

            //private
            function bindDeleteLogFileToDeleteButtons($table) {
                //onclick:
                $table.find('a[id^="delete_file_button_"]').click(function () {
                    var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
                    var pressedDeleteFileButton = this;

                    //send request
                    $.post(deleteLogFileUrl,
                        {
                            id: logFileKey,
                            fileName: fileName,
                            myTime: Date.now(),
                            caseId: caseId
                        },
                        function () {
                            $(pressedDeleteFileButton).parents('tr:first').remove();
                        });
                });
            }

            //private
            function changeState(locked) {
                var buttonsToLock = $([this._elements.sendButton, this._elements.btnLoadFromClipboard, this._elements.btnUploadFile]);

                if (locked) {
                    buttonsToLock.disable(true);
                    buttonsToLock.css("pointer-events", "none");
                    this._elements.sendInidicator.css("display", "inline-block");
                } else {
                    buttonsToLock.disable(false);
                    buttonsToLock.css("pointer-events", "");
                    this._elements.sendInidicator.css("display", "none");
                }
            }
        }; //CaseLog
        
    })($);

    //UI handlers
    $("#imgFilename").on('change', function () {
        if ($("#imgFilename").val() == "")
            $('#imageNameRequired').show();
        else
            $('#imageNameRequired').hide();
    });

    $('#btnSendLog').click(function (e) {
        e.preventDefault();
        var src = $(this).data('src');
        selfService.caseLog.saveLogMessage(src);
    });
    $('#btnSendLog2').click(function (e) {
        e.preventDefault();
        var src = $(this).data('src');
        selfService.caseLog.saveLogMessage(src);
    });

    $("a[href='#upload_clipboard_file_popup_2']").on('click', function (e) {
        e.preventDefault();
        $('#upload_clipboard_file_popup_2').modal('show');
    });

    $('#btnReOpen').click(function (e) {
        e.preventDefault();
        $("#logNote").val("");
        $("#popupError").css("display", "none");
        $('#logNotePopup').modal('show');

    });

    selfService.caseLog.init();
});