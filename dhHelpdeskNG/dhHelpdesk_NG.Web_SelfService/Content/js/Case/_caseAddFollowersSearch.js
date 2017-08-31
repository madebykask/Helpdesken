var _searchUriPath = "";
function InitCaseAddFollowersSearch(searchUriPath) {

    _searchUriPath = searchUriPath;
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

    $("#case_add_followers_popup").on('hidden.bs.modal', function (e) {
        changeFakeInputValueForView();        
    });

    $("#case_add_followers_popup").on("shown", function () {
        popupFollowersInput.focus();
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

    

    mainFakeFollowersInput.typeahead(getCasesAddFollowersSearchOptions());

    mainFakeFollowersInput.keyup(function (e) {
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, mainFakeFollowersInput);
        }
    });

    mainFakeFollowersInput.keydown(function (e) {
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, mainFakeFollowersInput, mainFollowersInput);
        }
        if (e.keyCode === 46) {
            onDeleteKeyDown(e, mainFakeFollowersInput, mainFollowersInput);
        }
    });

    popupFollowersInput.typeahead(getCasesAddFollowersSearchOptions());

    popupFollowersInput.keyup(function (e) {
        if (e.keyCode === 13 || e.keyCode === 186) {
            onEnterKeyUp(e, popupFollowersInput);
        }
    });

    popupFollowersInput.keydown(function (e) {
        if (e.keyCode === 8) {
            onBackspaceKeyDown(e, popupFollowersInput, mainFollowersInput);
            popupFollowersInput.focus();
        }
        if (e.keyCode === 46) {
            onDeleteKeyDown(e, popupFollowersInput, mainFollowersInput);
            popupFollowersInput.focus();
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

    function generateRandomKey() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + '-' + s4() + '-' + s4();
    }

    var lastInitiatorSearchKey;

    function getCasesAddFollowersSearchOptions() {
        var options = {
            items: 20,
            minLength: 2,
            source: function (query, process) {
                var arr = query.split(";");
                var searchText = $.trim(arr[arr.length - 1]);
                lastInitiatorSearchKey = generateRandomKey();
                return $.ajax({
                    url: searchUriPath,
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
                                    name: item.FirstName + " " + item.SurName,
                                    email: item.Emails,
                                    groupType: item.GroupType,
                                    name_family: item.SurName + " " + item.FirstName
                                };
                                return JSON.stringify(aItem);
                            });
                        if (resultList.length === 0) {
                            var noRes = {
                                name: window.parameters.noResultLabel,
                                isNoResult: true
                            }
                            resultList.push(JSON.stringify(noRes));
                        }
                        return process(resultList);
                    }
                });
            },

            matcher: function (obj) {
                var item = JSON.parse(obj);
                if (~item.isNoResult) {
                    return 1;
                }
                var tquery = getSimpleQuery(this.query);
                if (!tquery) return false;
                if (~item.email) {
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
                var grType = window.parameters.initGroup + ": ";
                var userId = item.userId != null ? item.userId + ' - ' : "";
                var query = getSimpleQuery(this.query);
                var result = item.name + " - " + userId + item.email;
                var resultNameFamily = item.name_family + " - " + userId + item.email;
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
                if (item.isNoResult) {
                    return "";
                }
                var emailsToAdd = [];
                $.each(item.email, function (index, value) {
                    if (checkAndAddEmailsTo(value) === true)
                        emailsToAdd.push(value);
                });
                if (emailsToAdd.length > 0)
                    return this.$element.val().replace(/[^; ]*$/, "") + emailsToAdd.join("; ") + "; ";
                return this.$element.val().replace(/[^;]*$/, " ");
            }
        };

        return options;
    }

    function checkAndAddEmailsTo(value) {
        if (isValidEmailAddress(value)) {
            var newToEmail = value + "; ";
                if (mainFollowersInput.val().indexOf(newToEmail) < 0)
                    mainFollowersInput.val(mainFollowersInput.val() + newToEmail).change();
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
            var newToEmail = value + "; ";
            if (mainFollowersInput.val().indexOf(newToEmail) < 0) {
                mainFollowersInput.val(mainFollowersInput.val() + newToEmail).change();
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
        var text = mainFollowersInput.val();        
        mainFakeFollowersInput.val(text);
    }

}