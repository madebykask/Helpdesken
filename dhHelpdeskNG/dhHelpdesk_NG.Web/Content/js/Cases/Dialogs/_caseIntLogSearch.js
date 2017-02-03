function InitializeInternalLogSendDialog(admins, emailGroups, workingGroups) {

    var toType = 1;
    var ccType = 2;
    var dialogType = toType;
    var mainIntLogInputTo = $("#caseLog_EmailRecepientsInternalLogTo");
    var mainIntLogInputCc = $("#caseLog_EmailRecepientsInternalLogCc");
    var popupIntLogInput = $("#caseInternalLogModalInput");
    var fakeInputTo = $("#fake_CaseLog_EmailRecepientsInternalLogTo");
    var fakeInputCc = $("#fake_CaseLog_EmailRecepientsInternalLogCc");

    fakeInputTo.html(getHtmlFromEmails(mainIntLogInputTo.val()));
    fakeInputCc.html(getHtmlFromEmails(mainIntLogInputCc.val()));

    initEditableDiv();

    $("a[href='#case_internal_log_to_btn']").on("click", function (e) {
        var $src = $(this);
        var $target = $("#case_internal_log_popup");
        popupIntLogInput.html(getHtmlFromEmails(mainIntLogInputTo.val()));
        $target.attr("data-src", $src.attr("data-src"));
        dialogType = toType;
        $target.modal("show");
    });

    $("a[href='#case_internal_log_cc_btn']").on("click", function (e) {
        var $src = $(this);
        var $target = $("#case_internal_log_popup");
        popupIntLogInput.html(getHtmlFromEmails(mainIntLogInputCc.val()));
        $target.attr("data-src", $src.attr("data-src"));
        dialogType = ccType;
        $target.modal("show");
    });

    $("#case_internal_log_popup").on("hide", function () {
        $(".toast-container").removeClass("case-followers-toastmessage");
        changeFakeInputValueForView();
    });

    $("#case_internal_log_popup").on("shown", function () {
        placeCaretAtEnd(popupIntLogInput);
    });

    $(".case-intlog-multiselect")
        .multiselect({
            enableFiltering: true,
            filterPlaceholder: "",
            maxHeight: 250,
            buttonClass: "btn",
            buttonContainer: '<span class="btn-group" />',
            buttonText: function(options) {
                return '-- <i class="caret"></i>';
            },
            onChange: function (element, checked) {
                if (element.parent().attr("id") === "caseInternalLogEmailGroupsDropdown") {
                        appendDropdownsEmails(emailGroups, element.val());
                }
                if (element.parent().attr("id") === "caseInternalLogWorkingGroupsDropdown") {
                        appendDropdownsEmails(workingGroups, element.val());
                }
                if (element.parent().attr("id") === "caseInternalLogAdministratorsDropdown") {
                        checkAndAddEmailsFromDrops(element.val());
                }
            }
        });

    function appendDropdownsEmails(array, selectedId) {
        var arr = $.grep(array,
                        function (a) {
                            return a.Id == selectedId;
                        });
        for (var j = 0; j < arr[0].Emails.length; j++) {
            checkAndAddEmailsFromDrops(arr[0].Emails[j]);
        }
    }

    function changeFakeInputValueForView() {
        var textTo = mainIntLogInputTo.val();
        var textCc = mainIntLogInputCc.val();
        fakeInputTo.html(getHtmlFromEmails(textTo));
        fakeInputCc.html(getHtmlFromEmails(textCc));
        if (dialogType === toType)
            popupIntLogInput.html(getHtmlFromEmails(textTo));
        if (dialogType === ccType)
            popupIntLogInput.html(getHtmlFromEmails(textCc));
    }

    popupIntLogInput.typeahead(getCasesIntLogEmailSearchOptions());

    popupIntLogInput.keydown(function (e) {
        if (e.keyCode === 13 || e.keyCode === 186) {
            if (dialogType === 1) {
                onEnterKeyUp(e, popupIntLogInput, mainIntLogInputTo);
            }
            if (dialogType === 2) {
                onEnterKeyUp(e, popupIntLogInput, mainIntLogInputCc);
            }
        }
    });

    popupIntLogInput.keydown(function (e) {
        if (e.keyCode === 8 || e.keyCode === 46) {
            if (dialogType === 1) {
                onRemoveKeyDown(e, popupIntLogInput, mainIntLogInputTo);
            }
            if (dialogType === 2) {
                onRemoveKeyDown(e, popupIntLogInput, mainIntLogInputCc);
            }
        }
    });

    fakeInputTo.typeahead(getCasesIntLogEmailSearchOptions());

    fakeInputTo.keydown(function (e) {
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, fakeInputTo, mainIntLogInputTo);
        }
    });

    fakeInputTo.keydown(function (e) {
        dialogType = toType;
        if (e.keyCode === 8 || e.keyCode === 46) {
            onRemoveKeyDown(e, fakeInputTo, mainIntLogInputTo);
        }
    });

    fakeInputCc.typeahead(getCasesIntLogEmailSearchOptions());

    fakeInputCc.keydown(function (e) {
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, fakeInputCc, mainIntLogInputCc);
        }
    });

    fakeInputCc.keydown(function (e) {
        dialogType = ccType;
        if (e.keyCode === 8 || e.keyCode === 46) {
            onRemoveKeyDown(e, fakeInputCc, mainIntLogInputCc);
        }
    });

    function onEnterKeyUp(e, fakeInput, mainInput) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var emails = $(e.target).html();
        var arr = getEmailsFromHtml(emails);
        var newEmail = "";
        if (e.keyCode === 13 || e.keyCode === 186) {
            newEmail = arr[arr.length - 1];
            if (newEmail !== "" && newEmail !== "&nbsp" && newEmail.indexOf("@") >= 0) {
                checkAndAddEmailsTo(newEmail);
                fakeInput.html(getHtmlFromEmails(mainInput.val()));
                placeCaretAtEnd(fakeInput);
            }
        }
    }

    function getCasesIntLogEmailSearchOptions() {
        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.replace("&nbsp;","").replace(/<[^>]*>/g, "").split(";");
                var searchText = $.trim(arr[arr.length - 1]);
                if (searchText) {
                    var lastInitiatorSearchKey = generateRandomKey();
                    return $.ajax({
                        url: "/cases/CaseSearchUserEmails",
                        type: "post",
                        data: { query: searchText, searchKey: lastInitiatorSearchKey },
                        dataType: "json",
                        success: function(result) {
                            if (result.searchKey !== lastInitiatorSearchKey)
                                return;

                            var resultList = $.map(result.result,
                                function(item) {
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
                }
                return;
            },

            matcher: function (item) {
                var arr = this.query.replace(/<[^>]*>/g, "").split(";");
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
                var query = extractor(this.query.replace(/<[^>]*>/g, "")).replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
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
                placeCaretAtEnd(this.$element);
                return;
            }
        };

        return options;
    }

    function checkAndAddEmailsTo(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value;
            if (dialogType === toType) {
                if (mainIntLogInputCc.val().indexOf(newToEmail) >= 0)
                    mainIntLogInputCc.val(mainIntLogInputCc.val().replace(newToEmail + ";", "")); //remove if exist in cc
                if (mainIntLogInputTo.val().indexOf(newToEmail) < 0)
                    mainIntLogInputTo.val(mainIntLogInputTo.val() + newToEmail + ";");
                else {
                    return false;
                }
            }
            if (dialogType === ccType)
                if (mainIntLogInputTo.val().indexOf(newToEmail) >= 0) {
                    ShowToastModalMessage(value + " : " + window.parameters.emailAlreadyAdded, "warning");
                    return false;
                }
                else {
                    if (mainIntLogInputCc.val().indexOf(newToEmail) < 0)
                        mainIntLogInputCc.val(mainIntLogInputCc.val() + newToEmail + ";");
                    else {
                        return false;
                    }
                }
            return true;
        } else {
            ShowToastModalMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }
    }

    function checkAndAddEmailsFromDrops(value) {
        if (isValidEmailAddress(value)) {
            if (dialogType === toType) {
                if (mainIntLogInputCc.val().indexOf(value) >= 0) {
                    mainIntLogInputCc.val(mainIntLogInputCc.val().replace(value + ";", ""));
                }
                if (mainIntLogInputTo.val().indexOf(value) < 0) {
                    mainIntLogInputTo.val(mainIntLogInputTo.val() + value + ";");
                    popupIntLogInput.html(getHtmlFromEmails(mainIntLogInputTo.val()));
                }
                else {
                    return false;
                }
            }
            if (dialogType === ccType)
                if (mainIntLogInputTo.val().indexOf(value) >= 0) {
                    ShowToastModalMessage(value + " : " + window.parameters.emailAlreadyAdded, "warning");
                    return false;
                }
                else {
                    if (mainIntLogInputCc.val().indexOf(value) < 0) {
                        mainIntLogInputCc.val(mainIntLogInputCc.val() + value + ";");
                        popupIntLogInput.html(getHtmlFromEmails(mainIntLogInputCc.val()));
                    }
                    else {
                        return false;
                    }
                }
            return true;
        } else {
            ShowToastModalMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }
    }
}