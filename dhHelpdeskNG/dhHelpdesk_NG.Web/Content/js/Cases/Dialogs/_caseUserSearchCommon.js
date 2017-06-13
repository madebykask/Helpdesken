function onRemoveKeyDown(e, fakeInput, mainInput) {
    e.stopImmediatePropagation();
    var text = mainInput.val();
    var email = getEmailsToRemove();
    if (email === "&nbsp;" || email.trim() === "") {
        e.preventDefault();
        if (e.keyCode !== 46) {
            var arr = text.split(";");
            var lastEmail = arr[arr.length - 2];
            if (lastEmail) {
                text = text.replace(lastEmail + ";", "");
                mainInput.val(text);
                fakeInput.html(getHtmlFromEmails(mainInput.val()));
                placeCaretAtEnd(fakeInput);
            }
        }
    } else {
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

function getSimpleQuery(query) {
    var arr = query.replace(/<[^>]*>/g, "").split(";");
    var searchText = arr[arr.length - 1];
    return extractor(searchText);
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
        if ($(selection.anchorNode.parentNode).html() === "&nbsp;") {
            return "&nbsp;";
        } else {
            return selection.anchorNode.textContent;
        }
}

function getHtmlFromEmails(emails) {
    var arr = emails.split(";");
    var result = [];
    for (var i = 0; i < arr.length - 1; i++) {
        result.push("<span class='case-email-selected'>" + arr[i] + ";</span>");
    }
    if (result.length > 0) result.push("<span>&nbsp</span>");
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

function placeCaretAtEnd(node) {
    node[0].focus();
    var textNode = node[0].lastChild;
    if (textNode) {
        var range = document.createRange();
        range.setStart(textNode, 1);
        range.setEnd(textNode, 1);
        var sel = window.getSelection();
        sel.removeAllRanges();
        sel.addRange(range);
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
