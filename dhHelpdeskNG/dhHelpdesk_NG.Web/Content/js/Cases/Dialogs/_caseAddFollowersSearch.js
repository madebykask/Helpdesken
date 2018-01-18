function InitCaseAddFollowersSearch(admins, emailGroups, workingGroups) {

    var mainFollowersInput = $("#caseFollowerUsersInput");
    var mainFakeFollowersInput = $("#fakeCaseFollowerUsersInput");
    var popupFollowersInput = $("#caseAddFollowersModalInput");
    var searchSelected = false;

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

    mainFakeFollowersInput.typeahead(getUserSearchOptions(mainFollowersInput, mainFakeFollowersInput, popupFollowersInput));

    mainFakeFollowersInput.keydown(function (e) {
        if (e.keyCode === 8 || e.keyCode === 46 ) {
            onRemoveKeyDown(e, mainFakeFollowersInput, mainFollowersInput);
        }
        if (e.keyCode === 13 || e.keyCode === 186 || e.keyCode === 32) {
            onEnterKeyUp(e, mainFakeFollowersInput, mainFollowersInput);
        }
    });

    mainFakeFollowersInput.on('blur',
        function (e) {
            onEnterKeyUp(e, mainFakeFollowersInput, mainFollowersInput);
        });

    popupFollowersInput.typeahead(getUserSearchOptions(mainFollowersInput, mainFakeFollowersInput, popupFollowersInput));

    popupFollowersInput.keydown(function (e) {
        if (e.keyCode === 8 || e.keyCode === 46) {
            onRemoveKeyDown(e, popupFollowersInput, mainFollowersInput);
        }
        if (e.keyCode === 13 || e.keyCode === 186 || e.keyCode === 32) {
            onEnterKeyUp(e, popupFollowersInput, mainFollowersInput);
        }
    });

    popupFollowersInput.on('blur',
        function (e) {
            onEnterKeyUp(e, popupFollowersInput, mainFollowersInput);
        });

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