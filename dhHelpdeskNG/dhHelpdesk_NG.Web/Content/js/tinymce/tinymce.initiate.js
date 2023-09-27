
function toggleWarningVisibility(editor, maxLength) {
    var textContent = editor.getContent({ format: 'text' });
    var warningElement = document.getElementById("charCountFaq");

    if (textContent.length > maxLength || textContent.length == maxLength) {
        warningElement.style.display = "block";
    }
    else {
        warningElement.style.display = "none";
    }

}
function debounce(func, wait) {
    let timeout;
    return function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), wait);
    };
}
function enforceMaxLength(editor, maxLength) {
    var textContent = editor.getContent({ format: 'text' }); // Get plain text, not HTML
    if (textContent.length > maxLength) {
        textContent = textContent.substring(0, maxLength);
        editor.setContent(textContent);  // This sets the plain text, removing all formatting
    }
}

window.tinymceDefaultOptions = {
    initOnPageLoad: true,
    selector: "textarea.richtexteditor",
    maxLength: 2000,
    maxLengthFaq: 3000,
    showMaxLimitErrorMessage: true,
    maxLimitErrorMessage: '',

    setup: function (editor) {
        var editorId = editor.id;
        if (editorId === "faqAnswer") {
            editor.on('keydown', function (e) {

                var maxLength = (editorId !== "faqAnswer") ? this.settings.maxLength : this.settings.maxLengthFaq;
                var textContent = editor.getContent({ format: 'text' });

                // If content is at the maximum length and the pressed key isn't a control key
                if (textContent.length >= maxLength && !e.ctrlKey && !e.metaKey &&
                    e.keyCode !== 8 && e.keyCode !== 46 && e.keyCode !== 37 &&
                    e.keyCode !== 38 && e.keyCode !== 39 && e.keyCode !== 40) {
                    e.preventDefault();
                }
            });
            editor.on('paste', function (e) {
                // The paste event is asynchronous, so we wait a tick to get the pasted content
                setTimeout(function () {
                    var maxLength = (editor.id !== "faqAnswer") ? editor.settings.maxLength : editor.settings.maxLengthFaq;

                    // Enforce max length and toggle warning visibility
                    enforceMaxLength(editor, maxLength);
                    toggleWarningVisibility(editor, maxLength);
                }, 0);
            });
            editor.on('keyup change', debounce(function (e) {
                var maxLength = (editor.id !== "faqAnswer") ? this.settings.maxLength : this.settings.maxLengthFaq;
                toggleWarningVisibility(editor, maxLength);
            }, 100));
        }
        else {
            editor.on('change', function (e) {

                //save to hidden input 
                this.save();

                //call validator if any
                $("#" + editor.id).valid();

                var content = this.getContent();
                //strip all html tags
                // var regex = /(<([^>]+)>)/ig; 
                // content = content.replace(regex, "");

                var maxLength = this.settings.maxLength;

                if (content.length > maxLength) {
                    if (this.settings.showMaxLimitErrorMessage) {
                        var msg = this.settings.maxLimitErrorMessage ||
                            'The text is too long. Field has limitation of ' + this.settings.maxLength + ' characters.';
                        alert(msg);
                    }


                };
            });
        }
 
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
