function InitializeInternalLogSendDialog() {
    var toType = 1;
    var ccType = 2;

    $('#SendIntLogCase')
        .dialog({
            autoOpen: false,
            modal: false,
            resizable: false,
            heigth: 200,
            width: 350,
            dialogType: toType,
            dialogClass: 'overflow-visible',
            buttons: [
                {
                    text: window.parameters.closeBtn,
                    click: function () {
                        $(this).dialog("close");
                        $("#casesIntLogSendInput").val("");
                    },
                    'class': 'btn'
                }
            ],

            open: function () {
                InitCaseIntLogSendSearch();
            },
            close: function () {
                OnCloseCaseInternalLogSendDialog();
            }
        });
}

function OnCloseCaseInternalLogSendDialog() {

    $.fn.textWidth = function (text, font) {
        if (!$.fn.textWidth.fakeEl) $.fn.textWidth.fakeEl = $('<span>').hide().appendTo(document.body);
        $.fn.textWidth.fakeEl.text(text || this.val() || this.text()).css('font', font || this.css('font'));
        return $.fn.textWidth.fakeEl.width();
    };

    var dialogType = $("#SendIntLogCase").data("uiDialog").options.dialogType;
    var fakeInput = $("#Fake_CaseLog_EmailRecepientsInternalLogTo");
    if (dialogType === 2)
        fakeInput = $("#Fake_CaseLog_EmailRecepientsInternalLogCc");
    var valueInput = $("#casesIntLogSendInput");
    var text = valueInput.val();
    var elementWidht = fakeInput.width();
    if (valueInput.textWidth() > elementWidht) {
        for (var i = 35; i > 20; i--) {
            fakeInput.val(text.substring(0, i) + "..+");
            if (fakeInput.textWidth() <= elementWidht)
                return;
        }
    } else {
        fakeInput.val(text);
    }
}

function InitCaseIntLogSendSearch() {

    var textBoxEmailsTo = $("#CaseLog_EmailRecepientsInternalLogTo");
    var textBoxEmailsCc = $("#CaseLog_EmailRecepientsInternalLogCc");
    var emailInput = $("#casesIntLogSendInput");
    var dialogWindow = $("#SendIntLogCase");

    emailInput.typeahead(getCasesIntLogEmailSearchOptions());

    emailInput.keyup(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                e.stopImmediatePropagation();
                var emails = $(this).val();
                var arr = emails.split(';');
                var newEmail = arr[arr.length - 1].replace("\n", "");
                if (newEmail.trim() !== "" && checkAndAddEmailsTo(newEmail)) {
                    emailInput.val(emails.replace("\n", "") + ";");
                }
            }
        });

//    emailInput.keydown(function (e) {
//        if (e.keyCode === 8) {
//            e.stopImmediatePropagation();
//            if (emailInput.val().substr(emailInput.val().length - 1) === ";") {
//                e.preventDefault();
//                var arr = emailInput.val().split(';').filter(v => v !== "");
//                var lastEmail = arr[arr.length - 1] + ";";
//                emailInput.val(emailInput.val().replace(lastEmail, ""));
//                var dialogType = dialogWindow.data("uiDialog").options.dialogType;
//                if (dialogType === 1) {
//                    textBoxEmailsTo.val(emailInput.val());
//                }
//                if (dialogType === 2) {
//                    textBoxEmailsCc.val(emailInput.val());
//                }
//            }
//        }
//    });

    emailInput.keydown(function (e) {
        if (e.keyCode === 8) {
            e.preventDefault();
            e.stopImmediatePropagation();
            var caretPos = emailInput[0].selectionStart;
            var lastEmail = returnEmailBeforeCaret(emailInput.val(), caretPos);
            if (lastEmail !== "") {
                emailInput.val(emailInput.val().replace(lastEmail, ""));
                var dialogType = dialogWindow.data("uiDialog").options.dialogType;
                if (dialogType === 1) {
                    textBoxEmailsTo.val(emailInput.val());
                }
                if (dialogType === 2) {
                    textBoxEmailsCc.val(emailInput.val());
                }
            }
        }
    });

    function returnEmailBeforeCaret(text, caretPos) {
        var preText = text.substring(0, caretPos);
        if (preText.indexOf(";") > 0) {
            var words = preText.split(";");
            if (words[words.length - 1] === "")
                return words[words.length - 2] + ";"; //return last email
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

    function getCasesIntLogEmailSearchOptions() {
        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.split(';');
                var searchText = arr[arr.length - 1];
                var lastInitiatorSearchKey = generateRandomKey();
                var isWgChecked = $('#searchIntSendToCheckboxWg').prop("checked");
                var isInitChecked = $('#searchIntSendToCheckboxInit').prop("checked");
                var isAdmChecked = $('#searchIntSendToCheckboxAdm').prop("checked");
                var isEgChecked = $('#searchIntSendToCheckboxEg').prop("checked");
                return $.ajax({
                    url: '/cases/SearchCaseIntLogEmails',
                    type: 'post',
                    data: { query: searchText, searchKey: lastInitiatorSearchKey, isWgChecked: isWgChecked, isInitChecked: isInitChecked, isAdmChecked: isAdmChecked, isEgChecked: isEgChecked },
                    dataType: 'json',
                    success: function (result) {
                        if (result.searchKey !== lastInitiatorSearchKey)
                            return;

                        var resultList = jQuery.map(result.result,
                            function (item) {
                                var aItem = {
                                    userId: item.UserId,
                                    name: item.Name,
                                    email: item.Emails,
                                    groupType: item.GroupType
                                };
                                return JSON.stringify(aItem);
                            });

                        return process(resultList);
                    }
                });
            },

            matcher: function (item) {
                var arr = this.query.split(';');
                var searchText = arr[arr.length - 1];
                var tquery = extractor(searchText);
                if (!tquery) return false;
                return ~item.toLowerCase().indexOf(tquery.toLowerCase());
            },

            highlighter: function (obj) {
                var item = JSON.parse(obj);
                var grType = "";
                if (item.groupType === 0)
                    grType = window.parameters.initGroup + ": ";
                if (item.groupType === 1)
                    grType = window.parameters.adminGroup + ": ";
                if (item.groupType === 2)
                    grType = window.parameters.wgGroup + ": ";
                if (item.groupType === 3)
                    grType = window.parameters.emailGroup + ": ";
                var userId = item.userId != null ? item.userId + ' - ' : "";

                var result = item.name + ' - ' + userId + item.email;
                var query = extractor(this.query).replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                return grType + result.replace(new RegExp('(' + query + ')', 'ig'),
                    function($1, match) {
                        return '<strong>' + match + '</strong>';
                    });
            },

            updater: function (obj) {
                var item = JSON.parse(obj);
                var emailsToAdd = [];
                $.each(item.email, function (index, value) {
                    if (checkAndAddEmailsTo(value) === true)
                        emailsToAdd.push(value);
                });
                if (emailsToAdd.length > 0)
                    return this.$element.val().replace(/[^;]*$/, '') + emailsToAdd.join(";") + ';';
                return this.$element.val().replace(/[^;]*$/, '');
            }
        };

        return options;
    }

    function checkAndAddEmailsTo(value) {
        if (isValidEmailAddress(value)) {
            var dialogType = dialogWindow.data("uiDialog").options.dialogType;

            var newToEmail = value + ";";
            if (dialogType === 1) {
                if (textBoxEmailsCc.val().indexOf(newToEmail) >= 0)
                    textBoxEmailsCc.val(textBoxEmailsCc.val().replace(newToEmail, ""));
                if (textBoxEmailsTo.val().indexOf(newToEmail) < 0)
                    textBoxEmailsTo.val(textBoxEmailsTo.val() + newToEmail);
                else {
                    return false;
                }
            }
            if (dialogType === 2)
                if (textBoxEmailsTo.val().indexOf(newToEmail) >= 0) {
                    ShowToastMessage(value + " : " + window.parameters.emailAlreadyAdded, 'warning');
                    return false;
                }
                else {
                    if (textBoxEmailsCc.val().indexOf(newToEmail) < 0)
                        textBoxEmailsCc.val(textBoxEmailsCc.val() + newToEmail);
                    else {
                        return false;
                    }
                }
            return true;
        } else {
            ShowToastMessage(value + " : " + window.parameters.emailNotValid, 'error');
            return false;
        }
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
        return pattern.test(emailAddress);
    };


}