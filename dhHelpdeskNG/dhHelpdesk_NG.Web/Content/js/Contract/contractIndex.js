$(function () {
    (function ($) {

        window.Params = window.Params || {};
        var showSearchResult = window.Params.ShowSearchResult;

        var contractcategoryList = "#lstContractCategories";
        var supplierList = "#lstSuppliers";
        
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

    })($);

});
