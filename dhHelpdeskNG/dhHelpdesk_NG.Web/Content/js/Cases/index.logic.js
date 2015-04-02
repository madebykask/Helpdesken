"use strsict";

var GRID_STATE = {
    IDLE: 0,
    LOADING: 1,
    BAD_CONFIG: 2
};

(function($) {
    /// message types
    var ERROR_MSG_TYPE = 0;
    var LOADING_MSG_TYPE = 1;
    var NODATA_MSG_TYPE = 2;
    var BADCONFIG_MSG_TYPE = 3;
    
    var SORT_ASC = 0;
    var SORT_DESC = 1;
    var JOINER = '';

    function strJoin() {
        return Array.prototype.join.call(arguments, JOINER);
    }

    function getClsForSortDir(sortDir) {
        if (sortDir === SORT_ASC) {
            return "icon-chevron-up";
        }
        if (sortDir === SORT_DESC) {
            return "icon-chevron-down";
        }
        return '';
    }

    function Page() {};

    Page.prototype.init = function(gridInitSettings) {
        var me = this;
        //// Bind elements
        me.$table = $('.table-cases');
        me.$tableHeader = $('table.table-cases thead');
        me.$tableBody = $('.table-cases tbody');
        me.$tableLoaderMsg = ' div.loading-msg';
        me.$tableNoDataMsg = ' div.no-data-msg';
        me.$tableErrorMsg = ' div.error-msg';
        me.$noFieldsMsg = $('#search_result div.nofields-msg');
        me.$buttonsToDisableWhenGridLoads = $('ul.secnav a.btn, ul.secnav div.btn-group button, ul.secnav input[type=button], #btnSearch, #btnClearFilter');
        me.$searchField = '#txtFreeTextSearch';
        me.$filterForm = $('#frmCaseSearch');
        me.$caseFilterType = '#lstfilterCaseProgress';
        //// Bind events
        $('#btnSearch, a.refresh-grid').on('click', function (ev) {
            ev.preventDefault();
            if (me._gridState !== window.GRID_STATE.IDLE) {
                return false;
            }
            setFilterIcon();
            me.onSearchClick.apply(me);
            return false;
        });
        /////// moved from _Search.cshtml
        $("#btnClearFilter").click(function (ev) {
            if (me.getGridState() !== window.GRID_STATE.IDLE) {
                ev.preventDefault();
                return false;
            }
            window.location.href = '/Cases/Index/?clearFilters=True';
            return false;
        });

        $("#btnMore").click(function (e) {
            e.preventDefault();
            $("#hiddeninfo").toggle();
            if ($("#icoPlus").hasClass('icon-minus-sign')) {
                $("#icoPlus").removeClass('icon-minus-sign').addClass('icon-plus-sign');
            }
            else {
                $("#icoPlus").removeClass('icon-plus-sign').addClass('icon-minus-sign');
            }
        });

        $('#txtFreeTextSearch').keydown(function (e) {
            if (e.keyCode == 13) {
                $("#btnSearch").click();
            }
        });

        $('ul.secnav #btnNewCase a.btn').on('click', function(ev) {
            if (window.app.getGridState() !== window.GRID_STATE.LOADING) {
                ev.preventDefault();
                return false;
            }
            return true;
        });

        me.setGridState(window.GRID_STATE.IDLE);
        me.setGridSettings(gridInitSettings);
    };

    Page.prototype.onGridRowClick = function(caseId) {
        var me = this;
        window.location.href = '/Cases/Edit/' + caseId;
    };

    Page.prototype.setGridState = function(gridStateId) {
        var me = this;
        me._gridState = gridStateId;
        if (gridStateId == window.GRID_STATE.IDLE) {
            me.$buttonsToDisableWhenGridLoads.removeClass('disabled');
        } else {
            me.$buttonsToDisableWhenGridLoads.addClass('disabled');
        }
    };

    Page.prototype.getGridState = function() {
        return this._gridState;
    };

    /**
    * @param { {String: id, String: displayName } gridSettings
    */
    Page.prototype.setGridSettings = function(gridSettings) {
        var me = this;
        var out = ['<tr><th style="width:18px;"></th>'];
        var sortCallback = function () {
            me.setSortField.call(me, $(this).attr('fieldname'), $(this));
        };
        me.gridSettings = gridSettings;
        if (me.gridSettings.columnDefs.length == 0) {
            me.showMsg(BADCONFIG_MSG_TYPE);
            me.setGridState(GRID_STATE.BAD_CONFIG);
            return false;
        }
        me.visibleFieldsCount = 1; //// we have at least one column with icon
        $.each(me.gridSettings.columnDefs, function (idx, fieldSetting) {
            var sortCls = '';
            if (!fieldSetting.isHidden) {
                me.visibleFieldsCount += 1;
                if (me.gridSettings.sortOptions != null && fieldSetting.field === me.gridSettings.sortOptions.sortBy) {
                    sortCls = getClsForSortDir(me.gridSettings.sortOptions.sortDir);
                }
                out.push(strJoin('<th class="thpointer ', fieldSetting.field, ' ', fieldSetting.cls, '" fieldname="', fieldSetting.field, '">', fieldSetting.displayName, '<i class="', sortCls, '"></i>'));
            }
        });
        out.push('</tr>');
        me.$table.addClass(me.gridSettings.cls);
        me.$tableHeader.html(out.join(JOINER));
        me.$tableHeader.find('th.thpointer').on('click', sortCallback);
        me.fetchData();
    };

    Page.prototype.setSortField = function(fieldName, $el) {
        var me = this;
        var oldEl;
        var sortOpt = me.gridSettings.sortOptions;
        var oldCls = getClsForSortDir(sortOpt.sortDir);
        if (window.app.getGridState() === window.GRID_STATE.LOADING) {
            return false;
        }
        if (sortOpt.sortBy === fieldName) {
            sortOpt.sortDir = (sortOpt.sortDir == SORT_ASC) ? SORT_DESC : SORT_ASC;
            oldEl = $el;
        } else {
            oldEl = $(me.$table).find('thead [fieldname="' + sortOpt.sortBy + '"]');
            sortOpt.sortBy = fieldName;
            sortOpt.sortDir = SORT_DESC;
        }
        if (oldEl.length > 0) {
            $(oldEl).find('i').removeClass(oldCls);
        }
        $($el).find('i').addClass(getClsForSortDir(sortOpt.sortDir));
        me.fetchData();
    },

    Page.prototype.showMsg = function(msgType) {
        var me = this;
        me.hideMessage();
        if (msgType === LOADING_MSG_TYPE) {
            $(me.$tableLoaderMsg).show();
            return;
        }
        if (msgType === ERROR_MSG_TYPE) {
            $(me.$tableBody).html('');
            $(me.$tableErrorMsg).show();
            return;
        }
        if (msgType === NODATA_MSG_TYPE) {
            $(me.$tableBody).html('');
            $(me.$tableNoDataMsg).show();
            return;
        }
        if (msgType === BADCONFIG_MSG_TYPE) {
            me.$table.hide();
            me.$noFieldsMsg.show();
            return;
        }
        console.warn('not implemented');
    };

    Page.prototype.hideMessage = function () {
        var me = this;
        $(me.$tableLoaderMsg).hide();
        $(me.$tableErrorMsg).hide();
        $(me.$tableNoDataMsg).hide();
    };

    Page.prototype.getClsRow = function(record) {
        var res = [];
        if (record.isUnread) {
            res.push('textbold');
        }
        if (record.isUrgent) {
            res.push('textred');
        }
        return res.join(' ');
    };

    Page.prototype.formatCell = function (caseId, cellValue) {
        var out = [strJoin('<td>')];
        out.push(cellValue == null ? '' : cellValue);
        out.push('</td>');
        return out.join(JOINER);
    };

    Page.prototype.loadData = function(data) {
        var me = this;
        var out = [];
        
        if (data && data.length > 0) {
            me.hideMessage();
            $.each(data, function (idx, record) {
                var firstCell = strJoin('<td><img title="', record.caseIconTitle, '" alt="', record.caseIconTitle, '" src="', record.caseIconUrl, '" /></td>');
                var rowOut = [strJoin('<tr class="', me.getClsRow(record), '" caseid="', record.case_id, '">'), firstCell];
                $.each(me.gridSettings.columnDefs, function (idx, columnSettings) {
                    if (!columnSettings.isHidden) {
                        if (record[columnSettings.field] == null) {
                            rowOut.push(me.formatCell(record.case_id, ''));
                            if (Page.isDebug) 
                                console.warn('could not find field "' + columnSettings.field + '" in record');
                        } else {
                            rowOut.push(me.formatCell(record.case_id, record[columnSettings.field]));
                        }
                    }
                });
                rowOut.push('</tr>');
                out.push(rowOut.join(JOINER));
            });
            me.$tableBody.html(out.join(JOINER));
            me.$tableBody.find('tr').on('click', function (ev) {
                me.onGridRowClick.call(me, $(this).attr('caseid'));
            });
        } else {
            me.showMsg(NODATA_MSG_TYPE);
        }
        me.setGridState(window.GRID_STATE.IDLE);
        $(document).trigger("OnCasesLoaded");
    };

    Page.prototype.onSearchClick = function () {
        var me = this;
        var searchStr = $(me.$searchField).val();
        if (searchStr.length > 0 && searchStr[0] === "#") {
            /// if looking by case number - set case state filter to "All"
            $(me.$caseFilterType).val(-1);
            /// and reset search filter
            unsetSearchFilter();
        }
        me.fetchData();
    };

    Page.prototype.onGetData = function (response) {
        var me = this;
        if (response && response.result === 'success' && response.data) {
            me.loadData(response.data);
        } else {
            me.showMsg(ERROR_MSG_TYPE);
            me.setGridState(window.GRID_STATE.IDLE);
        }
    };

    Page.prototype.fetchData = function() {
        var me = this;
        var fetchParams = me.$filterForm.serializeArray();
        fetchParams.push({ name: 'sortBy', value: me.gridSettings.sortOptions.sortBy },
            { name: 'sortDir', value: me.gridSettings.sortOptions.sortDir },
            { name: 'pageIndex', value: me.gridSettings.pageOptions.pageIndex },
            { name: 'recPerPage', value: me.gridSettings.pageOptions.recPerPage });
        me.setGridState(window.GRID_STATE.LOADING);
        me.showMsg(LOADING_MSG_TYPE);
        $.ajax('/Cases/SearchAjax', {
            type: 'POST',
            dataType: 'json',
            data: fetchParams,
            success: function() {
                me.onGetData.apply(me, arguments);
            },
            error: function() {
                me.showMsg(ERROR_MSG_TYPE);
                me.setGridState(window.GRID_STATE.IDLE);
            }
        });
    };
   
    window.app = new Page();
    $(document).ready(function() {
        app.init.call(window.app, window.gridSettings);
    });
    $(function () {
        setFilterIcon();
    });
})($);


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
                    var remainingTimeUntil = $('<input name="CaseRemainingTimeUntil" type="hidden" />');
                    var remainingTimeMax = $('<input name="CaseRemainingTimeMax" type="hidden" />');
                    var remainingTimeHours = $('<input name="CaseRemainingTimeHours" type="hidden" />');
                    remainingTime.val($this.attr('data-remaining-time'));
                    remainingTimeUntil.val($this.attr('data-remaining-time-until'));
                    remainingTimeMax.val($this.attr('data-remaining-time-max'));
                    remainingTimeHours.val($this.attr('data-remaining-time-hours'));

                    searchForm.append(remainingTime);
                    searchForm.append(remainingTimeUntil);
                    searchForm.append(remainingTimeMax);
                    searchForm.append(remainingTimeHours);
                    $this.after(loader);
                    search();
                    remainingTime.remove();
                    remainingTimeUntil.remove();
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