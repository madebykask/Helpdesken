"use strsict";


(function ($) {    
$('#lstfilterCustomers.chosen-select').on('change', function (evt, params) {
    SetSpecificConditions(true);
});


function SetSpecificConditions(resetFilterObjs) {
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
        $.get(window.getSpecificFilterDataUrl,
                {
                    selectedCustomerId: customerId,
                    resetFilter: resetFilterObjs,
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

(function ($) {    
    var caseTypeDropDown = window.Params.CaseTypeDropDown;
    var productAreaDropDown = window.Params.ProductAreaDropDown;
    var closingReasonDropDown = window.Params.ClosingReasonDropDown;

    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";    

    $('#' + caseTypeDropDown + ' ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $(breadCrumbsPrefix + caseTypeDropDown).text(window.getBreadcrumbs(this));
        $(hiddenPrefix + caseTypeDropDown).val(val);
    });

    //$('#' + productAreaDropDown + ' ul.dropdown-menu li a').click(function (e) {
    //    e.preventDefault();
    //    var val = $(this).attr('value');
    //    $(breadCrumbsPrefix + productAreaDropDown).text(window.getBreadcrumbs(this));
    //    $(hiddenPrefix + productAreaDropDown).val(val);
    //});

    //$('#' + closingReasonDropDown + ' ul.dropdown-menu li a').click(function (e) {
    //    e.preventDefault();
    //    var val = $(this).attr('value');
    //    $(breadCrumbsPrefix + closingReasonDropDown).text(window.getBreadcrumbs(this));
    //    $(hiddenPrefix + closingReasonDropDown).val(val);
    //});


})($);