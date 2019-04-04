"use strict";

(function ($) {
    window.Params = window.Params || {};
    var defaultFocusObj = window.Params.DefaultFocusObject;
    var caseTypeDropDown = window.Params.CaseTypeDropDown;
    var productAreaDropDown = window.Params.ProductAreaDropDown;
    var closingReasonDropDown = window.Params.ClosingReasonDropDown;

    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";
        
    $(".chosen-select").chosen({
        width: "300px",
        'placeholder_text_multiple': placeholder_text_multiple,
        'no_results_text': no_results_text
    });       

    $(".chosen-single-select").chosen({
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

    $('#' + productAreaDropDown + ' ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $(breadCrumbsPrefix + productAreaDropDown).text(getBreadcrumbs(this));
        $(hiddenPrefix + productAreaDropDown).val(val);
    });

    $('#' + closingReasonDropDown + ' ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $(breadCrumbsPrefix + closingReasonDropDown).text(getBreadcrumbs(this));
        $(hiddenPrefix + closingReasonDropDown).val(val);
    });
})($);
