$(function () {
    $("#ordertypes_dropdown").on("change", function () {
        $("#OrderFieldSettingsSearchForm").submit();
    });

    $("#SaveOrderFieldSettings").on("click", function() {
        $("#SaveSettingsForm").submit();
    });
});