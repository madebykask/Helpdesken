"use strict";

window.advancedSearchPage =
 (function ($) {

    var UI_STATE = {
        IDLE: 0,
        LOADING: 1,
        BAD_CONFIG: 2,
        NO_COL_SELECTED: 3
    };

     /// message types
     var ERROR_MSG_TYPE = 0;
     var NODATA_MSG_TYPE = 2;
     var BADCONFIG_MSG_TYPE = 3;
     var NO_COL_SELECTED_MSG_TYPE = 4;

     var SORT_ASC = 0;
     var SORT_DESC = 1;
     var EMPTY_STR = '';
     var JOINER = EMPTY_STR;

    $.fn.serializeObject = function () {
        var arrayData = this.serializeArray();
        var objectData = {};

        $.each(arrayData, function (idx, el) {
            var value;
            if (this.value != null) {
                value = this.value;
            } else {
                value = '';
            }

            if (objectData[this.name] != null) {
                if (!objectData[this.name].push) {
                    objectData[this.name] = [objectData[this.name]];
                }
                objectData[this.name].push(value);
            } else {
                objectData[this.name] = value;
            }
        });

        return objectData;
    };

    function AdvancedSearchPage() {

        this.init = function (params) {
            var self = this;
            
            this.requests = []; // stores ajax pending requests

            this.messagesMap = self.createMessagesMap();

            //set params
            self.getSpecificFilterDataUrl = params.getSpecificFilterDataUrl;
            self.searchActionUrl = params.searchActionUrl;
            self.specificSearchTabBehavior = new SpecificSearchTabBehavior(params);
            self.sortSettings = params.sortOptions;

            this.isExtendedSearch = false;
            this._gridState = UI_STATE.IDLE;

            //controls 
            this.$filterForm = $('#frmAdvanceSearch');
            this.$txtFreeTextSearch = $('#txtFreeTextSearch');
            this.$btnSearch = $("#btnSearch");
            this.$SpecificFilterDataPartial = $('#SpecificFilterDataPartial');
            this.$chkSearchThruFiles = $('#chkSearchThruFiles');
            this.$includeExtendedCaseValues = $('#includeExtendedCaseValues');
            this.$customerCaseArea = $('div.customer-cases-area');

            this.$tableLoaderMsg = $('div.loading-msg');
            this.txtsToSearchByEnterKey = '#CaseInitiatorFilter, #txtFreeTextSearch, #txtCaseNumberSearch, #txtCaptionSearch';
            this.$buttonsToDisableWhenGridLoads = $('ul.secnav a.btn, ul.secnav div.btn-group button, ul.secnav input[type=button], .submit, #btnClearFilter');
            this.$caseAdvSearchRecordCount = $('[data-field="TotalAdvSearchCount"]');
            
            // populate customers arrays
            this.allCustomers = [];
            this.extendedCustomers = [];
            this.availableCustomers = [];
            this.totalItemsCount = 0;
            this.customersGridSettings = {};
            
            if (params.userCustomers.length) {
                $.each(params.userCustomers, function (idx, el) {
                    var customerItem = mapToCustomerItem(el);
                    self.availableCustomers.push(customerItem);
                    self.allCustomers.push(customerItem);
                });
            }

            if (params.extendedCustomers.length) {
                $.each(params.extendedCustomers, function (idx, el) {
                    var customerItem = mapToCustomerItem(el);
                    if (self.availableCustomers.filter(function(el, i) {
                        return el.customerId === customerItem.customerId;
                        }).length === 0) {
                        self.extendedCustomers.push(customerItem);
                        self.allCustomers.push(customerItem);
                    }
                });
            }

            self.subscribeEvents();
            
            self.setUIState(UI_STATE.IDLE);
            self.hideProgress();
            self.resetSearch();
            
            self.setSpecificConditionTab(false);

            self.setSearchThruFileState();

            if (params.isExtSearch) {
                self.onExtSearchLoading();
            }

            //run search on page start 
            if (params.doSearchAtBegining) {
                self.onSearchClick();
            }

            $('#btnSearch').focus();
        }

        this.createMessagesMap = function () {
            var msgMap = [];
            msgMap[ERROR_MSG_TYPE] = 'error-msg';
            msgMap[NODATA_MSG_TYPE] = 'no-data-msg';
            msgMap[BADCONFIG_MSG_TYPE] = 'noavailablefields-msg';
            msgMap[NO_COL_SELECTED_MSG_TYPE] = 'nofields-msg';
            return msgMap;
        }

        this.subscribeEvents = function () {
            var self = this;

            // search by buttons (search, refresh) click 
            $('.submit, a.refresh-grid').on('click', function (ev) {
                ev.preventDefault();

                if (self._gridState !== UI_STATE.IDLE) return false; 
                self.onSearchClick();

                return false;
            });

            self.$txtFreeTextSearch.on('input', function (evt, params) {
                self.setSearchThruFileState();
            });

            $('#lstfilterCustomers.chosen-select').on('change', function (evt, params) {
                self.setSpecificConditionTab(true);
            });

            $("#extendedSearchEnabled").on("change", function (evt, params) {
                self.isExtendedSearch = $(this).prop("checked");
                if (self.isExtendedSearch) {
                    $("#lstfilterCustomers.chosen-select").val("").trigger("chosen:updated");
                }
                self.setSpecificConditionTab(self.isExtendedSearch);
            });

            //search by enter
            $(self.txtsToSearchByEnterKey).keydown(function (e) {
                if (+e.keyCode === 13) {
                    e.preventDefault();
                    self.onSearchClick();
                    return false;
                }
                return true;
            });
        }

        this.onExtSearchLoading = function () {
            $("#extendedSearchEnabled").prop("checked", true);
            this.isExtendedSearch = true;
            this.setSpecificConditionTab(true);
        };

        this.onSearchClick = function () {
            var self = this;
            
            //reset prev search state
            self.resetSearch();

            var selectedCustomers = self.getSelectedCustomers() || [];
            if (selectedCustomers.length === 0)
                selectedCustomers = self.availableCustomers;

            if (selectedCustomers.length > 0) {

                var customerIds = $.map(selectedCustomers, function (val, i) {
                    return val.customerId;
                });

                //add extended customers to search request
                if (self.isExtendedSearch && self.extendedCustomers.length) {
                    $.each(self.extendedCustomers, function (index, el) {
                        customerIds.push(+el.customerId);
                    });
                }

                this.runSearсh(customerIds);
            }
        }

        //SEARCH Function
        this.runSearсh = function(customerIds) {
            var self = this;
            
            self.setUIState(UI_STATE.LOADING);
            self.showProgress();

            //prepare search data
            
            var filterData = this.getSearchFilterData();

            for (var i = 0; i < customerIds.length; i++) {
                
                var customerId = customerIds[i];

                //run search 
                var request =
                    self.runCustomerSearch(customerId, filterData).done(function (res) {
                        if (res.response.errorMsg) {
                            alert(res.response.errorMsg);
                        }
                        else {
                            self.processCustomerSearchResults(res.customerId, res.response);
                        }
                        
                    }).fail(function (res) {
                        self.processCustomerSearchError(res.customerId, res.err);
                    });
              
                self.requests.push(request);
            }

            //handle all requests complete
            if (this.requests.length > 0) {
                $.when.apply($, this.requests).always(function () {
                    //console.log('>>> All search requests are complete.');
                    self.hideProgress();
                    self.setUIState(UI_STATE.IDLE);
                    self.tableCleanUp();
                    self.$caseAdvSearchRecordCount.text(self.totalItemsCount);
                    self.requests = [];

                    $("#btnSearch").focus();
                });
            }
        }

        this.runCustomerSearch = function (customerId, filterData) {
            var self = this;
            self.hideCustomerMessages(customerId);
            
            //set customer specific params
            filterData.customerId = customerId;
            filterData.IsExtendedSearch = self.isExtendedCustomer(customerId);

            var sortOptions = self.getSortOptionsOrDefaults(customerId);
            self.setSortOptionsParams(sortOptions, filterData);

            console.log('Fetching data for customer: ' + customerId + "Filterdata: " + filterData);
            var request = self.fetchData(filterData);
            return request;
        }
        
        // Run Search By Sort
        this.sortByField = function (customerId, fieldName, $el) {
            var self = this;
            var oldEl;

            var sortOpt = self.customersGridSettings[customerId].sortOptions;
            var oldCls = getClsForSortDir(sortOpt.sortDir);

            if (self._gridState !== UI_STATE.IDLE) {
                return;
            }

            if (sortOpt.sortBy === fieldName) {
                sortOpt.sortDir = (sortOpt.sortDir === SORT_ASC) ? SORT_DESC : SORT_ASC;
                oldEl = $el;
            } else {
                oldEl = $(self.$table).find('thead [fieldname="' + sortOpt.sortBy + '"]');
                sortOpt.sortBy = fieldName;
                sortOpt.sortDir = SORT_DESC;
            }

            if (oldEl.length > 0) {
                $(oldEl).find('i').removeClass(oldCls);
            }

            var curCls = getClsForSortDir(sortOpt.sortDir);
            $($el).find('i').addClass(curCls);

            //update sort options for customer
            self.setCustomerSortOptions(customerId, sortOpt);

            //running search only for selected customer
            var filterData = self.getSearchFilterData();
            self.runCustomerSearch(customerId, filterData).done(function(res) {
                self.processCustomerSearchResults(res.customerId, res.response);
            }).fail(function(res) {
                self.processCustomerSearchError(res.customerId, res.err);
            });
        };

        this.processCustomerSearchError = function (customerId, err) {
            this.showMsg(ERROR_MSG_TYPE, customerId);

            var errMsg = '';
            if (err && err.ErrorMessage) {
                errMsg = err.ErrorMessage;
            } else if (err.Error) {
                errMsg = err.Error;
            } else {
                errMsg = err || 'Unknown error';
            }
            console.error('Fetch data failed. CustomerId: %s. Error: %s', customerId, errMsg);
        }

        this.getSortOptionsOrDefaults = function (customerId) {
            var self = this;
            var sortOptions = self.sortSettings; //default
            if (self.customersGridSettings.hasOwnProperty(customerId)) {
                var ss = self.customersGridSettings[customerId].sortOptions;
                if (ss) {
                    sortOptions = ss;
                }
            }
            return sortOptions;
        }

        this.setSortOptionsParams = function(sortOptions, inputData) {
            inputData.SortBy = sortOptions.sortBy;
            inputData.SortDir = sortOptions.sortDir;
            //inputData.PageIndex = sortOptions.pageIndex;
            //inputData.RecordsPerPage = sortOptions.recPerPage;
        }

        this.setCustomerSortOptions = function (customerId, sortOptions) {
            var self = this;

            if (self.customersGridSettings[customerId] === undefined) {
                self.customersGridSettings[customerId] = {
                    sortOptions: self.sortSettings
                }
            };

            //set customer specific sorting options from response
            if (sortOptions) {
                self.customersGridSettings[customerId].sortOptions = {
                    sortBy: sortOptions.sortBy,
                    sortDir: +sortOptions.sortDir === 1 ? SORT_DESC : SORT_ASC
                };
            }
        }

        this.getSearchFilterData = function (sortOptions) {
            var self = this;
            var fd = $('#frmAdvanceSearch').serializeObject();
            console.log('formData: ', fd);

            const nomalizeParamValue = function (value) {
                if (Array.isArray(value)) {
                    return value.join(',');
                }
                return value;
            }

            var data = {
                IsExtendedSearch: fd.isExtendedSearch || 'false',
                Customers: nomalizeParamValue(fd.lstfilterCustomers || ''),
                CaseProgress: fd.lstfilterCaseProgress || '',
                UserPerformer: nomalizeParamValue(fd.lstfilterPerformer || ''),
                Initiator: fd.CaseInitiatorFilter || '',
                InitiatorSearchScope: fd["CaseSearchFilterData.InitiatorSearchScope"],
                CaseRegistrationDateStartFilter: fd.CaseRegistrationDateStartFilter || '', //date
                CaseRegistrationDateEndFilter: fd.CaseRegistrationDateEndFilter || '', //date
                CaseClosingDateStartFilter: fd.CaseClosingDateStartFilter || '', //date
                CaseClosingDateEndFilter: fd.CaseClosingDateEndFilter || '', //date
                CaseNumber: fd.txtCaseNumberSearch || '',
                CaptionSearch: fd.txtCaptionSearch || '',
                FreeTextSearch: fd.txtFreeTextSearch || '',
                MaxRows: fd.lstfilterMaxRows,
                SearchThruFiles: self.$chkSearchThruFiles.bootstrapSwitch('state'),
                IncludeExtendedCaseValues: self.$includeExtendedCaseValues.bootstrapSwitch('state'),
                // customer filter fields
                WorkingGroup: '',
                Department: '',
                Priority: '',
                StateSecondary: '',
                CaseType: '0', 
                ProductArea: '', 
                CaseClosingReasonFilter: '' 
            };

            if (sortOptions) {
                self.setSortOptionsParams(sortOptions, data);
            }

            // set only if one customer is selected
            if (fd.lstfilterCustomers && !Array.isArray(fd.lstfilterCustomers) && fd.lstfilterCustomers.length >= 0) {
                data.WorkingGroup =  nomalizeParamValue(fd.lstfilterWorkingGroup || '');
                data.Department = nomalizeParamValue(fd.lstfilterDepartment || '');
                data.Priority = nomalizeParamValue(fd.lstfilterPriority || '');
                data.StateSecondary = nomalizeParamValue(fd.lstfilterStateSecondary || '');
                data.CaseType = fd.hid_CaseTypeDropDown || '0';
                data.ProductArea = fd.hid_ProductAreaDropDown || '';
                data.CaseClosingReasonFilter = fd.hid_ClosingReasonDropDown || '';
            } 

            //console.log("filter: %s", data);

            return data;
        }

        this.fetchData = function (inputData) {
            var self = this; 
            var $searchReq = new $.Deferred();
            var customerId = inputData.customerId;
            var actionUrl = self.searchActionUrl;

            //console.log('>>> Customer search cases called. Action: ' + actionUrl + ', Data: ' + JSON.stringify(inputData));

            $.ajax({
                cache: false,
                type: "POST",
                url: actionUrl,
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(inputData),
                success: function (response) {
                    var data = {
                        customerId: customerId,
                        response: response
                    };
                    $searchReq.resolve(data);
                },
                error: function (err) {
                    var data = {
                        customerId: customerId,
                        err: err
                    };
                    $searchReq.reject(data);
                }
            });

            return $searchReq.promise();
        }

        this.processCustomerSearchResults = function(customerId, response) {
            var self = this;
            
            if (response && response.result === 'success') {
                console.log('>>>Fetch data completed successfully. Customer: ' + customerId);
                var responseData = response.data;
                if (responseData.searchResults.length === 0) {
                    $('#customer_sr_' + customerId).hide();
                    return;
                }
                var customer = self.getCustomer(customerId);

                //save sort settings for each customer separately
                if (responseData.gridSettings && responseData.gridSettings.sortOptions)
                    self.setCustomerSortOptions(customerId, responseData.gridSettings.sortOptions);

                //build search results table html 
                var markup = self.buildCustomerSearchResults(customer, responseData.searchResults, responseData.gridSettings);
                self.showCustomerSearchResults(customer.customerId, markup);
                
                var itemsCount = responseData.searchResults.length || 0;
                var totalCount = responseData.count || 0;
                self.showCustomerSearchResultsCount(customerId, itemsCount, totalCount);

                if (responseData.searchResults.length === 0) {
                    self.showMsg(NODATA_MSG_TYPE, customerId);
                } else {
                    //add sorting callback
                    $('#customerTable' + customerId).find('th.thpointer.sr' + customerId).click(function(e) {
                        self.sortByField(customerId, $(this).attr('fieldname'), $(this));
                    });
                    self.totalItemsCount += responseData.count;
                }
            } else {
                self.showMsg(ERROR_MSG_TYPE, customerId);
            };
        }
        
        this.buildCustomerSearchResults = function(customer, data, gridSettings) {
            var self = this;
            var out = [];
            var customerId = customer.customerId;


            if (gridSettings.availableColumns === 0) {
                self.showMsg(BADCONFIG_MSG_TYPE, customerId);
                self.setUIState(UI_STATE.BAD_CONFIG);
                return null;
            }

            if (gridSettings.columnDefs.length === 0) {
                self.showMsg(NO_COL_SELECTED_MSG_TYPE, customerId);
                //self.setUIState(UI_STATE.NO_COL_SELECTED);
                return null;
            }

            // Add Search results Table Header based on customer grid settings
            var header = self.buildGridHeader(customer, gridSettings);
            if (header === null)
                return out;
                
            out.push(header);
            out.push('<tbody>');

            if (data && data.length > 0) {
                $.each(data, function (rowId, record) {

                    //todo: refactor url construction
                    var url = encodeURIComponent(strJoin('/Cases/AdvancedSearch?', 'doSearchAtBegining=true', "&isExtSearch=", self.isExtendedSearch));
                    var caseImg = strJoin('<img title="', record.caseIconTitle, '" alt="', record.caseIconTitle, '" src="', record.caseIconUrl, '" />');
                    if (record.isCaseLocked) {
                        caseImg = caseImg + strJoin('<img class="img-case-locked" title="', record.caseLockedIconTitle, '" alt="', record.caseLockedIconTitle, '" src="', record.caseLockedIconUrl, '" />');
                    }
                    var firstCell = strJoin('<td><a class="img-case-parent" href="/Cases/Edit/', record.case_id, '?backUrl=', url, '">', caseImg, '</a></td>');

                    if (self.isExtendedSearch && !record.ExtendedAvailable) {
                        firstCell = strJoin('<td >', caseImg, '</td>');
                    }

                    var rowClass = getClsRow(record);
                    var rowOut = [strJoin('<tr class="', rowClass, '" caseid="', record.case_id, '">'), firstCell];

                    $.each(gridSettings.columnDefs, function (colId, columnSettings) {
                        if (!columnSettings.isHidden) {
                            var formattedCell = '';
                            var cellId = getCellUniqueId(customerId, rowId + 1, colId + 1);
                            if (record[columnSettings.field] == null) {
                                formattedCell = self.formatCell(record.case_id, columnSettings, false, false, record.ExtendedAvailable, cellId);
                                rowOut.push(formattedCell);
                                console.warn('could not find field \'%s\' in the record. CustomerId: %s, RowId: %s', columnSettings.field, customerId, rowId);
                            } else {
                                var isBold = jQuery.inArray('textbold', rowClass) >= 0 || rowClass === 'textbold';
                                formattedCell = self.formatCell(record.case_id, record[columnSettings.field], columnSettings, isBold, record.ExtendedAvailable, cellId);
                                rowOut.push(formattedCell);
                            }
                        }
                    });
                    rowOut.push('</tr>');
                    out.push(rowOut.join(JOINER));
                });

                out.push('</tbody>');
                out.push('</table>');
            }
            
            return out.join(JOINER);
        };
        
        this.buildGridHeader = function (customer, gridSettings) {
            var self = this;
            var out = [];
            var customerId = customer.customerId;

            out.push('<thead>');
            out.push('<tr><th style="min-width: 18px;width:18px;"></th>');
            
            $.each(gridSettings.columnDefs, function (idx, fieldSetting) {
                var sortCls = '';
                if (!fieldSetting.isHidden) {
                    if (gridSettings.sortOptions != null && fieldSetting.field === gridSettings.sortOptions.sortBy) {
                        sortCls = getClsForSortDir(gridSettings.sortOptions.sortDir);
                    }
                    var headerRow =
                        strJoin('<th class="thpointer sr', customerId, ' ', fieldSetting.field, ' ', fieldSetting.cls,
                        '" fieldname="', fieldSetting.field, '">', fieldSetting.displayName, '<i class="', sortCls,'"></i></th>');
                    out.push(headerRow);
                }
            });

            out.push('</tr>');
            out.push('</thead>');

            return out;
        };

        this.formatCell = function (caseId, cellValue, colSetting, isBold, extendedAvailable, uniqId) {
            var self = this;
            var out = '';
            var addExtendedInfo = false;


            if (self.isExtendedSearch && !extendedAvailable) {
                if (colSetting.field !== "CaseNumber" && colSetting.field !== "Persons_Name" && colSetting.field !== "Caption") {
                    cellValue = null;
                }
                addExtendedInfo = true;
            }

            var url = encodeURIComponent(strJoin('/Cases/AdvancedSearch?', 'doSearchAtBegining=true', "&isExtSearch=" + self.isExtendedSearch));
            cellValue = isNullOrUndefined(cellValue) ? '&nbsp;' : cellValue.toString(); 


            //if (colSetting.field == "Description") {
            //    cellValue = $("<p/>").html(cellValue).text();

            //    //Remove the width elements
            //    $(cellValue).find('*').each(function () {

            //        console.log($(this).css('width'))

            //        $(this).css('width', '');
            //    });

            //}
            
            
            if (colSetting.isExpandable) {

                out = addExtendedInfo 
                        ? strJoin(
                            '<td style="width:', colSetting.width, '">',
                            '<div id="divExpand_' + uniqId + '" class="expandable_' + caseId + '" style="height: 15px; overflow: hidden;">', //max-width:500px;
                            '<i class="icon-plus-sign ico-right expandable_', caseId, '" data-uniqId="iIcon_', uniqId, '" id="btnExpander_', caseId, '" onclick="toggleRowExpanation(', caseId, ')"></i> ' +
                            '<a style="line-height:15px;" data-isbold="', isBold, '" data-uniqId="', uniqId, '" data-rowId="', caseId, '" class="exp" >', cellValue, '</a>',
                            '</div>',
                            '</td>') 
                        : strJoin(
                            '<td style="width:', colSetting.width, '">',
                            '<div id="divExpand_' + uniqId + '" class="expandable_' + caseId + '" style="height: 15px; overflow: hidden;">', //max-width:500px;
                            '<i class="icon-plus-sign ico-right expandable_', caseId, '" data-uniqId="iIcon_', uniqId, '" id="btnExpander_', caseId, '" onclick="toggleRowExpanation(', caseId, ')"></i> ' +
                            '<a style="line-height:15px;" data-isbold="', isBold, '" data-uniqId="', uniqId, '" data-rowId="', caseId, '" class="exp" href="/Cases/Edit/', caseId, '?backUrl=', url, '">', cellValue, '</a>',
                            '</div>',
                            '</td>');
            } else {
                out = addExtendedInfo 
                        ? strJoin('<td style="width:', colSetting.width, '"> <a style="line-height:15px;" >', cellValue, '</a></td>')
                        : strJoin(
                            '<td style="width:', colSetting.width, '"> ' +
                            '<a style="line-height:15px;" href="/Cases/Edit/', caseId, '?backUrl=', url, '">', cellValue, '</a>' +
                            '</td>');
            }

            return out;
        };

        this.tableCleanUp = function () {

            $('a.exp').each(function () {
                var t = $(this).html();
                var uniqId = $(this).attr("data-uniqId");
                var caseId = $(this).attr("data-rowId");
                var isBold = $(this).attr("data-isbold");

                var limit = isBold === "true" ? 60 : 75;

                if (t.length < limit) {
                    doExpanation(caseId, true, true, uniqId);
                }
            });
        };

        this.setSearchThruFileState = function () {
            var self = this;
            if (self.$txtFreeTextSearch.val().trim() === "") {
                self.$chkSearchThruFiles.bootstrapSwitch('state', false);
                self.$chkSearchThruFiles.bootstrapSwitch('disabled', true);
                self.$includeExtendedCaseValues.bootstrapSwitch('state', false);
                self.$includeExtendedCaseValues.bootstrapSwitch('disabled', true);
            } else {
                self.$chkSearchThruFiles.bootstrapSwitch('disabled', false);
                self.$includeExtendedCaseValues.bootstrapSwitch('disabled', false);
            }
        }

        this.setSpecificConditionTab = function (resetFilterObjs) {
            var self = this;
            var selectedCustomers = self.getSelectedCustomers();

            if (selectedCustomers.length === 1 && !self.isExtendedSearch) {
                var customerId = selectedCustomers[0].customerId;
                var data = {
                    selectedCustomerId: customerId,
                    resetFilter: resetFilterObjs,
                    //curTime: new Date().getTime()
                };

                //todo: add progress here 

                $.get(self.getSpecificFilterDataUrl,
                      $.param(data),
                      function (specificFilterHtml) {
                          self.$SpecificFilterDataPartial.html(specificFilterHtml);
                          //console.log('>>> setSpecificConditionTab called');
                          self.$SpecificFilterDataPartial.ready(function () {
                              //console.log('>>> html is ready - can call apply behavior now.');
                              self.specificSearchTabBehavior.apply();
                          });
                      });
                self.$SpecificFilterDataPartial.attr('style', '');
                self.$SpecificFilterDataPartial.attr('data-field', customerId);
            } else {
                self.$SpecificFilterDataPartial.attr('style', 'display:none');
                self.$SpecificFilterDataPartial.attr('data-field', '');
            }
        }

        this.getSelectedCustomers = function () {
            var selectedCustomers = [];

            $('#lstfilterCustomers option:selected').each(function () {
                selectedCustomers.push({
                    customerId: +$(this).val(),
                    customerName: $(this).text()
                });
            });
            return selectedCustomers;
        }

        this.getCustomer = function (customerId) {
            var res = this.allCustomers.filter(function (el, i) {
                return el.customerId === +customerId;
            });
            return res && res.length ? res[0] : null;
        }

        this.isExtendedCustomer = function (customerId) {
            if (this.extendedCustomers) {
                var res = this.extendedCustomers.filter(function (el, index) {
                    return el.customerId === customerId;
                });
                return res && res.length > 0;
            }
            return false;
        }

        this.showCustomerSearchResults = function (customerId, content) {
            $('#customer_sr_' + customerId).show();
            $('#customerTable' + customerId).html(content);
        }

        this.showCustomerSearchResultsCount = function (customerId, itemsCount, totalCount) {
            var $itemsCount = $('#customer_sr_' + customerId).find('.itemsCount');
            $itemsCount.html('(' + itemsCount.toString() + ' / ' + totalCount.toString() +')');
        }

        this.showMsg = function(msgType, customerId) {
            var self = this;
            var msgCls = self.messagesMap[msgType];
            
            if (msgCls && msgCls.length) {
                var $customerMessages = $('#customer_sr_' + customerId + ' div.search-result-messages');
                $customerMessages.find('.' + msgCls).show();
                $customerMessages.show();

            } else {
                console.warn('Message type \'%s\' is not supported', msgType);
            }
        };

        this.showProgress = function () {
            this.$tableLoaderMsg.show();
        }

        this.hideProgress = function () {
            this.$tableLoaderMsg.hide();
        }

        this.hideCustomerMessages = function(customerId) {
            var $customerMessages = $('#customer_sr_' + customerId + ' div.search-result-messages');
            $customerMessages.find(".msg").each(function() {
                $(this).hide();
            });
            $customerMessages.hide();
        };

        this.setUIState = function (stateId) {
            var self = this;
            self._gridState = stateId;

            switch (stateId) {
            case UI_STATE.IDLE:
                self.$buttonsToDisableWhenGridLoads.removeClass('disabled');
                break;
            case UI_STATE.NO_COL_SELECTED:
                //self.$buttonsToDisableWhenNoColumns.addClass('disabled');
                break;
            //case UI_STATE.BAD_CONFIG:
            default:
                self.$buttonsToDisableWhenGridLoads.addClass('disabled');
            }
        };

        this.resetSearch = function () {
            //reset prev search results state
            this.totalItemsCount = 0;
            this.requests = [];
            this.$caseAdvSearchRecordCount.text('0');

            //hide prev search results
            $('div[id^=customer_sr_]').each(function() {
                $(this).hide();
                $(this).find('.search-result-messages div.msg').each(function() {
                    $(this).hide();
                });
                $(this).find('.table-cases').html('');
                $(this).find('.itemsCount').html('');
            });
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

        function getClsRow(record) {
            var res = [];
            if (record.isUnread) {
                res.push('textbold');
            }
            if (record.isUrgent) {
                res.push('textred');
            }
            return res.join(' ');
        };

        function getCellUniqueId(customerId, rowId, colId) {
            return 'cell_' + customerId + '_' + rowId + '_' + colId;
        }

        function mapToCustomerItem(item) {
            return {
                customerId: +item.Value,
                customerName: item.Name
            }
        }

        function isNullOrUndefined(val) {
            return val === null || val == undefined;
        }

        function strJoin() {
            return Array.prototype.join.call(arguments, JOINER);
        }
    };
    
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

    //create new page instance
    return new AdvancedSearchPage();
 })(jQuery);


function toggleRowExpanation(caseId) {
    var curState = $("#btnExpander_" + caseId).attr('class');
    var expandablePlusIcon = 'icon-plus-sign ico-right expandable_' + caseId;

    if (curState === expandablePlusIcon) {
        doExpanation(caseId, true, false, '');
    } else {
        doExpanation(caseId, false, false, '');
    }
};

function doExpanation(caseId, doExpand, removeExpanation, uniqId) {
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