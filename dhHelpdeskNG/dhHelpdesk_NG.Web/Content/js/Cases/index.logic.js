"use strict";

var GRID_STATE = {
    IDLE: 0,
    LOADING: 1,
    BAD_CONFIG: 2,
    NO_COL_SELECTED: 3
};

var statisticsExpandHiddenElm = '#hidExpandedGroup';
var groupStatisticsCaptionPrefix = '#Caption_';

function saveExpanded(id) {
    var curExpanded = $(statisticsExpandHiddenElm).val();
    switch (curExpanded) {
        case "":
            $(statisticsExpandHiddenElm).val(id);
            $(groupStatisticsCaptionPrefix + id).text(getExpandCaption($(groupStatisticsCaptionPrefix + id).text()));
            break;

        case id:
            $(statisticsExpandHiddenElm).val('');
            $(groupStatisticsCaptionPrefix + id).text(getCollapseCaption($(groupStatisticsCaptionPrefix + id).text()));
            break;

        default:
            $(groupStatisticsCaptionPrefix + id).text(getExpandCaption($(groupStatisticsCaptionPrefix + id).text()));
            $(groupStatisticsCaptionPrefix + curExpanded).text(getCollapseCaption($(groupStatisticsCaptionPrefix + curExpanded).text()));
            $(statisticsExpandHiddenElm).val(id);
            break;
    }

    var methodUrl = "/Cases/SaveStatisticsStateInSession/";
    var dataValue = $(statisticsExpandHiddenElm).val();
    $.get(methodUrl,
        {
            value: dataValue,
            myTime: Date.now()
        }
    );
}

function getExpandCaption(cap) {
    return cap.replace("+ ", "- ");
}

function getCollapseCaption(cap) {
    return cap.replace("- ", "+ ");
}

(function ($) {
    /// message types
    var ERROR_MSG_TYPE = 0;
    var LOADING_MSG_TYPE = 1;
    var NODATA_MSG_TYPE = 2;
    var BADCONFIG_MSG_TYPE = 3;
    var NO_COL_SELECTED_MSG_TYPE = 4;

    function Page() { };

    /**
    * Initialization method. Called when page ready
    * @param { object } appSettings
    */

    var globalTimerStartTime = 0;
    Page.prototype.init = function (appSettings) {
        "use strict";
        var self = this;
        
        globalTimerStartTime = performance.now();
        self.msgs = appSettings.messages || {};

        self.settings = {
            filterSettings: appSettings.searchFilter.data,
            refreshContent: appSettings.refreshContent
        };
        if (self.settings.refreshContent > 0) {
            self.settings.refreshInterval = setInterval(function () {
                self.autoReloadCheck.call(self);
            }, 5000);
        }

        //// Bind elements
        self.$table = $("#caseResults");
        //self.$tableHeader = $('table.table-cases thead');
        self.$tableBody = $('.table-cases tbody');
        self.$tableLoaderMsg = ' div.loading-msg';
        self.$tableNoDataMsg = ' div.no-data-msg';
        self.$tableErrorMsg = ' div.error-msg';
        self.$noFieldsMsg = $('#search_result div.nofields-msg');
        self.$noAvailableFieldsMsg = $('#search_result div.noavailablefields-msg');
        self.$buttonsToDisableWhenGridLoads = $('ul.secnav a.btn, ul.secnav div.btn-group button, ul.secnav input[type=button], .submit, #btnClearFilter');
        self.$buttonsToDisableWhenNoColumns = $('#btnNewCase a.btn, #btnCaseTemplate a.btn, .submit, #btnClearFilter');
        self.$caseRecordCount = $('[data-field="TotalAmountCases"]');
        self.$statisticsViewPlace = $('[data-field="caseStatisticsPlace"]');
        self.$hidExpandedGroup = $('#hidExpandedGroup');

        self.filterForm = new FilterForm();
        self.filterForm.init({
            $el: $("#frmCaseSearch"),
            filter: appSettings.searchFilter.data,
            favorites: appSettings.userFilterFavorites,
            onBeforeSearch: callAsMe(self.canMakeSearch, self),
            onSearch: Utils.applyAsMe(function () { globalTimerStartTime = performance.now();  self.table.ajax.reload.call(self); }, self, [{ isSearchInitByUser: true }])
        });

        self.$remainingView = $('[data-field="caseRemainingTimeHidePlace"]');

        if (appSettings.gridSettings.availableColumns === 0) {
            self.showMsg(BADCONFIG_MSG_TYPE);
            self.setGridState(GRID_STATE.BAD_CONFIG);
            return;
        }

        if (appSettings.gridSettings.columnDefs.length === 0) {
            self.showMsg(NO_COL_SELECTED_MSG_TYPE);
            self.setGridState(GRID_STATE.NO_COL_SELECTED);
            return;
        }

        self.gridSettings = appSettings.gridSettings;

        self.$table.addClass(appSettings.gridSettings.cls);
        
        var columns = self.getColumnSettings(appSettings.gridSettings);

        var sortIndex = null;
        for (var i = 0; i < columns.length; i++) {
            if (columns[i].data === appSettings.gridSettings.sortOptions.sortBy) {
                sortIndex = i;
                break;
            }
        }

        function sanitizeColumns(jsonArray) {
            var dataProp;
            jsonArray.forEach(function (item) {
                if (item['data'] !== null) {
                    dataProp = item['data'].replace(/\s/g, '').replace(/\./g, '');
                    item['data'] = dataProp;
                }
            });
            return jsonArray;
        }

        function sanitizeData(jsonArray) {
            var newKey;
            jsonArray.forEach(function (item) {
                for (var key in item) {
                    if (item.hasOwnProperty(key)) {
                        newKey = key.replace(/\s/g, '').replace(/\./g, '');
                        if (key != newKey) {
                            item[newKey] = item[key];
                            delete item[key];
                        }
                    }
                }
            });
            return jsonArray;
        } 

        self.table = InitDataTable("caseResults", appSettings.perPageText, appSettings.perShowingText,
            {
                "sDom": "<'row-fluid'<'span6'l><'span6'p>r>t<'row-fluid'<'span6'i><'span6'p>>",
                processing: true,
                serverSide: true,
                ordering: true,
                ajax: {
                    url: "/Cases/SearchAjax",
                    type: "POST",
                    data: function (data) {
                        var params = self.getFetchParams(appSettings.gridSettings);
                        params.push({ name: "start", value: data.start });
                        params.push({ name: "length", value: data.length });
                        params.push({ name: "order", value: data.order.length === 1 ? data.columns[data.order[0].column].data : "" });
                        params.push({ name: "dir", value: data.order.length === 1 ? data.order[0].dir : "" });
                        return params;
                    },
                    error: function (xhr, textStatus, error) {
                        console.log("Error occured on loading SearchAjax");
                        clearInterval(self.settings.refreshInterval);
                    },
                    dataSrc: "data"
                },
                createdRow: function (row, data, dataIndex) {
                    if (data) {
                        $(row).addClass(self.getClsRow(data) + " caseid=" + data.case_id);
                        var html = [];
                        html.push('<a class="img-case-parent" href="/Cases/Edit/', data.case_id, '">');
                        html.push('<img class="img-case-parent" title="', data.caseIconTitle, '" alt="', data.caseIconTitle, '" src="', data.caseIconUrl, '" />');
                        if (data.isCaseLocked) {
                            html.push('<img class="img-case-locked" title="', data.caseLockedIconTitle, '" alt="', data.caseLockedIconTitle, '" src="', data.caseLockedIconUrl, '" />');
                        }
                        html.push('</a>');
                        row.cells[0].innerHTML = html.join("");
                        let caseClosedOrLocked = (data.isClosed == true || data.isCaseLocked == true);

                        html = [];
                        html.push('<input type="checkbox" class="bulkEditCaseSelect' + (caseClosedOrLocked ? 'Disabled' : '') + '" onclick="onClick_cbxBulkEditCaseSelect()" ' + (caseClosedOrLocked  ? 'disabled' : '') + ' id="cbxBulkEditSelectCaseId_' + data.case_id + '" data-caseid="' + data.case_id +'" /> ');
                        row.cells[1].innerHTML = html.join("");

                        html = [];
                        if (data.ParentId > 0 || data.isParent) {
                            var link = '/Cases/Edit/' + data.ParentId;
                            var text = appSettings.isParentText; 
                            var icon = 'fa fa-link';

                            if (data.isParent) {
                                text = appSettings.isChildText;
                                link = '/Cases/Edit/' + data.case_id + '#childcases-tab';
                                icon = 'fa fa-sitemap'; 
                            }
                            if (data.IsMergeParent) {
                                text = 'Merge Parent';
                                link = '/Cases/Edit/' + data.case_id + '#childcases-tab';
                                icon = 'fa fa-umbrella';
                            }
                            if (data.IsMergeChild) {
                                text = 'Merge Child';
                                link = '/Cases/Edit/' + data.case_id + '#childcases-tab';
                                icon = 'fa fa-child';
                            }
                           html.push('<a class="btn btn-mini" href="' + link + '" title="' + text + '"><i style="color: #000 !important;" class="' + icon + '"></i></a>');
                        }
                                                    
                        row.cells[row.cells.length-1].innerHTML = html.join("");
                    }
                },
                columns: sanitizeColumns(columns),
                order: sortIndex != null ? [[sortIndex, appSettings.gridSettings.sortOptions.sortDirString]] : [],
                "bAutoWidth": false,
                "lengthMenu": [appSettings.gridSettings.pageSizeList, appSettings.gridSettings.pageSizeList],
                "iDisplayLength": appSettings.gridSettings.pageOptions.recPerPage,
                "displayStart": appSettings.gridSettings.pageOptions.recPerPage * appSettings.gridSettings.pageOptions.pageIndex
            }, function (e, settings, techNote, message) {
                console.log("An error has been reported by DataTable: ", message);
                var textStatus = arguments[1];
                if (textStatus !== "abort") {
                    self.showMsg(ERROR_MSG_TYPE);
                    self.setGridState(window.GRID_STATE.IDLE);
                }
            }, undefined, undefined, sanitizeData);

        self.table
            //.on("init.dt", function () {
            //    //$(document).trigger("OnCasesLoaded");
            //})
            //.on("preXhr.dt", function (e, settings, data) {

            //})
            .on("xhr.dt", function (e, settings, json) {

                self._gridUpdated = (new Date()).getTime();

                if (json && json.result === "success") {
                    
                    if (!json.data || json.data.length < 1) {
                        self.showMsg(NODATA_MSG_TYPE);
                    }

                    self.$caseRecordCount.text(json.recordsTotal);

                    var newCaseType = self.filterForm.getSearchCaseType();
                    $("#caseLimitInfo").toggle((newCaseType === -1 || newCaseType === 1) && json.recordsTotal === 500);

                    if (json.remainingView) {
                        self.loadRemainingView(json.remainingView);
                    }
                    if (json.statisticsView) {
                        self.loadStatisticsView(json.statisticsView);
                    }

                    var endTime = performance.now();
                    var processDuration = json.processDuration;
                    if (appSettings.curUserId == "admin") {
                        $('#performanceData').html('');
                        $('#performanceData').append("Server side duration: " + processDuration + " milliseconds. <br/>")
                        $('#performanceData').append("Total duration: " + Math.round(endTime - globalTimerStartTime) + " milliseconds.")
                    }

                } else {
                    self.showMsg(ERROR_MSG_TYPE);
                    self.setGridState(window.GRID_STATE.IDLE);
                }

                $(document).trigger("OnCasesLoaded");
                self.filterForm.saveSeacrhCaseTypeValue.call(self.filterForm);
            })
            .on("processing.dt", function (e, settings, processing) {
                if (processing) {
                    self.setGridState(window.GRID_STATE.LOADING);
                    self.showMsg(LOADING_MSG_TYPE);
                } else {
                    self.hideMessage();
                    self.setGridState(window.GRID_STATE.IDLE);
                }
            })
            .on("order.dt", function (a, b, c) {

            });

        //// Bind events
        $("a.refresh-grid").on('click', function (ev) {            
            ev.preventDefault();
            if (self._gridState !== window.GRID_STATE.IDLE) {
                return false;
            }
            self.table.ajax.reload.call(self, { isSearchInitByUser: true });
            return false;
        });

        $("#btnNewCase a, #divCaseTemplate a:not(.category)").click(function () {
            //self.abortAjaxReq();
            return true;
        });

    };

    Page.prototype.getFetchParams = function () {
        "use strict";
        var self = this;
        var fetchParams;
        var expandedGroup = $(self.$hidExpandedGroup).val();
        var baseParams = self.filterForm.getFilterToSend();
        var p = favoritParams || {};
        baseParams.push({ name: "expandedGroup", value: expandedGroup });

        if (self.appendFetch != null && self.appendFetch.length > 0) {
            fetchParams = baseParams.concat(self.appendFetch);
            delete self.appendFetch;
        } else {
            fetchParams = baseParams;
        }
        return fetchParams;
    };

    Page.prototype.autoReloadCheck = function () {
        var self = this;
        if (self.getGridUpdatedAgo() >= self.settings.refreshContent && self.getGridState() === window.GRID_STATE.IDLE) {
            self.table.ajax.reload();
        }
    };

    Page.prototype.getGridUpdatedAgo = function () {
        var self = this;
        if (self._gridUpdated == null) {
            return Number.MAX_VALUE;
        }
        return ((new Date()).getTime() - self._gridUpdated) / 1000;
    };

    /**
    * @private
    * @returns bool
    */
    Page.prototype.canMakeSearch = function () {
        var self = this;
        if (self._gridState === GRID_STATE.IDLE) {
            return true;
        };
        if (self._gridState === GRID_STATE.LOADING) {
            //self.abortAjaxReq();
            return true;
        }
        return false;
    };

    Page.prototype.setGridState = function (newGridState) {
        "use strict";
        var self = this;
        switch (newGridState) {
            case window.GRID_STATE.IDLE:
                self.$buttonsToDisableWhenGridLoads.removeClass("disabled");
                break;
            case window.GRID_STATE.NO_COL_SELECTED:
                self.$buttonsToDisableWhenNoColumns.addClass("disabled");
                break;
            case window.GRID_STATE.LOADING:
                if (self._gridState !== window.GRID_STATE.IDLE) {
                    self.$buttonsToDisableWhenGridLoads.addClass("disabled");
                }
                break;
            default:
                self.$buttonsToDisableWhenGridLoads.addClass("disabled");
        }
        self._gridState = newGridState;
    };

    Page.prototype.getGridState = function () {
        return this._gridState;
    };

    Page.prototype.getColumnSettings = function (gridSettings) {
        "use strict";
        var self = this;

        //var out = ['<tr><th style="width:18px;"></th>'];
        //var sortCallback = function () {
        //    me.setSortField.call(me, $(this).attr('fieldname'), $(this));
        //};
        var columns = [];
        columns.push({ data: null, width: "18px", orderable: false, defaultContent: "&nbsp;" });
        columns.push({ data: null, width: "10px", orderable: false, defaultContent: "&nbsp;", title: "<input type='checkbox' onclick='onClick_cbxBulkCaseEditAll(this)' id='cbxBulkCaseEditAll'/>" });
   
        $.each(gridSettings.columnDefs, function (idx, fieldSetting) {

            columns.push({
                data: fieldSetting.field,
                title: fieldSetting.displayName,
                className: "thpointer " + fieldSetting.field + " " + fieldSetting.cls,
                visible: !fieldSetting.isHidden,
                width: fieldSetting.width,
                defaultContent: "&nbsp;",
                createdCell: function (td, cellData, rowData, row, col) {
                    if (!fieldSetting.isHidden) {
                        if (cellData === null || cellData === undefined) {
                            td.innerHTML = self.formatCell(rowData.case_id, '');
                            if (Page.isDebug)
                                console.warn('could not find field "' + fieldSetting.field + '" in record');
                        } else {
                            td.innerHTML = self.formatCell(rowData.case_id, cellData);
                        }
                    }
                }
            });
        });


        columns.push({ data: null, width: "22px", orderable: false, defaultContent: "&nbsp;" });

        return columns;
    };

    Page.prototype.showMsg = function (msgType) {
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
        if (msgType === BADCONFIG_MSG_TYPE) {
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

    Page.prototype.getClsRow = function (record) {
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
        var out = [strJoin('<a href="/Cases/Edit/', caseId, '">', cellValue == null ? '&nbsp;' : cellValue.replace(/<[^>]+>/ig, ""), '</a>')];
        return out.join(JOINER);
    };

    Page.prototype.onRemainingViewClick = function (aElement) {
        var self = this;
        self.appendFetch = [
            { 'name': "CaseRemainingTime", 'value': $(aElement).attr("data-remaining-time") },
            { 'name': "CaseRemainingTimeUntil", 'value': $(aElement).attr("data-remaining-time-until") },
            { 'name': "CaseRemainingTimeMax", 'value': $(aElement).attr("data-remaining-time-max") },
            { 'name': "CaseRemainingTimeHours", 'value': $(aElement).attr("data-remaining-time-hours") }
        ];
        self.table.ajax.reload();
    };

    Page.prototype.loadRemainingView = function (htmlContent) {
        var self = this;
        self.$remainingView.html(htmlContent);
        self.$remainingView.find('a').on('click', function (ev) {
            ev.preventDefault();
            self.onRemainingViewClick.call(self, this);
            return false;
        });
    };

    Page.prototype.loadStatisticsView = function (statisticsData) {
        var self = this;
        self.$statisticsViewPlace.html(statisticsData);
    };

    Page.prototype.abortAjaxReq = function () {
        var self = this;
        //TODO: customize data table behaviour
        //if (self.table.ajax != null && self.table.ajax.status == null) {
        //    self.table.ajax.abort();
        //}
    };

    window.app = new Page();

    $(document).ready(function () {
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

function onClick_cbxBulkEditCaseSelect() {
    showHideBulkCaseEditBtn();
}

function onClick_cbxBulkCaseEditAll(e) {
    let bulkCaseEditAll = document.getElementById(e.id);
    if (bulkCaseEditAll.checked) {
        $('.bulkEditCaseSelect:checkbox').prop('checked', true);
    }
    else {
        $('.bulkEditCaseSelect:checkbox').prop('checked', false);
    }

    showHideBulkCaseEditBtn();
}

function showHideBulkCaseEditBtn() {
    let selectedCases = $('.bulkEditCaseSelect:checkbox:checked').length;

    if (selectedCases > 0) {
        $('#liBulkCaseEdit').show();
    }
    else {
        $('#liBulkCaseEdit').hide();
    }
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

        var moveCaseRemainingTimeIntoHidePlace = function () {
            caseRemainingTime.detach().appendTo(caseRemainingTimeHidePlace);
        }

        var moveCaseRemainingTimeIntoShowPlace = function () {
            caseRemainingTime.detach().appendTo(caseRemainingTimeShowPlace);
        }

        var isCaseFilterHided = function () {
            return $("#icoPlus").hasClass('icon-minus-sign');
        }

        var moveCaseRemainingTime = function () {
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

        var bindCaseRemainingTime = function () {
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

        dhHelpdesk.casesList.utils.onEvent("OnCasesLoaded", function () {
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