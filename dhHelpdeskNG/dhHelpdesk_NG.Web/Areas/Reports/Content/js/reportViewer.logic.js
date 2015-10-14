"use strsict";

(function ($) {
    window.Params = window.Params || {};
    var specificFilterDataUrl = window.Params.SpecificFilterDataUrl;
    var showReportUrl = window.Params.ShowReportUrl;

    var showReportButton = window.Params.ShowReportButton;
    var reportList = window.Params.ReportList;
    var customerDropDown = window.Params.CustomerDropDown;
    var AdministratorDropDown = window.Params.AdministratorDropDown;
    var CaseCreateFrom = window.Params.CaseCreateFrom;
    var CaseCreateTo = window.Params.CaseCreateTo;
    var DepartmentDropDown = window.Params.DepartmentDropDown;
    var WorkingGroupDropDown = window.Params.WorkingGroupDropDown;
    var caseTypeDropDown = window.Params.CaseTypeDropDown;

    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";

    setTimeout(function () { $(reportList).focus(); }, 200);

    $(showReportButton).click(function (e) {
        var _reportName = $(reportList + " option:selected").val();
        var _customers = "";
        var _departments = "";
        var _workingGroup = "";
        var _administrator = "";
        var _caseType = "";

        $(customerDropDown + ' option:selected').each(function () {            
            _customers += $(this).val() + ",";
        });   

        $(DepartmentDropDown + ' option:selected').each(function () {            
            _departments += $(this).val() + ",";
        });

        $(WorkingGroupDropDown + ' option:selected').each(function () {            
            _workingGroup += $(this).val() + ",";
        });

        $(AdministratorDropDown + ' option:selected').each(function () {
            _administrator += $(this).val() + ",";
        });

        _caseType = $(hiddenPrefix + caseTypeDropDown).val();
        if (_caseType == "0")
            _caseType = "";

        var _regDateFrom = $(CaseCreateFrom).val();
        var _regDateTo = $(CaseCreateTo).val();       

        $.get(showReportUrl,
                {
                    reportName: _reportName,
                    'filter.Customers': _customers, 
                    'filter.Departments': _departments,                    
                    'filter.WorkingGroups': _workingGroup,
                    'filter.Administrators': _administrator,
                    'filter.CaseTypes': _caseType,
                    'filter.RegisterFrom': _regDateFrom,
                    'filter.RegisterTo': _regDateTo,        
                    curTime: new Date().getTime()
                },
                function (_reportPresentation) {
                    $("#reportPresentationArea").html(_reportPresentation);
                }
             );
    });

    SetSpecificConditions();

    $('#lstfilterCustomers.chosen-select').on('change', function (evt, params) {
        SetSpecificConditions();
    });

    function SetSpecificConditions() {
        var selectedCustomers = $('#lstfilterCustomers.chosen-select option');
        var selectedCount = 0;
        var customerId = 0;

        $.each(selectedCustomers, function (idx, value) {
            if (value.selected) {
                customerId = value.value;
                selectedCount++;
            }
        });

        if (selectedCount == 1) {
            $.get(specificFilterDataUrl,
                    {
                        selectedCustomerId: customerId,                        
                        curTime: new Date().getTime()
                    }, function (_SpecificFilterData) {
                        $("#CustomerSpecificFilterPartial").html(_SpecificFilterData);

                    });

            $('#CustomerSpecificFilterPartial').attr('style', '');
            $('#CustomerSpecificFilterPartial').attr('data-field', customerId);
        }
        else {
            $('#CustomerSpecificFilterPartial').attr('style', 'display:none');
            $('#CustomerSpecificFilterPartial').attr('data-field', '');
        }
    }
      
})($);
