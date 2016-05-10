"use strsict";

(function ($) {
    window.Params = window.Params || {};
    var specificFilterDataUrl = window.Params.SpecificFilterDataUrl;
    var showReportUrl = window.Params.ShowReportUrl;
    var showGeneratedReportUrl = window.Params.ShowGeneratedReportUrl;

    var showReportButton = window.Params.ShowReportButton;
    var reportList = window.Params.ReportList;
    var AdministratorDropDown = window.Params.AdministratorDropDown;
    var CaseCreateFrom = window.Params.CaseCreateFrom;
    var CaseCreateTo = window.Params.CaseCreateTo;
    var DepartmentDropDown = window.Params.DepartmentDropDown;
    var WorkingGroupDropDown = window.Params.WorkingGroupDropDown;
    var CaseTypeDropDown = window.Params.CaseTypeDropDown;
    var ProductAreaDropDown = window.Params.ProductAreaDropDown;
    var currentCustomerId = window.Params.CurrentCustomerId;
    var statusList = window.Params.StatusList;
    //var btnShow = $("#showReport");
    //var btnExcel = $('#excelReport');
    var reportGeneratorFields = "#reportGeneratorFields";

    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";

    //$(reportGeneratorFields).multiselect();

    $(showReportButton).on("click", function (e) {
        var _reportName = $(reportList + " option:selected").val();
        var _customer = "";
        var _deps_OUs = "";
        var _workingGroup = "";
        var _administrator = "";
        var _caseType = "";
        var _productArea = "";
        var _status = "";

        _customer = currentCustomerId;

        $(DepartmentDropDown + ' option:selected').each(function () {
            _deps_OUs += $(this).val() + ",";
        });

        $(WorkingGroupDropDown + ' option:selected').each(function () {
            _workingGroup += $(this).val() + ",";
        });

        $(AdministratorDropDown + ' option:selected').each(function () {
            _administrator += $(this).val() + ",";
        });

        $(CaseTypeDropDown + ' option:selected').each(function () {
            _caseType += $(this).val() + ",";
        });

        $(ProductAreaDropDown + ' option:selected').each(function () {
            _productArea += $(this).val() + ",";
        });

        _status = $(statusList + " option:selected").val();

        var _regDateFrom = $(CaseCreateFrom).val();
        var _regDateTo = $(CaseCreateTo).val();

        $.get(showReportUrl,
                {
                    reportName: _reportName,
                    'filter.Customers': _customer,
                    'filter.Deps_OUs': _deps_OUs,
                    'filter.WorkingGroups': _workingGroup,
                    'filter.Administrators': _administrator,
                    'filter.CaseTypes': _caseType,
                    'filter.ProductAreas': _productArea,
                    'filter.CaseStatus': _status,
                    'filter.RegisterFrom': _regDateFrom,
                    'filter.RegisterTo': _regDateTo,
                    curTime: new Date().getTime()
                },
                function (_reportPresentation) {
                    $("#reportPresentationArea").html(_reportPresentation);
                }
             );
    });

    $("#showReport, #excelReport").click(function () {
        var deps_OUs = [];
        var workingGroup = [];
        var caseType = [];
        var productArea = [];
        var fields = [];
        var administrator = [];
        var caseStatues = [];

        $(DepartmentDropDown + ' option:selected').each(function () {
            deps_OUs.push($(this).val());
        });

        $(WorkingGroupDropDown + ' option:selected').each(function () {
            workingGroup.push($(this).val());
        });

        $(CaseTypeDropDown + ' option:selected').each(function () {
            caseType.push($(this).val());
        });

        $(AdministratorDropDown + ' option:selected').each(function () {
            administrator.push($(this).val());
        });

        $(statusList + ' option:selected').each(function () {
            caseStatues.push($(this).val());
        });

        $(ProductAreaDropDown + ' option:selected').each(function () {
            productArea.push($(this).val());
        });

        $(reportGeneratorFields + ' option:selected').each(function () {
            fields.push($(this).val());
        });

        var regDateFrom = $(CaseCreateFrom).val();
        var regDateTo = $(CaseCreateTo).val();

        var getParams = $.param({
            DepartmentIds: deps_OUs,
            WorkingGroupIds: workingGroup,
            CaseTypeIds: caseType,
            AdministratorsIds: administrator,
            CaseStatusIds: caseStatues,
            ProductAreaIds: productArea,
            PeriodFrom: regDateFrom,
            PeriodUntil: regDateTo,
            FieldIds: fields,
            IsExcel: $(this).data("excel") || false,
            SortName: "",
            SortBy: ""
        }, true);

        if ($(this).data("excel") === true) {
            window.open(showGeneratedReportUrl + "?" + getParams, "_blank");
        } else {
            $('#showReportLoader').show();
            $.ajax(
            {
                url: showGeneratedReportUrl,
                type: "GET",
                traditional: true,
                data: getParams,
                dataType: 'html',
                success: function(htmlData) {
                    $("#generateReportContainer").html(htmlData);
                },
                complete: function() {
                    $('#showReportLoader').hide();
                }
        });
        }
    });

    $(".chosen-select").chosen({
        width: "300px",
        'placeholder_text_multiple': placeholder_text_multiple,
        'no_results_text': no_results_text
    });


})($);
