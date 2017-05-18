"use strict";

var searchOverview = '/QuickLink/Search';

var btnSearch = '#btnQuickLinkSearch';
var edtSearch = '#quickLinkSearch';
var quickLinksRowsArea = '#quickLinksRowsArea';
var customerId = "#qlCustomerId";
var groups = '#quickLinkFilterGroup';


$(function () {
    $(btnSearch).click(function () {
        var searchText = $(edtSearch).val();
        var groupIds = $(groups).val();
        doSearch(searchText, groupIds);
    });

    $(edtSearch).keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            var searchText = $(edtSearch).val();
            var groupIds = $(groups).val();
            doSearch(searchText, groupIds);
            return false;
        }
    });

    function doSearch(searchText, groupIds) {
        $.ajax({
            url: searchOverview,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                searchText: searchText,
                customerId: $(customerId).val(),
                groupIds: groupIds
            }),
            success: function (filteredModel) {
                $(quickLinksRowsArea).html(filteredModel);
                $(edtSearch).focus();
                $(edtSearch).val(searchText);
                $(edtSearch).select();
            }
        });
    }

});
