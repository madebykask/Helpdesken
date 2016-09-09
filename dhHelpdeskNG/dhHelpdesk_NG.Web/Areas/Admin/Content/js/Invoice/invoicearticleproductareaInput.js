
function SaveArticleProduct() {
    window.Params = window.Params || {};
    var save = window.Params.Save;


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

    var customerId = $('#currentCustomerId').val();
    var article = "";
    var productarea = "";


    $(articleList + " option:selected").each(function () {
        article += $(this).val() + ",";
    });

    $(productareaList + " option:selected").each(function () {
        productarea += $(this).val() + ",";
    });

    $.post(save,
        {
            'filter.CustomerId': customerId,
            'filter.InvoiceArticles': article,
            'filter.ProductAreas': productarea,
            curTime: new Date().getTime()
        },

        function (result) {            
            if (result != null) {
                if (result.res == "sucess") {                    
                    window.location.href = result.data;
                    return;
                }
                else {
                    ShowToastMessage(result.data, "error");
                    return;
                }
            }
            ShowToastMessage('Unexpected error!','error');                      
        }
    );
}

