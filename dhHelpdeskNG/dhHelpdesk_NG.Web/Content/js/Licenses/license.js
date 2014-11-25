function applyBehavior(parameters) {
    if (!parameters.id) throw new Error('id must be specified.');
    if (!parameters.uploadFileUrl) throw new Error('uploadFileUrl must be specified.');
    if (!parameters.deleteFileUrl) throw new Error('deleteFileUrl must be specified.');
    
    $('#license_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { entityId: parameters.id },
        max_file_size: '10mb',

        init: {
            FileUploaded: function (uploader, uploadedFile, responseContent) {
                $('#license_files_container').html(responseContent.response);
            }
        }
    });

    window.deleteFile = function (fileName, filesContainerId) {
        $.post(parameters.deleteFileUrl, { entityId: parameters.id, fileName: fileName }, function (markup) {
            $('#' + filesContainerId).html(markup);
        });
    };
}

$(function() {
    $(".license-product").focus();
});