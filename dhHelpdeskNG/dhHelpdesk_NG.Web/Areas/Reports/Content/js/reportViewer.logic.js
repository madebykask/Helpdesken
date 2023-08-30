$(function () {
    (function ($) {

        window.Params = window.Params || {};
        var showReportUrl = window.Params.ShowReportUrl;
        var showGeneratedReportUrl = window.Params.ShowGeneratedReportUrl;
        var saveFiltersUrl = window.Params.SaveFiltersUrl;
        var getReportFilterOptionsUrl = window.Params.GetReportFilterOptionsUrl;
        var deleteReportFavoriteUrl = window.Params.DeleteReportFavoriteUrl;
        var getExtendedCaseFormFields = window.Params.GetExtendedCaseFormFields;
        var stackByDefaultValue = '5';

        var reportList = "#lstReports";
        var administratorDropDown = "#lstfilterAdministrator";
        var caseCreateFrom = "#ReportFilter_CaseCreationDate_FromDate";
        var caseCreateTo = "#ReportFilter_CaseCreationDate_ToDate";
        var departmentDropDown = "#lstfilterDepartment";
        var workingGroupDropDown = "#lstfilterWorkingGroup";
        var historicalWorkingGroupDropDown = '#historicalWorkingGroups';
        var caseTypeDropDown = "#lstfilterCaseType";
        var productAreaDropDown = "#lstfilterProductArea";
        var currentCustomerId = window.Params.CurrentCustomerId;
        var statusList = "#lstStatus";
        var reportGeneratorFields = "#reportGeneratorCaseFields";
        var caseCloseFrom = "#ReportFilter_CaseClosingDate_FromDate";
        var caseCloseTo = "#ReportFilter_CaseClosingDate_ToDate";
        var caseLogNoteDateFrom = '#ReportFilter_LogNoteDate_FromDate';
        var caseLogNoteDateTo = '#ReportFilter_LogNoteDate_ToDate';
        var reportCategoryDropdown = "#lstfilterReportCategory";
        var reportCategoryDropdownRt = "#lstfilterReportCategoryRt";
        var reportCategoryDropdownSt = "#lstfilterReportCategory_SolvedInTime";
        var changeDateFrom = "#ReportFilter_CaseChangeDate_FromDate";
        var changeDateTo = "#ReportFilter_CaseChangeDate_ToDate";
        var extendedCaseReport = ".reportGeneratorExtendedCase";
        var extendedCaseForms = "#reportGeneratorExtendedCaseForms";
        var extendedCaseFormFields = "#reportGeneratorExtendedCaseFormFields";
        var lstExtendedCaseForms = "#lstExtendedCaseForms";
        var lstExtendedCaseFormFields = "#lstExtendedCaseFormFields";
        var lstGroupBy = '#lstGroupBy';
        var lstStackBy = '#lstStackBy';

        var filterExtendedCaseFieldIds = [];

        // Make disabled (inactive) options selectable 
        $(lstExtendedCaseForms + ' option').each(function (i, opt) {
            if ($(opt).prop('disabled'))
            {
                $(opt).removeAttr('disabled');
                $(opt).css('color', 'lightgray');
            }
        });


        $(lstExtendedCaseFormFields).multiselect('buildSelectAll');

        var extendedCaseFormChanged = function (e) {
            var id = $(lstExtendedCaseForms).val();
            if (id != "" && id != null) {
                $.ajax({
                    url: getExtendedCaseFormFields,
                    data: { extendedCaseFormId: id },
                    type: 'POST',
                    success: function (d) {
                        $(extendedCaseFormFields).show();
                        options = $(lstExtendedCaseFormFields).multiselect()[0].options;
                        while (options.length != 0)
                            options.remove(0);

                        var values = $.each(d, function (e, val) {
                            var selected = filterExtendedCaseFieldIds.indexOf(val.FieldId) >= 0;
                            var option = new Option(val.Text, val.FieldId, selected, selected);
                            options.add(option);
                        });
                        $(lstExtendedCaseFormFields).multiselect('rebuild');
                        $(lstExtendedCaseFormFields).multiselect('buildDropdown');
                        
                    }
                });
            }
            else {
                $(extendedCaseFormFields).hide();
                $(lstExtendedCaseFormFields).val('');
            }
        };

        $(lstExtendedCaseForms).on('change', extendedCaseFormChanged);

        var reportObjNames = {};
        reportObjNames[dhHelpdesk.reports.reportType.ReportedTime] = 'reportedTimeReport';
        reportObjNames[dhHelpdesk.reports.reportType.NumberOfCases] = 'numberOfCasesReport';
        reportObjNames[dhHelpdesk.reports.reportType.SolvedInTime] = 'solvedInTimeReport';

        window.dhHelpdesk = window.dhHelpdesk || {};
        window.dhHelpdesk.reports = window.dhHelpdesk.reports || {};
        window.dhHelpdesk.reports.errorClass = 'error';

        var getBaseFilters = function () {
            var filters = {
                departments: [],
                workingGroups: [],
                caseTypes: [],
                productAreas: [],
                fields: [],
                extendedCaseFormId: null,
                extendedCaseFormFields: [],
                administrators: [],
                caseStatuses: [],
                regDateFrom: null,
                regDateTo: null,
                closeDateFrom: null,
                closeDateTo: null
            };

            $(departmentDropDown + " option:selected").each(function () {
                filters.departments.push($(this).val());
            });

            $(workingGroupDropDown + " option:selected").each(function () {
                filters.workingGroups.push($(this).val());
            });

            $(caseTypeDropDown + " option:selected").each(function () {
                filters.caseTypes.push($(this).val());
            });

            $(administratorDropDown + " option:selected").each(function () {
                filters.administrators.push($(this).val());
            });

            $(statusList + " option:selected").each(function () {
                filters.caseStatuses.push($(this).val());
            });

            $(productAreaDropDown + " option:selected").each(function () {
                filters.productAreas.push($(this).val());
            });

            $("#lstFields option:selected").each(function () {
                var v = $(this).val();
                filters.fields.push(v);
            });

            filters.extendedCaseFormId = $("#lstExtendedCaseForms").val();

            $("#lstExtendedCaseFormFields option:selected").each(function () {
                filters.extendedCaseFormFields.push($(this).val());
            });

            filters.regDateFrom = $(caseCreateFrom).val();
            filters.regDateTo = $(caseCreateTo).val();

            filters.closeDateFrom = $(caseCloseFrom).val();
            filters.closeDateTo = $(caseCloseTo).val();

            return filters;
        };

        dhHelpdesk.reports.reportType = dhHelpdesk.reports.reportType || {
            ReportGenerator: 18
        };

        dhHelpdesk.reports.applyFilterV1 = function (filters) {
            var applyChosen = function (selector, values) {
                var $control = $(selector);
                $control
                    .find("option")
                    .prop("selected", false);
                $control.val(values);
                $control.trigger("chosen:updated");
            }

            var applyMultiSelect = function (selector, values) {
                var $control = $(selector);
                $control
                    .find("option")
                    .prop("selected", false);
                $control.val(values);
                $control.multiselect("refresh");
            }

            var applyDropDown = function (selector, values) {
                var $control = $(selector);
                var options = $control
                    .find("option");
                options.prop("selected", false);
                if (values.length <= 0) {
                    options.first().prop("selected", true);
                } else {
                    $control.val(values);
                }
            }

            applyChosen(departmentDropDown, filters.departments);
            applyChosen(workingGroupDropDown, filters.workingGroups);
            // applyChosen(historicalWorkingGroupDropDown, filters.historicalWorkingGroups);
            applyChosen(administratorDropDown, filters.administrators);
            applyChosen(caseTypeDropDown, filters.caseTypes);
            applyChosen(productAreaDropDown, filters.productAreas);

            // Save selected extended case fields (for use after retrieved)
            filterExtendedCaseFieldIds = filters.extendedCaseFormFields;

            // Set extended case form
            applyChosen(lstExtendedCaseForms, filters.extendedCaseFormId);

            // Trigger change of extended case form to retrieve fields for form
            $(lstExtendedCaseForms).change();

            //applyMultiSelect(lstExtendedCaseFormFields, filters.extendedCaseFormFields);

            applyMultiSelect("#lstFields", filters.fields);
            applyDropDown(statusList, filters.caseStatuses);

            $(caseCreateFrom).val(filters.regDateFrom);
            $(caseCreateTo).val(filters.regDateTo);

            $(caseCloseFrom).val(filters.closeDateFrom);
            $(caseCloseTo).val(filters.closeDateTo);

            $(reportCategoryDropdown).val(filters.reportCategory);
            $(reportCategoryDropdownRt).val(filters.reportCategoryRt);
            $(reportCategoryDropdownSt).val(filters.reportCategorySt);

            $(lstGroupBy).val(filters.groupBy);
            $(lstStackBy).val(filters.stackBy);
            $(caseLogNoteDateFrom).val(filters.logNoteDateFrom);
            $(caseLogNoteDateTo).val(filters.logNoteDateTo);
            $(changeDateFrom).val(filters.historicalChangeDateFrom);
            $(changeDateTo).val(filters.historicalChangeDateTo);

            applyChosen(historicalWorkingGroups, filters.historicalWorkingGroups);
        };

        dhHelpdesk.reports.loadFilter = function (filterId) {
            $("#showReportLoader").show();
            $.ajax(
            {
                url: getReportFilterOptionsUrl,
                type: "GET",
                traditional: true,
                data: { id: filterId },
                dataType: "json"
            })
            .done(function (data) {
                if (!data) {
                    return;
                }
                var filters = $.parseJSON(data.Filters);
                switch (filters.version) {
                    case "1":
                    case "2":
                        {
                            dhHelpdesk.reports.applyFilterV1(filters);
                        }
                    default:
                        return;
                }
            })
            .always(function () {
                $("#showReportLoader").hide();
            });

        };

        dhHelpdesk.reports.onGeneratedShow = function () {
            if (!dhHelpdesk.reports.doValidation())
                return;

            var filters = getBaseFilters();

            var reportName = $(reportList).find("option:selected").data("identity");

       
            var reportTypeId = $('#lstReports option:selected').attr("data-orig-report-id");
            if (!reportTypeId)
                reportTypeId = $('#lstReports option:selected').attr("data-id");

            var getDataForParams = {
                ReportTypeId: reportTypeId,
                DepartmentIds: filters.departments,
                WorkingGroupIds: filters.workingGroups,
                CaseTypeIds: filters.caseTypes,
                AdministratorsIds: filters.administrators,
                CaseStatusIds: filters.caseStatuses,
                ProductAreaIds: filters.productAreas,
                PeriodFrom: filters.regDateFrom,
                PeriodUntil: filters.regDateTo,
                FieldIds: filters.fields,
                ExtendedCaseFormId: filters.extendedCaseFormId,
                ExtendedCaseFormFieldIds: filters.extendedCaseFormFields,
                IsExcel: $(this).data("excel") || false,
                IsPreview: $(this).data("preview") || false,
                SortName: "",
                SortBy: "",
                CloseFrom: filters.closeDateFrom,
                CloseTo: filters.closeDateTo,
                ReportName: reportName
            };


            var getParams = $.param(getDataForParams, true);


            var isPreview = ($(this).data("preview") === true);
            if ($(this).data("excel") === true) {

                $("#showReportLoader").show();

                fetch(showGeneratedReportUrl, {
                    method: 'POST',
                    body: JSON.stringify(getDataForParams),
                    headers: {
                        'Content-Type': 'application/json'  // specifying how we're sending the data
                    }

                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("Network response was not ok");
                        }

                        // Extract filename from the Content-Disposition header
                        const contentDisposition = response.headers.get('Content-Disposition');
                        let filename = 'default_filename.xlsx';  // Default filename if not found in header

                        if (contentDisposition) {
                            const filenameMatch = contentDisposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/);
                            if (filenameMatch && filenameMatch[1]) {
                                filename = filenameMatch[1].replace(/['"]/g, '');  // Remove quotes if present
                            }
                        }

                        return response.blob().then(blob => ({ blob, filename }));
                    })
                    .then(data => {
                        var downloadUrl = URL.createObjectURL(data.blob);
                        var downloadLink = document.createElement('a');
                        downloadLink.href = downloadUrl;
                        downloadLink.download = data.filename;
                        document.body.appendChild(downloadLink);
                        downloadLink.click();
                        document.body.removeChild(downloadLink);

                        // Hide the loader.
                        $("#showReportLoader").hide();
                    })
                    .catch(error => {
                        console.log("Error:", error);
                        $("#showReportLoader").hide();
                    });

            } else {
                $("#showReportLoader").show();
                $.ajax(
                {
                    url: showGeneratedReportUrl,
                    type: "POST",
                    traditional: true,
                    data: getParams,
                    dataType: "html",
                    success: function (htmlData) {
                        if (isPreview) {                      
                            if ($(htmlData).find('#showrun').val() === 'true') {                           
                                dhHelpdesk.reports.togglePreviewMode(false);
                            }
                            else {
                                $("#excelReport").each(function () { this.disabled = false; });
                            }
                        }
                        $("#generateReportContainer").html(htmlData);
                    },
                    complete: function () {
                        $("#showReportLoader").hide();
                    }
                });
            }
        };

        dhHelpdesk.reports.resetErrors = function () {
            const errorClass = window.dhHelpdesk.reports.errorClass;

            const $createDateFrom = $(caseCreateFrom);
            const $createDateTo = $(caseCreateTo);
            const $closeDateFrom = $(caseCloseFrom);
            const $closeDateTo = $(caseCloseTo);
            const $changeDateFrom = $(changeDateFrom);
            const $changeDateTo = $(changeDateTo);
            const $logNoteDateFrom = $(caseLogNoteDateFrom);
            const $logNoteDateTo = $(caseLogNoteDateTo);

            $createDateFrom.removeClass(errorClass);
            $createDateTo.removeClass(errorClass);
            $closeDateFrom.removeClass(errorClass);
            $closeDateTo.removeClass(errorClass);
            $changeDateFrom.removeClass(errorClass);
            $changeDateTo.removeClass(errorClass);
            $logNoteDateFrom.removeClass(errorClass);
            $logNoteDateTo.removeClass(errorClass);
        }

        dhHelpdesk.reports.doValidation = function (isRegDateRequiredOnly) {

            isRegDateRequiredOnly = isRegDateRequiredOnly !== false; // default true

            const $createDateFrom = $(caseCreateFrom);
            const $createDateTo = $(caseCreateTo);
            const $closeDateFrom = $(caseCloseFrom);
            const $closeDateTo = $(caseCloseTo);
            const $changeDateFrom = $(changeDateFrom);
            const $changeDateTo = $(changeDateTo);
            const $logNoteDateFrom = $(caseLogNoteDateFrom);
            const $logNoteDateTo = $(caseLogNoteDateTo);

            dhHelpdesk.reports.resetErrors();

            const validatePair = function($from, $to) {
                const isFromEmpty = $from.val() === '';
                const isToEmpty = $to.val() === '';
                var errorControls = [];
                if (isFromEmpty && isToEmpty) {
                    errorControls.push($from);
                    errorControls.push($to);
                } else if (isFromEmpty && !isToEmpty) {
                    errorControls.push($from);
                } else if (!isFromEmpty && isToEmpty) {
                    errorControls.push($to);
                }
                return errorControls;
            };

            var emptyControls = [];
            var invalidControls = [];
            if (isRegDateRequiredOnly) {
                if (!$createDateFrom.is(':hidden') && !$createDateFrom.is(':disabled'))
                    emptyControls.push(validatePair($createDateFrom, $createDateTo));
            } else {
                if (!$createDateFrom.is(':hidden') && !$createDateFrom.is(':disabled'))
                    emptyControls.push(validatePair($createDateFrom, $createDateTo));
                if (!$closeDateFrom.is(':hidden') && !$closeDateFrom.is(':disabled'))
                    emptyControls.push(validatePair($closeDateFrom, $closeDateTo));
                if (!$changeDateFrom.is(':hidden') && !$changeDateFrom.is(':disabled'))
                    emptyControls.push(validatePair($changeDateFrom, $changeDateTo));
                if (!$logNoteDateFrom.is(':hidden') && !$logNoteDateFrom.is(':disabled'))
                    emptyControls.push(validatePair($logNoteDateFrom, $logNoteDateTo));
            }

            if (emptyControls.every(function(e) { // all empty
                return e.length === 2;
            })) {
                emptyControls.forEach(function (e) { invalidControls = invalidControls.concat(e); });
            } else { // only pairs where 1 field is filled
                emptyControls.filter(function(e) {
                    return e.length === 1;
                }).forEach(function(e) {
                     invalidControls = invalidControls.concat(e);
                }); 
            }

            var isValid = invalidControls.length === 0;
            if (!isValid) {
                invalidControls.forEach(function(e) {
                    e.addClass(window.dhHelpdesk.reports.errorClass);
                });
                ShowToastMessage(window.Params.DateIsEmptyMessage, 'warning');
            }
            
            return isValid;
        }

        function getFilters() {
            var customer = '';
            var deps_OUs = '';
            var workingGroup = '';
            var administrator = '';
            var caseType = '';
            var productArea = '';
            var status = '';
            var reportCategory = '';
            var reportCategoryRt = '';
            var reportCategorySt = '';

            customer = currentCustomerId;

            $(departmentDropDown + ' option:selected').each(function () {
                deps_OUs += $(this).val() + ',';
            });

            $(workingGroupDropDown + ' option:selected').each(function () {
                workingGroup += $(this).val() + ',';
            });

            $(administratorDropDown + ' option:selected').each(function () {
                administrator += $(this).val() + ',';
            });

            $(caseTypeDropDown + ' option:selected').each(function () {
                caseType += $(this).val() + ',';
            });

            $(productAreaDropDown + ' option:selected').each(function () {
                productArea += $(this).val() + ',';
            });

            status = $(statusList + ' option:selected').val();

            reportCategory = $(reportCategoryDropdown + ' option:selected').val();
            reportCategoryRt = $(reportCategoryDropdownRt + ' option:selected').val();
            reportCategorySt = $(reportCategoryDropdownSt + ' option:selected').val();

            var regDateFrom = $(caseCreateFrom).val();
            var regDateTo = $(caseCreateTo).val();

            var closeDateFrom = $(caseCloseFrom).val();
            var closeDateTo = $(caseCloseTo).val();


            return {
                'customer': customer,
                'deps_OUs': deps_OUs,
                'workingGroup': workingGroup,
                'administrator': administrator,
                'caseType': caseType,
                'productArea': productArea,
                'status': status,
                'regDateFrom': regDateFrom,
                'regDateTo': regDateTo,
                'closeDateFrom': closeDateFrom,
                'closeDateTo': closeDateTo,
                'reportCategory': reportCategory,
                'reportCategoryRt': reportCategoryRt,
                'reportCategorySt': reportCategorySt
            }
        }

        function getCommonFilter() {
            var departments = [];
            var workingGroups = [];
            var administrators = [];
            var caseTypes = [];
            var productAreas = [];

            $(departmentDropDown + ' option:selected').each(function () {
                departments.push($(this).val());
            });

            $(workingGroupDropDown + ' option:selected').each(function () {
                workingGroups.push($(this).val());
            });

            $(administratorDropDown + ' option:selected').each(function () {
                administrators.push($(this).val());
            });

            $(caseTypeDropDown + ' option:selected').each(function () {
                caseTypes.push($(this).val());
            });

            $(productAreaDropDown + ' option:selected').each(function () {
                productAreas.push($(this).val());
            });

            var regDateFrom = $(caseCreateFrom).val();
            var regDateTo = $(caseCreateTo).val();

            var closeDateFrom = $(caseCloseFrom).val();
            var closeDateTo = $(caseCloseTo).val();

            var logNoteDateFrom = $(caseLogNoteDateFrom).val();
            var logNoteDateTo = $(caseLogNoteDateTo).val();

            var groupBy = null;
            var $reportCategory = $(reportCategoryDropdown);
            var $reportCategoryRt = $(reportCategoryDropdownRt);
            var $reportCategorySt = $(reportCategoryDropdownSt);

            if ($reportCategory.is(':visible')) {
                groupBy = $reportCategory.val();
            } else if ($reportCategoryRt.is(':visible')) {
                groupBy = $reportCategoryRt.val();
            } else if ($reportCategorySt.is(':visible')) {
                groupBy = $reportCategorySt.val();
            }

            var status = $(statusList + ' option:selected').val();

            return {
                'departments': departments,
                'workingGroups': workingGroups,
                'administrators': administrators,
                'caseTypes': caseTypes,
                'productAreas': productAreas,
                'regDateFrom': regDateFrom,
                'regDateTo': regDateTo,
                'closeDateFrom': closeDateFrom,
                'closeDateTo': closeDateTo,
                'logNoteDateFrom': logNoteDateFrom,
                'logNoteDateTo': logNoteDateTo,
                'status': status,
                'groupBy': groupBy
            };

        }

        function getHistoricalFilters() {
            var historicalWorkingGroups = [];

            $(historicalWorkingGroupDropDown + ' option:selected').each(function () {
                historicalWorkingGroups.push($(this).val());
            });

            var groupBy = $(lstGroupBy).val();
            var stackBy = $(lstStackBy).val();

            var historicalChangeDateFrom = $(changeDateFrom).val();
            var historicalChangeDateTo = $(changeDateTo).val();
            var commonFilter = getCommonFilter();

            return $.extend(commonFilter, {
                'historicalChangeDateFrom': historicalChangeDateFrom,
                'historicalChangeDateTo': historicalChangeDateTo,
                'historicalWorkingGroups': historicalWorkingGroups,
                'groupBy': groupBy,
                'stackBy': stackBy
            });
        }

        dhHelpdesk.reports.getAllFiltersForFavorite = function(reportId) {
            var baseFilters = getBaseFilters(); // legacy favorites support TODO: fix mess with a lot of filters methods and namings 
            var filters = $.extend(baseFilters,
                {
                    reportCategory: null,
                    reportCategoryRt: null,
                    reportCategorySt: null,
                    groupBy: null,
                    stackBy: null,
                    logNoteDateFrom: null,
                    logNoteDateTo: null,
                    historicalChangeDateFrom: null,
                    historicalChangeDateTo: null,
                    historicalWorkingGroups: []
                });
            filters.reportCategory = $(reportCategoryDropdown).val();
            filters.reportCategoryRt = $(reportCategoryDropdownRt).val();
            filters.reportCategorySt = $(reportCategoryDropdownSt).val();
            filters.groupBy = $(lstGroupBy).val();
            filters.stackBy = $(lstStackBy).val();
            filters.logNoteDateFrom = $(caseLogNoteDateFrom).val();
            filters.logNoteDateTo = $(caseLogNoteDateTo).val();
            filters.historicalChangeDateFrom = $(changeDateFrom).val();
            filters.historicalChangeDateTo = $(changeDateTo).val();

            var historicalWorkingGroups = [];
            $(historicalWorkingGroupDropDown + ' option:selected').each(function () {
                historicalWorkingGroups.push($(this).val());
            });
            filters.historicalWorkingGroups = historicalWorkingGroups;
            return filters;

        }

        dhHelpdesk.reports.onShowReport = function (e) {
            var control$ = $('#lstReports').find('option:selected');
            var origReportId = control$.data("origReportId");
            var reportId = origReportId || control$.data('id');

            switch (reportId) {
                case dhHelpdesk.reports.reportType.HistoricalReport: 
                    dhHelpdesk.reports.onHistoricalShow.call(this, e);
                    break;
                case dhHelpdesk.reports.reportType.ReportedTime:
                case dhHelpdesk.reports.reportType.NumberOfCases:
                case dhHelpdesk.reports.reportType.SolvedInTime: 
                    dhHelpdesk.reports.onJSReportShow.call(this, e, reportObjNames[reportId]);
                    break;
                default:
                    dhHelpdesk.reports.onOtherShow.call(this, e);
                    break;
            }
        }

        dhHelpdesk.reports.onHistoricalShow = function (e) {
            if (!dhHelpdesk.reports.doValidation(false))
                return;

            dhHelpdesk.reports.historicalReport.show();
            dhHelpdesk.reports.historicalReport.update(getHistoricalFilters());
        }

        dhHelpdesk.reports.onJSReportShow = function (e, name) {
            if (!dhHelpdesk.reports.doValidation(false))
                return;

            dhHelpdesk.reports[name].show();
            dhHelpdesk.reports[name].update(getCommonFilter());
        }

        dhHelpdesk.reports.onOtherShow = function(e) {

            if (!dhHelpdesk.reports.doValidation())
                return;

            var origReportId = $(reportList).find("option:selected").data("origReportId");
            var isSavedFilter = typeof origReportId !== "undefined";

            var reportName = isSavedFilter ? origReportId : $(reportList + " option:selected").val();

            var params = getFilters();

            $.get(showReportUrl,
                {
                    reportName: reportName,
                    'filter.Customers': params.customer,
                    'filter.Deps_OUs': params.deps_OUs,
                    'filter.WorkingGroups': params.workingGroup,
                    'filter.Administrators': params.administrator,
                    'filter.CaseTypes': params.caseType,
                    'filter.ProductAreas': params.productArea,
                    'filter.CaseStatus': params.status,
                    'filter.RegisterFrom': params.regDateFrom,
                    'filter.RegisterTo': params.regDateTo,
                    'filter.CloseFrom': params.closeDateFrom,
                    'filter.CloseTo': params.closeDateTo,
                    'filter.ReportCategory': params.reportCategory,
                    'filter.ReportCategoryRt': params.reportCategoryRt,
                    'filter.ReportCategorySt': params.reportCategorySt,
                    curTime: new Date().getTime()
                },
                function (reportPresentation) {
                    $("#reportPresentationArea").html(reportPresentation);
                }
            );
        };

        dhHelpdesk.reports.onReportChange = function (e) {
            var origReportId = $(this).find("option:selected").data("origReportId");
            var isSavedFilter = typeof origReportId !== "undefined";
            var selectedId = $(this).find("option:selected").data("id");
            var reportId = isSavedFilter ? origReportId : selectedId;

            var $extraParameters = $("#reportCategoryParam");
            var $btnPreview = $("#btnPreviewReport");
            var $btnShow = $("#showReport");
            var $btnSaveFilter = $("#btnSaveFilter");
            var $btnSaveAsFilter = $("#btnSaveAsFilter");
            var $btnExcel = $("#excelReport");
            var $btnShowReport = $("#btnShowReport");
            var $reportGeneratorFields = $("#reportGeneratorFields");
            var $reportGeneratorExtendedCaseForms = $("#reportGeneratorExtendedCaseForms");
            var $reportGeneratorExtendedCase = $(".reportGeneratorExtendedCase");
            var $lstExtendedCaseForms = $("#lstExtendedCaseForms");
            var $reportGeneratorExtendedCaseFormFields = $("#reportGeneratorExtendedCaseFormFields");
            var $lstExtendedCaseFormFields = $("#lstExtendedCaseFormFields");
            var $otherReportsContainer = $("#otherReportsContainer");
            var $generateReportContainer = $("#generateReportContainer");
            var $jsReportContainer = $("#jsReportContainer");
            var $fieldsSelect = $("#lstFields");
            var $stackBy = $('#lstStackBy');
            var $groupBy = $('#groupBy');
            var $logNoteDateFields = $('#logNoteDateFields');
            var $historicalFilters = $('#historicalFilters');
            var $statusFilter = $('#lstStatus');
            var $caseRegistrationFromDate = $('#CaseRegistrationFromDate');
            var $caseRegistrationToDate = $('#CaseRegistrationToDate');
            var $caseClosingFromDate = $('#CaseClosingFromDate');
            var $caseClosingToDate = $('#CaseClosingToDate');

            var forceDateFrom = new Date();
            forceDateFrom.setMonth(forceDateFrom.getMonth() - 1);
            var forceDateTo = new Date();

            var historicalReportControls = [$btnShowReport, $groupBy, $jsReportContainer, $historicalFilters];

            dhHelpdesk.reports.togglePreviewMode(true);
            dhHelpdesk.reports.resetErrors();

            $lstExtendedCaseForms.val("");
            $lstExtendedCaseFormFields.val("");

            if (reportId === dhHelpdesk.reports.reportType.ReportGenerator || reportId === dhHelpdesk.reports.reportType.ReportGeneratorExtendedCase) {
                $btnPreview.show();
                $btnShow.show();
                $btnExcel.show();
                $extraParameters.show();
                $historicalFilters.hide();
                $btnShowReport.hide();
                $reportGeneratorFields.find('select option').prop('selected', false);
                $fieldsSelect.multiselect('refresh');
                $reportGeneratorFields.show();
                $otherReportsContainer.hide();
                $logNoteDateFields.hide();
                dhHelpdesk.reports.historicalReport.hide();
                $jsReportContainer.hide();
                $stackBy.val('');
                $stackBy.prop('disabled', true);
                $caseRegistrationFromDate.find('input').prop('disabled', false);               
                $caseRegistrationToDate.find('input').prop('disabled', false);
                $caseRegistrationFromDate.datepicker({
                    format: 'yyyy-mm-dd',
                    clearBtn: true,
                    todayHighlight: true,
                    weekStart: 1
                });
                $caseRegistrationToDate.datepicker({
                    format: 'yyyy-mm-dd',
                    clearBtn: true,
                    todayHighlight: true,
                    weekStart: 1
                });
                $caseClosingFromDate.datepicker('setDate', null);
                $caseClosingToDate.datepicker('setDate', null);
                $statusFilter.prop('disabled', false);
                $statusFilter.val('');
                $groupBy.hide();
                $generateReportContainer.html('');
                $generateReportContainer.show();

                if (reportId === dhHelpdesk.reports.reportType.ReportGeneratorExtendedCase) {

                    $reportGeneratorExtendedCase.show();
                    $caseRegistrationFromDate.datepicker({
                        format: 'yyyy-mm-dd',
                        clearBtn: true,
                        todayHighlight: true,
                        weekStart: 1
                    });
                    $caseRegistrationToDate.datepicker({
                        format: 'yyyy-mm-dd',
                        clearBtn: true,
                        todayHighlight: true,
                        weekStart: 1
                    });
                    $caseClosingFromDate.datepicker('setDate', null);
                    $caseClosingToDate.datepicker('setDate', null);
                    $lstExtendedCaseForms.change();
                }
                else {

                    $reportGeneratorExtendedCase.hide();
                    $reportGeneratorExtendedCaseForms.val('');
                    $reportGeneratorExtendedCaseFormFields.hide();
                    $reportGeneratorExtendedCaseFormFields.val('');
                }


            } else if (reportId === dhHelpdesk.reports.reportType.HistoricalReport) {
                $btnPreview.hide();
                $btnShow.hide();
                $btnExcel.hide();
                $reportGeneratorFields.hide();
                $generateReportContainer.hide();
                $otherReportsContainer.hide();
                $extraParameters.hide();
                $logNoteDateFields.hide();
                $caseRegistrationFromDate.find('input').prop('disabled', false);                
                $caseRegistrationFromDate.datepicker({
                    format: 'yyyy-mm-dd',
                    clearBtn: true,
                    todayHighlight: true,
                    weekStart: 1
                });
                $caseRegistrationToDate.find('input').prop('disabled', false);                
                $caseRegistrationToDate.datepicker({
                    format: 'yyyy-mm-dd',
                    clearBtn: true,
                    todayHighlight: true,
                    weekStart: 1
                });
                $caseClosingFromDate.datepicker('setDate', null);
                $caseClosingToDate.datepicker('setDate', null);
                $.each(historicalReportControls, function (i, v) { v.show(); });
                $stackBy.val(stackByDefaultValue);
                $stackBy.prop('disabled', false);
                window.dhHelpdesk.reports.historicalReport.init();
            } else {
                $btnPreview.hide();
                $btnShow.hide();
                $btnExcel.hide();
                $btnShowReport.show();
                $extraParameters.show();
                $reportGeneratorFields.hide();
                $('#reportPresentationArea').html('');
                $generateReportContainer.hide();
                $caseRegistrationFromDate.find('input').prop('disabled', false);                
                $caseRegistrationFromDate.datepicker({
                    format: 'yyyy-mm-dd',
                    clearBtn: true,
                    todayHighlight: true,
                    weekStart: 1
                });
                $caseRegistrationToDate.find('input').prop('disabled', false);                
                $caseRegistrationToDate.datepicker({
                    format: 'yyyy-mm-dd',
                    clearBtn: true,
                    todayHighlight: true,
                    weekStart: 1
                });
                $caseClosingFromDate.datepicker('setDate', null);
                $caseClosingToDate.datepicker('setDate', null);
                $statusFilter.prop('disabled', false);
                $statusFilter.val('');
                $stackBy.val('');
                $stackBy.prop('disabled', true);
                $groupBy.hide();
                dhHelpdesk.reports.historicalReport.hide();
                $historicalFilters.hide();
                $logNoteDateFields.hide();
                $jsReportContainer.hide();
                $otherReportsContainer.show();
                if (reportId === dhHelpdesk.reports.reportType.ReportedTime) {
                    $caseRegistrationFromDate.datepicker({
                        format: 'yyyy-mm-dd',
                        clearBtn: true,
                        todayHighlight: true,
                        weekStart: 1
                    });
                    $caseRegistrationToDate.datepicker({
                        format: 'yyyy-mm-dd',
                        clearBtn: true,
                        todayHighlight: true,
                        weekStart: 1
                    });
                    $caseClosingFromDate.datepicker('setDate', null);
                    $caseClosingToDate.datepicker('setDate', null);
                    $logNoteDateFields.show();
                    $otherReportsContainer.hide();
                    $jsReportContainer.show();
                    window.dhHelpdesk.reports[reportObjNames[reportId]].init();
                }
                if (reportId === dhHelpdesk.reports.reportType.NumberOfCases) {
                    $caseRegistrationFromDate.datepicker({
                        format: 'yyyy-mm-dd',
                        clearBtn: true,
                        todayHighlight: true,
                        weekStart: 1
                    });
                    $caseRegistrationToDate.datepicker({
                        format: 'yyyy-mm-dd',
                        clearBtn: true,
                        todayHighlight: true,
                        weekStart: 1
                    });
                    $caseClosingFromDate.datepicker('setDate', null);
                    $caseClosingToDate.datepicker('setDate', null);
                    $otherReportsContainer.hide();
                    $jsReportContainer.show();
                    window.dhHelpdesk.reports[reportObjNames[reportId]].init();
                }
                if (reportId === dhHelpdesk.reports.reportType.SolvedInTime) {
                    $statusFilter.val(dhHelpdesk.reports.statuses.ClosedCases);
                    $statusFilter.prop('disabled', true);
                    $caseRegistrationFromDate.datepicker('setDate', null);
                    $caseRegistrationToDate.datepicker('setDate', null);
                    $caseRegistrationFromDate.find('input').prop('disabled', true);
                    $caseRegistrationFromDate.datepicker('remove');
                    $caseRegistrationToDate.find('input').prop('disabled', true);
                    $caseRegistrationToDate.datepicker('remove');
                    $caseClosingFromDate.datepicker("setDate", forceDateFrom);
                    $caseClosingToDate.datepicker("setDate", forceDateTo);
                    $jsReportContainer.show();
                    window.dhHelpdesk.reports[reportObjNames[reportId]].init();
                }                
            }

            if (isSavedFilter) {
                dhHelpdesk.reports.loadFilter(selectedId);
                $btnSaveFilter.show();
                $btnSaveAsFilter.show();
                $("#btnDeleteFavorite").show();
            } else {
                //show/hide proper buttons
                filterExtendedCaseFieldIds = [];
                $btnSaveFilter.hide();
                $btnSaveAsFilter.show();
                $("#btnDeleteFavorite").hide();
            }
            
            dhHelpdesk.reports.showExtraParameters(reportId);
        };

        dhHelpdesk.reports.deleteFavorite = function () {
            var $reports = $("#lstReports");
            var selected = $reports.find("option:selected");
            var favoriteId = selected.data("id");
            $("#showReportLoader").show();
            $.ajax({
                type: "POST",
                data: {
                    id: favoriteId
                },
                url: deleteReportFavoriteUrl
            })
                .done(function (data) {
                    $reports.val("");
                    selected.remove();
                    $reports.trigger("change");
                })
                .always(function () {
                    $("#showReportLoader").hide();
                });
        };

        dhHelpdesk.reports.onSave = function (e, data, saveAs) {
            var currentVersion = "2";
            saveAs = saveAs || false;
            var $reportsControl = $("#lstReports");
            var selectedReport = $reportsControl.find("option:selected");
            var name = saveAs ? "" : selectedReport.val();
            var optionTemplate = "<option value='{name}' data-id='{id}' data-orig-report-id='{origReportId}'>{name}</option>";

            var $modal = $("#saveFilterDialog");

            $modal.off("show").on("show", function () {
                $modal.find("#requiredFilterText").hide();
                $modal.find("#filterDialogError").hide();
                $modal.find("#txtFilterName").val(name);

                if (saveAs) {
                    $modal.find("#saveAsCaption").show();
                    $modal.find("#saveCaption").hide();
                    $modal.find("#saveBody").hide();
                    $modal.find("#saveAsBody").show();
                } else {
                    $modal.find("#saveAsCaption").hide();
                    $modal.find("#saveCaption").show();
                    $modal.find("#saveBody").show();
                    $modal.find("#saveAsBody").hide();
                }
            });

            $modal.find("#btnSaveFilter").off("click").on("click", function (e) {
                var filters = dhHelpdesk.reports.getAllFiltersForFavorite();
                filters.version = currentVersion;
                var id = saveAs ? null : selectedReport.data("id"); //if saving new - no id
                var originalReportId = selectedReport.data("origReportId") ? selectedReport.data("origReportId") : selectedReport.data("id");
                if (saveAs) {
                    var nameControl = $modal.find("#txtFilterName");
                    name = nameControl.val();
                    if (name.length <= 0) {
                        nameControl.focus();
                        $modal.find("#requiredFilterText").show();
                        return;
                    }
                }

                $("#showReportLoader").show();
                $.ajax({
                    type: "POST",
                    data: JSON.stringify({
                        Id: id,
                        OriginalReportId: originalReportId,
                        Name: name,
                        Filters: JSON.stringify(filters)
                    }),
                    url: saveFiltersUrl,
                    contentType: "application/json"
                })
                    .done(function (data) {
                        if (data < 0) {
                            $modal.find("#filterDialogError").show();
                        } else {
                            if (saveAs) {
                                var newOption = optionTemplate.replace(/{name}/g, name).replace("{id}", data).replace("{origReportId}", originalReportId);
                                $reportsControl.append(newOption);
                                var options = $reportsControl.find("option");
                                options.prop("selected", false);
                                options.last().prop("selected", true);
                                dhHelpdesk.reports.onReportChange.call($reportsControl[0]);
                            }
                            $modal.modal("hide");
                        }
                    })
                    .always(function () {
                        $("#showReportLoader").hide();
                    });

            });

            $modal.modal();
        };

        dhHelpdesk.reports.togglePreviewMode = function (state) {
            $("#showReport").each(function () { this.disabled = state; });
            $("#excelReport").each(function () { this.disabled = state; });
        }

        dhHelpdesk.reports.showExtraParameters = function (reportId) {
  
            switch (reportId) { 
                case dhHelpdesk.reports.reportType.NumberOfCases:
                    $("#lstfilterReportCategory").show();
                    $("#lstfilterReportCategoryRt").hide();
                    $("#lstfilterReportCategory_repl").hide();
                    $("#lstfilterReportCategory_SolvedInTime").hide();
                    break;

                case dhHelpdesk.reports.reportType.ReportedTime:
                    $("#lstfilterReportCategory").hide();
                    $("#lstfilterReportCategoryRt").show();
                    $("#lstfilterReportCategory_repl").hide();
                    $("#lstfilterReportCategory_SolvedInTime").hide();
                    break;
                case dhHelpdesk.reports.reportType.SolvedInTime:
                    $("#lstfilterReportCategory").hide();
                    $("#lstfilterReportCategoryRt").hide();
                    $("#lstfilterReportCategory_repl").hide();
                    $("#lstfilterReportCategory_SolvedInTime").show();
                    break;
                default:
                    $("#lstfilterReportCategory").hide();
                    $("#lstfilterReportCategoryRt").hide();
                    $("#lstfilterReportCategory_SolvedInTime").hide();
                    $("#lstfilterReportCategory_repl").show();
                    break;
            }
        }
      
        dhHelpdesk.reports.init = function () {

            var showReportButton = $("#btnShowReport");

            dhHelpdesk.reports.onReportChange.call(document.getElementById("lstReports"));

            $("#lstReports").on("change", dhHelpdesk.reports.onReportChange);


            $(showReportButton).on("click", dhHelpdesk.reports.onShowReport);
            $("#btnDeleteFavorite").on('click', dhHelpdesk.reports.deleteFavorite);

            $("#showReport, #excelReport, #btnPreviewReport").on("click", dhHelpdesk.reports.onGeneratedShow);
            $("#btnSaveFilter").on("click", function (e, d) { return dhHelpdesk.reports.onSave.call(this, e, d, false) });
            $("#btnSaveAsFilter").on("click", function (e, d) { return dhHelpdesk.reports.onSave.call(this, e, d, true) });
        
            $("#lstStatus, #lstfilterAdministrator, #lstfilterDepartment, #lstfilterWorkingGroup, #lstfilterCaseType, #lstfilterProductArea, #CaseRegistrationFromDate, #CaseRegistrationToDate")
                .on("change", function (e, d) { return dhHelpdesk.reports.togglePreviewMode(true); });

            $(".chosen-select").chosen({
                width: "300px",
                'placeholder_text_multiple': placeholder_text_multiple,
                'no_results_text': no_results_text
            });

            $(".report-chosen-single-select").chosen({
                width: "300px",
                'placeholder_text_multiple': placeholder_text_multiple,
                'no_results_text': no_results_text
            });

        }

    })($);


    dhHelpdesk.reports.init();
});
