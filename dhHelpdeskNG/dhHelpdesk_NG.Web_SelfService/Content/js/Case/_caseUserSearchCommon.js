function onBackspaceKeyDown(e, fakeInput, mainInput) {
    e.stopImmediatePropagation();
    var text = fakeInput.val();
    var caretPos = fakeInput[0].selectionStart;
    var caretPosEnd = fakeInput[0].selectionEnd;
    var email = getEmailsByCaretPos(text, caretPos, caretPosEnd, e);
    if (email !== "") {
        e.preventDefault();
        text = text.replace(email, "");
        fakeInput.val(text.replace(/^ /, ""));
        mainInput.val(fakeInput.val());
    }
}

function onDeleteKeyDown(e, fakeInput, mainInput) {
    e.stopImmediatePropagation();
    var caretPos = fakeInput[0].selectionStart;
    var caretPosEnd = fakeInput[0].selectionEnd;
    var text = fakeInput.val();
    var email = getEmailsByCaretPos(text, caretPos, caretPosEnd, e);
    if (email !== "") {
        e.preventDefault();
        text = text.replace(email, "");
        fakeInput.val(text.replace(/^ /, ""));
        mainInput.val(fakeInput.val());
    }
}

function getEmailsByCaretPos(text, posStart, posEnd, e) {
    var indexes = [];
    for (var i = 0; i < text.length; i++) {
        if (text[i] === ";" && text[i + 1] === " ") indexes.push(i + 1);
    }
    var aaa = indexes[indexes.length - 1];
    if ((aaa + 1) === posStart) {
        posStart = posStart - 2;
    }
    var result1 = Math.max.apply(Math, indexes.filter(function (x) { return x <= posStart }));
    var result2 = Math.min.apply(Math, indexes.filter(function (x) { return x >= posEnd }));
    if (!isFinite(result2) && e.keyCode === 46)
        return "";
    if (posStart === posEnd && posStart === 0 && e.keyCode === 8)
        return "";
    var emails = text.substring(result1, result2);
    if (emails.indexOf("@") > 0)
        return emails.replace(/\s*$/, "");
    return "";
}

function returnEmailBeforeCaret(text, caretPos) {
    var preText = text.substring(0, caretPos);
    if (preText.indexOf(";") > 0) {
        var words = preText.split(";");
        if (words[words.length - 1] === "")
            return words[words.length - 2]; //return last email
    }
    else {
        return "";
    }
    return "";
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

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress.trim());
};