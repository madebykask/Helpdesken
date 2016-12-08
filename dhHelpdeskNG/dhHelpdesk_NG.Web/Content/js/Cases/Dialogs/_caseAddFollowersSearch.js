function InitCaseAddFollowersSearch(admins, emailGroups, workingGroups) {

    var mainFollowersInput = $("#caseFollowerUsersInput");
    var mainFakeFollowersInput = $("#fakeCaseFollowerUsersInput");
    var popupFollowersInput = $("#caseAddFollowersModalInput");

    $("a[href='#case_add_followers_btn']").on("click", function () {
        var $src = $(this);
        var $target = $("#case_add_followers_popup");
        popupFollowersInput.val(mainFollowersInput.val());
        $target.attr("data-src", $src.attr("data-src"));
        $target.modal("show");
    });

    $("#case_add_followers_popup").on("hide", function () {
        changeFakeInputValueForView();
        dropdownDeselectAll();
    });

    $("#case_add_followers_popup").on("shown", function () {
        $("#caseAddFollowersModalInput").focus();
    });

    $(".case-usersearch-multiselect").multiselect({
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
                var selected = '';
                options.each(function() {
                    selected += $(this).text() + ', ';
                });
                return selected.substr(0, selected.length - 2) + ' <i class="caret"></i>';
            }
            return '-- <i class="caret"></i>';
        }
    });

    $("#caseFollowers_emailGroupsDiv").change(function () {
        appendGroupEmails();
    });
    $("#caseFollowers_workingGroupsDiv").change(function () {
        appendWorkingGroupEmails();
    });
    $("#caseFollowers_adminsDiv").change(function () {
        appendAdminEmails();
    });

    function appendGroupEmails() {
        var selectedGroupIds = $("#caseFollowersEmailGroupsDropdown").val() || [];
        for (var i = 0; i < selectedGroupIds.length; i++) {
            var arr = jQuery.grep(emailGroups,
                function(a) {
                    return a.Id == selectedGroupIds[i];
                });
            for (var j = 0; j < arr[0].Emails.length; j++) {
                checkAndAddEmailsFromDropdown(arr[0].Emails[j]);
            }
        }
    }

    function appendWorkingGroupEmails() {
        var selectedGroupIds = $("#caseFollowersWorkingGroupsDropdown").val() || [];
        for (var i = 0; i < selectedGroupIds.length; i++) {
            var arr = jQuery.grep(workingGroups,
                function (a) {
                    return a.Id == selectedGroupIds[i];
                });
            for (var j = 0; j < arr[0].Emails.length; j++) {
                checkAndAddEmailsFromDropdown(arr[0].Emails[j]);
            }
        }
    }

    function appendAdminEmails() {
        var selectedAdministratorEmails = $("#caseFollowersAdministratorsDropdown").val() || [];
        for (var i = 0; i < selectedAdministratorEmails.length; i++) {
            checkAndAddEmailsFromDropdown(selectedAdministratorEmails[i]);
        }
    }

    mainFakeFollowersInput.typeahead(getCasesAddFollowersSearchOptions());

    mainFakeFollowersInput.keyup(function (e) {
        if (e.keyCode === 13) {
            onEnterKeyUp(e, mainFakeFollowersInput);
        }
    });

    mainFakeFollowersInput.keydown(function (e) {
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, mainFakeFollowersInput, mainFollowersInput);
        }
    });

    popupFollowersInput.typeahead(getCasesAddFollowersSearchOptions());

    popupFollowersInput.keyup(function (e) {
        if (e.keyCode === 13) {
            onEnterKeyUp(e, popupFollowersInput);
        }
    });

    popupFollowersInput.keydown(function (e) {
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, popupFollowersInput, mainFollowersInput);
        }
    });

    function onEnterKeyUp(e, fakeInput) {
        if (e.keyCode === 13) {
            e.preventDefault();
            e.stopImmediatePropagation();
            var emails = $(e.target).val();
            var arr = emails.split(";");
            var newEmail = arr[arr.length - 1].replace("\n", "");
            if (newEmail.trim() !== "" && checkAndAddEmailsTo(newEmail)) {
                fakeInput.val(emails.replace("\n", "") + ";");
            }
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

    function getCasesAddFollowersSearchOptions() {
        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.split(";");
                var searchText = arr[arr.length - 1];
                var lastInitiatorSearchKey = generateRandomKey();
                return $.ajax({
                    url: "/cases/CaseSearchUserEmails",
                    type: "POST",
                    data: { query: searchText, searchKey: lastInitiatorSearchKey },
                    dataType:"json",
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
                var arr = this.query.split(";");
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
                if (emailsToAdd.length > 0)
                    return this.$element.val().replace(/[^;]*$/, "") + emailsToAdd.join(";") + ";";
                return this.$element.val().replace(/[^;]*$/, "");
            }
        };

        return options;
    }

    function checkAndAddEmailsTo(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + ";";
                if (mainFollowersInput.val().indexOf(newToEmail) < 0)
                    mainFollowersInput.val(mainFollowersInput.val() + newToEmail);
                else {
                    ShowToastMessage(value + " : " + window.parameters.emailAlreadyAdded, "warning");
                    return false;
                }
            return true;
        } else {
            ShowToastMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }
    }

    function checkAndAddEmailsFromDropdown(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + ";";
            if (mainFollowersInput.val().indexOf(newToEmail) < 0) {
                mainFollowersInput.val(mainFollowersInput.val() + newToEmail);
                popupFollowersInput.val(popupFollowersInput.val() + newToEmail);
            } else {
                return false;
            }
            return true;
        } else {
            ShowToastMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }
    }

    function changeFakeInputValueForView() {
        var fakeInput = $("#fakeCaseFollowerUsersInput");
        var text = mainFollowersInput.val();
        fakeInput.val(text);
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

    function dropdownDeselectAll() {
        $(".case-usersearch-multiselect").each(function () {
            var select = $(this);
            $("option", select).each(function () {
                select.multiselect("deselect", $(this).val());
            });
        });
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

    function isValidEmailAddress(emailAddress) {
        var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
        return pattern.test(emailAddress);
    };


}