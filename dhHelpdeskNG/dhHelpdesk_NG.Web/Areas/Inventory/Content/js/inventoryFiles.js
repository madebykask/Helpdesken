"use strict";

var InventoryFiles = function ($) {

    //private
    var id = 0;
    var currentLanguageId = 1;
    var fileDownloadUrlMask = '';
    var $fileUploaderEl = null;
    var pluploadInst = null;
    var $fileUploaderPopup = $('#upload_files_popup');
    var $attachedFileEl = $('#attached_file');
    var $btnDelete = $('#btnDeleteFile');
    var $confirmDialog = $('#confirm-delete');
    var attachedFileName = '';
    var fileKey = '';

    //public: INIT
    this.init = function (options) {
        //set options
        id = options.id;
        currentLanguageId = options.currentLanguageId;
        attachedFileName = options.attachedFileName;
        fileKey = options.documentFileKey;
        fileDownloadUrlMask = options.fileDownloadUrlMask;

        //init file upload
        initFileUpload(options.id, options.uploadDocumentFileUrl, options.documentFileKey);
        PluploadTranslation(+options.CurrentLanguageId);

        // SUBSCRIBE EVENTS
        $btnDelete.on('click', function (e) {
            e.preventDefault();
            showConfirmDialog($confirmDialog, function () {
                var fileId = id > 0 ? id.toString() : fileKey;
                deleteFile(fileId, attachedFileName, options.fileDeleteUrl);
            }, function () {});
        });

        //stop uploading on hide
        $fileUploaderPopup.on('hide', function () {
            resetUploader(pluploadInst);
        });
        
    };

    //private
    function showConfirmDialog(d, onOk, onCancel) {
        d.on("show", function () {
            d.find(".btn-cancel").on("click", function (e) {
                onCancel();
                d.modal('hide');
            });

            d.find(".btn-ok").on("click", function (e) {
                onOk();
                d.modal('hide');
            });
        });

        d.on("hide", function () {
            d.find(".btn-ok").off("click");
            d.find(".btn-cancel").off("click");
        });

        d.on("hidden", function () { d.remove(); });

        d.modal({
            "backdrop": "static",
            "keyboard": true,
            "show": true
        });
    }

    //private
    function initFileUpload(entityId, uploadDocumentFileUrl, documentFileKey) {

        $fileUploaderEl = $('#file_uploader').pluploadQueue({
            runtimes: 'html5,html4',
            url: uploadDocumentFileUrl,
            multipart_params: { id: entityId > 0 ? entityId : documentFileKey, myTime: Date.now() },
            max_file_size: '10mb',
            multi_selection: false,
            max_files: 1,
            multiple_queues: true,

            //uploader event handlers
            init: {

                FilesAdded: function (up, files) {
                    // hide browse button
                    if (up.files.length >= up.settings.max_files) {
                        $(up.settings.browse_button).hide();

                        //hide file input to allow clicking on start upload button
                        $fileUploaderPopup.find("input[type='file']")
                            .prop('disabled', true).parent().css("z-index", -10000);
                    }

                    //remove other files if exceeed max files count
                    if (up.files.length > up.settings.max_files) {
                        up.splice(up.files.length - up.settings.max_files);
                    }
                },

                FilesRemoved: function (up, files) {
                    // show browse button back if files less then max allowed
                    if (up.files.length < up.settings.max_files) {
                        $(up.settings.browse_button).show();
                        $fileUploaderPopup.find("input[type='file']")
                            .prop('disabled', false).parent().css("z-index", 0);
                    }
                },

                FileUploaded: function (uploader, uploadedFile, responseContent) {
                    $attachedFileEl.empty();
                    var data = JSON.parse(responseContent.response);
                    if (data && data.success) {
                        //add file link
                        attachedFileName = uploadedFile.name;
                        var fileIdKey = entityId > 0 ? entityId.toString() : documentFileKey;
                        $('<a href="' + fileDownloadUrlMask + '?id=' + fileIdKey + '&name=' + encodeURIComponent(attachedFileName) + '">' +
                            '<i class="icon-file"></i>' + attachedFileName + '</a>').appendTo($attachedFileEl);

                        // show delete attache file btn
                        $btnDelete.show();

                        //show browse button back and close uploader
                        $fileUploaderPopup.modal('hide');
                    }
                },

                UploadComplete: function (up, file) {
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");
                },

                Error: function (uploader, e) {
                    if (e.status !== 409) {
                        console.error(e);
                        return;
                    }
                }

            }
        });

        //get uploader instance
        pluploadInst = $fileUploaderEl.pluploadQueue();
    }

    //private
    function resetUploader(uploader) {
        if (uploader.files.length > 0) {
            if (uploader.state === plupload.UPLOADING) {
                uploader.stop();
            }
            for (var i = 0; i < uploader.files.length; i++) {
                uploader.removeFile(uploader.files[i]);
            }
        }
        uploader.splice();
        uploader.refresh();
    }

    //private
    function deleteFile(entityId, fileName, fileDeleteUrl) {
        var data = {
            id: entityId,
            name: encodeURIComponent(fileName)
        };

        $.post(fileDeleteUrl, $.param(data), function (res) {
            if (res.success) {
                $attachedFileEl.empty();
                $btnDelete.hide();
            }
        });
    }
};