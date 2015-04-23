'use strict';

function search(sortBy, sortByAsc) {

    var caseIds = $('#caseIds');

    $.get('/Cases/CaseByIdsContent?caseIds=' + caseIds.val() +
                    '&sortBy=' + sortBy +
                    '&sortByAsc=' + sortByAsc, function (result) {
                        $('#search_result').html(result);
                    })
    .always(function () {
        $(document).trigger("OnCasesLoaded");
    });

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
