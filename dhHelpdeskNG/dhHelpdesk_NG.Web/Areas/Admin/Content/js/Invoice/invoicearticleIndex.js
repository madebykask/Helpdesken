$(function () {
    (function ($) {

        window.Params = window.Params || {};
        var showSearchResult = window.Params.ShowSearchResult;

        var articleList = "#lstInvoiceArticles";
        var productareaList = "#lstProductAreas";
        
        var getFilters = function () {
            var filters = {
                articles: [],
                productareas: [],
               
            };

            $(articleList + " option:selected").each(function () {
                filters.articles.push($(this).val());
            });

            $(productareaList + " option:selected").each(function () {
                filters.productareas.push($(this).val());
            });

            return filters;
        };

        $('#ShowSearchResult').click(function() {
            
            var customerId = $('#currentCustomerId').val();
            var article = "";
            var productarea = "";
                       
            
            $(articleList + " option:selected").each(function () {
                article += $(this).val() + ",";
            });

            $(productareaList + " option:selected").each(function () {
                productarea += $(this).val() + ",";
            });

           
            $.get(showSearchResult,
                {
                    'filter.CustomerId': customerId,
                    'filter.InvoiceArticles': article,
                    'filter.ProductAreas': productarea,
                    curTime: new Date().getTime()
                },
                function (articleRows) {                    
                    $("#ArticleIndexRows").html(articleRows);
                }
            );
        });

    })($);

});
