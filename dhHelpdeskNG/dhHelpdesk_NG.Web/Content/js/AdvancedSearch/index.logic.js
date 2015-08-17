"use strsict";

var GRID_STATE = {
    IDLE: 0,
    LOADING: 1,
    BAD_CONFIG: 2,
    NO_COL_SELECTED: 3
};


(function ($) {    
        
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
    currentCustomerName = '';
    var allCustomers = [];
    var customerTableRepository = [];
    var globalCounter = 0;

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
    
    function isNullOrEmpty(str) {
        return str == null || str == EMPTY_STR;
    }
    
    function Page() { };

    SetSpecificConditionTab(false);
    
    Page.prototype.init = function(gridInitSettings, doSearchAtBegining) {
        var me = this;        
        //// Bind elements
        customerTableId = 0;
        me.$customerCaseArea = $('div.customer-cases-area');        
        me.$tableLoaderMsg = ' div.loading-msg';
        me.$tableNoDataMsg = ' div.no-data-msg';
        me.$tableErrorMsg = ' div.error-msg';
        me.$noFieldsMsg = $('#search_result div.nofields-msg');
        me.$noAvailableFieldsMsg = $('#search_result div.noavailablefields-msg');
        me.$buttonsToDisableWhenGridLoads = $('ul.secnav a.btn, ul.secnav div.btn-group button, ul.secnav input[type=button], .submit, #btnClearFilter');        
        me.$searchField = '#txtFreeTextSearch';
        me.$filterForm = $('#frmAdvanceSearch');        
        me.$availableCustomer = [];
        $('#lstfilterCustomers option').each(function () {
            me.$availableCustomer.push({
                customerId: $(this).val(),
                customerName: $(this).text()
            });

            allCustomers.push({
                customerId: $(this).val(),
                customerName: $(this).text()
            });
        });
        
        me.$table = [];
        
        me.hideMessage();        
        $('.submit, a.refresh-grid').on('click', function (ev) {
            ev.preventDefault();                        
            if (me._gridState !== window.GRID_STATE.IDLE) {
                return false;
            }
            me.onSearchClick.apply(me);
            return false;
        });                

        $('#CaseInitiatorFilter').keydown(function (e) {
            if (e.keyCode == 13) {
                $("#btnSearch").click();
            }
        });

        $('#txtFreeTextSearch').keydown(function (e) {
            if (e.keyCode == 13) {
                $("#btnSearch").click();
            }
        });

        $('#txtCaseNumberSearch').keydown(function (e) {
            if (e.keyCode == 13) {
                $("#btnSearch").click();                
            }
        });
               
        me.setGridState(window.GRID_STATE.IDLE);
        me.gridSettings = gridInitSettings;
        me.sortSettings = gridInitSettings.sortOptions;                
        $('.input-append.date').datepicker({
            format: 'yyyy-mm-dd',
            autoclose: true
        });

        if (doSearchAtBegining)
            me.onSearchClick();                
               
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

        out.push('<tr><th style="width:18px;" ></th>');
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
                out.push(strJoin('<th class="thpointer ', customerTableId, ' ', fieldSetting.field, ' ', fieldSetting.cls, '" fieldname="', fieldSetting.field, '" >', fieldSetting.displayName, '<i class="', sortCls, '"></i>'));                
            }
        });
        out.push('</tr>');
        out.push('</thead>');
        currentCustomerTable += out.join(JOINER);
    };
              
    Page.prototype.formatCell = function (caseId, cellValue) {
        var out = [strJoin('<td><a href="/Cases/Edit/', caseId, '?backUrl=', '/Cases/AdvancedSearch?', 'doSearchAtBegining=true', '">', cellValue == null ? '&nbsp;' : cellValue, '</a></td>')];
        return out.join(JOINER);
    };

    Page.prototype.loadData = function (data, gridSettings) {
        var me = this;
        var out = [];
        
        // Add Table with Header
        me.setGridSettings(gridSettings);

        out.push('<tbody>');
            
        if (data && data.length > 0) {            
            $.each(data, function (idx, record) {
                var firstCell = strJoin('<td><a href="/Cases/Edit/', record.case_id, '?backUrl=', '/Cases/AdvancedSearch?', 'doSearchAtBegining=true', '"><img title="', record.caseIconTitle, '" alt="', record.caseIconTitle, '" src="', record.caseIconUrl, '" /></a></td>');
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

            out.push('</tbody>')
            out.push('</table>');
            currentCustomerTable += out.join(JOINER);            
        } else {
            me.showMsg(NODATA_MSG_TYPE);
        }
        me.setGridState(window.GRID_STATE.IDLE);        
    };         

    Page.prototype.resetSearch = function () {
        var me = this;
        allCustomerTable = 0;
        showableCustomerCount = 0;
        me.$customerCaseArea.html('');
        customerTableRepository = [];
        globalCounter = 0;
    }

    // DoSearch By Button
    Page.prototype.onSearchClick = function () {        
        var me = this;
        var searchStr = $(me.$searchField).val();        

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
            selectedCustomer = me.$availableCustomer;
        
        if (selectedCustomer.length > 0) {
            me.showMsg(LOADING_MSG_TYPE);
            me.setGridState(window.GRID_STATE.LOADING);
            showableCustomerCount = selectedCustomer.length;            
            
            $.each(selectedCustomer,function (idx, value) {
                curCustomerId = value.customerId;
                curCustomerName = value.customerName;
                me.fetchData([{ 'name': 'currentCustomerId', 'value': curCustomerId }]);
            });
        }
                
    };    

    Page.prototype.onGetData = function (response) {
        var me = this;
        if (response && response.result === 'success' && response.data) {
            if(response.data.length > 0)
                me.loadData(response.data, response.gridSettings);            
        } else {
            me.showMsg(ERROR_MSG_TYPE);
            me.setGridState(window.GRID_STATE.IDLE);
        }
    };

    Page.prototype.fetchData = function(addFetchParam) {
        var me = this;
        var fetchParams;
        var baseParams = me.$filterForm.serializeArray();        

        var sortCallback = function () {
            me.setSortField.call(me, $(this).attr('fieldname'), $(this));
        };

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
            var newTable = {
                CustomerName : currentCustomerName,
                TableId: customerTableId,
                TableData: currentCustomerTable
            }
            customerTableRepository.push(newTable);                                           
            globalCounter += 1;
            currentCustomerTable = '';
            currentCustomerName = '';
            if (globalCounter >= showableCustomerCount) {                
                me.DrawTables(sortCallback);
            }
            customerTableId += 1;
            $("#btnSearch").focus();
        });
    };

    Page.prototype.DrawTables = function (callBack) {
        var me = this;
        var hasData = false;
        
        if (customerTableRepository.length > 0) {
            // Sort customers by name
            var length = customerTableRepository.length - 1;
            do {
                var swapped = false;
                for (var i = 0; i < length; ++i) {                    
                    if (customerTableRepository[i].CustomerName.toLowerCase() > customerTableRepository[i + 1].CustomerName.toLowerCase()) {
                        var temp = customerTableRepository[i];
                        customerTableRepository[i] = customerTableRepository[i + 1];
                        customerTableRepository[i + 1] = temp;
                        swapped = true;
                    }
                }
            }
            while (swapped == true)

            $.each(customerTableRepository, function (idx, value) {
                var tableId = value.TableId;
                var tableData = value.TableData;
                if (tableData != '') {
                    me.$customerCaseArea.append(tableData);
                    me.$customerCaseArea.find('th.thpointer.' + tableId).on('click', callBack);
                    hasData = true;
                }
            });            
        }

        customerTableId = 0;
        currentCustomerTable = '';
        globalCounter = 0;
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
            selectedCustomer = me.$availableCustomer;

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

    window.app = new Page();

    $(document).ready(function() {
        app.init.call(window.app, window.gridSettings, window.doSearchAtBegining);        
    });

    $('#lstfilterCustomers.chosen-select').on('change', function (evt, params) {
        SetSpecificConditionTab(true);
    });

    function SetSpecificConditionTab(resetFilterObjs) {
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
                        $("#SpecificFilterDataPartial").html(_SpecificFilterData);

            });

            $('#SpecificFilterDataPartial').attr('style', '');
            $('#SpecificFilterDataPartial').attr('data-field', customerId);
        }
        else {            
            $('#SpecificFilterDataPartial').attr('style', 'display:none');
            $('#SpecificFilterDataPartial').attr('data-field', '');
        }

        
    }
    
})($);


function getBreadcrumbs(a) {
    var path = $(a).text(), $parent = $(a).parents("li").eq(1).find("a:first");

    if ($parent.length == 1) {
        path = getBreadcrumbs($parent) + " - " + path;
    }
    return path;
}


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


(function ($) {    
    var caseTypeDropDown = window.Params.CaseTypeDropDown;
    var productAreaDropDown = window.Params.ProductAreaDropDown;
    var closingReasonDropDown = window.Params.ClosingReasonDropDown;

    var breadCrumbsPrefix = "#divBreadcrumbs_";
    var hiddenPrefix = "#hid_";    

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