function InitializeInternalLogSendDialog(admins, emailGroups, workingGroups) {

    var toType = 1;
    var ccType = 2;
    var dialogType = toType;
    var mainIntLogInputTo = $("#caseLog_EmailRecepientsInternalLogTo");
    var mainIntLogInputCc = $("#caseLog_EmailRecepientsInternalLogCc");
    var popupIntLogInput = $("#caseInternalLogModalInput");
    var fakeInputTo = $("#fake_CaseLog_EmailRecepientsInternalLogTo");
    var fakeInputCc = $("#fake_CaseLog_EmailRecepientsInternalLogCc");
    var isNeedToRemove = true;

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
        isNeedToRemove = false;
        dropdownDeselectAll();
        isNeedToRemove = true;
    });

    $("#case_internal_log_popup").on("shown", function () {
        popupIntLogInput.focus();
    });

    $(".case-intlog-multiselect")
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
            },
            onChange: function (element, checked) {
                if (element.parent().attr("id") === "caseInternalLogEmailGroupsDropdown") {
                    if (checked) {
                        appendDropdownsEmails(emailGroups, element.val());
                    } else if (isNeedToRemove) {
                        removeDropdownEmails(emailGroups, element.val());
                    }
                }
                if (element.parent().attr("id") === "caseInternalLogWorkingGroupsDropdown") {
                    if (checked) {
                        appendDropdownsEmails(workingGroups, element.val());
                    } else if (isNeedToRemove) {
                        removeDropdownEmails(workingGroups, element.val());
                    }
                }
                if (element.parent().attr("id") === "caseInternalLogAdministratorsDropdown") {
                    if (checked) {
                        checkAndAddEmailsFromDrops(element.val());
                    } else if (isNeedToRemove) {
                        removeEmailsFromDrops(element.val());
                    }
                }
            }
        });

    function appendDropdownsEmails(array, selectedId) {
        var arr = jQuery.grep(array,
                        function (a) {
                            return a.Id == selectedId;
                        });
        for (var j = 0; j < arr[0].Emails.length; j++) {
            checkAndAddEmailsFromDrops(arr[0].Emails[j]);
        }
    }

    function removeDropdownEmails(array, selectedId) {
        var arr = jQuery.grep(array,
                        function (a) {
                            return a.Id == selectedId;
                        });
        for (var j = 0; j < arr[0].Emails.length; j++) {
            removeEmailsFromDrops(arr[0].Emails[j]);
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
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, popupIntLogInput);
        }
    });

    popupIntLogInput.keydown(function (e) {
        if (e.keyCode === 8) {
            e.stopImmediatePropagation();
            var text = popupIntLogInput.val();
            var caretPos = popupIntLogInput[0].selectionStart;
            var caretPosEnd = popupIntLogInput[0].selectionEnd;
            var email = getEmailsByCaretPos(text, caretPos, caretPosEnd, e);
            if (email !== "") {
                e.preventDefault();
                text = text.replace(email, "");
                popupIntLogInput.val(text.replace(/^ /, ""));
                if (dialogType === 1) {
                    mainIntLogInputTo.val(popupIntLogInput.val());
                }
                if (dialogType === 2) {
                    mainIntLogInputCc.val(popupIntLogInput.val());
                }
            }
        }
        if (e.keyCode === 46) {
            e.stopImmediatePropagation();
            var caretPos = popupIntLogInput[0].selectionStart;
            var caretPosEnd = popupIntLogInput[0].selectionEnd;
            var text = popupIntLogInput.val();
            var email = getEmailsByCaretPos(text, caretPos, caretPosEnd, e);
            if (email !== "") {
                e.preventDefault();
                text = text.replace(email, "");
                popupIntLogInput.val(text.replace(/^ /, ""));
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
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, fakeInputTo);
        }
    });

    fakeInputTo.keydown(function (e) {
        dialogType = toType;
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, fakeInputTo, mainIntLogInputTo);
        }
        if (e.keyCode === 46) {
            onDeleteKeyDown(e, fakeInputTo, mainIntLogInputTo);
        }
    });

    fakeInputCc.typeahead(getCasesIntLogEmailSearchOptions());

    fakeInputCc.keyup(function (e) {
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, fakeInputCc);
        }
    });

    fakeInputCc.keydown(function (e) {
        dialogType = ccType;
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, fakeInputCc, mainIntLogInputCc);
        }
        if (e.keyCode === 46) {
            onDeleteKeyDown(e, fakeInputCc, mainIntLogInputCc);
        }
    });

    function onEnterKeyUp(e, fakeInput) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var emails = $(e.target).val();
        var arr = emails.split(";");
        var newEmail = "";
        if (e.keyCode === 13) {
            newEmail = arr[arr.length - 1].replace("\n", "");
            if (newEmail.trim() !== "" && checkAndAddEmailsTo(newEmail)) {
                fakeInput.val(emails.replace("\n", "") + "; ");
            }
        }
        if (e.keyCode === 186) {
            newEmail = arr[arr.length - 2];
            if (newEmail.trim() !== "" && checkAndAddEmailsTo(newEmail)) {
                fakeInput.val(emails + " ");
            }
        }
    }

    function getCasesIntLogEmailSearchOptions() {
        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.split(";");
                var searchText = $.trim(arr[arr.length - 1]);
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
                return this.$element.val().replace(/[^;]*$/, " ");
            }
        };

        return options;
    }

    function checkAndAddEmailsTo(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + "; ";
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
            ShowToastMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }
    }

    function checkAndAddEmailsFromDrops(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + "; ";
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
            ShowToastMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }
    }

    function removeEmailsFromDrops(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + "; ";
            if (dialogType === toType) {
                mainIntLogInputTo.val(mainIntLogInputTo.val().replace(newToEmail, ""));
                popupIntLogInput.val(popupIntLogInput.val().replace(newToEmail, ""));
            }
            if (dialogType === ccType)
                mainIntLogInputCc.val(mainIntLogInputCc.val().replace(newToEmail, ""));
            popupIntLogInput.val(popupIntLogInput.val().replace(newToEmail, ""));
        } else {
            ShowToastMessage(value + " : " + window.parameters.emailNotValid, "error");
        }
    }

    function dropdownDeselectAll() {
        $(".case-intlog-multiselect").each(function () {
            var select = $(this);
            $("option", select).each(function () {
                select.multiselect("deselect", $(this).val());
            });
        });
    }
}