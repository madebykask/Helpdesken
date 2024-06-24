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
    var EMPTY_STR = '';
    var JOINER = EMPTY_STR;

    var showableCustomerCount = 0;
    var customerTableId;
    var currentCustomerTable = '';
    var currentCustomerName = '';
    var allCustomers = [];
    var customerTableRepository = [];
    var globalCounter = 0;
    var cellUniqueId = 0;
    var isExtendedSearch = false;
    
    function strJoin() {
        return Array.prototype.join.call(arguments, JOINER);
    }

    function getClsForSortDir(sortDir) {
        if (+sortDir === SORT_ASC) {
            return "icon-chevron-up";
        }
        if (+sortDir === SORT_DESC) {
            return "icon-chevron-down";
        }
        return '';
    }

    function isNullOrEmpty(str) {
        return str == null || str == EMPTY_STR;
    }

    $.fn.textWidth = function (isBold) {
        var html_org = $(this).html();
        var cls = isBold == 'true' ? "textbold" : "";
        var html_calc = '<span class="' + cls + '">' + html_org + '</span>';
        $(this).html(html_calc);
        var width = $(this).find('span:first').width();
        $(this).html(html_org);
        return width;
    };

    ////////////////////////////////////////////////////////////////////////////
    function Page() {
    };
    
    Page.prototype.init = function(params) {
        var me = this;

        //set params
        me.gridSettings = params.gridSettings;
        me.doSearchAtBegining = params.doSearchAtBegining;
        me.isExtSearch = params.isExtSearch;
        me.getSpecificFilterDataUrl = params.getSpecificFilterDataUrl;
        me.specificSearchTabBehavior = new SpecificSearchTabBehavior(params);

        //// Bind elements
        me.$chkSearchThruFiles = $('#chkSearchThruFiles');
        me.$includeExtendedCaseValues = $('#includeExtendedCaseValues');
        me.$customerCaseArea = $('div.customer-cases-area');
        me.$tableLoaderMsg = $('div.loading-msg');
        me.$tableNoDataMsg = $('div.no-data-msg');
        me.$tableErrorMsg = $('div.error-msg');
        me.$noFieldsMsg = $('#search_result div.nofields-msg');
        me.$noAvailableFieldsMsg = $('#search_result div.noavailablefields-msg');
        me.$buttonsToDisableWhenGridLoads = $('ul.secnav a.btn, ul.secnav div.btn-group button, ul.secnav input[type=button], .submit, #btnClearFilter');
        me.txtsToSearchByEnterKey = '#CaseInitiatorFilter, #txtFreeTextSearch, #txtCaseNumberSearch, #txtCaptionSearch';
        me.$searchField = $('#txtFreeTextSearch');
        me.$filterForm = $('#frmAdvanceSearch');
        me.$caseAdvSearchRecordCount = $('[data-field="TotalAdvSearchCount"]');

        me.availableCustomers = [];
        customerTableId = 0;

        $('#lstfilterCustomers option').each(function () {
            me.availableCustomers.push({
                customerId: $(this).val(),
                customerName: $(this).text()
            });

            allCustomers.push({
                customerId: $(this).val(),
                customerName: $(this).text()
            });
        });

        $('#lstIncludedCustomers option').each(function () {
            allCustomers.push({
                customerId: $(this).val(),
                customerName: $(this).text()
            });
        });
        
        $('.input-append.date').datepicker({
            format: 'yyyy-mm-dd',
            autoclose: true
        });

        me.$table = [];
        me.setGridState(window.GRID_STATE.IDLE);
        me.sortSettings = me.gridSettings.sortOptions;

        me.hideMessage();
        me.SetSpecificConditionTab(false);

        $('#txtFreeTextSearch').on('input', function (evt, params) {
            me.SetSearchThruFileState();
        });

        $('#lstfilterCustomers.chosen-select').on('change', function (evt, params) {
            me.SetSpecificConditionTab(true);
        });

        $("#extendedSearchEnabled").on("change", function (evt, params) {
            me.isExtendedSearch = $(this).prop("checked");
            if (me.isExtendedSearch) {
                $("#lstfilterCustomers.chosen-select").val("").trigger("chosen:updated");
            }
            me.SetSpecificConditionTab(isExtendedSearch);
        });

        // search by buttons (search, refresh) click 
        $('.submit, a.refresh-grid').on('click', function (ev) {
            ev.preventDefault();                        
            if (me._gridState !== window.GRID_STATE.IDLE) {
                return false;
            }
            me.onSearchClick.apply(me);
            return false;
        });

        //search by enter
        $(me.txtsToSearchByEnterKey).keydown(function (e) {
            if (+e.keyCode === 13) {
                e.preventDefault();
                $("#btnSearch").click();
                return false;
            }
            return true;
        });

        // run extended search on page load
        if (me.isExtSearch) {
            me.onExtSearchLoading();
        }

        //run search on page start if has settings
        if (me.doSearchAtBegining)
            me.onSearchClick();

        $('#btnSearch').focus();
    };

    Page.prototype.setGridState = function(gridStateId) {
        var me = this;
        me._gridState = gridStateId;
        switch (gridStateId) {
            case window.GRID_STATE.IDLE:
                me.$buttonsToDisableWhenGridLoads.removeClass('disabled');
                break;
            case window.GRID_STATE.NO_COL_SELECTED:
                me.$buttonsToDisableWhenNoColumns.addClass('disabled');
                break;
            default:
                me.$buttonsToDisableWhenGridLoads.addClass('disabled');
        }
    };

    Page.prototype.getGridState = function() {
        return this._gridState;
    };    

    Page.prototype.getCustomerName = function(customerId) {
        var res = '';
        $.each(allCustomers, function (index, value) {
            if (value.customerId == customerId)
                res = value.customerName;
        });

        return res;
    }

    Page.prototype.setGridSettings = function(gridSettings) {
        var me = this;
        var hasColSpecialClass = '';
        
        me.sortSettings = gridSettings.sortOptions;

        currentCustomerTable = '';
        currentCustomerName = me.getCustomerName(gridSettings.CustomerId);

        var out = ['<div> <br/> <h5> ' + currentCustomerName + ' </h5> </div>'];
        out.push('<table class="table table-striped table-bordered table-hover table-cases customer' + customerTableId + '">');
        out.push('<thead>');
        out.push('<tr><th style="min-width: 18px;width:18px;" ></th>');

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
                out.push(strJoin('<th class="thpointer ', customerTableId, ' ', fieldSetting.field, ' ',
                     fieldSetting.cls, '" fieldname="', fieldSetting.field, '" >', fieldSetting.displayName, '<i class="', sortCls, '"></i>'));
            }
        });

        out.push('</tr>');
        out.push('</thead>');

        currentCustomerTable += out.join(JOINER);
    };

    Page.prototype.tableCleanUp = function () {
        var expandables = $(".exp");
        var rowsNeedExpandation = [];

        $('a.exp').each(function () {
            var t = $(this).html();                        
            var uniqId = $(this).attr("data-uniqId");
            var caseId = $(this).attr("data-rowId");
            var isBold = $(this).attr("data-isbold");            

            var limit = isBold == "true" ? 60 : 75;

            if (t.length < limit) {
                doExpanation(caseId, true, true, uniqId);                
            }           
        });     
    };

    Page.prototype.onExtSearchLoading = function () {
        $("#extendedSearchEnabled").prop("checked", true);
        isExtendedSearch = true;
        SetSpecificConditionTab(true);
    }

    Page.prototype.resetSearch = function () {
        var me = this;
        allCustomerTable = 0;
        showableCustomerCount = 0;
        me.$customerCaseArea.html('');
        customerTableRepository = [];
        globalCounter = 0;
    }

    // SEARCH 
    Page.prototype.onSearchClick = function () {        
        var me = this;

        var curCustomerId = 0;
        var curCustomerName = '';
        me.resetSearch();

        var selectedCustomer = [];
        $('#lstfilterCustomers option:selected').each(function () {
                selectedCustomer.push({
                    customerId: $(this).val(),
                    customerName: $(this).text()                    
                });            
        });       

        if (selectedCustomer.length <= 0)
            selectedCustomer = me.availableCustomers;

        var customers = [];
        
        if (selectedCustomer.length > 0) {
            me.showMsg(LOADING_MSG_TYPE);
            me.setGridState(window.GRID_STATE.LOADING);
            showableCustomerCount = selectedCustomer.length;            
            cellUniqueId = 0;
            $.each(selectedCustomer,function (idx, value) {
                curCustomerId = value.customerId;
                curCustomerName = value.customerName;
                //me.fetchData([{ 'name': 'currentCustomerId', 'value': curCustomerId }]);
                customers.push(curCustomerId);
            });

            me.fetchData([{ 'name': 'currentCustomerId', 'value': customers }]);
        }
    };
    
    Page.prototype.fetchData = function(addFetchParam) {
        var me = this;
        var fetchParams;
        var baseParams = me.$filterForm.serializeArray();        

        var sortCallback = function () {
            me.setSortField.call(me, $(this).attr('fieldname'), $(this));
        };

        baseParams.push({ name: 'searchThruFiles', value: me.$chkSearchThruFiles.bootstrapSwitch('state') });
        baseParams.push({ name: 'includeExtendedCaseValues', value: me.$includeExtendedCaseValues.bootstrapSwitch('state') });

        baseParams.push(
            { name: 'sortBy', value: me.sortSettings.sortBy },
            { name: 'sortDir', value: me.sortSettings.sortDir },
            { name: 'pageIndex', value: me.sortSettings.pageIndex },
            { name: 'recPerPage', value: me.sortSettings.recPerPage });

        if (addFetchParam != null && addFetchParam.length > 0) {
            fetchParams = baseParams.concat(addFetchParam);
        } else {
            fetchParams = baseParams;
        }

        me.setGridState(window.GRID_STATE.LOADING);                

        $.ajax('/Cases/DoAdvancedSearch', {
            type: 'POST',
            dataType: 'json',
            data: fetchParams,
            success: function () {
                me.onGetData.apply(me, arguments);
            },
            error: function () {
                me.showMsg(ERROR_MSG_TYPE);
                me.setGridState(window.GRID_STATE.IDLE);
            }
        }).done(function () {
            //var newTable = {
            //    CustomerName : currentCustomerName,
            //    TableId: customerTableId,
            //    TableData: currentCustomerTable
            //}
            //customerTableRepository.push(newTable);                                           
            //globalCounter += 1;
            //currentCustomerTable = '';
            //currentCustomerName = '';
            //if (globalCounter >= showableCustomerCount) {                
            //    me.DrawTables(sortCallback);
            //}
            //customerTableId += 1;
            me.DrawTables(sortCallback);
            $("#btnSearch").focus();
        });
    };

    //processes search result and builds results table for each customer
    Page.prototype.onGetData = function (response) {
        var me = this;

        if (response && response.result === 'success' && response.data && response.data.Items) {
            for (var i = 0; i < response.data.Items.length; i++) {
                if (response.data.Items[i].data.length > 0) {
                    currentCustomerTable = '';
                    currentCustomerName = '';

                    //load data: builds html and adds formatted data to the output html 
                    me.loadData(response.data.Items[i].data, response.data.Items[i].gridSettings);

                    var newTable = {
                        CustomerName: currentCustomerName,
                        TableId: i,
                        TableData: currentCustomerTable
                    }
                    customerTableRepository.push(newTable);
                }
            }
            me.$caseAdvSearchRecordCount.text(response.data.TotalCount);
        } else {
            me.$caseAdvSearchRecordCount.text("0");
            me.showMsg(ERROR_MSG_TYPE);
            me.setGridState(window.GRID_STATE.IDLE);
        }
    };

    Page.prototype.loadData = function (data, gridSettings) {
        var me = this;
        var out = [];

        // Add Table with Header
        me.setGridSettings(gridSettings);

        out.push('<tbody>');

        if (data && data.length > 0) {
            $.each(data, function (idx, record) {
                var url = encodeURIComponent(strJoin('/Cases/AdvancedSearch?', 'doSearchAtBegining=true', "&isExtSearch=", isExtendedSearch));
                var caseImg = strJoin('<img title="', record.caseIconTitle, '" alt="', record.caseIconTitle, '" src="', record.caseIconUrl, '" />');
                if (record.isCaseLocked) {
                    caseImg = caseImg + strJoin('<img class="img-case-locked" title="', record.caseLockedIconTitle, '" alt="', record.caseLockedIconTitle, '" src="', record.caseLockedIconUrl, '" />');
                }
                var firstCell = strJoin('<td > <a class="img-case-parent" href="/Cases/Edit/', record.case_id, '?backUrl=', url, '">', caseImg, '</a></td>');;

                if (isExtendedSearch && !record.ExtendedAvailable) {
                    firstCell = strJoin('<td >', caseImg, '</td>');
                }

                var rowClass = me.getClsRow(record);
                var rowOut = [strJoin('<tr class="', rowClass, '" caseid="', record.case_id, '">'), firstCell];

                $.each(me.gridSettings.columnDefs, function (idx, columnSettings) {
                    if (!columnSettings.isHidden) {
                        if (record[columnSettings.field] == null) {

                            rowOut.push(me.formatCell(record.case_id, columnSettings, false, false, record.ExtendedAvailable));

                            if (Page.isDebug) { 
                                console.warn('could not find field "' + columnSettings.field + '" in record');
                            }
                        } else {
                            var isBold = jQuery.inArray('textbold', rowClass) >= 0 || rowClass == 'textbold';

                            rowOut.push(me.formatCell(record.case_id, record[columnSettings.field], columnSettings, isBold, record.ExtendedAvailable));

                        }
                    }
                });
                rowOut.push('</tr>');
                out.push(rowOut.join(JOINER));
            });

            out.push('</tbody>');
            out.push('</table>');
            currentCustomerTable += out.join(JOINER);
        } else {
            me.showMsg(NODATA_MSG_TYPE);
        }
        me.setGridState(window.GRID_STATE.IDLE);

        //var tbl = {
        //    CustomerName: currentCustomerName,
        //    TableData: currentCustomerTable
        //}
        //return tbl;
    };

    Page.prototype.formatCell = function (caseId, cellValue, colSetting, isBold, extendedAvailable) {
        var out = [];

        // Unique id rest after each search
        var uniqId = cellUniqueId++;

        if (isExtendedSearch && !extendedAvailable) {
            if (colSetting.field !== "CaseNumber" && colSetting.field !== "Persons_Name" && colSetting.field !== "Caption") {
                cellValue = null;
            }
        }

        var cellValueFormatted = cellValue == null && cellValue === undefined ? '&nbsp;' : cellValue.toString();

        var url = encodeURIComponent(strJoin('/Cases/AdvancedSearch?', 'doSearchAtBegining=true', "&isExtSearch=", isExtendedSearch));
        if (colSetting.isExpandable) {
            if (isExtendedSearch && !extendedAvailable) {
                out = strJoin(
                    '<td style="width:', colSetting.width, '">',
                    '<div id="divExpand_' + uniqId + '" class="expandable_' + caseId + '" style="height: 15px; overflow: hidden;">', //max-width:500px;
                    '<i class="icon-plus-sign ico-right expandable_', caseId, '" data-uniqId="iIcon_', uniqId, '" id="btnExpander_', caseId, '" onclick="toggleRowExpanation(', caseId, ')"></i> ' +
                    '<a style="line-height:15px;" data-isbold="', isBold, '" data-uniqId="', uniqId, '" data-rowId="', caseId, '" class="exp" >', cellValueFormatted, '</a>',
                    '</div>', '</td>');
            } else {
                out = strJoin(
                    '<td style="width:', colSetting.width, '">',
                    '<div id="divExpand_' + uniqId + '" class="expandable_' + caseId + '" style="height: 15px; overflow: hidden;">', //max-width:500px;
                    '<i class="icon-plus-sign ico-right expandable_', caseId, '" data-uniqId="iIcon_', uniqId, '" id="btnExpander_', caseId, '" onclick="toggleRowExpanation(', caseId, ')"></i> ' +
                    '<a style="line-height:15px;" data-isbold="', isBold, '" data-uniqId="', uniqId, '" data-rowId="', caseId, '" class="exp" href="/Cases/Edit/', caseId, '?backUrl=',
                    url, '">', cellValueFormatted, '</a>',
                    '</div>', '</td>');
            }
        } else {
            if (isExtendedSearch && !extendedAvailable) {
                out = strJoin(
                    '<td style="width:', colSetting.width, '"><a style="line-height:15px;" >', cellValueFormatted, '</a></td>');
            } else {
                out = strJoin(
                    '<td style="width:', colSetting.width, '">',
                    '<a style="line-height:15px;" href="/Cases/Edit/', caseId, '?backUrl=', url, '">', cellValueFormatted, '</a></td>');
            }
        }
        return out;
    };

    Page.prototype.DrawTables = function (callBack) {
        var me = this;
        var hasData = false;
        
        if (customerTableRepository.length > 0) {
            // Sort customers by name
            var length = customerTableRepository.length - 1;
            var swapped;
            do {
                swapped = false;
                for (var i = 0; i < length; ++i) {
                    if (customerTableRepository[i].CustomerName.toLowerCase() >
                        customerTableRepository[i + 1].CustomerName.toLowerCase()) {
                        var temp = customerTableRepository[i];
                        customerTableRepository[i] = customerTableRepository[i + 1];
                        customerTableRepository[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped === true);

            $.each(customerTableRepository, function (idx, value) {
                var tableId = value.TableId;
                var tableData = value.TableData;
                if (tableData !== '') {
                    me.$customerCaseArea.append(tableData);
                    me.$customerCaseArea.find('th.thpointer.' + tableId).on('click', callBack);
                    hasData = true;
                }
            });            
        }

        customerTableId = 0;
        currentCustomerTable = '';
        globalCounter = 0;
        me.tableCleanUp();
        me.hideMessage();

        if (!hasData)
            me.showMsg(NODATA_MSG_TYPE);

        me.setGridState(window.GRID_STATE.IDLE);       
    };
    
    // DoSearch By Sort
    Page.prototype.setSortField = function (fieldName, $el) {
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

        me.sortSettings = sortOpt;

        var curCustomerId = 0;
        var curCustomerName = '';

        me.resetSearch();

        var selectedCustomer = [];
        $('#lstfilterCustomers option:selected').each(function () {
            selectedCustomer.push({
                customerId: $(this).val(),
                customerName: $(this).text()
            });
        });

        if (selectedCustomer.length <= 0)
            selectedCustomer = me.availableCustomers;

        // Start Customers Loop
        if (selectedCustomer.length > 0) {
            me.showMsg(LOADING_MSG_TYPE);
            me.setGridState(window.GRID_STATE.LOADING);
            showableCustomerCount = selectedCustomer.length;
            $.each(selectedCustomer, function (idx, value) {
                curCustomerId = value.customerId;
                curCustomerName = value.customerName;
                me.fetchData([{ 'name': 'currentCustomerId', 'value': curCustomerId }]);
            });
        }
        // End Customers Loop          
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

    Page.prototype.showMsg = function (msgType) {
        var me = this;
        me.hideMessage();
        if (msgType === LOADING_MSG_TYPE) {
            me.$tableLoaderMsg.show();
            return;
        }
        if (msgType === ERROR_MSG_TYPE) {
            me.$tableBody.html('');
            me.$tableErrorMsg.show();
            return;
        }
        if (msgType === NODATA_MSG_TYPE) {
            me.$tableBody.html('');
            me.$tableNoDataMsg.show();
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
        me.$tableLoaderMsg.hide();
        me.$tableErrorMsg.hide();
        me.$tableNoDataMsg.hide();
    };

    Page.prototype.SetSearchThruFileState = function () {
        var me = this;
        if (me.$txtFreeTextSearch.val().trim() === "") {
            me.$chkSearchThruFiles.bootstrapSwitch('state', false).bootstrapSwitch('disabled', true);
            me.$includeExtendedCaseValues.bootstrapSwitch('state', false).bootstrapSwitch('disabled', true);
            //Todo
        } else {
            me.$chkSearchThruFiles.bootstrapSwitch('disabled', false);
            me.$includeExtendedCaseValues.bootstrapSwitch('disabled', false);
        }
    }

    Page.prototype.SetSpecificConditionTab = function (resetFilterObjs) {
        var me = this;
        var selectedCustomers = $('#lstfilterCustomers.chosen-select option');
        var selectedCount = 0;
        var customerId = 0;

        $.each(selectedCustomers, function (idx, value) {
            if (value.selected) {
                customerId = value.value;
                selectedCount++;
            }
        });

        if (selectedCount === 1 && !isExtendedSearch) {

            $.get(me.getSpecificFilterDataUrl,
                {
                    selectedCustomerId: customerId,
                    resetFilter: resetFilterObjs,
                    curTime: new Date().getTime()
                },
                function (specificFilterDataHtml) {
                    
                    $("#SpecificFilterDataPartial").html(specificFilterDataHtml);

                    //apply init logic: event handlers, chosen.... 
                    me.specificSearchTabBehavior.apply();
                });

            $('#SpecificFilterDataPartial').attr('style', '');
            $('#SpecificFilterDataPartial').attr('data-field', customerId);
        } else {
            $('#SpecificFilterDataPartial').attr('style', 'display:none');
            $('#SpecificFilterDataPartial').attr('data-field', '');
        }
    }
    //////////////////////////////////////////////////////////////////////////////////

    //create Page instance
    window.app = new Page();

})(jQuery);


toggleRowExpanation = function (caseId) {

    console.log('>>>toggle row called: CaseId: caseId');

    var curState = $("#btnExpander_" + caseId).attr('class');
    var expandablePlusIcon = 'icon-plus-sign ico-right expandable_' + caseId;

    if (curState == expandablePlusIcon) {
        doExpanation(caseId, true, false, '');
    } else {
        doExpanation(caseId, false, false, '');
    }
};

doExpanation = function (caseId, doExpand, removeExpanation, uniqId) {
    var expanableDivs = '.expandable_' + caseId;
    var expandablePlusIcon = 'icon-plus-sign ico-right expandable_' + caseId;
    var expandableMinusIcon = 'icon-minus-sign ico-right expandable_' + caseId;

    var expandablePlusIcons = '.icon-plus-sign.ico-right.expandable_' + caseId;
    var expandableMinusIcons = '.icon-minus-sign.ico-right.expandable_' + caseId;

    if (doExpand) {
        if (removeExpanation) {
            var divToExpand = '#divExpand_' + uniqId;
            $(divToExpand).css("height", "auto");
            $(divToExpand).css("overflow", "visible");
            $(document).find("[data-uniqId='iIcon_" + uniqId + "']").remove();
        } else {
            $(expanableDivs).css("height", "auto");
            $(expanableDivs).css("overflow", "visible");
            $(expandablePlusIcons).attr('class', expandableMinusIcon);
            $(expandableMinusIcons).attr("style", "");
        }
    } else {
        $(expanableDivs).css("height", "15px");
        $(expanableDivs).css("overflow", "hidden");
        $(expandableMinusIcons).attr('class', expandablePlusIcon);
        $(expandablePlusIcons).attr("style", "");
    }
};

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
        var that = {};

        var searchForm = $('#frmAdvanceSearch');
        var loader = $('<img src="/Content/icons/ajax-loader.gif" />');                

        return that;
    }   
});

//todo: move to separate file and share with new search
function SpecificSearchTabBehavior(params) {
    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";

    var caseTypeDropDown = params.CaseTypeDropDown;
    var productAreaDropDown = params.ProductAreaDropDown;
    var closingReasonDropDown = params.ClosingReasonDropDown;


    this.apply = function () {

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
    }
};
