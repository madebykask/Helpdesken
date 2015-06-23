'use strict';

$(function () {
    tinymce.init({
        selector: "textarea.richtexteditor",
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks fullscreen",
            "insertdatetime media table contextmenu paste charmap "
        ],
        menubar: false,
        toolbar: "undo redo | removeformat | fontsizeselect bold italic | bullist numlist | link unlink | charmap | code"
    });
});