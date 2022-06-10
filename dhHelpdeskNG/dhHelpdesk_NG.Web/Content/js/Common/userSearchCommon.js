
function SetFollowers() {
    var followers = $("#extraEmailsInput").val();
    $("#fakeExtraEmailsInput").html(getHtmlFromEmails(followers));
}

function InitExtraEmailsSearch(admins, emailGroups, workingGroups) {

    var mainInput = $("#extraEmailsInput");
    var mainFakeInput = $("#fakeExtraEmailsInput");
    var popupInput = $("#extraEmailsModalInput");

    initEditableDiv();

    $("a[href='#add_extra_emails_btn']").on("click", function () {
        var $src = $(this);
        var $target = $("#extra_emails_popup");
        popupInput.html(getHtmlFromEmails(mainInput.val()));
        $target.attr("data-src", $src.attr("data-src"));
        $target.modal("show");
    });

    $("#extra_emails_popup").on("hide", function () {
        $(".toast-container").removeClass("case-followers-toastmessage");
        changeFakeInputValueForView();
    });

    $("#extra_emails_popup").on("shown", function () {
        placeCaretAtEnd(popupInput);
    });

    $(".case-usersearch-multiselect").multiselect({
        enableFiltering: true,
        filterPlaceholder: "",
        maxHeight: 250,
        buttonClass: "btn",
        buttonContainer: '<span class="btn-group" />',
        buttonText: function (options) {
            return '-- <i class="caret"></i>';
        },
        onChange: function (element, checked) {
            if (element.parent().attr("id") === "extraEmailsEmailGroupsDropdown") {
                appendDropdownsEmails(emailGroups, element.val());
            }
            if (element.parent().attr("id") === "extraEmailsWorkingGroupsDropdown") {
                appendDropdownsEmails(workingGroups, element.val());
            }
            if (element.parent().attr("id") === "extraEmailsAdministratorsDropdown") {
                checkAndAddEmailsFromDropdown(element.val());
            }
        }

    });

    function appendDropdownsEmails(array, selectedId) {
        var arr = $.grep(array, function (a) {
            return a.Id == selectedId;
        });

        for (var j = 0; j < arr[0].Emails.length; j++) {
            checkAndAddEmailsFromDropdown(arr[0].Emails[j]);
        }
    }

    mainFakeInput.html(getHtmlFromEmails(mainInput.val()));
    mainFakeInput.typeahead(getUserSearchOptions(mainInput, mainFakeInput, popupInput));

    mainFakeInput.keydown(function(e) {
        if (e.which === 8 || e.which === 46) {
            onRemoveKeyDown(e, mainFakeInput, mainInput);
        }
    });

    mainFakeInput.keypress(function (e) {
        if (e.which === 13 || e.which === 186 || e.which === 59) {
            processEmails(e, mainFakeInput, mainInput);
        }
    });

    mainFakeInput.on('blur', function(e) {
        var relatedTarget = e.relatedTarget || document.activeElement;
        if ($(relatedTarget).parents('ul.typeahead.dropdown-menu').length === 0) {
            processEmails(e, mainFakeInput, mainInput);
        }
    });

    popupInput.typeahead(getUserSearchOptions(mainInput, mainFakeInput, popupInput));

    popupInput.keydown(function (e) {
        if (e.which === 8 || e.which === 46) {
            onRemoveKeyDown(e, popupInput, mainInput);
        }
    });

    popupInput.keypress(function(e) {
        if (e.which === 13 || e.which === 186 ||
            e.which === 59) {
            processEmails(e, popupInput, mainInput);
        }
    });

    popupInput.on('blur', function(e) {
        var relatedTarget = e.relatedTarget || document.activeElement;
        if ($(relatedTarget).parents('ul.typeahead.dropdown-menu').length === 0) {
            processEmails(e, popupInput, mainInput);
        }
    });

    function checkAndAddEmailsFromDropdown(value) {
        if (isValidEmailAddress(value)) {
            if (mainInput.val().indexOf(value) === -1) {
                mainInput.val(mainInput.val() + value + ";");
                popupInput.html(getHtmlFromEmails(mainInput.val()));
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
        var text = mainInput.val();
        mainFakeInput.html(getHtmlFromEmails(text));
    }

    function isNewEmail(newEmail, mainInput) {
        var emails = mainInput.val().split(';');
        return emails.indexOf(newEmail) < 0;
    }

    function processEmails(e, fakeInput, mainInput) {
        if (e.which === 13 && searchSelected)
            return;

        e.preventDefault();
        e.stopImmediatePropagation();

        if (e.which === 13 || e.which === 186 || e.type === 'blur' || e.which === 59) {
            var emails = $(e.target).html();
            var arr = getEmailsFromHtml(emails);
            for (var i = 0; i < arr.length; i++) {
                var newEmail = arr[i] || '';
                if (newEmail !== '' && newEmail !== '&nbsp' && isNewEmail(newEmail, mainInput)) {
                    checkAndAddEmailsTo(newEmail, mainInput);
                }
            }
            console.log(getExtraEmails(mainInput.val()));    
            fakeInput.html(getHtmlFromEmails(mainInput.val()));
            if (e.type !== 'blur') placeCaretAtEnd(fakeInput);
        }
    }
}

var searchSelected = false;

function onRemoveKeyDown(e, fakeInput, mainInput) {
    e.stopImmediatePropagation();
    var text = mainInput.val();
    var email = getEmailsToRemove();
    if (email === "&nbsp;" || email.trim() === "") {
        if (e.which !== 46) {
            var arr = text.split(";");
            var lastEmail = arr[arr.length - 2];
            if (lastEmail) {
                text = text.replace(lastEmail + ";", "");
                mainInput.val(text);
                fakeInput.html(getHtmlFromEmails(mainInput.val()));
                placeCaretAtEnd(fakeInput);
                e.preventDefault();
            }
        } else {
            e.preventDefault();
        }

    } else {
        if (email !== "" && email.indexOf(";") >= 0) {
            text = text.replace(email, "");
            mainInput.val(text);
            fakeInput.html(getHtmlFromEmails(mainInput.val()));
            placeCaretAtEnd(fakeInput);
            e.preventDefault();
        }
    }
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

function ShowToastModalMessage(msg, msgType) {
    ShowToastMessage(msg, msgType);
    $(".toast-container").addClass("case-followers-toastmessage");
}

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress.trim());
};

function getEmailsToRemove() {
    var selection = window.getSelection();
    var parentNode = $(selection.anchorNode.parentNode);
    if (parentNode.html() === "&nbsp;") {
        return "&nbsp;";
    } else {
        // return $(parentNode.text().split(";")).last()[0];
        return selection.anchorNode.textContent;
    }
}

function getHtmlFromEmails(emails) {
    if (emails == undefined) {
        return [];
    }
    var arr = emails.split(";");
    var result = [];
    for (var i = 0; i < arr.length - 1; i++) {
        result.push("<span class='case-email-selected'>" + arr[i] + ";</span>");
    }
    if (result.length > 0) result.push("<span>&nbsp</span>");

    return result.join("");
}
function getHtmlFromEmails(emails) {
    if (emails == undefined) {
        return [];
    }
    var arr = emails.split(";");
    var result = [];
    for (var i = 0; i < arr.length - 1; i++) {
        result.push("<span class='case-email-selected'>" + arr[i] + ";</span>");
    }
    if (result.length > 0) result.push("<span>&nbsp</span>");

    return result.join("");
}
function getExtraEmails(emails) {
    if (emails == undefined) {
        return [];
    }
    var arr = emails.split(";");
    var result = [];
    for (var i = 0; i < arr.length - 1; i++) {
        result.push( arr[i] + " ");
    }

    return result.join("");
}
function getEmailsFromHtml(html) {
    var result = [];
    var arr = html.replace(/<[^>]*>/g, "").split(";");
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] !== "")
            result.push(arr[i].trim());
    }
    return result;
}

function placeCaretAtEnd(node) {
    node[0].focus();
    var textNode = node[0].lastChild;
    if (textNode && $(textNode).text().length) {
        var range = document.createRange();
        range.setStart(textNode, 1);
        range.setEnd(textNode, 1);
        var sel = window.getSelection();
        sel.removeAllRanges();
        sel.addRange(range);
    }
}

function initEditableDiv() {
    var original = $.fn.val;
    $.fn.val = function () {
        if ($(this).is('*[contenteditable=true]')) {
            return $.fn.html.apply(this, arguments);
        };
        return original.apply(this, arguments);
    };
}

//this for case extra followers and circular extra emails
function getUserSearchOptions(mainInput, mainFakeInput, popupInput) {
    var lastAddFollowersSearchSearchKey = '';
    var delayFunc = CommonUtils.createDelayFunc();

    var options = {
        items: 20,
        minLength: 2,
        source: function (query, process) {
            var arr = query.replace(/<[^>]*>/g, "").split(";");
            var searchText = $.trim(arr[arr.length - 1]);
            if (searchText) {
                lastAddFollowersSearchSearchKey = generateRandomKey();
                delayFunc(function () {
                    $.ajax({
                        url: "/cases/CaseSearchUserEmails",
                        type: "POST",
                        data: { query: searchText, searchKey: lastAddFollowersSearchSearchKey },
                        dataType: "json",
                        success: function (result) {
                            if (result.searchKey !== lastAddFollowersSearchSearchKey) {
                                return;
                            }

                            var resultList = $.map(result.result,
                                function (item) {
                                    var aItem = {
                                        userId: item.UserId || '',
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
                            } else {
                                var noRes = {
                                    name: " -- ",
                                    isNoResult: true
                                }
                                if (document.parameters.noResultLabel) {
                                    noRes.name = document.parameters.noResultLabel;
                                } else {
                                    noRes.name = window.parameters.noResultLabel;
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
            if (item.groupType === 4)
                grType = document.parameters.usersLabel + ": ";

            var userId = (item.userId != null && item.userId !== '') ? item.userId + ' - ' : "";
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
                $.each(item.email, function (index, value) {
                    if (checkAndAddEmailsTo(value, mainInput) === true)
                        emailsToAdd.push(value);
                });
                mainFakeInput.html(getHtmlFromEmails(mainInput.val()));
                if (popupInput) {
                    popupInput.html(getHtmlFromEmails(mainInput.val()));
                }
                placeCaretAtEnd(this.$element);
            }
            return;
        }
    };

    return options;
}

function checkAndAddEmailsTo(value, mainInput) {
    if (isValidEmailAddress(value)) {
        var newToEmail = value;
        var emails = mainInput.val().split(";");
        if (emails.indexOf(newToEmail) < 0)
            mainInput.val(mainInput.val() + newToEmail + ";");
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


