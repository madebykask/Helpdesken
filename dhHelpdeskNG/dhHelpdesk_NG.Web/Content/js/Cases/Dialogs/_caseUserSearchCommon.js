function onRemoveKeyDown(e, fakeInput, mainInput) {
    e.stopImmediatePropagation();
    var text = mainInput.val();
    var email = getEmailsToRemove();
    if (email === "&nbsp;" || email.trim() === "")
        e.preventDefault();
    else {
        if (email !== "" && email.indexOf(";") >= 0) {
            e.preventDefault();
            text = text.replace(email, "");
            mainInput.val(text);
            fakeInput.html(getHtmlFromEmails(mainInput.val()));
            placeCaretAtEnd(fakeInput);
        }
    }
}

function extractor(query) {
    var result = /([^;]+)$/.exec(query);
    if (result && result[1])
        return result[1].trim();
    return '';
}

function generateRandomKey() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    return s4() + '-' + s4() + '-' + s4();
}

function ShowToastModalMessage(msg, msgType) {
    ShowToastMessage(msg, msgType);
    $(".toast-container").addClass("case-followers-toastmessage");
}

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress.trim());
};

function getEmailsToRemove() {
    var selection = window.getSelection();
    if (selection.type === "Range") {
        var range = selection.getRangeAt(0);
        var allWithinRangeParent = range.commonAncestorContainer.getElementsByTagName("*");
        var allSelected = [];
        for (var i = 0, el; el = allWithinRangeParent[i]; i++) {
            if (selection.containsNode(el, true)) {
                allSelected.push(el.textContent);
            }
        }
        return allSelected.join("");
    } else {
        if ($(selection.anchorNode.parentNode).html() === "&nbsp;") {
            return "&nbsp;";
        } else {
            return selection.anchorNode.textContent;
        }
    }
}

function getHtmlFromEmails(emails) {
    var aaa = emails.split(";");
    var result = [];
    for (var i = 0; i < aaa.length -1; i++) {
        result.push("<span class='case-email-selected'>" + aaa[i] + ";</span>");
    }
    if (result.length > 0)result.push("<span>&nbsp</span>");
    return result.join("");
}

function getEmailsFromHtml(html) {
    var result = [];
    var arr = html.replace(/<[^>]*>/g, "").split(";");
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] !== "")
            result.push(arr[i].trim());
    }
    return  result;
}

function placeCaretAtEnd(el) {
    el.focus();
    if (typeof window.getSelection != "undefined"
            && typeof document.createRange != "undefined") {
        var range = document.createRange();
        range.selectNodeContents(el[0]);
        range.collapse(false);
        var sel = window.getSelection();
        sel.removeAllRanges();
        sel.addRange(range);
    } else if (typeof document.body.createTextRange != "undefined") {
        var textRange = document.body.createTextRange();
        textRange.moveToElementText(el[0]);
        textRange.collapse(false);
        textRange.select();
    }
}

function initEditableDiv() {
    var original = $.fn.val;
    $.fn.val = function () {
        if ($(this).is('*[contenteditable=true]')) {
            return $.fn.html.apply(this, arguments);
        };
        return original.apply(this, arguments);
    };
}
