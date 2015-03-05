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
        },

        raiseEvent: function (eventType, extraParameters) {
            $(document).trigger(eventType, extraParameters);
        },

        onEvent: function (event, handler) {
            $(document).on(event, handler);
        }
    }

    dhHelpdesk.reports.reportsManager = function (spec, my) {
        my = my || {};
        var that = {};

        var workingGroupUsersRoute = spec.workingGroupUsersRoute || '';

        var getCurrentReport = function (type) {
            switch (type) {
                case dhHelpdesk.reports.reportType.RegistratedCasesDay:
                    return dhHelpdesk.reports.registratedCasesDayReport({
                        workingGroupUsersRoute: workingGroupUsersRoute
                    });
                case dhHelpdesk.reports.reportType.CaseTypeArticleNo:
                    return dhHelpdesk.reports.caseTypeArticleNoReport();
                case dhHelpdesk.reports.reportType.CaseSatisfaction:
                    return dhHelpdesk.reports.caseSatisfactionReport();
                case dhHelpdesk.reports.reportType.ReportGenerator:
                    return dhHelpdesk.reports.reportGeneratorReport();
                case dhHelpdesk.reports.reportType.LeadtimeFinishedCases:
                    return dhHelpdesk.reports.leadtimeFinishedCases();
                case dhHelpdesk.reports.reportType.LeadtimeActiveCases:
                    return dhHelpdesk.reports.leadtimeActiveCases();
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

        var getCanPrint = function () {
            return canPrint;
        }

        var getCanExcel = function () {
            return canExcel;
        }

        var isPrint = $('#IsPrint');
        var isExcel = $('#IsExcel');
        var reportForm = $('#reportForm');
        var showReportLoader = $('#showReportLoader');
        var reportContainer = $("#reportContainer");

        var buildReport = function () {
            reportForm.validate();
            if (!reportForm.valid()) {
                return false;
            }

            isPrint.val(false);
            isExcel.val(false);
            showReportLoader.show();
            $.post(reportForm.attr("action"), reportForm.serialize())
            .done(function (data) {
                reportContainer.html(data);
            }).always(function () {
                showReportLoader.hide();
            });

            return false;
        }

        var excelReport = function () {
            isPrint.val(false);
            isExcel.val(true);
            reportForm.submit();
        }

        var printReport = function () {
            isPrint.val(true);
            isExcel.val(false);
            reportForm.submit();
        }

        that.getCanPrint = getCanPrint;
        that.getCanExcel = getCanExcel;
        that.buildReport = buildReport;
        that.printReport = printReport;
        that.excelReport = excelReport;

        my.baseReportsUrl = baseReportsUrl;
        my.reportContent = reportContent;

        return that;
    }

    dhHelpdesk.reports.registratedCasesDayReport = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report(spec, my);

        var workingGroupUsersRoute = spec.workingGroupUsersRoute || '';

        dhHelpdesk.reports.utils.onEvent('onLoadOptions', function () {
            var administrators = $("#AdministratorId");
            var workingGroups = $("#WorkingGroupId");
            var onWorkingGroupChanged = function () {
                administrators.prop('disabled', true);
                $.getJSON(workingGroupUsersRoute + '?workingGroupId=' + workingGroups.val(), function (data) {
                    administrators.empty();
                    administrators.append('<option />');
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        administrators.append("<option value='" + item.Value + "'>" + item.Name + "</option>");
                    }
                })
                .always(function () {
                    administrators.prop('disabled', false);
                });
            }
            workingGroups.off("change").on("change", onWorkingGroupChanged);
            onWorkingGroupChanged();
        });

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
            my.reportContent.html('<img src="' + getReportHandler() + '" />');
        }

        that.getReportHandler = getReportHandler;
        that.buildReport = buildReport;

        return that;
    }

    dhHelpdesk.reports.caseTypeArticleNoReport = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report({ canPrint: true, canExcel: true }, my);

        return that;
    }

    dhHelpdesk.reports.caseSatisfactionReport = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report({ canPrint: true }, my);

        return that;
    }

    dhHelpdesk.reports.reportGeneratorReport = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report({ canExcel: true }, my);

        return that;
    }

    dhHelpdesk.reports.leadtimeFinishedCases = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report({ }, my);

        return that;
    }

    dhHelpdesk.reports.leadtimeActiveCases = function (spec, my) {
        my = my || {};

        var that = dhHelpdesk.reports.report({ }, my);

        return that;
    }

    dhHelpdesk.reports.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var workingGroupUsersRoute = spec.workingGroupUsersRoute || '';

        var onLoadOptions = function () {
            dhHelpdesk.reports.utils.raiseEvent('onLoadOptions');
        }

        that.onLoadOptions = onLoadOptions;

        var reportType = $("#ReportId");
        var btnShow = $("#showReport");
        var btnPrint = $('#printReport');
        var btnExcel = $('#excelReport');
        var manager = dhHelpdesk.reports.reportsManager({
            workingGroupUsersRoute: workingGroupUsersRoute
        });

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

        btnShow.click(function () {
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

        return that;
    }
});