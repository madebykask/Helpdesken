
$(function() {

    var onSearchReport = function() {
        $("#search_report_form").submit();
    };

    $("[data-report-type]").change(function() {
        onSearchReport();
    });

    onSearchReport();
});