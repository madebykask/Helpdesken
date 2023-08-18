'use strict';

// more config at https://www.tiny.cloud/docs-4x/general-configuration-guide/basic-setup/#toolbarmenuconfiguration
window.tinymceDefaultOptions = {
    initOnPageLoad: true,
    selector: "textarea.richtexteditor",
    maxLength: 2000,
    maxLengthFaq: 4000,
    showMaxLimitErrorMessage: true, //custom
    maxLimitErrorMessage: '', //custom

    setup: function (editor) {

        // on text change handler
        editor.on('change', function (e) {
            //save to hidden input 
            this.save();
            //call validator if any
            $("#" + editor.id).valid();
            var editorId = editor.id;
            var content = this.getContent();
            //strip all html tags
            // var regex = /(<([^>]+)>)/ig; 
            // content = content.replace(regex, "");
            var maxLength = this.settings.maxLength;
            var maxLengthFaq = this.settings.maxLengthFaq;
            if (editorId != "faqAnswer") {
                if (content.length > maxLength) {
                    if (this.settings.showMaxLimitErrorMessage) {
                        var msg = this.settings.maxLimitErrorMessage ||
                            'The text is too long. Field has limitation of ' + this.settings.maxLength + ' characters.';
                        alert(msg);
                    }
                }
            }
            else {
                if (content.length > maxLengthFaq) {
                    if (this.settings.showMaxLimitErrorMessage) {
                        var msg = this.settings.maxLimitErrorMessage ||
                            'The text is too long. Field has limitation of ' + this.settings.maxLengthFaq + ' characters.';
                        alert(msg);
                    }
                }
            }
            
        });
    },
        
    plugins: [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks fullscreen textcolor",
        "insertdatetime media table contextmenu paste charmap "
    ],
    menubar: false,
    toolbar: "undo redo | removeformat | fontsizeselect forecolor bold italic | bullist numlist | link unlink | charmap "
};

$(function () {
    if (window.tinymceDefaultOptions && window.tinymceDefaultOptions.initOnPageLoad === true) {
        tinymce.init(window.tinymceDefaultOptions);
    }
});

