
$(function() {

    var onSearchReport = function() {
        $("#search_report_form").submit();
    };

    $("[data-report-type]").change(function() {
        onSearchReport();
    });

    onSearchReport();

    $("#reports_show").click(function() {
        var form = $("#report_form");
        form.find("[data-isprint]").val("false");
        $.post(form.attr("action"), form.serialize())
            .done(function(data) {
                $("#report_container").html(data);
            });
        return false;
    });

    $("#reports_print").click(function () {
        var form = $("#report_form");
        form.find("[data-isprint]").val("true");
        form.submit();
    });
});