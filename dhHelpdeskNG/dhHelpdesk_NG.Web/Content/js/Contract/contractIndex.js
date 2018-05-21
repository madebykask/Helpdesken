
function createContractsPage(JQuery) {

    var contractsPage = (function ($) {

        this._self = this;
        this.settings = {};
        
        this.contractcategoryList = "#lstContractCategories";
        this.supplierList = "#lstSuppliers";
        
        this.rowIds = ['contractnumber', 'casenumber', 'contractcategory_id', 'supplier_id', 'department_id',
            'responsibleuser_id', 'contractstartdate', 'contractenddate', 'noticedate', 'info',
            'running', 'finished', 'followupinterval', 'followupresponsibleuser_id', 'filename'];

        this.getFilters = function () {
            var filters = {
                categories: [],
                suppliers: []
            };

            $(this.contractcategoryList + " option:selected").each(function () {
                filters.categories.push($(this).val());
            });

            $(this.supplierList + " option:selected").each(function () {
                filters.suppliers.push($(this).val());
            });

            return filters;
        };

        this.changeLanguage = function () {
            var curLang = $(this).val();
            for (var i = 0; i < rowIds.length; i++) {
                this.setFieldCaption(rowIds[i], curLang);
            }
        };

        this.setFieldCaption = function(fieldId, languageId) {
            var ret = "";
            $('.' + fieldId).each(function() {
                var _id = $(this).attr("id");

                if (_id == 'fieldCaption') {
                    if (languageId == 1)
                        $(this).val($(this).attr("swedishLable"));
                    else
                        $(this).val($(this).attr("englishLable"));
                }
            });
        };

        this.showProgress = function(show) {
            $("#globalProgress").toggle(show);
        }

        this.sortBy = function(colName) {
            var self = this;

            var currentSortCol = $("#currentSortCol").val();
            var currentSortOrder = $("#currentSortOrder").val() || 'asc';

            var isAsc = currentSortOrder === 'asc' ? true : false;
            var newIsAsc = currentSortCol === colName ? !isAsc : true;

            var data = {
                customerId: $('#currentCustomerId').val(),
                colName: colName,
                isAsc: newIsAsc
            };

            self.showProgress(true);

            $.ajax({
                url: self.settings.SortActionUrl,
                type: 'POST',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: 'html'
            }).done(function (res) {
                $('#contracts_grid').html(res);
            }).always(function() {
                self.showProgress(false);
            });
        };

        this.searchContracts = function() {

            var self = this;
            
            var data = {
                CustomerId: $('#currentCustomerId').val(),
                Categories: $('#lstContractCategories').val(),
                Suppliers: $('#lstSuppliers').val(),
                ResponsibleUsers: $('#lstResponsible').val(),
                Departments: $('#lstDepartment').val(),
                ShowContracts: $('#lstShow').val(),

                StartDateTo: $("#StartDateTo").val(),
                StartDateFrom: $("#StartDateFrom").val(),
                EndDateTo: $("#EndDateTo").val(),
                EndDateFrom: $("#EndDateFrom").val(),
                NoticeDateTo: $("#NoticeDateTo").val(),
                NoticeDateFrom: $("#NoticeDateFrom").val(),

                SearchText: $('#txtSearch').val(),
                SortColName: $("#currentSortCol").val() || '',
                SortAsc: $("#currentSortOrder").val() === 'asc'
            };

            self.showProgress(true);

            $.ajax({
                url: self.settings.SearchActionUrl,
                type: 'POST',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: 'html'
            }).done(function(res) {
                $('#contracts_grid').html(res);
            }).always(function() {
                self.showProgress(false);
            });
        };

        this.saveSettings = function() {
            var self = this;
            var allData = [];

            for (var i = 0; i < rowIds.length; i++) {
                var rowData = getRowData(rowIds[i]);
                allData.push(rowData);
            }

            self.showProgress(true);

            $.ajax({
                type: "POST",
                url: self.settings.saveContractFieldsSettings,
                datatype: "json",
                traditional: true,
                data: JSON.stringify(allData),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.state) {
                        //ShowToastMessage(result.message, "success");
                        window.location.href = self.settings.indexUrl;
                    } else {
                        ShowToastMessage("Unexpected error! <br/>" + result.message, "error");
                    }
                }
            }).always(function() {
                self.showProgress(false);
            });
        };

        this.getRowData = function (fieldId) {
            var show = false;
            var caption_Sv = "";
            var caption_Eng = "";
            var showInList = false;
            var required = false;
            var dataId = 0;
            var curLanguageId = $('#Languages').val();

            $('.' + fieldId).each(function(){
                var _id = $(this).attr("id");
                dataId = $(this).attr("dataId");

                switch (_id) {
                case 'fieldShow':
                    show = $(this).is(":checked");
                    break;
                case 'fieldCaption':
                    if (curLanguageId == 1) {
                        caption_Sv = $(this).val();
                        caption_Eng = $(this).attr("englishLable");
                    } else {
                        caption_Eng = $(this).val();
                        caption_Sv = $(this).attr("swedishLable");
                    }

                    break;
                case 'fieldMandatory':
                    required = $(this).is(":checked");
                    break;
                case 'fieldShowInList':
                    showInList = $(this).is(":checked");
                    break;
                }
            });

            var ret = {
                Id: dataId,
                ContractField: fieldId, Caption_Eng: caption_Eng, Caption_Sv: caption_Sv,
                Required: required, ShowInList: showInList, Show: show,
                LanguageId: curLanguageId
            };

            return ret;
        }

        //remove? 
        this.showSearchResults = function () {
            var self = this;
            var customerId = $('#currentCustomerId').val();
            var category = "";
            var supplier = "";
            
            $(contractcategoryList + " option:selected").each(function () {
                category += $(this).val() + ",";
            });

            $(supplierList + " option:selected").each(function () {
                supplier += $(this).val() + ",";
            });
            
            $.get(self.settings.showSearchResult,
                {
                    'filter.CustomerId': customerId,
                    'filter.ContractCategories': category,
                    'filter.Suppliers': supplier,
                    curTime: new Date().getTime()
                },
                function (contractRows) {                    
                    $("#ContractIndexRows").html(contractRows);
                }
            );
        };

        this.init = function(opt) {

            var self = this;
            self.settings = opt;

            //subscribe events
            $('#search_form').submit(function (e) {
                self.searchContracts();
                e.preventDefault();
                return false;
            });

            $('#btnSave').click(function(e) {
                self.saveSettings();
                e.preventDefault();
                return false;
            });

            $('#Languages').change(function () {
                self.changeLanguage();
            });

            $('#ShowSearchResult').click(function (e) {
                self.showSearchResults();
                e.preventDefault();
                return false;
            });
        };

        return {
            Init: function (opt) {
                _self.init(opt);
            },

            Sort: function(colName) {
                _self.sortBy(colName);
            }
        };

    }(JQuery));

    return contractsPage;
}