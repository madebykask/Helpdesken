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
                case dhHelpdesk.reports.reportType.CaseTypeArticleNo:
                    return dhHelpdesk.reports.caseTypeArticleNoReport();
                case dhHelpdesk.reports.reportType.CaseSatisfaction:
                    return dhHelpdesk.reports.caseSatisfactionReport();
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
        var canPrint = spec.canPrint || false;
        var canExcel = spec.canExcel || false;

        var getCanPrint = function() {
            return canPrint;
        }

        var getCanExcel = function() {
            return canExcel;
        }

        that.getCanPrint = getCanPrint;
        that.getCanExcel = getCanExcel;

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

        var buildReport = function () {
            var form = $('#reportForm');
            form.validate();
            if (!form.valid()) {
                return false;
            }

            my.reportContent.html('<img src="' + getReportHandler() + '" />');
        }

        that.getReportHandler = getReportHandler;
        that.buildReport = buildReport;

        return that;
    }

    dhHelpdesk.reports.caseTypeArticleNoReport = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report({ canPrint: true, canExcel: true }, my);

        var buildReport = function () {
            $('#IsPrint').val(false);
            $('#IsExcel').val(false);
            var form = $('#reportForm');
            $.post(form.attr("action"), form.serialize())
            .done(function (data) {
                $("#reportContainer").html(data);
            });
            return false;
        }

        var excelReport = function () {
            $('#IsPrint').val(false);
            $('#IsExcel').val(true);
            $('#reportForm').submit();
        }

        var printReport = function () {
            $('#IsPrint').val(true);
            $('#IsExcel').val(false);
            $('#reportForm').submit();
        }

        that.buildReport = buildReport;
        that.printReport = printReport;
        that.excelReport = excelReport;

        return that;
    }

    dhHelpdesk.reports.caseSatisfactionReport = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report({ canPrint: true }, my);

        var buildReport = function () {
            $('#IsPrint').val(false);
            var form = $('#reportForm');
            $.post(form.attr("action"), form.serialize())
            .done(function (data) {
                $("#reportContainer").html(data);
            });
            return false;
        }

        var printReport = function () {
            $('#IsPrint').val(true);
            $('#reportForm').submit();
        }

        that.buildReport = buildReport;
        that.printReport = printReport;

        return that;
    }

    var reportType = $("#ReportId");
    var btnPrint = $('#printReport');
    var btnExcel = $('#excelReport');
    var manager = dhHelpdesk.reports.reportsManager();

    var onGetReportOptions = function () {
        var report = manager.getCurrentReport(parseInt(reportType.val()));

        if (report.getCanPrint()) {
            btnPrint.show();
        } else {
            btnPrint.hide();
        }

        if (report.getCanExcel()) {
            btnExcel.show();
        } else {
            btnExcel.hide();
        }


        $("#getReportOptionsForm").submit();
    }

    reportType.change(function () {
        onGetReportOptions();
    });

    onGetReportOptions();

    $("#showReport").click(function() {
        var report = manager.getCurrentReport(parseInt(reportType.val()));

        report.buildReport();
    });

    btnPrint.click(function () {
        var report = manager.getCurrentReport(parseInt(reportType.val()));
        if (report.getCanPrint()) {
            report.printReport();
        }
    });

    btnExcel.click(function () {
        var report = manager.getCurrentReport(parseInt(reportType.val()));
        if (report.getCanExcel()) {
            report.excelReport();
        }
    });
});