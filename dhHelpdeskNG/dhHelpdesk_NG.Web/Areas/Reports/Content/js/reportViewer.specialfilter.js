"use strsict";

(function ($) {
    window.Params = window.Params || {};    
    var caseTypeDropDown = window.Params.CaseTypeDropDown;
    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";


    $(".chosen-select").chosen({
        width: "300px",
        'placeholder_text_multiple': placeholder_text_multiple,
        'no_results_text': no_results_text
    });

    $('#' + caseTypeDropDown + ' ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $(breadCrumbsPrefix + caseTypeDropDown).text(getBreadcrumbs(this));
        $(hiddenPrefix + caseTypeDropDown).val(val);
    });

})($);
