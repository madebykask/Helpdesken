function InitCaseAddFollowersSearch(admins, emailGroups, workingGroups) {

    var mainFollowersInput = $("#caseFollowerUsersInput");
    var mainFakeFollowersInput = $("#fakeCaseFollowerUsersInput");
    var popupFollowersInput = $("#caseAddFollowersModalInput");

    mainFakeFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));

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
                var lastInitiatorSearchKey = generateRandomKey();
                return $.ajax({
                    url: "/cases/CaseSearchUserEmails",
                    type: "POST",
                    data: { query: searchText, searchKey: lastInitiatorSearchKey },
                    dataType:"json",
                    success: function (result) {
                        if (result.searchKey !== lastInitiatorSearchKey)
                            return;

                        var resultList = $.map(result.result,
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
                mainFakeFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));
                popupFollowersInput.html(getHtmlFromEmails(mainFollowersInput.val()));
                placeCaretAtEnd(this.$element);
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
                ShowToastModalMessage(value + " : " + window.parameters.emailAlreadyAdded, "warning");
                return false;
            }
            return true;
        } else {
            ShowToastModalMessage(value + " : " + window.parameters.emailNotValid, "error");
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
            ShowToastModalMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }
    }

    function changeFakeInputValueForView() {
        var text = mainFollowersInput.val();
        mainFakeFollowersInput.html(getHtmlFromEmails(text));
    }

}