"use strict";

// controller methods:
var sortOverview = '/CaseSolution/Sort';
var searchOverview = '/CaseSolution/Search';

// html defination
var sortableCol = '.sortable';
var caseSolutionRowsArea = '#caseTemplateRowsArea';
var attrSortField = 'data-sort';
var attrSortDir = 'data-asc';

var btnSearch = '#btnCaseSolutionSearch';
var edtSearch = '#caseSolutionSearch';

$(function () {

    $(sortableCol).click(function () {
        var sortBy = $(this).attr(attrSortField);
        var asc = $(this).attr(attrSortDir);
        DoSort(sortBy, asc);
    });

    function DoSort(sortBy, asc) {
        $.get(sortOverview,
                {
                    fieldName: sortBy,
                    asc: asc,
                    myTime: Date.now()
                },
                function (sortedModel) {
                    $(caseSolutionRowsArea).html(sortedModel);
                });
    }


    $(btnSearch).click(function () {
        var searchText = $(edtSearch).val();        
        DoSearch(searchText);
    });

    function DoSearch(searchText) {
        $.get(searchOverview,
                {
                    searchText: searchText,                    
                    myTime: Date.now()
                },
                function (filteredModel) {
                   // $(caseSolutionRowsArea).html(filteredModel);
                });
    }
});
