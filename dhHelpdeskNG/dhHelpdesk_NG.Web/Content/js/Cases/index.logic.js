"use strict";

$('#divCaseType ul.dropdown-menu li a').click(function (e) {
    e.preventDefault();
    var val = $(this).attr('value');
    $("#divBreadcrumbs_CaseType").text(getBreadcrumbs(this));
    $("#hidFilterCaseTypeId").val(val);
});

$('#divProductArea ul.dropdown-menu li a').click(function (e) {
    e.preventDefault();
    var val = $(this).attr('value');
    $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
    $("#hidFilterProductAreaId").val(val);
});


$('#divClosingReason ul.dropdown-menu li a').click(function (e) {
    e.preventDefault();
    var val = $(this).attr('value');
    $("#divBreadcrumbs_ClosingReason").text(getBreadcrumbs(this));
    $("#hidFilterClosingReasonId").val(val);
});

$('#btnSearch').click(function (e) {
    e.preventDefault();
    search();
    setFilterIcon();
});

$('#SettingTab').click(function (e) {
    $('#btnSaveCaseSetting').show();

    $('#btnNewCase').hide();
    $('#btnCaseTemplate').hide();
});

$('#CasesTab').click(function (e) {
    $('#btnNewCase').show();
    $('#btnCaseTemplate').show();
    $('#btnSaveCaseSetting').hide();
});

/**
* @param { blockElementJqueryId } jquery-like element id of the bootstrap dropdown t.ex "#mainblock"
*/
function unsetBootstrapsDropdown(blockElementJqueryId) {
    var el = $(blockElementJqueryId).find('.dropdown-menu li a').first();
    var text = el.text();
    var val = el.attr('value');
    $(blockElementJqueryId).find('.btn:first-child').text(text);
    $(blockElementJqueryId).find('input[type=hidden]').val(val);
}

/**
* Sets search filter to empty values in sake to fetch more data as possible
*/
function unsetSearchFilter() {
    /// filterRegion,filterCountry,filterDepartment,filterCaseUser, 
    $("#lstFilterRegion, #lstFilterCountry, #lstfilterDepartment, #lstfilterUser").val('').trigger("chosen:updated");
    /// filterCaseType
    unsetBootstrapsDropdown('#divCaseType');
    /// filterProductArea
    unsetBootstrapsDropdown('#divProductArea');
    /// filterCategory, filterWorkingGroup, filterUser, filterPerformer, filterPriority, filterStatus, filterStateSecondary
    $('#lstfilterCategory, #lstfilterWorkingGroup, #lstfilterResponsible, #lstfilterPerformer, #lstfilterPriority, #lstfilterStatus, #lstfilterStateSecondary').val('').trigger("chosen:updated");
    /// CaseRegistrationDateFilterShow, CaseWatchDateFilterShow, CaseClosingDateFilterShow
    $('.date-block input[type=text]').val('');
    //ClosingReasons
    unsetBootstrapsDropdown('#divClosingReason');
    // @TODO: InitiatorName
}


function search() {
    var searchStr = $('#txtFreeTextSearch').val();
    if (searchStr.length > 0 && searchStr[0] === "#") {
        /// if looking by case number - set case state filter to "All"
        $('#lstfilterCaseProgress').val(-1);
        /// and reset search filter
        unsetSearchFilter();
    }
    $.post('/Cases/Search/', $("#frmCaseSearch").serialize(), function (result) {
        $('#search_result').html(result);
    });
}

function sortCases(sortBy) {
    var asc = $("#hidSortByAsc").val();
    var selected = $("#hidSortBy").val();

    if (sortBy == selected) {
        asc = asc.toLowerCase().match('true') ? 'false' : 'true';
    }
    else {
        asc = 'true';
    }

    $("#hidSortBy").val(sortBy);
    $("#hidSortByAsc").val(asc);
    search();
}

// visa filter ikon på case/ärende sök
function setFilterIcon() {
    $('#icoFilter').hide();
    var sel = $('.chosen-select option:selected').length;
    var hid = 'undefined';

    if ($('#hidFilterProductAreaId').length != 0) {
        var hid = hid + $('#hidFilterProductAreaId').val();
    }
    if ($('#hidFilterCaseTypeId').length != 0) {
        var hid = hid + $('#hidFilterCaseTypeId').val();
    }
    hid = hid.replace('undefined', '');
    if (sel > 0 || hid != '') {
        $('#icoFilter').show();
    }
}

$(function () {
    setFilterIcon();
});

window.onload = function () {
    $('#btnNewCase').show();
    $('#btnCaseTemplate').show();
};

$('#btnSaveCaseSetting').click(function (e) {
    e.preventDefault();
    window.SaveSetting(e);
});

function SaveSetting(e) {
    $.post('/Cases/SaveSetting/', $("#frmCaseSetting").serialize(), function (result) {
    });

    $.post('/Cases/SaveColSetting/', $("#frmColCaseSetting").serialize(), function (result) {
        window.location.href = '/cases/';
    });
}

// msgType : Success,Warning,Notice,Error
function ShowToastMessage(message, msgType) {
    $().toastmessage('showToast', {
        text: message,
        sticky: false,
        position: 'top-center',
        type: msgType,
        closeText: '',
        stayTime: 3000,
        inEffectDuration: 1000,
        close: function () {
            //console.log("toast is closed ...");
        }
    });
}

$(function () {
    if (!window.dhHelpdesk) {
        window.dhHelpdesk = {};
    }

    if (!window.dhHelpdesk.casesList) {
        window.dhHelpdesk.casesList = {};
    }

    dhHelpdesk.casesList.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var getDepartmentsUrl = spec.getDepartmentsUrl || '';
        var regions = $('[data-field="region"]');
        var departments = $('[data-field="department"]');
        var administrators = $('[data-field="administrator"]');

        return that;
    }
});