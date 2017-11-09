function applyBehavior(parameters) {
    if (!parameters.id) throw new Error('id must be specified.');
    if (!parameters.uploadFileUrl) throw new Error('uploadFileUrl must be specified.');
    if (!parameters.deleteFileUrl) throw new Error('deleteFileUrl must be specified.');

    PluploadTranslation(parameters.languageId);

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

function PluploadTranslation(languageId) {
    if (languageId == 1) {
        plupload.addI18n({
            'Select files': 'Välj filer',
            'Add files to the upload queue and click the start button.': 'Lägg till filer till kön och tryck på lägg till.',
            'Filename': 'Filnamn',
            'Status': 'Status',
            'Size': 'Storlek',
            'Add files': 'Lägg till filer',
            'Add files.': 'nnnnn',
            'Start upload': 'ssss',
            'Stop current upload': 'Stoppa uppladdningen',
            'Start uploading queue': 'Starta uppladdningen',
            'Drag files here.': 'Dra filer hit'
        });
    }

    if (languageId == 2) {
        plupload.addI18n({
            'Select files': 'Select files',
            'Add files to the upload queue and click the start button.': 'Add files to the upload queue and click the start button.',
            'Filename': 'Filename',
            'Status': 'Status',
            'Size': 'Size',
            'Add files': 'Add files',
            'Stop current upload': 'Stop current upload',
            'Start uploading queue': 'Start uploading queue',
            'Drag files here.': 'Drag files here.'
        });
    }

}
$(function() {
    $(".license-product").focus();
});