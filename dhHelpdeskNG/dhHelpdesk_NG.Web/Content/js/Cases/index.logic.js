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
    })
    .always(function () {
        $(document).trigger("OnCasesLoaded");
    });;
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

    dhHelpdesk.casesList.utils = {
        raiseEvent: function (eventType, extraParameters) {
            $(document).trigger(eventType, extraParameters);
        },

        onEvent: function (event, handler) {
            $(document).on(event, handler);
        }
    }

    dhHelpdesk.casesList.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var caseRemainingTime = $('[data-field="caseRemainingTime"]');
        var caseRemainingTimeHidePlace = $('[data-field="caseRemainingTimeHidePlace"]');
        var caseRemainingTimeShowPlace = $('[data-field="caseRemainingTimeShowPlace"]');
        var showCaseFilters = $('#btnMore');
        var searchForm = $('#frmCaseSearch');

        var moveCaseRemainingTimeIntoHidePlace = function() {
            caseRemainingTime.detach().appendTo(caseRemainingTimeHidePlace);
        }

        var moveCaseRemainingTimeIntoShowPlace = function() {
            caseRemainingTime.detach().appendTo(caseRemainingTimeShowPlace);
        }

        var isCaseFilterHided = function() {
            return $("#icoPlus").hasClass('icon-minus-sign');
        }

        var moveCaseRemainingTime = function() {
            if (isCaseFilterHided()) {
                moveCaseRemainingTimeIntoShowPlace();
            } else {
                moveCaseRemainingTimeIntoHidePlace();
            }
        }
        moveCaseRemainingTime();

        showCaseFilters.click(function () {
            moveCaseRemainingTime();
        });

        var loader = $('<img src="/Content/icons/ajax-loader.gif" />');

        var bindCaseRemainingTime = function() {
            $('[data-remaining-time]').each(function () {
                $(this).click(function () {
                    var $this = $(this);
                    var remainingTime = $('<input name="CaseRemainingTime" type="hidden" />');
                    var remainingTimeMax = $('<input name="CaseRemainingTimeMax" type="hidden" />');
                    var remainingTimeHours = $('<input name="CaseRemainingTimeHours" type="hidden" />');
                    remainingTime.val($this.attr('data-remaining-time'));
                    remainingTimeMax.val($this.attr('data-remaining-time-max'));
                    remainingTimeHours.val($this.attr('data-remaining-time-hours'));

                    searchForm.append(remainingTime);
                    searchForm.append(remainingTimeMax);
                    searchForm.append(remainingTimeHours);
                    $this.after(loader);
                    search();
                    remainingTime.remove();
                    remainingTimeMax.remove();
                    remainingTimeHours.remove();
                });
            });
        }
        bindCaseRemainingTime();

        dhHelpdesk.casesList.utils.onEvent("OnCasesLoaded", function() {
            caseRemainingTime.remove();
            caseRemainingTime = $('[data-field="caseRemainingTime"]');
            if (isCaseFilterHided()) {
                moveCaseRemainingTimeIntoShowPlace();
            } else {
                moveCaseRemainingTimeIntoHidePlace();
            }

            bindCaseRemainingTime();
        });

        return that;
    }
});