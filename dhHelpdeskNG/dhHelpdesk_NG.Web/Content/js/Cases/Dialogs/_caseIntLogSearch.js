function InitializeInternalLogSendDialog(admins, emailGroups, workingGroups) {

    var toType = 1;
    var ccType = 2;
    var dialogType = toType;
    var mainIntLogInputTo = $("#caseLog_EmailRecepientsInternalLogTo");
    var mainIntLogInputCc = $("#caseLog_EmailRecepientsInternalLogCc");
    var popupIntLogInput = $("#caseInternalLogModalInput");
    var fakeInputTo = $("#fake_CaseLog_EmailRecepientsInternalLogTo");
    var fakeInputCc = $("#fake_CaseLog_EmailRecepientsInternalLogCc");

    $("a[href='#case_internal_log_to_btn']").on("click", function (e) {
        var $src = $(this);
        var $target = $("#case_internal_log_popup");
        popupIntLogInput.val(mainIntLogInputTo.val());
        $target.attr("data-src", $src.attr("data-src"));
        dialogType = toType;
        $target.modal("show");
    });

    $("a[href='#case_internal_log_cc_btn']").on("click", function (e) {
        var $src = $(this);
        var $target = $("#case_internal_log_popup");
        popupIntLogInput.val(mainIntLogInputCc.val());
        $target.attr("data-src", $src.attr("data-src"));
        dialogType = ccType;
        $target.modal("show");
    });

    $("#case_internal_log_popup").on("hide", function () {
        changeFakeInputValueForView();
        dropdownDeselectAll();
    });

    $("#case_internal_log_popup").on("shown", function () {
        $("#caseInternalLogModalInput").focus();
    });

    $(".case-usersearch-multiselect")
        .multiselect({
            enableFiltering: true,
            filterPlaceholder: "",
            maxHeight: 250,
            buttonClass: "btn",
            buttonContainer: '<span class="btn-group" />',
            buttonText: function(options) {
                if (options.length === 0) {
                    return '-- <i class="caret"></i>';
                } else if (options.length > 1) {
                    return options.length + " " + window.parameters.selectedLabel + '  <i class="caret"></i>';
                } else if (options.length > 0) {
                    var selected = "";
                    options.each(function() {
                        selected += $(this).text() + ", ";
                    });
                    return selected.substr(0, selected.length - 2) + ' <i class="caret"></i>';
                }
                return '-- <i class="caret"></i>';
            }
        });

    $("#caseInternalLog_emailGroupsDiv").change(function () {
        appendGroupEmails();
    });
    $("#caseInternalLog_workingGroupsDiv").change(function () {
        appendWorkingGroupEmails();
    });
    $("#caseInternalLog_adminsDiv").change(function () {
        appendAdminEmails();
    });

    function appendGroupEmails() {
        var selectedGroupIds = $("#caseInternalLogEmailGroupsDropdown").val() || [];
        for (var i = 0; i < selectedGroupIds.length; i++) {
            var arr = jQuery.grep(emailGroups,
                function (a) {
                    return a.Id == selectedGroupIds[i];
                });
            for (var j = 0; j < arr[0].Emails.length; j++) {
                checkAndAddEmailsFromDrops(arr[0].Emails[j]);
            }
        }
    }

    function appendWorkingGroupEmails() {
        var selectedGroupIds = $("#caseInternalLogWorkingGroupsDropdown").val() || [];
        for (var i = 0; i < selectedGroupIds.length; i++) {
            var arr = jQuery.grep(workingGroups,
                function (a) {
                    return a.Id == selectedGroupIds[i];
                });
            for (var j = 0; j < arr[0].Emails.length; j++) {
                checkAndAddEmailsFromDrops(arr[0].Emails[j]);
            }
        }
    }

    function appendAdminEmails() {
        var selectedAdministratorEmails = $("#caseInternalLogAdministratorsDropdown").val() || [];
        for (var i = 0; i < selectedAdministratorEmails.length; i++) {
            checkAndAddEmailsFromDrops(selectedAdministratorEmails[i]);
        }
    }

    function changeFakeInputValueForView() {
        var textTo = mainIntLogInputTo.val();
        var textCc = mainIntLogInputCc.val();
        fakeInputTo.val(textTo);
        fakeInputCc.val(textCc);
        if (dialogType === toType)
            popupIntLogInput.val(textTo);
        if (dialogType === ccType)
            popupIntLogInput.val(textCc);
    }

    popupIntLogInput.typeahead(getCasesIntLogEmailSearchOptions());

    popupIntLogInput.keyup(function (e) {
        if (e.keyCode === 13) {
            onEnterKeyUp(e, popupIntLogInput);
        }
    });

    popupIntLogInput.keydown(function (e) {
        if (e.keyCode === 8) {
            e.stopImmediatePropagation();
            var caretPos = popupIntLogInput[0].selectionStart;
            var lastEmail = returnEmailBeforeCaret(popupIntLogInput.val(), caretPos);
            if (lastEmail !== "" && isValidEmailAddress(lastEmail)) {
                e.preventDefault();
                popupIntLogInput.val(popupIntLogInput.val().replace(lastEmail + ";", ""));
                if (dialogType === 1) {
                    mainIntLogInputTo.val(popupIntLogInput.val());
                }
                if (dialogType === 2) {
                    mainIntLogInputCc.val(popupIntLogInput.val());
                }
            }
        }
    });

    fakeInputTo.typeahead(getCasesIntLogEmailSearchOptions());

    fakeInputTo.keyup(function (e) {
        if (e.keyCode === 13) {
            onEnterKeyUp(e, fakeInputTo);
        }
    });

    fakeInputTo.keydown(function (e) {
        dialogType = toType;
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, fakeInputTo, mainIntLogInputTo);
        }
    });

    fakeInputCc.typeahead(getCasesIntLogEmailSearchOptions());

    fakeInputCc.keyup(function (e) {
        if (e.keyCode === 13) {
            onEnterKeyUp(e, fakeInputCc);
        }
    });

    fakeInputCc.keydown(function (e) {
        dialogType = ccType;
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, fakeInputCc, mainIntLogInputCc);
        }
    });

    function onEnterKeyUp(e, fakeInput) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var emails = $(e.target).val();
        var arr = emails.split(';');
        var newEmail = arr[arr.length - 1].replace("\n", "");
        if (newEmail.trim() !== "" && checkAndAddEmailsTo(newEmail)) {
            fakeInput.val(emails.replace("\n", "") + ";");
        }
    }

    function onBackspaceKeyDown(e, fakeInput, mainInput) {
        e.stopImmediatePropagation();
        var caretPos = fakeInput[0].selectionStart;
        var lastEmail = returnEmailBeforeCaret(fakeInput.val(), caretPos);
        if (lastEmail !== "" && isValidEmailAddress(lastEmail)) {
            e.preventDefault();
            fakeInput.val(fakeInput.val().replace(lastEmail + ";", ""));
            mainInput.val(fakeInput.val());
        }
    }

    function getCasesIntLogEmailSearchOptions() {
        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.split(";");
                var searchText = arr[arr.length - 1];
                var lastInitiatorSearchKey = generateRandomKey();
                return $.ajax({
                    url: "/cases/CaseSearchUserEmails",
                    type: "post",
                    data: { query: searchText, searchKey: lastInitiatorSearchKey },
                    dataType: "json",
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
                    function ($1, match) {
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
                changeFakeInputValueForView();
                return this.$element.val().replace(/[^;]*$/, "");
            }
        };

        return options;
    }

    function checkAndAddEmailsTo(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + ";";
            if (dialogType === toType) {
                if (mainIntLogInputCc.val().indexOf(newToEmail) >= 0)
                    mainIntLogInputCc.val(mainIntLogInputCc.val().replace(newToEmail, "")); //remove if exist in cc
                if (mainIntLogInputTo.val().indexOf(newToEmail) < 0)
                    mainIntLogInputTo.val(mainIntLogInputTo.val() + newToEmail);
                else {
                    return false;
                }
            }
            if (dialogType === ccType)
                if (mainIntLogInputTo.val().indexOf(newToEmail) >= 0) {
                    ShowToastMessage(value + " : " + window.parameters.emailAlreadyAdded, "warning");
                    return false;
                }
                else {
                    if (mainIntLogInputCc.val().indexOf(newToEmail) < 0)
                        mainIntLogInputCc.val(mainIntLogInputCc.val() + newToEmail);
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

    function checkAndAddEmailsFromDrops(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + ";";
            if (dialogType === toType) {
                if (mainIntLogInputCc.val().indexOf(newToEmail) >= 0) {
                    mainIntLogInputCc.val(mainIntLogInputCc.val().replace(newToEmail, ""));
                }
                if (mainIntLogInputTo.val().indexOf(newToEmail) < 0) {
                    mainIntLogInputTo.val(mainIntLogInputTo.val() + newToEmail);
                    popupIntLogInput.val(popupIntLogInput.val() + newToEmail);
                }
                else {
                    return false;
                }
            }
            if (dialogType === ccType)
                if (mainIntLogInputTo.val().indexOf(newToEmail) >= 0) {
                    ShowToastMessage(value + " : " + window.parameters.emailAlreadyAdded, "warning");
                    return false;
                }
                else {
                    if (mainIntLogInputCc.val().indexOf(newToEmail) < 0) {
                        mainIntLogInputCc.val(mainIntLogInputCc.val() + newToEmail);
                        popupIntLogInput.val(popupIntLogInput.val() + newToEmail);
                    }
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

    function dropdownDeselectAll() {
        $(".case-usersearch-multiselect").each(function () {
            var select = $(this);
            $("option", select).each(function () {
                select.multiselect("deselect", $(this).val());
            });
        });
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