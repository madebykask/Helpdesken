"use strsict";

var GRID_STATE = {
    IDLE: 0,
    LOADING: 1,
    BAD_CONFIG: 2,
    NO_COL_SELECTED: 3
};

(function($) {
    /// message types
    var ERROR_MSG_TYPE = 0;
    var LOADING_MSG_TYPE = 1;
    var NODATA_MSG_TYPE = 2;
    var BADCONFIG_MSG_TYPE = 3;
    var NO_COL_SELECTED_MSG_TYPE = 4;

    var SORT_ASC = 0;
    var SORT_DESC = 1;


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

    /**
    * Initialization method. Called when page ready
    * @param { object } appSettings
    */
    Page.prototype.init = function(appSettings) {
        var me = this;
        me.hXHR = null;
        me.msgs = appSettings.messages || {};
        me.settings = {
            filterSettings: appSettings.searchFilter.data,
            refreshContent: appSettings.refreshContent
        };
        if (me.settings.refreshContent > 0) {
            setInterval(function() {
                me.autoReloadCheck.call(me);
            }, 500);
        }
        //// Bind elements
        me.$table = $('.table-cases');
        me.$tableHeader = $('table.table-cases thead');
        me.$tableBody = $('.table-cases tbody');
        me.$tableLoaderMsg = ' div.loading-msg';
        me.$tableNoDataMsg = ' div.no-data-msg';
        me.$tableErrorMsg = ' div.error-msg';
        me.$noFieldsMsg = $('#search_result div.nofields-msg');
        me.$noAvailableFieldsMsg = $('#search_result div.noavailablefields-msg');
        me.$buttonsToDisableWhenGridLoads = $('ul.secnav a.btn, ul.secnav div.btn-group button, ul.secnav input[type=button], .submit, #btnClearFilter');
        me.$buttonsToDisableWhenNoColumns = $('#btnNewCase a.btn, #btnCaseTemplate a.btn, .submit, #btnClearFilter');
        me.$caseRecordCount = $('[data-field="TotalAmountCases"]');
        
        me.filterForm = new FilterForm();
        me.filterForm.init({
            $el: $('#frmCaseSearch'),
            filter: appSettings.searchFilter.data,
            favorites: appSettings.userFilterFavorites,            
            onBeforeSearch: callAsMe(me.canMakeSearch, me),
            onSearch: Utils.applyAsMe(me.fetchData, me, [{ isSearchInitByUser: true }])
        });
        
        me.$remainingView = $('[data-field="caseRemainingTimeHidePlace"]');
        
        //// Bind events
        $('a.refresh-grid').on('click', function(ev) {
            ev.preventDefault();
            if (me._gridState !== window.GRID_STATE.IDLE) {
                return false;
            }
            me.fetchData.call(me, { isSearchInitByUser: true });
            return false;
        });
        

        $('#btnNewCase a, #divCaseTemplate a:not(.category)').click(function() {
            me.abortAjaxReq();
            return true;
        });
        
        me.setGridState(window.GRID_STATE.IDLE);
        me.setGridSettings(appSettings.gridSettings);
    };

    Page.prototype.autoReloadCheck = function() {
        var me = this;
        if (me.getGridUpdatedAgo() >= me.settings.refreshContent && me.getGridState() == window.GRID_STATE.IDLE) {
            me.fetchData();
        }
    };

    Page.prototype.getGridUpdatedAgo = function() {
        var me = this;
        if (me._gridUpdated == null) {
            return Number.MAX_VALUE;
        }
        return ((new Date()).getTime() - me._gridUpdated) / 1000;
    };

    /**
    * @private
    * @returns bool
    */
    Page.prototype.canMakeSearch = function() {
        var me = this;
        if (me._gridState == GRID_STATE.IDLE) {
            return true;
        };
        if (me._gridState == GRID_STATE.LOADING) {
            me.abortAjaxReq();
            return true;
        }
        return false;
    };

    Page.prototype.setGridState = function(newGridState) {
        var me = this;
        switch (newGridState) {
            case window.GRID_STATE.IDLE:
                me.$buttonsToDisableWhenGridLoads.removeClass('disabled');
                break;
            case window.GRID_STATE.NO_COL_SELECTED:
                me.$buttonsToDisableWhenNoColumns.addClass('disabled');
                break;
            case window.GRID_STATE.LOADING:
                if (me._gridState != window.GRID_STATE.IDLE) {
                    me.$buttonsToDisableWhenGridLoads.addClass('disabled');
                }
                break;
            default:
                me.$buttonsToDisableWhenGridLoads.addClass('disabled');
        }
        me._gridState = newGridState;
    };

    Page.prototype.getGridState = function() {
        return this._gridState;
    };

    /**
    * @param { {String: id, String: displayName } gridSettings
    */
    Page.prototype.setGridSettings = function(gridSettings) {
        var me = this;
        var hasColSpecialClass = '';
        var out = ['<tr><th style="width:18px;"></th>'];
        var sortCallback = function () {
            me.setSortField.call(me, $(this).attr('fieldname'), $(this));
        };
        me.gridSettings = gridSettings;
        if (me.gridSettings.availableColumns == 0) {
            me.showMsg(BADCONFIG_MSG_TYPE);
            me.setGridState(GRID_STATE.BAD_CONFIG);
            return;
        }

        if (me.gridSettings.columnDefs.length == 0) {
            me.showMsg(NO_COL_SELECTED_MSG_TYPE);
            me.setGridState(GRID_STATE.NO_COL_SELECTED);
            return;
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
        me.$table.addClass([me.gridSettings.cls, hasColSpecialClass].join(' '));
        me.$tableHeader.html(out.join(JOINER));
        me.$tableHeader.find('th.thpointer').on('click', sortCallback);

        me.fetchData({ isSearchInitByUser: true });
    };

    Page.prototype.setSortField = function(fieldName, $el) {
        var me = this;
        var oldEl;
        var sortOpt = me.gridSettings.sortOptions;
        var oldCls = getClsForSortDir(sortOpt.sortDir);
        if (window.app.getGridState() !== window.GRID_STATE.IDLE) {
            return;
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

        // I did not put isSearchInitByUser: true due to it shows irritating message 
        //  when more than 500 rows fetchied
        me.fetchData();        
    };

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
        if (msgType === NO_COL_SELECTED_MSG_TYPE) {
            me.$table.hide();
            me.$noFieldsMsg.show();
            return;
        }
        if (msgType == BADCONFIG_MSG_TYPE) {
            me.$table.hide();
            me.$noAvailableFieldsMsg.show();
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
        var out = [strJoin('<td><a href="/Cases/Edit/', caseId, '">', cellValue == null ? '&nbsp;' : cellValue, '</a></td>')];
        return out.join(JOINER);
    };

    Page.prototype.loadData = function(data, opt) {
        var me = this;
        var out = [];
        if (data)
        {
            var totalCaseAmount = data.length;
            me.$caseRecordCount.text(totalCaseAmount);
        }

        var RECORDS_TRUNCATED_IDX = 499;
        var CASE_TYPE_ALL = -1;
        var CASE_TYPE_CLOSED = 1;
        var isRecordsLimitReached = false;
        opt = opt || {};
        
        if (data && data.length > 0) {
            me.hideMessage();
            $.each(data, function (idx, record) {
                var firstCell = strJoin('<td><a href="/Cases/Edit/', record.case_id, '"><img title="', record.caseIconTitle, '" alt="', record.caseIconTitle, '" src="', record.caseIconUrl, '" /></a></td>');
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
                if (idx === RECORDS_TRUNCATED_IDX) {
                    isRecordsLimitReached = true;
                }
            });
            me.$tableBody.html(out.join(JOINER));
            var oldCaseType = me.filterForm.getSavedSeacrhCaseTypeValue();
            var newCaseType = me.filterForm.getSearchCaseType();

            if (isRecordsLimitReached && opt.isSearchInitByUser
                && newCaseType !== oldCaseType &&  (newCaseType === CASE_TYPE_ALL || newCaseType === CASE_TYPE_CLOSED)) {
                ShowToastMessage(me.msgs.information + '<br/>' + me.msgs.records_limited_msg, 'notice');
            }
        } else {
            me.showMsg(NODATA_MSG_TYPE);
        }
        me.setGridState(window.GRID_STATE.IDLE);
        $(document).trigger("OnCasesLoaded");
    };

    Page.prototype.onRemainingViewClick = function(aElement) {
        var me = this;
        me.fetchData( {
            isSearchInitByUser: true, 
            appendFetch: [
                { 'name': 'CaseRemainingTime', 'value': $(aElement).attr('data-remaining-time') },
                { 'name': 'CaseRemainingTimeUntil', 'value': $(aElement).attr('data-remaining-time-until') },
                { 'name': 'CaseRemainingTimeMax', 'value': $(aElement).attr('data-remaining-time-max') },
                { 'name': 'CaseRemainingTimeHours', 'value': $(aElement).attr('data-remaining-time-hours') }]
        });
    };

    Page.prototype.loadRemainingView = function(htmlContent) {
        var me = this;
        me.$remainingView.html(htmlContent);
        me.$remainingView.find('a').on('click', function (ev) {
            ev.preventDefault();
            me.onRemainingViewClick.call(me, this);
            return false;
        });
    };


    Page.prototype.onGetData = function (response, opt) {
        var me = this;
        if (response && response.result === 'success' && response.data) {
            me.loadData(response.data, opt);
            if (response.remainingView) {
                me.loadRemainingView(response.remainingView);
            }
        } else {
            me.showMsg(ERROR_MSG_TYPE);
            me.setGridState(window.GRID_STATE.IDLE);
        }
    };

    Page.prototype.abortAjaxReq = function() {
        var me = this;
        if (me.hXHR != null && me.hXHR.status == null) {
            me.hXHR.abort();
        }
    };

    Page.prototype.fetchData = function(params) {
        var me = this;
        var fetchParams;
        var baseParams = me.filterForm.getFilterToSend();
        var p = params || {};
        baseParams.push(
            { name: 'sortBy', value: me.gridSettings.sortOptions.sortBy },
            { name: 'sortDir', value: me.gridSettings.sortOptions.sortDir },
            { name: 'pageIndex', value: me.gridSettings.pageOptions.pageIndex },
            { name: 'recPerPage', value: me.gridSettings.pageOptions.recPerPage });

        if (p.appendFetch != null && p.appendFetch.length > 0) {
            fetchParams = baseParams.concat(p.appendFetch);
        } else {
            fetchParams = baseParams;
        }
        me.setGridState(window.GRID_STATE.LOADING);
        me.showMsg(LOADING_MSG_TYPE);
        me.hXHR = $.ajax('/Cases/SearchAjax', {
                type: 'POST',
                dataType: 'json',
                data: fetchParams
            })
            .always(function() {
                me._gridUpdated = (new Date()).getTime();
            })
            .fail(function() {
                var textStatus = arguments[1];
                if (textStatus !== 'abort') {
                    me.showMsg(ERROR_MSG_TYPE);
                    me.setGridState(window.GRID_STATE.IDLE);
                }
            })
            .done(function() {
                me.onGetData.call(me, arguments[0], { isSearchInitByUser: p.isSearchInitByUser });
            })
            .always(function() {
                me.filterForm.saveSeacrhCaseTypeValue.call(me.filterForm);
            });
    };
   
    window.app = new Page();
    $(document).ready(function() {
        app.init.call(window.app, window.pageSettings);
    });
})($);



$('#SettingTab').click(function (e) {
    $('#btnSaveCaseSetting').show();
    $('#btnMyCases').hide();
    $('#btnNewCase').hide();
    $('#btnCaseTemplate').hide();
});

$('#CasesTab').click(function (e) {
    $('#btnNewCase').show();
    $('#btnMyCases').show();
    $('#btnCaseTemplate').show();
    $('#btnSaveCaseSetting').hide();
});


    
/**
* @param { string } message
* @param { string } msgType one of 'notice', 'warning', 'error', 'success'
*/
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