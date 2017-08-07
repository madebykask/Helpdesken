"use strict";

// controller methods:
var sortOverview = '/CaseSolution/Sort';
var searchOverview = '/CaseSolution/Search';
var rememberSelectedTab = '/CaseSolution/RememberTab';

// rows html defination
var sortableCol = '.sortable';
var caseSolutionRowsArea = '#caseTemplateRowsArea';
var attrSortField = 'data-sort';
var attrSortDir = 'data-asc';

// index html defination
var tabCaseTemplate = '#CaseTemplateTab';
var tabCategory = '#CategoriesTab';
var btnSearch = '#btnCaseSolutionSearch';
var edtSearch = '#caseSolutionSearch';
var categories = '#caseSolFilterCategory';

var substatus = '#caseSolFilterSubstatus';
var Wgroup = '#caseSolFilterWGroup';
var Priority = '#caseSolFilterPriority';
var Status = '#caseSolFilterStatus';
var ProductArea = '#caseSolFilterProductArea';
var UserWGroup = '#caseSolFilterUserWGroup';
var TemplateProductArea = '#caseSolFilterCaseTemplateProductArea';
var Application = '#caseSolFilterApplication';


$(function () {

    
    $(sortableCol).click(function () {
        var sortBy = $(this).attr(attrSortField);
        var asc = $(this).attr(attrSortDir);
        DoSort(sortBy, asc);
    });

    $(btnSearch).click(function () {

        
        
        var searchText = $(edtSearch).val();
        var categoryIds = $(categories).val();

        var subStatusIds = $(substatus).val();
        var WgroupIds = $(Wgroup).val();
        var PriorityIds = $(Priority).val();
        var StatusIds = $(Status).val();
        var ProductAreaIds = $(ProductArea).val();
        var UserWGroupIds = $(UserWGroup).val();
        var TemplateProductAreaIds = $(TemplateProductArea).val();
        var ApplicationIds = $(Application).val();

        doSearch(searchText, categoryIds, subStatusIds, WgroupIds, PriorityIds, StatusIds, ProductAreaIds, UserWGroupIds, TemplateProductAreaIds, ApplicationIds);
    });

    $(edtSearch).keydown(function (e) {
       
        if (e.keyCode == 13) {
            e.preventDefault();
            var searchText = $(edtSearch).val();
            var categoryIds = $(categories).val();

            var subStatusIds = $(substatus).val();
            var WgroupIds = $(Wgroup).val();
            var PriorityIds = $(Priority).val();
            var StatusIds = $(Status).val();
            var ProductAreaIds = $(ProductArea).val();
            var UserWGroupIds = $(UserWGroup).val();
            var TemplateProductAreaIds = $(TemplateProductArea).val();
            var ApplicationIds = $(Application).val();

            doSearch(searchText, categoryIds, subStatusIds, WgroupIds, PriorityIds, StatusIds, ProductAreaIds, UserWGroupIds, TemplateProductAreaIds, ApplicationIds);
            return false;
        }
    });

    $(tabCaseTemplate).click(function () {
        
        $.get(rememberSelectedTab,
            {
                topic: "CaseSolution",
                tab: "CaseTemplate",
                myTime: Date.now()
            },
            function (data) {
                $('#menu1').show();
                $('#menu2').hide();
            });
    });

    $(tabCategory).click(function () {
        
        $.get(rememberSelectedTab,
            {
                topic: "CaseSolution",
                tab: "Categories",
                myTime: Date.now()
            },
            function (data) {
                $('#menu2').show();
                $('#menu1').hide();
            });
    });

    var DoSort = function (sortBy, asc) {
      
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

    function doSearch(searchText, categoryIds, subStatusIds, WgroupIds, PriorityIds, StatusIds, ProductAreaIds, UserWGroupIds, TemplateProductAreaIds, ApplicationIds) {
        //dataType: "json",
        //async: true,
        $.ajax({      
            
            url: searchOverview,
            type: "POST",
            contentType: "application/json",            
            data: JSON.stringify({
                searchText: searchText,
                categoryIds: categoryIds,
                subStatusIds: subStatusIds,                
                WgroupIds: WgroupIds,
                PriorityIds: PriorityIds,
                StatusIds: StatusIds,
                ProductAreaIds: ProductAreaIds,
                UserWGroupIds: UserWGroupIds,
                TemplateProductAreaIds: TemplateProductAreaIds,
                ApplicationIds: ApplicationIds,

            }),
            success: function (filteredModel) {
                $(caseSolutionRowsArea).html(filteredModel);
                $(edtSearch).focus();
                $(edtSearch).val(searchText);
                $(edtSearch).select();
            }
        });
    }

});
