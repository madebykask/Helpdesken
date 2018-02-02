//$(function () {
//    (function ($) {

//        window.Params = window.Params || {};
//        var showSearchResult = window.Params.ShowSearchResult;

//        var articleList = "#lstInvoiceArticles";
//        var productareaList = "#lstProductAreas";
        
//        var getFilters = function () {
//            var filters = {
//                articles: [],
//                productareas: [],
               
//            };

//            $(articleList + " option:selected").each(function () {
//                filters.articles.push($(this).val());
//            });

//            $(productareaList + " option:selected").each(function () {
//                filters.productareas.push($(this).val());
//            });

//            return filters;
//        };

//        $('#ShowSearchResult').click(function() {
            
//            var customerId = $('#currentCustomerId').val();
//            var article = "";
//            var productarea = "";
                       
            
//            $(articleList + " option:selected").each(function () {
//                article += $(this).val() + ",";
//            });

//            $(productareaList + " option:selected").each(function () {
//                productarea += $(this).val() + ",";
//            });

           
//            $.get(showSearchResult,
//                {
//                    'filter.CustomerId': customerId,
//                    'filter.InvoiceArticles': article,
//                    'filter.ProductAreas': productarea,
//                    curTime: new Date().getTime()
//                },
//                function (articleRows) {                    
//                    $("#ArticleIndexRows").html(articleRows);
//                }
//            );
//        });

//    })($);

//});

var articleList = "#lstInvoiceArticles";
var productareaList = "#lstProductAreas";

function InvoiceArticles(options) {
    "use strict";

    this.options = $.extend({}, this.getDefaults(), options);
    this.init();

}

InvoiceArticles.prototype = {
    init: function () {
        "use strict";
        var self = this;

        self._initGrid();

        $("#ShowSearchResult").on("click", function () {
            self.table.ajax.reload();
        });
    },

    getDefaults: function () {
        "use strict";

        return {

        };
    },

    _initGrid: function () {
        "use strict";
        var self = this;

        self.table = InitDataTable("tblInviceArticle", self.options.perPageText, self.options.perShowingText,
            {
                "sDom": "<'row-fluid'r>t",
                processing: true,
                serverSide: true,
                ordering: true,
                ajax: {
                    url: self.options.getListUrl,
                    type: "GET",
                    data: function (data) {                                              

                        var article = "";
                        var productarea = "";
                        $(articleList + " option:selected").each(function () {
                            article += $(this).val() + ",";
                        });

                        $(productareaList + " option:selected").each(function () {
                            productarea += $(this).val() + ",";
                        });

                        var filters = {
                            'jsfilter.CustomerId': self.options.customerId,
                            'jsfilter.Order': data.order.length === 1 ? data.order[0].column : "",
                            'jsfilter.Dir': data.order.length === 1 ? data.order[0].dir : "",
                            'jsfilter.SelectedInvoiceArticles': article,
                            'jsfilter.SelectedProductAreas': productarea
                        };                       

                        return filters;
                    },
                    dataSrc: ""
                },
                rowId: "CaseId",
                columns: [
                    {
                        "data": "",
                        "render": function (data, type, row) {
                            return row.InvoiceArticleNumber + "-" + row.InvoiceArticleName + "-" + row.InvoiceArticleNameEng;
                        }
                    },
                    { "data": "ProductAreaName" },
                    {
                        "data": "",
                        "className": "align-center",
                        "render": function (data, type, row) {
                            return row.ProductAreaName ? "<a id='btnDelete' class='btn deleteDialog' deleteDialogText='" + self.options.deleteDialogText + "' href='" + self._getDeleteLink(row.InvoiceArticleId, row.ProductAreaId) + "'>" + self.options.deleteText + "</a>" : "";
                        },
                        "sortable": false
                    }
                ],
                order: [[self.options.initOrder, self.options.initDir]],
                "bPaginate": false,
                //"bAutoWidth": false,
                //"lengthMenu": [appSettings.gridSettings.pageSizeList, appSettings.gridSettings.pageSizeList],
                //"iDisplayLength": appSettings.gridSettings.pageOptions.recPerPage,
                //"displayStart": appSettings.gridSettings.pageOptions.recPerPage * appSettings.gridSettings.pageOptions.pageIndex
            }, function (e, settings, techNote, message) {
                console.log("An error has been reported by DataTable: ", message);
            });
    },

    _getArticlesGridParams: function () {
        var res = {};
        var ia = $("#lstInvoiceArticles").val();
        var pa = $("#lstProductAreas").val();
        if (ia)
            res.SelectedInvoiceArticles = ia;
        if (pa)
            res.SelectedProductAreas = pa;
        return res;
    },

    _getDeleteLink: function(articleId, productAreaId) {
        "use strict";
        var self = this;

        var params = {
            customerid: self.options.customerId,
            articleid: articleId,
            productareaid: productAreaId
        }
        return self.options.deleteUrl + "?" + $.param(params);
    }
}