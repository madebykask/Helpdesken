'use strict';

$(function () {
    tinymce.init({
        selector: "textarea.richtexteditor",
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table contextmenu paste charmap "
        ],
        menubar: false,
        //toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
        toolbar: "undo redo | removeformat | fontsizeselect bold italic | bullist numlist | link unlink | charmap | code "
    });
});