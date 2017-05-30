function LoadFeedbackCases(pageSettings, caseIds) {
    "use strict";

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

        function FSC() {};

        FSC.prototype.init = function (appSettings) {
            "use strict";
            var self = this;

            self.msgs = appSettings.messages || {};

            self.settings = {
//                filterSettings: appSettings.searchFilter.data,
            };

            //// Bind elements
            self.$table = $("#feedback_caseResults");
            self.$tableBody = $('.table-cases tbody');
            self.$tableLoaderMsg = ' div.loading-msg';
            self.$tableNoDataMsg = ' div.no-data-msg';
            self.$tableErrorMsg = ' div.error-msg';
            self.$noFieldsMsg = $('#feedback_cases_result div.nofields-msg');
            self.$noAvailableFieldsMsg = $('#feedback_cases_result div.noavailablefields-msg');
            self.$buttonsToDisableWhenGridLoads = $('ul.secnav a.btn, ul.secnav div.btn-group button, ul.secnav input[type=button], .submit');
            self.$caseRecordCount = $('[data-field="TotalAmountCases"]');

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

            self.table = InitDataTable("feedback_caseResults",
                appSettings.perPageText,
                appSettings.perShowingText,
                {
                    "sDom": "<'row-fluid'<'span6'l><'span6'p>r>t<'row-fluid'<'span6'i><'span6'p>>",
                    processing: true,
                    serverSide: true,
                    ordering: true,
                    destroy: true,
                    ajax: {
                        url: "/FeedBack/GetCases",
                        type: "POST",
                        data: function(data) {
                            var params = self.getFetchParams(appSettings.gridSettings);
                            params.push({ name: "start", value: data.start });
                            params.push({ name: "length", value: data.length });
                            params.push({ name: "caseIds", value: caseIds });
                            params.push({
                                name: "order",
                                value: data.order.length === 1 ? data.columns[data.order[0].column].data : ""
                            });
                            params.push({ name: "dir", value: data.order.length === 1 ? data.order[0].dir : "" });
                            return params;
                        },
                        dataSrc: "data"
                    },
                    createdRow: function(row, data, dataIndex) {
                        if (data) {
                            $(row).addClass(self.getClsRow(data) + " caseid=" + data.case_id);
                            row.cells[0].innerHTML = strJoin('<a href="/Cases/Edit/', data.case_id, '"><img title="', data.caseIconTitle, '" alt="', data.caseIconTitle, '" src="', data.caseIconUrl, '" /></a>');
                        }
                    },
                    columns: columns,
                    order: sortIndex != null ? [[sortIndex, appSettings.gridSettings.sortOptions.sortDirString]] : [],
                    "bAutoWidth": false,
                    "lengthMenu": [appSettings.gridSettings.pageSizeList, appSettings.gridSettings.pageSizeList],
                    "iDisplayLength": appSettings.gridSettings.pageOptions.recPerPage,
                    "displayStart": appSettings.gridSettings.pageOptions.recPerPage *
                        appSettings.gridSettings.pageOptions.pageIndex
                },
                function(e, settings, techNote, message) {
                    console.log("An error has been reported by DataTable: ", message);
                    var textStatus = arguments[1];
                    if (textStatus !== "abort") {
                        self.showMsg(ERROR_MSG_TYPE);
                        self.setGridState(GRID_STATE.IDLE);
                    }
                });

            self.table.on("xhr.dt",
                    function(e, settings, json) {

                        self._gridUpdated = (new Date()).getTime();

                        if (json && json.result === "success") {

                            if (!json.data || json.data.length < 1) {
                                self.showMsg(NODATA_MSG_TYPE);
                            }

                            self.$caseRecordCount.text(json.recordsTotal);

                            $("#feedback_caseLimitInfo")
                                .toggle(json.recordsTotal === 500);
                        } else {
                            self.showMsg(ERROR_MSG_TYPE);
                            self.setGridState(GRID_STATE.IDLE);
                        }

                        $(document).trigger("OnCasesLoaded");
                    })
            .on("processing.dt",
                    function(e, settings, processing) {
                        if (processing) {
                            self.setGridState(GRID_STATE.LOADING);
                            self.showMsg(LOADING_MSG_TYPE);
                        } else {
                            self.hideMessage();
                            self.setGridState(GRID_STATE.IDLE);
                        }
                    });

            //// Bind events
            $("a.refresh-grid")
                .on('click',
                    function(ev) {
                        ev.preventDefault();
                        if (self._gridState !== GRID_STATE.IDLE) {
                            return false;
                        }
                        self.table.ajax.reload.call(self, { isSearchInitByUser: true });
                        return false;
                    });
        };

        FSC.prototype.getFetchParams = function () {
            "use strict";
            var self = this;
            var fetchParams;
            var expandedGroup = $(self.$hidExpandedGroup).val();
            var baseParams = [];
            baseParams.push({ name: "expandedGroup", value: expandedGroup });

            if (self.appendFetch != null && self.appendFetch.length > 0) {
                fetchParams = baseParams.concat(self.appendFetch);
                delete self.appendFetch;
            } else {
                fetchParams = baseParams;
            }
            return fetchParams;
        };

        /**
        * @private
        * @returns bool
        */
        FSC.prototype.canMakeSearch = function () {
            var self = this;
            if (self._gridState === GRID_STATE.IDLE) {
                return true;
            };
            if (self._gridState === GRID_STATE.LOADING) {
                return true;
            }
            return false;
        };

        FSC.prototype.setGridState = function (newGridState) {
            "use strict";
            var self = this;
            switch (newGridState) {
            case GRID_STATE.IDLE:
                self.$buttonsToDisableWhenGridLoads.removeClass("disabled");
                break;
            case GRID_STATE.NO_COL_SELECTED:
                self.$buttonsToDisableWhenNoColumns.addClass("disabled");
                break;
            case GRID_STATE.LOADING:
                if (self._gridState !== GRID_STATE.IDLE) {
                    self.$buttonsToDisableWhenGridLoads.addClass("disabled");
                }
                break;
            default:
                self.$buttonsToDisableWhenGridLoads.addClass("disabled");
            }
            self._gridState = newGridState;
        };

        FSC.prototype.getGridState = function () {
            return this._gridState;
        };

        FSC.prototype.getColumnSettings = function (gridSettings) {
            "use strict";
            var self = this;

            var columns = [];
            columns.push({ data: null, width: "18px", orderable: false, defaultContent: "&nbsp;" });
            $.each(gridSettings.columnDefs,
                function(idx, fieldSetting) {

                    columns.push({
                        data: fieldSetting.field,
                        title: fieldSetting.displayName,
                        className: "thpointer " + fieldSetting.field + " " + fieldSetting.cls,
                        visible: !fieldSetting.isHidden,
                        width: fieldSetting.width,
                        defaultContent: "&nbsp;",
                        createdCell: function(td, cellData, rowData, row, col) {
                            if (!fieldSetting.isHidden) {
                                if (cellData === null || cellData === undefined) {
                                    td.innerHTML = self.formatCell(rowData.case_id, '');
                                } else {
                                    td.innerHTML = self.formatCell(rowData.case_id, cellData);
                                }
                            }
                        }
                    });
                });

            return columns;
        };

        FSC.prototype.showMsg = function (msgType) {
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

        FSC.prototype.hideMessage = function () {
            var me = this;
            $(me.$tableLoaderMsg).hide();
            $(me.$tableErrorMsg).hide();
            $(me.$tableNoDataMsg).hide();
        };

        FSC.prototype.getClsRow = function (record) {
            var res = [];
            if (record.isUnread) {
                res.push('textbold');
            }
            if (record.isUrgent) {
                res.push('textred');
            }
            return res.join(' ');
        };

        FSC.prototype.formatCell = function (caseId, cellValue) {
            var out = [strJoin('<a href="/Cases/Edit/', caseId, '">', cellValue == null ? "&nbsp;" : cellValue.replace(/<[^>]+>/ig, ""), "</a>")];
            return out.join(JOINER);
        };

        window.fsc = new FSC();
        fsc.init.call(window.fsc, pageSettings);
    })($);
};