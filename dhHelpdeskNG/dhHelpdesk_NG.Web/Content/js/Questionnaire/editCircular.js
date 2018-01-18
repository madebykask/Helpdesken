function InitEditCircular() {

    var mainInput = $("#ExtraEmails");
    var mainFakeInput = $("#fakeCircularExtraEmailsInput");
    var searchSelected = false;

    $("#subbut").click(function () {
        if (!$("#circular_new_form").valid()) {
            return;
        }
        $.each($('#circular_case_grid_form input').serializeArray(), function (i, obj) {
            $('<input type="hidden">').prop(obj).appendTo($('#circular_new_form'));
        });
        $('#circular_case_filter_form input:not(:hidden), select').each(function (i, obj) {
            $(obj).appendTo($('#hiddenDiv'));
        });

        $("#circular_new_form").submit();
    });

    $("#async_subbut").click(function () {
        $("#circular_case_filter_form").submit();
    });

    mainFakeInput.html(getHtmlFromEmails(mainInput.val()));
    mainFakeInput.typeahead(getUserSearchOptions(mainInput, mainFakeInput));

    initEditableDiv();

    mainFakeInput.keydown(function (e) {
        if (e.keyCode === 8 || e.keyCode === 46) {
            onRemoveKeyDown(e, mainFakeInput, mainInput);
        }
        if (e.keyCode === 13 || e.keyCode === 186 || e.keyCode === 32) {
            onEnterKeyUp(e, mainFakeInput, mainInput);
        }
    });

    mainFakeInput.on("blur",
        function(e) {
            onEnterKeyUp(e, mainFakeInput, mainInput);
        });
}