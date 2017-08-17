function InitCaseAddFollowersSearch(admins, emailGroups, workingGroups) {

    var mainFollowersInput = $("#caseFollowerUsersInput");
    var mainFakeFollowersInput = $("#fakeCaseFollowerUsersInput");
    var popupFollowersInput = $("#caseAddFollowersModalInput");
    var searchSelected = false;

//    mainFakeFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));

    initEditableDiv();

    $("a[href='#case_add_followers_btn']").on("click", function () {
        var $src = $(this);
        var $target = $("#case_add_followers_popup");
        popupFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));
        $target.attr("data-src", $src.attr("data-src"));
        $target.modal("show");
    });

    $("#case_add_followers_popup").on("hide", function () {
        $(".toast-container").removeClass("case-followers-toastmessage");
        changeFakeInputValueForView();
    });

    $("#case_add_followers_popup").on("shown", function () {
        placeCaretAtEnd(popupFollowersInput);
    });

    $(".case-usersearch-multiselect").multiselect({
        
        enableFiltering: true,
        filterPlaceholder: "",
        maxHeight: 250,
        buttonClass: "btn",
        buttonContainer: '<span class="btn-group" />',
        buttonText: function(options) {
            return '-- <i class="caret"></i>';
        },
        onChange: function (element, checked) {
            
            if (element.parent().attr("id") === "caseFollowersEmailGroupsDropdown") {
                    appendDropdownsEmails(emailGroups, element.val());
            }
            if (element.parent().attr("id") === "caseFollowersWorkingGroupsDropdown") {
                    appendDropdownsEmails(workingGroups, element.val());
            }
            if (element.parent().attr("id") === "caseFollowersAdministratorsDropdown") {
                    checkAndAddEmailsFromDropdown(element.val());
            }
        }

    });

    function appendDropdownsEmails(array, selectedId) {
        var arr = $.grep(array,
                        function (a) {
                            return a.Id == selectedId;
                        });
        for (var j = 0; j < arr[0].Emails.length; j++) {
            checkAndAddEmailsFromDropdown(arr[0].Emails[j]);
        }
    }

    mainFakeFollowersInput.typeahead(getCasesAddFollowersSearchOptions());

    mainFakeFollowersInput.keydown(function (e) {
        if (e.keyCode === 8 || e.keyCode === 46) {
            onRemoveKeyDown(e, mainFakeFollowersInput, mainFollowersInput);
        }
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, mainFakeFollowersInput);
        }
    });

    popupFollowersInput.typeahead(getCasesAddFollowersSearchOptions());

    popupFollowersInput.keydown(function (e) {
        if (e.keyCode === 8 || e.keyCode === 46) {
            onRemoveKeyDown(e, popupFollowersInput, mainFollowersInput);
        }
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, popupFollowersInput);
        }
    });

    function onEnterKeyUp(e, fakeInput) {
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
                fakeInput.html(getHtmlFromEmails(mainFollowersInput.val()));
                placeCaretAtEnd(fakeInput);
            }
        }
    }

    function getCasesAddFollowersSearchOptions() {
        
        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.replace(/<[^>]*>/g, "").split(";");
                var searchText = $.trim(arr[arr.length - 1]);
                if (searchText) {
                    var lastInitiatorSearchKey = generateRandomKey();
                    return $.ajax({
                        url: "/cases/CaseSearchUserEmails",
                        type: "POST",
                        data: { query: searchText, searchKey: lastInitiatorSearchKey },
                        dataType: "json",
                        success: function(result) {
                            if (result.searchKey !== lastInitiatorSearchKey)
                                return;

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
                                    name: window.parameters.noResultLabel,
                                    isNoResult: true
                                }
                                resultList.push(JSON.stringify(noRes));
                            }
                            return process(resultList);
                        }
                    });
                }
                return;
            },

            matcher: function (obj) {
                var item = JSON.parse(obj);
                var tquery = getSimpleQuery(this.query);
                if (!tquery) return false;
                if (~item.isNoResult) {
                    return 1;
                }
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
                    mainFakeFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));
                    popupFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));
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
            var emails = mainFollowersInput.val().split(";");
            if (emails.indexOf(newToEmail) < 0)
                mainFollowersInput.val(mainFollowersInput.val() + newToEmail + ";");
            else {
                ShowToastModalMessage(value + " : " + document.parameters.emailAlreadyAdded, "warning");
                return false;
            }
            return true;
        } else {
            ShowToastModalMessage(value + " : " + document.parameters.emailNotValid, "error");
            return false;
        }
    }

    function checkAndAddEmailsFromDropdown(value) {
        if (isValidEmailAddress(value)) {
            if (mainFollowersInput.val().indexOf(value) < 0) {
                mainFollowersInput.val(mainFollowersInput.val() + value + ";");
                popupFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));
            } else {
                return false;
            }
            return true;
        } else {
            ShowToastModalMessage(value + " : " + document.parameters.emailNotValid, "error");
            return false;
        }
    }

    function changeFakeInputValueForView() {
        var text = mainFollowersInput.val();
        mainFakeFollowersInput.html(getHtmlFromEmails(text));
    }

}