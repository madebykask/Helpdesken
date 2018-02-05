function search(sortBy, sortByAsc) {
    var relatedCasesInvId = $("#relatedCasesInvId");
    var relatedCasesInvType = $("#relatedCasesInvType");
    if (relatedCasesInvId) {
        $.get('/Inventory/Inventory/RelatedCasesRows?inventoryId=' + relatedCasesInvId.val() +
                        '&inventoryType=' + relatedCasesInvType.val() +
                        '&sortBy=' + sortBy +
                        '&sortByAsc=' + sortByAsc, function (result) {
                            $('#search_result').html(result);
                        });
        return;
    }
}

function sortCases(sortBy) {
    var asc = $("#hidSortByAsc").val();
    var selected = $("#hidSortBy").val();

    if (sortBy == selected) {
        asc = asc.toLowerCase().match('true') ? 'false' : 'true';
    }
    else {
        asc = 'true';
    }

    $("#hidSortBy").val(sortBy);
    $("#hidSortByAsc").val(asc);
    search(sortBy, asc);
}