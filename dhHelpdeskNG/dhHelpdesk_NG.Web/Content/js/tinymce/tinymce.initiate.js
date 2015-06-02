'use strict';

$(function () {
    tinymce.init({
        charLimit: 1998, // this is a default value which can get modified later
        setup: function (editor) {
            editor.on('change', function (e) {
                //define local variables
                var tinymax, tinylen, htmlcount;
                //alert('ok');
                //setting our max character limit
                tinymax = 1998;
                //grabbing the length of the curent editors content
                tinylen = this.getContent().length;
                //alert(tinylen);
                if (tinylen > tinymax) {
                    alert('The event text is to long. Calendar has limitation of characters.');
                }
            });
        },

        selector: "textarea.richtexteditor",
        maxLength: 2000,
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

