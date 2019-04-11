

function InitCaseAddFollowersSearch(searchUriPath) {

    var KeyCodes = {
        Backspace: 8,
        Delete: 46,
        Enter: 13,
        Space: 32,
        SemiColon: 186
    };

    var searchSelected = false;
    
    var mainFollowersInput = $("#caseFollowerUsersInput");
    var mainFakeFollowersInput = $("#fakeCaseFollowerUsersInput");
    var popupFollowersInput = $("#caseAddFollowersModalInput");

    initEditableDiv();

    //set up typeahead
    var options = getCasesAddFollowersSearchOptions();
    mainFakeFollowersInput.typeahead(options);
    popupFollowersInput.typeahead(options);
    
    changeFakeInputValueForView(mainFakeFollowersInput);

    $("a[href='#case_add_followers_btn']").on("click", function () {
        var $src = $(this);
        var $target = $("#case_add_followers_popup");

        changeFakeInputValueForView(popupFollowersInput);

        $target.attr("data-src", $src.attr("data-src"));
        $target.modal("show");
    });

    $("#case_add_followers_popup").on('hidden.bs.modal', function (e) {
        changeFakeInputValueForView(mainFakeFollowersInput);
    });

    $("#case_add_followers_popup").on("shown", function () {
        placeCaretAtEnd(popupFollowersInput);
    });

    mainFakeFollowersInput.keydown(function (e) {
        var keyCode = e.keyCode;
        //console.log('>>> key: ', e.key, '; keyCode: ', e.keyCode);

        if (keyCode === KeyCodes.Backspace || keyCode === KeyCodes.Delete) {
            onRemoveKeyDown(e, mainFakeFollowersInput, mainFollowersInput);
            popupFollowersInput.focus();
        }
        
        if (keyCode === KeyCodes.Enter || keyCode === KeyCodes.SemiColon || keyCode === KeyCodes.Space) {
            processEmails(e);
        }
    });

    mainFakeFollowersInput.on('blur', function (e) {
        var relatedTarget = e.relatedTarget || document.activeElement;
        if ($(relatedTarget).parents('ul.typeahead.dropdown-menu').length === 0) {
            processEmails(e);
        }
    });
    
    // popup follower input
    popupFollowersInput.keydown(function (e) {
        var keyCode = e.keyCode;
        if (keyCode === KeyCodes.Backspace || keyCode === KeyCodes.Delete) {
            onRemoveKeyDown(e, popupFollowersInput, mainFollowersInput);
            popupFollowersInput.focus();
        }

        if (keyCode === KeyCodes.Enter || keyCode === KeyCodes.SemiColon || keyCode === KeyCodes.Space) {
            processEmails(e);
        }
    });

    popupFollowersInput.on('blur', function (e) {
        var relatedTarget = e.relatedTarget || document.activeElement;
        if ($(relatedTarget).parents('ul.typeahead.dropdown-menu').length === 0) {
            processEmails(e);
        }
    });
    
    function processEmails(e) {
        var keyCode = e.keyCode;
        if (keyCode === KeyCodes.Enter && searchSelected)
            return;
        
        e.preventDefault();
        e.stopImmediatePropagation();
        
        if (keyCode === KeyCodes.Enter || keyCode === KeyCodes.SemiColon ||
            keyCode === KeyCodes.Space || e.type === 'blur') {

            var $target = $(e.target);
            var html = $target.html();
            var emails = getEmailsFromHtml(html);
            
            if (emails.length > 0) {
                var newEmail = (emails[emails.length - 1] || '').trim();
                if (newEmail !== '' && isNewEmail(newEmail)) {
                    checkAndAddEmailsTo(newEmail);
                }
            } else {
                //empty hidden field if all emails have been deleted
                mainFollowersInput.val('');
            }
            
            changeFakeInputValueForView($target);

            if (e.type !== 'blur')
                placeCaretAtEnd($target);
        }
    }
  
    var lastInitiatorSearchKey;

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

    function getCasesAddFollowersSearchOptions() {
        var delayFunc = CommonUtils.createDelayFunc();

        var options = {
            items: 20,
            minLength: 2,

            source: function (query, process) {
                var arr = query.replace(/<[^>]*>/g, "").split(";");
                var searchText = $.trim(arr[arr.length - 1]);
                if (searchText) {
                    lastInitiatorSearchKey = CommonUtils.generateRandomKey();
                    delayFunc(function() {
                        $.ajax({
                            url: searchUriPath,
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
                                            name_family: item.SurName + " " + item.FirstName
                                        };
                                        return JSON.stringify(aItem);
                                    });

                                if (resultList.length > 0) {
                                    searchSelected = true;
                                } else {
                                    var noRes = {
                                        name: window.parameters.noResultLabel,
                                        isNoResult: true
                                    }
                                    resultList.push(JSON.stringify(noRes));
                                    searchSelected = false;
                                }

                                process(resultList);
                            }
                        });
                    }, 300);
                }
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
                    return;
                }
                
                $.each(item.email, function (index, value) {
                    checkAndAddEmailsTo(value);
                });

                changeFakeInputValueForView(this.$element);
                placeCaretAtEnd(this.$element);
                return;
            }
        };

        return options;
    }

    function checkAndAddEmailsTo(value) {
        value = (value || '').trim();
        if (!isValidEmailAddress(value)) {
            ShowToastMessage(value + " : " + window.parameters.emailNotValid, "error");
            return false;
        }

        var newToEmail = value + "; ";
        if (~mainFollowersInput.val().indexOf(newToEmail)) {
            ShowToastMessage(value + " : " + window.parameters.emailAlreadyAdded, "warning");
            return false;
        }

        var existingEmails = (mainFollowersInput.val() || '').trim();
        mainFollowersInput.val(existingEmails + newToEmail).change();
        return true;
    }

    function changeFakeInputValueForView($el) {
        var val = mainFollowersInput.val();
        var markup = getHtmlFromEmails(val);
        $el.html(markup);
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

    function getEmailsFromHtml(html) {
        var result = [];
        var arr = html.replace(/<[^>]*>/g, "").replace('&nbsp', '').split(";");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] !== "")
                result.push(arr[i].trim());
        }
        return result;
    }

    function onRemoveKeyDown(e, fakeInput, mainInput) {
        e.stopImmediatePropagation();
        var text = mainInput.val();
        var email = getEmailsToRemove();
        console.log('>>> Emails to remove: ', email);
        if (email === "&nbsp;" || email.trim() === "") {
            if (e.keyCode !== KeyCodes.Delete) {
                var arr = text.split(";");
                var lastEmail = arr[arr.length - 2];
                if (lastEmail) {
                    text = text.replace(lastEmail + ";", "");
                    mainInput.val(text);
                    changeFakeInputValueForView(fakeInput);
                    placeCaretAtEnd(fakeInput);
                    e.preventDefault();
                }
            } else {
                e.preventDefault();
            }

        } else if (email !== "" && email.indexOf(";") >= 0) {
            text = text.replace(email, "");
            mainInput.val(text);
            changeFakeInputValueForView(fakeInput);
            placeCaretAtEnd(fakeInput);
            e.preventDefault();
        }
    }

    function getEmailsToRemove() {
        var selection = window.getSelection();
        var parentNode = $(selection.anchorNode.parentNode);
        if (parentNode.html() === "&nbsp;") {
            return "&nbsp;";
        } else {
            return $(parentNode.text().split(";")).last()[0];
            //return selection.anchorNode.textContent;
        }
    }
    
    function isValidEmailAddress(emailAddress) {
        var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
        return pattern.test(emailAddress.trim());
    };

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

    function isNewEmail(newEmail) {
        var emails = (mainFollowersInput.val() || '').replace(" ", '').split(';');
        return emails.indexOf(newEmail) === -1;
    }
}