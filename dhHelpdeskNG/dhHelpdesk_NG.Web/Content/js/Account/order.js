function applyOrderAccountBehavior(parameters) {
    if (!parameters.id) throw new Error('id must be specified.');
    if (!parameters.uploadFileUrl) throw new Error('uploadFileUrl must be specified.');
    if (!parameters.deleteFileUrl) throw new Error('deleteFileUrl must be specified.');

    $('#fileName_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { orderId: parameters.id },
        max_file_size: '10mb',

        init: {
            FileUploaded: function(uploader, uploadedFile, responseContent) {
                $('#filename_files_container').html(responseContent.response);
            }
        }
    });

    window.deleteFile = function(fileName, filesContainerId) {
        $.post(parameters.deleteFileUrl, { orderId: parameters.id, fileName: fileName }, function (markup) {
            $('#' + filesContainerId).html(markup);
        });
    };
}