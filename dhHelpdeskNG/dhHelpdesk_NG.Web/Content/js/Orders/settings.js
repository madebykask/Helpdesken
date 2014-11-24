$(function () {
    $("#ordertypes_dropdown").change(function () {
        $("#OrderFieldSettingsSearchForm").submit();
    });

    $("#SaveOrderFieldSettings").click(function() {
        $("#SaveSettingsForm").submit();
    });
});