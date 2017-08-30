$(function () {
    (function ($) {

        window.Params = window.Params || {};
        var showReportUrl = window.Params.ShowReportUrl;
        var showGeneratedReportUrl = window.Params.ShowGeneratedReportUrl;
        var saveFiltersUrl = window.Params.SaveFiltersUrl;
        var getReportFilterOptionsUrl = window.Params.GetReportFilterOptionsUrl;
        var deleteReportFavoriteUrl = window.Params.DeleteReportFavoriteUrl;

        var reportList = "#lstReports";
        var administratorDropDown = "#lstfilterAdministrator";
        var caseCreateFrom = "#ReportFilter_CaseCreationDate_FromDate";
        var caseCreateTo = "#ReportFilter_CaseCreationDate_ToDate";
        var departmentDropDown = "#lstfilterDepartment";
        var workingGroupDropDown = "#lstfilterWorkingGroup";
        var caseTypeDropDown = "#lstfilterCaseType";
        var productAreaDropDown = "#lstfilterProductArea";
        var currentCustomerId = window.Params.CurrentCustomerId;
        var statusList = "#lstStatus";
        var reportGeneratorFields = "#reportGeneratorFields";
        var caseCloseFrom = "#ReportFilter_CaseClosingDate_FromDate";
        var caseCloseTo = "#ReportFilter_CaseClosingDate_ToDate";

        /*specify all extra parameters element*/
        var $extraParameters = $("#reportCategoryParam");

        window.dhHelpdesk = window.dhHelpdesk || {};
        window.dhHelpdesk.reports = window.dhHelpdesk.reports || {};

        var getFilters = function () {
            var filters = {
                departments: [],
                workingGroups: [],
                caseTypes: [],
                productAreas: [],
                fields: [],
                administrators: [],
                caseStatuses: [],
                regDateFrom: null,
                regDateTo: null,
                closeDateFrom: null,
                closeDateTo : null
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

            $(reportGeneratorFields + " option:selected").each(function () {
                filters.fields.push($(this).val());
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
            applyChosen(administratorDropDown, filters.administrators);
            applyChosen(caseTypeDropDown, filters.caseTypes);
            applyChosen(productAreaDropDown, filters.productAreas);

            applyMultiSelect("#lstFields", filters.fields);
            applyDropDown(statusList, filters.caseStatuses);

            $(caseCreateFrom).val(filters.regDateFrom);
            $(caseCreateTo).val(filters.regDateTo);

            $(caseCloseFrom).val(filters.closeDateFrom);
            $(caseCloseTo).val(filters.closeDateTo);
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

            var filters = getFilters();
            
            var getParams = $.param({
                DepartmentIds: filters.departments,
                WorkingGroupIds: filters.workingGroups,
                CaseTypeIds: filters.caseTypes,
                AdministratorsIds: filters.administrators,
                CaseStatusIds: filters.caseStatuses,
                ProductAreaIds: filters.productAreas,
                PeriodFrom: filters.regDateFrom,
                PeriodUntil: filters.regDateTo,
                FieldIds: filters.fields,
                IsExcel: $(this).data("excel") || false,
                IsPreview: $(this).data("preview") || false,
                SortName: "",
                SortBy: "",
                CloseFrom: filters.closeDateFrom,
                CloseTo: filters.closeDateTo
            }, true);

            var isPreview = ($(this).data("preview") === true);
            if ($(this).data("excel") === true) {
                window.open(showGeneratedReportUrl + "?" + getParams, "_blank");
            } else {
                $("#showReportLoader").show();
                $.ajax(
                {
                    url: showGeneratedReportUrl,
                    type: "GET",
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

        dhHelpdesk.reports.doValidation = function () {

            $('#ReportFilter_CaseCreationDate_FromDate').removeClass("error");
            $('#ReportFilter_CaseCreationDate_ToDate').removeClass("error");

            if ($('#ReportFilter_CaseCreationDate_FromDate').val() == "") {
                var msg = window.Params.DateIsEmptyMessage;
                $('#ReportFilter_CaseCreationDate_FromDate').addClass("error");
                ShowToastMessage(msg, "warning");
                return false;
            }

            if ($('#ReportFilter_CaseCreationDate_ToDate').val() == "") {
                var msg = window.Params.DateIsEmptyMessage;
                $('#ReportFilter_CaseCreationDate_ToDate').addClass("error");
                ShowToastMessage(msg, "warning");
                return false;
            }
            
            return true;
        }

        dhHelpdesk.reports.onOtherShow = function (e) {
                        
            if (!dhHelpdesk.reports.doValidation())
                return;

            var origReportId = $(reportList).find("option:selected").data("origReportId");
            var isSavedFilter = typeof origReportId !== "undefined";

            var reportName = isSavedFilter ? origReportId : $(reportList + " option:selected").val();
            var customer = "";
            var deps_OUs = "";
            var workingGroup = "";
            var administrator = "";
            var caseType = "";
            var productArea = "";
            var status = "";

            customer = currentCustomerId;

            $(departmentDropDown + " option:selected").each(function () {
                deps_OUs += $(this).val() + ",";
            });

            $(workingGroupDropDown + " option:selected").each(function () {
                workingGroup += $(this).val() + ",";
            });

            $(administratorDropDown + " option:selected").each(function () {
                administrator += $(this).val() + ",";
            });

            $(caseTypeDropDown + " option:selected").each(function () {
                caseType += $(this).val() + ",";
            });

            $(productAreaDropDown + " option:selected").each(function () {
                productArea += $(this).val() + ",";
            });

            status = $(statusList + " option:selected").val();

            var regDateFrom = $(caseCreateFrom).val();
            var regDateTo = $(caseCreateTo).val();

            var closeDateFrom = $(caseCloseFrom).val();
            var closeDateTo = $(caseCloseTo).val();

            $.get(showReportUrl,
                {
                    reportName: reportName,
                    'filter.Customers': customer,
                    'filter.Deps_OUs': deps_OUs,
                    'filter.WorkingGroups': workingGroup,
                    'filter.Administrators': administrator,
                    'filter.CaseTypes': caseType,
                    'filter.ProductAreas': productArea,
                    'filter.CaseStatus': status,
                    'filter.RegisterFrom': regDateFrom,
                    'filter.RegisterTo': regDateTo,
                    'filter.CloseFrom': closeDateFrom,
                    'filter.CloseTo': closeDateTo,
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

            var $btnPreview = $("#btnPreviewReport");
            var $btnShow = $("#showReport");
            var $btnSaveFilter = $("#btnSaveFilter");
            var $btnSaveAsFilter = $("#btnSaveAsFilter");
            var $btnExcel = $("#excelReport");
            var $btnShowReport = $("#btnShowReport");
            var $reportGeneratorFields = $("#reportGeneratorFields");
            var $otherReportsContainer = $("#otherReportsContainer");
            var $generateReportContainer = $("#generateReportContainer");
            var $fieldsSelect = $("#lstFields");

            dhHelpdesk.reports.togglePreviewMode(true);

            if (reportId === dhHelpdesk.reports.reportType.ReportGenerator) {
                $btnPreview.show();
                $btnShow.show();
                $btnExcel.show();
                $btnShowReport.hide();
                $reportGeneratorFields.find("select option").prop("selected", false);
                $fieldsSelect.multiselect("refresh");
                $reportGeneratorFields.show();
                $otherReportsContainer.hide();
                $generateReportContainer.html("");
                $generateReportContainer.show();
            } else {
                $btnPreview.hide();
                $btnShow.hide();
                $btnExcel.hide();
                $btnShowReport.show();
                $reportGeneratorFields.hide();
                $("#reportPresentationArea").html("");
                $otherReportsContainer.show();
                $generateReportContainer.hide();
            }

            if (isSavedFilter) {
                dhHelpdesk.reports.loadFilter(selectedId);
                $btnSaveFilter.show();
                $btnSaveAsFilter.show();
                $("#btnDeleteFavorite").show();
            } else {
                //show/hide proper buttons
                $btnSaveFilter.hide();
                $btnSaveAsFilter.show();
                $("#btnDeleteFavorite").hide();
            }

            dhHelpdesk.reports.showExtraParameters($(reportList).find("option:selected").data('data-identity'));
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
            var currentVersion = "1";
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
                var filters = getFilters();
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

        dhHelpdesk.reports.showExtraParameters = function (reportName) {
            $($extraParameters).css("display","none");

            switch (reportName) {
                case "NumberOfCases":
                    $("#lstfilterReportCategory").css("display", "block");
                    break;

                default:
                    break;
            }
        }

        dhHelpdesk.reports.init = function () {

            var showReportButton = $("#btnShowReport");

            dhHelpdesk.reports.onReportChange.call(document.getElementById("lstReports"));

            $("#lstReports").on("change", dhHelpdesk.reports.onReportChange);

            $(showReportButton).on("click", dhHelpdesk.reports.onOtherShow);
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
