$(function () {

    if (!window.dhHelpdesk) {
        window.dhHelpdesk = {};
    }

    if (!window.dhHelpdesk.reports) {
        window.dhHelpdesk.reports = {};
    }

    dhHelpdesk.reports.reportType = {
        LeadtimeFinishedCases: 2,
        LeadtimeActiveCases: 3,
        FinishingCauseCustomer: 4,
        FinishingCauseCategoryCustomer: 19,
        ClosedCasesDay: 5,
        RegistratedCasesDay: 6,
        RegistratedCasesHour: 21,
        CasesInProgressDay: 7,
        ServiceReport: 8,
        RegistratedCasesCaseType: 17,
        CaseTypeArticleNo: 14,
        AverageSolutionTime: 13,
        RegistrationSource: 20,
        ResponseTime: 22,
        ReportGenerator: 18,
        CaseSatisfaction: 23
    }

    dhHelpdesk.reports.utils = {
        buildHandler:  function(url, params) {
            var handler = url + '?';
            for (var i = 0; i < params.length; i += 2) {
                handler += params[i] + '=' + params[i + 1] + '&';
            }
            return handler;
        }
    }

    dhHelpdesk.reports.reportsManager = function (spec, my) {
        my = my || {};
        var that = {};

        var getCurrentReport = function (type) {
            switch (type) {
                case dhHelpdesk.reports.reportType.RegistratedCasesDay:
                    return dhHelpdesk.reports.registratedCasesDayReport();
                default:
                    return null;
            }
        }

        that.getCurrentReport = getCurrentReport;

        return that;
    }

    dhHelpdesk.reports.report = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var baseReportsUrl = spec.baseReportsUrl || '/Reports/Report/';
        var reportContent = spec.reportContent || $('#reportContainer');

        my.baseReportsUrl = baseReportsUrl;
        my.reportContent = reportContent;

        return that;
    }

    dhHelpdesk.reports.registratedCasesDayReport = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report(spec, my);

        var getReportHandler = function () {

            var department = $("#DepartmentId").val();
            var caseTypes = '';
            $("#CaseTypeIds option:selected").each(function() {
                caseTypes += $(this).val() + ',';
            });
            var workingGroup = $("#WorkingGroupId").val();
            var administrator = $("#AdministratorId").val();
            var period = $("#Period").val();

            return dhHelpdesk.reports.utils.buildHandler(my.baseReportsUrl + 'GetRegistratedCasesDayReport', [
                                        'department', department,
                                        'caseTypes', caseTypes,
                                        'workingGroup', workingGroup,
                                        'administrator', administrator,
                                        'period', period]);
        }

        var buildReport = function() {
            my.reportContent.html('<img src="' + getReportHandler() + '" />');
        }

        that.getReportHandler = getReportHandler;
        that.buildReport = buildReport;

        return that;
    }

    var onGetReportOptions = function () {
        $("#getReportOptionsForm").submit();
    }

    var reportType = $("#ReportId");
    reportType.change(function () {
        onGetReportOptions();
    });

    onGetReportOptions();

    var manager = dhHelpdesk.reports.reportsManager();

    $("#showReport").click(function() {
        var report = manager.getCurrentReport(parseInt(reportType.val()));

        report.buildReport();
    });
});