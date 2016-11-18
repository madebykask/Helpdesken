$(function () {
    (function ($) {

        window.Params = window.Params || {};
        var showSearchResult = window.Params.ShowSearchResult;
        var saveContractFieldsSettings = window.Params.SaveContractFieldsSettings;
        var indexUrl = window.Params.IndexUrl;

        var contractcategoryList = "#lstContractCategories";
        var supplierList = "#lstSuppliers";
        
        var rowIds = ['contractnumber', 'casenumber', 'contractcategory_id', 'supplier_id', 'department_id',
                          'responsibleuser_id', 'contractstartdate', 'contractenddate', 'noticedate', 'info',
                          'running', 'finished', 'followupinterval', 'followupresponsibleuser_id', 'filename'];

        var getFilters = function () {
            var filters = {
                categories: [],
                suppliers: [],
               
            };

            $(contractcategoryList + " option:selected").each(function () {
                filters.categories.push($(this).val());
            });

            $(supplierList + " option:selected").each(function () {
                filters.suppliers.push($(this).val());
            });

            return filters;
        };

        $('#ShowSearchResult').click(function() {
            
            var customerId = $('#currentCustomerId').val();
            var category = "";
            var supplier = "";
                       
            
            $(contractcategoryList + " option:selected").each(function () {
                category += $(this).val() + ",";
            });

            $(supplierList + " option:selected").each(function () {
                supplier += $(this).val() + ",";
            });

           
            $.get(showSearchResult,
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
        });



        /*setting*/       

        $('#btnSave').click(function () {
            
            var allData = [];
            for (var i = 0; i < rowIds.length; i++) {
                var rowData = getRowData(rowIds[i]);
                allData.push(rowData);
            }

            $.ajax({
                type: "POST",
                url: saveContractFieldsSettings,
                datatype: "json",
                traditional: true,        
                data: JSON.stringify(allData),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.state) {
                        //ShowToastMessage(result.message, "success");
                        window.location.href = indexUrl;
                    } else {
                        ShowToastMessage("Unexpected error! <br/>" + result.message, "error");
                    }
                }
                
            });    
        });

        function getRowData(fieldId) {
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

        $('#Languages').change(function () {
            var curLang = $(this).val();
            for (var i = 0; i < rowIds.length; i++) {
                setFieldCaption(rowIds[i], curLang);
            }
        });

        function setFieldCaption(fieldId, languageId) {           
            var ret = "";    
            $('.' + fieldId).each(function () {
                    var _id = $(this).attr("id");
                    
                    if (_id == 'fieldCaption'){
                        if (languageId == 1) 
                            $(this).val($(this).attr("swedishLable"));
                        else
                            $(this).val($(this).attr("englishLable"));
                    }                    
            });
        }

    })($);

});
