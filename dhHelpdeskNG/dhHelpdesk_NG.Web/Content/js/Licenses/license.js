function applyBehavior(parameters) {
    if (!parameters.id) throw new Error('id must be specified.');
    if (!parameters.uploadFileUrl) throw new Error('uploadFileUrl must be specified.');
    if (!parameters.deleteFileUrl) throw new Error('deleteFileUrl must be specified.');
    if (!parameters.licenseType) throw new Error('licenseType must be specified.');

    $('#license_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { entityId: parameters.id, type: parameters.licenseType },
        max_file_size: '10mb',

        init: {
            FileUploaded: function (uploader, uploadedFile, responseContent) {
                $('#license_files_container').html(responseContent.response);
            }
        }
    });

    window.deleteFile = function (type, fileName, filesContainerId) {
        $.post(parameters.deleteFileUrl, { entityId: parameters.id, type: type, fileName: fileName }, function (markup) {
            $('#' + filesContainerId).html(markup);
        });
    };
}