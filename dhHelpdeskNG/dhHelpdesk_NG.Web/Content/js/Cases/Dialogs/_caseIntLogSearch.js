function InitializeInternalLogSendDialog(admins, emailGroups, workingGroups) {

    var toType = 1;
    var ccType = 2;
    var dialogType = toType;
    var mainIntLogInputTo = $("#caseLog_EmailRecepientsInternalLogTo");
    var mainIntLogInputCc = $("#caseLog_EmailRecepientsInternalLogCc");
    var popupIntLogInput = $("#caseInternalLogModalInput");
    var fakeInputTo = $("#fake_CaseLog_EmailRecepientsInternalLogTo");
    var fakeInputCc = $("#fake_CaseLog_EmailRecepientsInternalLogCc");
    var searchSelected = false;

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
        if (e.keyCode === 13 && searchSelected)
            return;
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
                changeFakeInputValueForView();
                placeCaretAtEnd(fakeInput);
            }
        }
    }

    function getCasesIntLogEmailSearchOptions() {

        var lastIntLogEmailSearchKey = '';
        var delayFunc = CommonUtils.createDelayFunc();

        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.replace(/<[^>]*>/g, "").split(";");
                var searchText = $.trim(arr[arr.length - 1]);
                if (searchText) {
                    lastIntLogEmailSearchKey = generateRandomKey();

                    delayFunc(function (){
                        //console.log('getCasesIntLogEmailSearchOptions: running ajax request.');
                        $.ajax({
                            url: "/cases/CaseSearchUserEmails",
                            type: "post",
                            data: { query: searchText, searchKey: lastIntLogEmailSearchKey, isInternalLog: true },
                            dataType: "json",
                            success: function(result) {
                                if (result.searchKey !== lastIntLogEmailSearchKey) {
                                    return;
                                }

                                var resultList = $.map(result.result,
                                    function(item) {
                                        var aItem = {
                                            userId: item.UserId,
                                            name: item.FirstName + " " + item.SurName,
                                            email: item.Emails,
                                            groupType: item.GroupType,
                                            departmentname: item.DepartmentName,
                                            name_family: item.SurName + " " + item.FirstName
                                        };
                                        return JSON.stringify(aItem);
                                    });

                                if (resultList.length > 0) {
                                    searchSelected = true;
                                }
                                else {
                                    var noRes = {
                                        name: document.parameters.noResultLabel,
                                        isNoResult: true
                                    }
                                    resultList.push(JSON.stringify(noRes));
                                    searchSelected = false;
                                }

                                process(resultList);
                            }
                        });    
                    },
                    300);
                }
                return;
            },

            matcher: function (obj) {
                var item = JSON.parse(obj);
                if (~item.isNoResult) {
                    return 1;
                }
                var tquery = getSimpleQuery(this.query);
                if (!tquery) return false;
                if (~item.email && (item.groupType === 0 || item.groupType === 1)) {
                    return ~item.name.toLowerCase().indexOf(tquery.toLowerCase()) ||
                        ~item.name_family.toLowerCase().indexOf(tquery.toLowerCase()) ||
                        ~item.userId.toLowerCase().indexOf(tquery.toLowerCase()) ||
                        ~item.email[0].toLowerCase().indexOf(tquery.toLowerCase());
                } else {
                    return ~item.name.toLowerCase().indexOf(tquery.toLowerCase())
                    || ~item.name_family.toLowerCase().indexOf(tquery.toLowerCase())
                    || ~item.userId.toLowerCase().indexOf(tquery.toLowerCase());
                }
            },

            sorter: function (items) {
                var beginswith = [], caseSensitive = [], caseInsensitive = [], other = [], item;
                var query = getSimpleQuery(this.query);
                while (aItem = items.shift()) {
                    item = JSON.parse(aItem);
                    if (item.groupType === 0) {
                        if (!item.userId.toLowerCase().indexOf(query.toLowerCase())) beginswith.push(JSON.stringify(item));
                        else if (~item.userId.indexOf(query)) caseSensitive.push(JSON.stringify(item));
                        else caseInsensitive.push(JSON.stringify(item));
                    } else {
                        other.push(JSON.stringify(item));
                    }
                }
                var initiators = beginswith.concat(caseSensitive, caseInsensitive);
                return initiators.concat(other);
            },

            highlighter: function (obj) {
                var item = JSON.parse(obj);
                if (item.isNoResult) {
                    return item.name;
                }
                var grType = "";
                if (item.groupType === 0)
                    grType = document.parameters.initLabel + ": ";
                if (item.groupType === 1)
                    grType = document.parameters.adminLabel + ": ";
                if (item.groupType === 2)
                    grType = document.parameters.wgLabel + ": ";
                if (item.groupType === 3)
                    grType = document.parameters.emailLabel + ": ";
                var userId = item.userId != null ? item.userId + ' - ' : "";
                var query = getSimpleQuery(this.query);
                var result = item.name + " - " + userId + item.email + " - " + item.departmentname;
                var resultNameFamily = item.name_family + " - " + userId + item.email + " - " + item.departmentname;
                if (result.toLowerCase().indexOf(query.toLowerCase()) > -1)
                    return grType + result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });
                else
                    return grType + resultNameFamily.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });
            },

            updater: function (obj) {
                var item = JSON.parse(obj);
                if (!item.isNoResult) {
                    var emailsToAdd = [];
                    $.each(item.email,
                        function(index, value) {
                            if (checkAndAddEmailsTo(value) === true)
                                emailsToAdd.push(value);
                        });
                    changeFakeInputValueForView();
                    placeCaretAtEnd(this.$element);
                }
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
                    ShowToastModalMessage(value + " : " + document.parameters.emailAlreadyAdded, "warning");
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
            ShowToastModalMessage(value + " : " + document.parameters.emailNotValid, "error");
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
                    ShowToastModalMessage(value + " : " + document.parameters.emailAlreadyAdded, "warning");
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
            ShowToastModalMessage(value + " : " + document.parameters.emailNotValid, "error");
            return false;
        }
    }
}