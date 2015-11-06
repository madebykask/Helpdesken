"use strsict";

(function ($) {
    window.Params = window.Params || {};
    var specificFilterDataUrl = window.Params.SpecificFilterDataUrl;
    var showReportUrl = window.Params.ShowReportUrl;

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

    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";       

    $(showReportButton).click(function (e) {
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

    $(".chosen-select").chosen({
        width: "300px",
        'placeholder_text_multiple': placeholder_text_multiple,
        'no_results_text': no_results_text
    });

      
})($);
